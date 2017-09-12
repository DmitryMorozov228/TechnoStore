using ProductStore.Domein.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;
        private ICategoryRepository category;
        public NavController(IProductRepository repo, ICategoryRepository category)
        {
            repository = repo;
            this.category = category;
        }
        public PartialViewResult Menu(string selectedCategory = null)
        {
            ViewBag.SelectedCategory = selectedCategory;
            IEnumerable<string> categories = category.Categories
                .Select(c => c.CategoryName)
                .Distinct()
                .OrderBy(c => c);
            return PartialView(categories);
        }
    }
}