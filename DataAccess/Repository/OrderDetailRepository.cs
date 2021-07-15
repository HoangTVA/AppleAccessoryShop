using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void addDetail(TblOrderDetail ord) => OrderDetailDAO.Instance.AddNew(ord);
    }
}
