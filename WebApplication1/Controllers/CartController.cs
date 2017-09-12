using ProductStore.Domein.Abstract;
using ProductStore.Domein.Entities;
using ProductStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        public IProductRepository repository;
        public IOrderProcessor orderProcess;
        public CartController(IProductRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcess = processor;
        }

        public ViewResult CheckOut()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult CheckOut(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }
            if (ModelState.IsValid)
            {
                orderProcess.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        public RedirectToRouteResult AddToCart(Cart cart, int ProductId, string returnUrl)
        {
            Product Product = repository.Products
                .FirstOrDefault(g => g.ProductId == ProductId);

            if (Product != null)
            {
                cart.AddItem(Product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int ProductId, string returnUrl)
        {
            Product Product = repository.Products
                .FirstOrDefault(g => g.ProductId == ProductId);

            if (Product != null)
            {
                cart.RemoveLine(Product);
                TempData["message"] = string.Format("Товар \"{0}\" был удалён!", Product.Name);
            }
            else
            {
                cart.Clear();
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            }
            );
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
    }
}