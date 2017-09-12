using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using ProductStore.Domein.Abstract;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using ProductStore.Domein.Entities;
using ProductStore.WebUI.Models;
using System.Collections;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        IProductRepository repository;
        ICategoryRepository category;
        public AdminController(IProductRepository repo, ICategoryRepository category)
        {
            repository = repo;
            this.category = category;
        }

        // GET: Admin
        public ActionResult Index()
        {
            var users = UserManager.Users.ToList();
            var roles = new List<string>();
            foreach (var user in users)
            {
                string str = "";
                var userRoles = UserManager.GetRoles(user.Id);
                foreach (var role in  userRoles)
                {
                    str = (str == "") ? str = role.ToString() : str + " - " + role.ToString();
                }
                roles.Add(str);
            }
            var model = new AdminViewModels
            {
                Users = users.ToList(),
                Roles = roles.ToList(),
                CountOfCategories = GetCategoriesSelectList().Count(),
                CountOfProducts = repository.Products.Count(),
                CountOfUsers = users.Count()
            };
            return View(model);
        }

        public async Task<ActionResult> DeleteUser(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["success"] = string.Format("Пользователь \"{0}\" был удалён!", user.UserName);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EditUser(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            var roles = await UserManager.GetRolesAsync(id);
            if(user != null)
            {
                if (roles[0] == "Administrator")
                {
                    IdentityResult result = await UserManager.RemoveFromRoleAsync(user.Id, "Administrator");
                    if (result.Succeeded)
                    {
                        await UserManager.AddToRolesAsync(user.Id, "User");
                        TempData["success"] = string.Format("Пользователь \"{0}\" лишён прав администратора!", user.UserName);
                    }
                        
                }
                else
                {
                    IdentityResult result = await UserManager.RemoveFromRoleAsync(user.Id, "User");
                    if (result.Succeeded)
                    {
                        await UserManager.AddToRolesAsync(user.Id, "Administrator");
                        TempData["success"] = string.Format("Пользователю \"{0}\" были выданы права администратора!", user.UserName);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult List()
        {
            var model = new ProductsListViewModel
            {
                Products = repository.Products,
                pagingInfo = null,
                CurrentCategory = null,
                CountOfProducts = repository.Products.Count(),
                CountOfCategories = GetCategoriesSelectList().Count(),
                CountOfUsers = UserManager.Users.Count()
            };
            return View(model);
        }

        public ActionResult DeleteProduct(int id)
        {
            Product deleteProduct = repository.DeleteProduct(id);
            if(deleteProduct != null)
            {
                TempData["success"] = String.Format("Товар \"{0}\" успешно удалён!", deleteProduct.Name);
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            var model = new CategoriesListViewModel
            {
                Product = repository.Products.FirstOrDefault(m => m.ProductId == id),
                Categories = GetCategoriesSelectList(),
                CountOfCategories = GetCategoriesSelectList().Count(),
                CountOfProducts = repository.Products.Count(),
                CountOfUsers = UserManager.Users.Count()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditProduct(CategoriesListViewModel model, HttpPostedFileBase image = null)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            model.Categories = GetCategoriesSelectList();
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    model.Product.ImageMimeType = image.ContentType;
                    model.Product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(model.Product.ImageData, 0, image.ContentLength);
                }
                if(!repository.Products.Where(m => m.ProductId != model.Product.ProductId).Select(m => m.Name).Contains(model.Product.Name))
                {
                    repository.SaveProduct(model.Product);
                    TempData["success"] = string.Format("Изменения в товаре \"{0}\" успешно сохранены!", model.Product.Name);
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["error"] = string.Format("Товар с названием \"{0}\" уже существует!", model.Product.Name);
                    return View(model);
                }
                
            }
            else
            {
                return View(model);
            }
        }

        public ViewResult CreateProduct()
        {
            CategoriesListViewModel model = new CategoriesListViewModel();
            model.Product = new Product();
            model.Categories = GetCategoriesSelectList();
            model.CountOfProducts = repository.Products.Count();
            model.CountOfUsers = UserManager.Users.Count();
            model.CountOfCategories = GetCategoriesSelectList().Count();
            return View("EditProduct", model);
        }

        public ActionResult ListOfCategories()
        {
            var model = new CategoriesListViewModel
            {
                Categories = GetCategoriesSelectList(),
                CountOfCategories = GetCategoriesSelectList().Count(),
                CountOfProducts = repository.Products.Count(),
                CountOfUsers = UserManager.Users.Count()
            };
            return View(model);
        }

        public ActionResult DeleteCategory(int id)
        {
            Category deleteCategory = category.DeleteCategory(id);
            if (deleteCategory != null)
            {
                TempData["success"] = String.Format("Категория \"{0}\" успешно удалена!", deleteCategory.CategoryName);
            }
            return RedirectToAction("ListOfCategories");
        }

        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            CategoryViewModel model = new CategoryViewModel
            {
                Id = id,
                Name = category.Categories.FirstOrDefault(m => m.ID == id).CategoryName,
                CountOfCategories = category.Categories.Count(),
                CountOfProducts = repository.Products.Count(),
                CountOfUsers = UserManager.Users.Count()
            }; 
            return View(model);
        }

        [HttpPost]
        public ActionResult EditCategory(CategoryViewModel model)
        {
            Category category = new Category { ID = model.Id, CategoryName = model.Name };
            if (ModelState.IsValid)
            {
                if (!this.category.Categories.Where(m => m.ID != model.Id).Select(m => m.CategoryName).Contains(model.Name))
                {
                    this.category.SaveCategory(category);
                    TempData["success"] = string.Format("Категория \"{0}\" успешно изменена!", category.CategoryName);
                    return RedirectToAction("ListOfCategories");
                }
                else
                {
                    TempData["error"] = string.Format("Категория с названием \"{0}\" уже существует!", model.Name);
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        public ViewResult CreateCategory()
        {
            CategoryViewModel model = new CategoryViewModel();
            model.CountOfProducts = repository.Products.Count();
            model.CountOfUsers = UserManager.Users.Count();
            model.CountOfCategories = GetCategoriesSelectList().Count();
            return View("EditCategory", model);
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private IEnumerable<SelectListItem> GetCategoriesSelectList()
        {
            return category.Categories.Select(
                b => new SelectListItem { Value = b.ID.ToString(), Text = b.CategoryName });
        }
    }
}