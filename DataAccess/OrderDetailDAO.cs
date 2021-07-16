using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class OrderDetailDAO
    {
        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<TblOrderDetail> GetOrderDetailList()
        {
            var orderDetails = new List<TblOrderDetail>();
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                orderDetails = context.TblOrderDetails.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetails;
        }
        
        public IEnumerable<TblOrderDetail> GetOrderDetailByOrderID(int oID)
        {
            IEnumerable<TblOrderDetail> orD = null;
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                orD = context.TblOrderDetails.Where(o => o.OrderId == oID).ToList();
                foreach (var item in orD)
                {
                    item.Product = context.TblProducts.SingleOrDefault(o => o.ProductId == item.ProductId);
                }
            }
            
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orD;
        }
        public TblOrderDetail GetDetailByOrderIDAndProductID(int orID, int proID)
        {
            TblOrderDetail orD = null;
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                orD = context.TblOrderDetails.SingleOrDefault(o => o.OrderId == orID && o.ProductId == proID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orD;
        }
        public void AddNew(TblOrderDetail or)
        {
            try
            {
                TblOrderDetail _or = GetDetailByOrderIDAndProductID(or.OrderId, or.ProductId);
                if (_or == null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblOrderDetails.Add(or);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Order detail is already existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(TblOrderDetail or)
        {
            try
            {
                TblOrderDetail _or = GetDetailByOrderIDAndProductID(or.OrderId, or.ProductId);
                if (_or != null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblOrderDetails.Update(or);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The order Detail is not already existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
