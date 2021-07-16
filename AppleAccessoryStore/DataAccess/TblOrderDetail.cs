using System;
using System.Collections.Generic;

#nullable disable

namespace AppleAccessoryStore.DataAccess
{
    public partial class TblOrderDetail
    {
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }

        public virtual TblOrder Order { get; set; }
        public virtual TblProduct Product { get; set; }
    }
}
