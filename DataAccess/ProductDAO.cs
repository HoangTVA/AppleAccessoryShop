using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class ProductDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<TblProduct> GetProductList()
        {
            var products = new List<TblProduct>();
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                products = context.TblProducts.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return products;
        }
        public IEnumerable<TblProduct> SearchByName(string name)
        {
            var products = new List<TblProduct>();
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                products = context.TblProducts.Where(p => p.ProductName.Contains(name)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return products;
        }
        public TblProduct GetProductByID(int proID)
        {
            TblProduct pro = null;
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                pro = context.TblProducts.SingleOrDefault(m => m.ProductId == proID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return pro;
        }
        public void AddNew(TblProduct pro)
        {
            try
            {
                TblProduct _pro = GetProductByID(pro.ProductId);
                if (_pro == null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblProducts.Add(pro);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Product is already existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(TblProduct pro)
        {
            try
            {
                TblProduct _pro = GetProductByID(pro.ProductId);
                if (_pro != null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblProducts.Update(pro);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product is not already existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Remove(int proID)
        {
            try
            {
                TblProduct pro = GetProductByID(proID);
                if (pro != null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblProducts.Remove(pro);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
