using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiCode.Commerce.RestService.DataObjects
{
    public class ReturnInfo
    {
        public string OrderNumber { get; set; }
        public List<ReturnItem> ReturnItems{get;set;}

        public ReturnInfo(){
            ReturnItems = new List<ReturnItem>();
        }
    }

    public class ReturnItem
    {
        public string Sku { get; set; }
        public int Quantity{get;set;}
        public string ReturnReason{get;set;}
    }
}
