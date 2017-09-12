using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProductStore.Domein.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductId { get; set; }
        [RegularExpression(@"[a-zA-Z0-9-]{3,50}", ErrorMessage = "Поле \'Name\' должно содержать только латинские буквы и цифры, состоять из 3-50 символов")]
        [Required(ErrorMessage = "Пожалуйста, введите название товара")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Поле \'Name\' должно содержать от 3 до 50 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [RegularExpression(@"[ :,.а-яА-Я0-9\r\n]{3,300}", ErrorMessage = "Поле \'Descriptions\' должно содержать только кириллические буквы и цифры, состоять из 3-300 символов")]
        [Required(ErrorMessage = "Пожалуйста, введите описание для товара")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Поле \'Descriptions\' должно содержать от 3 до 300 символов")]
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        [Range(0.01, 100000, ErrorMessage = "Цена должна быть положительной в диапазоне от 0.01 до 100000")]
        [Display(Name = "Цена ($)")]
        public double Price { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        [Required(ErrorMessage = "Пожалуйста, укажите категорию для товара")]
        [Display(Name = "Категория")]
        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }

    }
}

