using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<TblOrder> GetOrders();
        IEnumerable<TblOrder> GetOrderByuID(int uID);
        void AddOrder(TblOrder order);

         TblOrder GetOrderById(int oID);
        
    }
}
