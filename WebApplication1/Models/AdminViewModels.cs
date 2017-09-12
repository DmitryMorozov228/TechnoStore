using ProductStore.Domein.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class AdminViewModels : ViewModelsBase
    {
        public IList<ApplicationUser> Users { get; set; }
        public IList<string> Roles { get; set; }
    }
}