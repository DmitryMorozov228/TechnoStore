using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductStore.WebUI.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsForPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsForPage); }
        }
    }
}