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
        public OrderProductResponse OrderProduct([FromBody]OrderProductRequest request)
        {
            if (request == null || request.OrderedProducts == null || !request.OrderedProducts.Any())
            {
                return new OrderProductResponse { Message = "ไม่พบเมนูที่จะสั่ง", };
            }

            var productIds = request.OrderedProducts.Select(p => p.Key);
            var products = productRepo.GetAllProducts();
            var filteredProducts = products.Where(p => productIds.Contains(p.Id)).ToList();
            if (filteredProducts.Count() != productIds.Count())
            {
                return new OrderProductResponse { Message = "ไม่พบสินค้าบางรายการ กรุณาสั่งใหม่อีกครั้ง", };
            }

            if (filteredProducts.Any(p => p.HasStock && p.Stock < request.OrderedProducts.First(op => op.Key == p.Id).Value))
            {
                return new OrderProductResponse { Message = "สินค้าบางรายการมีไม่พอ กรุณาสั่งใหม่อีกครั้ง", };
            }
            var id = Guid.NewGuid().ToString();
            var order = new Order
            {
                Id = id,
                OrderedProducts = filteredProducts,
                ReferenceCode = id.Substring(5),
                OrderDate = DateTime.UtcNow,
                Username = request.Username,
            };
            orderRepo.CreateOrder(order);

            return new OrderProductResponse { Message = "สั่งเมนูสำเร็จ กรุณาชำระเงินที่เค้าเตอร์", ReferenceCode = order.ReferenceCode, };
        }
    }
}
