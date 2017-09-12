using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductStore.Domein.Entities;

namespace ProductStore.Domein.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }

        void SaveProduct(Product Product);

        Product DeleteProduct(int ProductId);
    }
}
