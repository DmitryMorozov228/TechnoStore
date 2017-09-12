using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductStore.Domein.Entities
{
    public class Category
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите название категории")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 30 символов")]
        [Display(Name = "Категория")]
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
            Products = new List<Product>();
        }
    }
}
