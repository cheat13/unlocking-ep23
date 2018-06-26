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
            if (order == null || order.OrderedProducts == null || !order.OrderedProducts.Any())
            {
                return new OrderProductResponse { Message = "ไม่พบเมนูที่จะสั่ง", };
            }

            var productIds = order.OrderedProducts.Select(p => p.Key);
            var products = productRepo.GetAllProducts();
            var filteredProducts = products.Where(p => productIds.Contains(p.Id));
            if (filteredProducts.Count() != productIds.Count())
            {
                return new OrderProductResponse { Message = "ไม่พบสินค้าบางรายการ กรุณาสั่งใหม่อีกครั้ง", };
            }

            if (filteredProducts.Any(p => p.Stock < order.OrderedProducts.First(op => op.Key == p.Id).Value))
            {
                return new OrderProductResponse { Message = "สินค้าบางรายการมีไม่พอ กรุณาสั่งใหม่อีกครั้ง", };
            }

            throw new NotImplementedException();
        }
    }
}
