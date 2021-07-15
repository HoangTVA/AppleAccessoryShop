using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        IEnumerable<TblProduct> GetProducts();
        TblProduct GetProductById(int uID);
        
        void InsertProduct(TblProduct user);
        void DeleteProduct(int uID);
        void UpdateProduct(TblProduct user);
    }
}
