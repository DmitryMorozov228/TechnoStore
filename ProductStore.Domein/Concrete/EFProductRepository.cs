using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductStore.Domein.Abstract;
using ProductStore.Domein.Entities;

namespace ProductStore.Domein.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(Product Product)
        {
            if (Product.ProductId == 0)
            {
                context.Products.Add(Product);
            }
            else
            {
                Product dbEntry = context.Products.Find(Product.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.Name = Product.Name;
                    dbEntry.CategoryID = Product.CategoryID;
                    dbEntry.Description = Product.Description;
                    dbEntry.Price = Product.Price;
                    dbEntry.ImageData = Product.ImageData;
                    dbEntry.ImageMimeType = Product.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int ProductId)
        {
            Product dbEntry = context.Products.Find(ProductId);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
