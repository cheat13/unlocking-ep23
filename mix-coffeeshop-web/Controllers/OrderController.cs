using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mix_coffeeshop_web.Models;

namespace mix_coffeeshop_web.Controllers
{
    [Route("[controller]/api/[action]")]
    public class OrderController : Controller
    {
        private IProductRepository productRepo;
        private IOrderRepository orderRepo;

        public OrderController(IProductRepository productRepo, IOrderRepository orderRepo)
        {
            this.productRepo = productRepo;
            this.orderRepo = orderRepo;
        }

        [HttpPost]
        public OrderProductResponse OrderProduct([FromBody]Order order)
        {
            if (order == null || order.ProductIds == null || !order.ProductIds.Any())
            {
                return new OrderProductResponse { Message = "ไม่พบเมนูที่จะสั่ง", };
            };

            throw new NotImplementedException();
        }
    }
}
