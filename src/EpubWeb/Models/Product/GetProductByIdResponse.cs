using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpubWeb.Models.Product
{
    public class GetProductByIdResponse
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}