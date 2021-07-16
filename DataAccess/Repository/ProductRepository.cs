using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public  class ProductRepository :IProductRepository
    {
        public IEnumerable<TblProduct> GetProducts() => ProductDAO.Instance.GetProductList();
        public TblProduct GetProductById(int uID) => ProductDAO.Instance.GetProductByID(uID);
        
        public void InsertProduct(TblProduct user) => ProductDAO.Instance.AddNew(user);
        public void DeleteProduct(int pID) => ProductDAO.Instance.Remove(pID);
        public void UpdateProduct(TblProduct user) => ProductDAO.Instance.Update(user);

        public IEnumerable<TblProduct> SearchProduct(string searchName) => ProductDAO.Instance.SearchByName(searchName);
    }
}
