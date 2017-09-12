using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProductStore.Domein.Entities;
using WebApplication1.Models;

namespace ProductStore.WebUI.Models
{
    public class ProductsListViewModel : ViewModelsBase
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo pagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}