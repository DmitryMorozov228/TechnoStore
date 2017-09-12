using ProductStore.Domein.Abstract;
using ProductStore.Domein.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Domein.Concrete
{
    public class EFCategoryRepository : ICategoryRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Category> Categories
        {
            get { return context.Categories; }
        }

        public void SaveCategory(Category category)
        {
            if (category.ID == 0)
            {
                context.Categories.Add(category);
            }
            else
            {
                Category dbEntry = context.Categories.Find(category.ID);
                if (dbEntry != null)
                {
                    dbEntry.CategoryName = category.CategoryName;
                    dbEntry.ID = category.ID;
                }
            }
            context.SaveChanges();
        }

        public Category DeleteCategory(int CategoryId)
        {
            Category dbEntry = context.Categories.Find(CategoryId);
            if (dbEntry != null)
            {
                context.Categories.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
