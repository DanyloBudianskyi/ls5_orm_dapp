using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ls5_orm_dapp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
    }
}
