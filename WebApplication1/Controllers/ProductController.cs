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
    public class ProductController : Controller
    {
        private IProductRepository repository;
        private ICategoryRepository category;
        public int pageSize = 8;
        public ProductController(IProductRepository repo, ICategoryRepository category)
        {
            repository = repo;
            this.category = category;
        }

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult List(string selectedCategory, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = selectedCategory == null ? repository.Products
                .OrderBy(Product => Product.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize) : category.Categories
                .Where(p => selectedCategory == null || p.CategoryName == selectedCategory)
                .First()
                .Products
                .ToList()
                .OrderBy(Product => Product.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsForPage = pageSize,
                    TotalItems = selectedCategory == null ?
                    repository.Products.Count() :
                    category.Categories.Where(Product => Product.CategoryName == selectedCategory).First().Products.Count()
                },

                CurrentCategory = selectedCategory
            };

            if(model.Products.Count() == 0)
            {
                TempData["find"] = "В данной категории отсутствуют товары!";
            }
            return View(model);
        }

        public ActionResult Find(string searchTehno, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                .Where(p => p.Name == searchTehno)
                .OrderBy(Product => Product.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsForPage = pageSize,
                    TotalItems = searchTehno == null ?
                    repository.Products.Count() :
                    repository.Products.Where(Product => Product.Name == searchTehno).Count()
                }
            };
            if (model.Products.Count() == 0)
            {
                TempData["find"] = "Поиск не дал результатов";
                return RedirectToAction("List", "Product");
            }
            else
            {
                return View(model);
            }
        }

        public FileContentResult GetImage(int ProductId)
        {
            Product product = repository.Products.FirstOrDefault(g => g.ProductId == ProductId);
            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}