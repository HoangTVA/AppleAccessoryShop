using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<TblOrder> GetOrderList()
        {
            var orders = new List<TblOrder>();
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                orders = context.TblOrders.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }
        public TblOrder GetOrderByID(int oID)
        {
            TblOrder or = null;
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                or = context.TblOrders.SingleOrDefault(m => m.OrderId == oID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return or;
        }
        public IEnumerable<TblOrder> GetOrderByUser(int uID)
        {
            IEnumerable<TblOrder> or = null;
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                or = context.TblOrders.Where(m => m.UserId == uID).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return or;
        }
        public void AddNew(TblOrder or)
        {
            try
            {
                TblOrder _or = GetOrderByID(or.OrderId);
                if (_or == null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblOrders.Add(or);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Order is already existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(TblOrder or)
        {
            try
            {
                TblOrder _pro = GetOrderByID(or.OrderId);
                if (_pro != null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblOrders.Update(or);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The order is not already existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Remove(int oID)
        {
            try
            {
                TblOrder or = GetOrderByID(oID);
                if (or != null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblOrders.Remove(or);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The order does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
