using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CategoryViewModel : ViewModelsBase
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите название категории")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 30 символов")]
        [RegularExpression(@"[ а-яА-я0-9]{3,30}", ErrorMessage = "Поле должно содержать только кириллические символы и состоять из 3-30 символов")]
        [Display(Name = "Категория")]
        public string Name { get; set; }
    }
}