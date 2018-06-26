using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;
using mix_coffeeshop_web.Controllers;
using mix_coffeeshop_web.Models;
using Moq;
using Xunit;
using System.Linq;

namespace mix_coffeeshop_web_test
{
    public class OrderProduct
    {
        public static IEnumerable<object[]> OrderNoneProductsData = new List<object[]>{
            new object[] { null, "ไม่พบเมนูที่จะสั่ง" },
            new object[] { new OrderProductRequest { OrderedProducts = null, Username = "john-doe@gmail.com", }, "ไม่พบเมนูที่จะสั่ง" },
            new object[] { new OrderProductRequest { OrderedProducts = { }, Username = "john-doe@gmail.com", }, "ไม่พบเมนูที่จะสั่ง" },
        };
        [Theory(DisplayName = "ไม่มีข้อมูล หรือไม่เลือกสินค้าที่จะสั่ง ให้แจ้งกลับว่า 'ไม่พบเมนูที่จะสั่ง' และไม่บันทึกข้อมูล")]
        [MemberData(nameof(OrderNoneProductsData))]
        public void OrderNoneProduct(OrderProductRequest data, string expectedMessage)
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repoProduct = mock.Create<IProductRepository>();
            var repoOrder = mock.Create<IOrderRepository>();
            var api = new OrderController(repoProduct.Object, repoOrder.Object);
            repoOrder.Setup(it => it.CreateOrder(It.IsAny<Order>()));

            var response = api.OrderProduct(data);

            repoProduct.VerifyNoOtherCalls();
            repoOrder.VerifyNoOtherCalls();
            response.ReferenceCode.Should().BeNullOrEmpty();
            response.Message.Should().Be(expectedMessage);
        }

        public static IEnumerable<object[]> OrderNotExistingProductsData = new List<object[]>{
            new object[] { new OrderProductRequest { OrderedProducts = new Dictionary<int, int> { { 4, 1 }, { 5, 1 }, }, Username = "john-doe@gmail.com", }, "ไม่พบสินค้าบางรายการ กรุณาสั่งใหม่อีกครั้ง" },
            new object[] { new OrderProductRequest { OrderedProducts = new Dictionary<int, int> { { 1, 1 }, { 4, 1 }, }, Username = "john-doe@gmail.com", }, "ไม่พบสินค้าบางรายการ กรุณาสั่งใหม่อีกครั้ง" },
        };
        [Theory(DisplayName = "ไม่พบสินค้าบางรายการ (หรือทุกรายการ) ให้แจ้งกลับว่า 'ไม่พบสินค้าบางรายการ กรุณาสั่งใหม่อีกครั้ง' และไม่บันทึกข้อมูล")]
        [MemberData(nameof(OrderNotExistingProductsData))]
        public void OrderNotexistingProduct(OrderProductRequest data, string expectedMessage)
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repoProduct = mock.Create<IProductRepository>();
            var repoOrder = mock.Create<IOrderRepository>();
            var api = new OrderController(repoProduct.Object, repoOrder.Object);
            repoProduct.Setup(it => it.GetAllProducts()).Returns(() => new List<Product>
            {
                new Product { Id = 1, Stock = 0, HasStock = true },
                new Product { Id = 2, Stock = 5, HasStock = true },
                new Product { Id = 3, Stock = 0, HasStock = true },
            });
            repoOrder.Setup(it => it.CreateOrder(It.IsAny<Order>()));

            var response = api.OrderProduct(data);

            repoProduct.Verify(dac => dac.GetAllProducts(), Times.Once);
            repoProduct.VerifyNoOtherCalls();
            repoOrder.VerifyNoOtherCalls();
            response.ReferenceCode.Should().BeNullOrEmpty();
            response.Message.Should().Be(expectedMessage);
        }

        public static IEnumerable<object[]> OrderNotEnoughProductsData = new List<object[]>{
            new object[] { new OrderProductRequest { OrderedProducts = new Dictionary<int, int> { { 1, 1 }, { 2, 1 }, }, Username = "john-doe@gmail.com", }, "สินค้าบางรายการมีไม่พอ กรุณาสั่งใหม่อีกครั้ง" },
            new object[] { new OrderProductRequest { OrderedProducts = new Dictionary<int, int> { { 1, 1 }, { 3, 1 }, }, Username = "john-doe@gmail.com", }, "สินค้าบางรายการมีไม่พอ กรุณาสั่งใหม่อีกครั้ง" },
        };
        [Theory(DisplayName = "สินค้าบางรายการ (หรือทุกรายการ) หมด หรือมีไม่พอให้สั่ง ให้แจ้งกลับว่า 'สินค้าบางรายการมีไม่พอ กรุณาสั่งใหม่อีกครั้ง' และไม่บันทึกข้อมูล")]
        [MemberData(nameof(OrderNotEnoughProductsData))]
        public void OrderNotEnoughProduct(OrderProductRequest data, string expectedMessage)
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repoProduct = mock.Create<IProductRepository>();
            var repoOrder = mock.Create<IOrderRepository>();
            var api = new OrderController(repoProduct.Object, repoOrder.Object);
            repoProduct.Setup(it => it.GetAllProducts()).Returns(() => new List<Product>
            {
                new Product { Id = 1, Stock = 0, HasStock = true },
                new Product { Id = 2, Stock = 5, HasStock = true },
                new Product { Id = 3, Stock = 0, HasStock = true },
            });
            repoOrder.Setup(it => it.CreateOrder(It.IsAny<Order>()));

            var response = api.OrderProduct(data);

            repoProduct.Verify(dac => dac.GetAllProducts(), Times.Once);
            repoProduct.VerifyNoOtherCalls();
            repoOrder.VerifyNoOtherCalls();
            response.ReferenceCode.Should().BeNullOrEmpty();
            response.Message.Should().Be(expectedMessage);
        }

        public static IEnumerable<object[]> OrderProductsSuccessData = new List<object[]>{
            new object[] { new OrderProductRequest { OrderedProducts = new Dictionary<int, int> { { 1, 1 }, { 2, 1 }, }, Username = "john-doe@gmail.com", }, "สั่งเมนูสำเร็จ กรุณาชำระเงินที่เค้าเตอร์" },
            new object[] { new OrderProductRequest { OrderedProducts = new Dictionary<int, int> { { 1, 1 }, { 3, 2 }, }, Username = "john-doe@gmail.com", }, "สั่งเมนูสำเร็จ กรุณาชำระเงินที่เค้าเตอร์" },
            new object[] { new OrderProductRequest { OrderedProducts = new Dictionary<int, int> { { 1, 1 }, { 4, 2 }, }, Username = "john-doe@gmail.com", }, "สั่งเมนูสำเร็จ กรุณาชำระเงินที่เค้าเตอร์" },
        };
        [Theory(DisplayName = "สั่งเมนูสำเร็จ ให้แจ้งกลับว่า 'สั่งเมนูสำเร็จ กรุณาชำระเงินที่เค้าเตอร์' พร้อมกับ ReferenceCode")]
        [MemberData(nameof(OrderProductsSuccessData))]
        public void OrderProductsSuccess(OrderProductRequest data, string expectedMessage)
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repoProduct = mock.Create<IProductRepository>();
            var repoOrder = mock.Create<IOrderRepository>();
            var api = new OrderController(repoProduct.Object, repoOrder.Object);
            repoProduct.Setup(it => it.GetAllProducts()).Returns(() => new List<Product>
            {
                new Product { Id = 1, Stock = 4, HasStock = true },
                new Product { Id = 2, Stock = 5, HasStock = true },
                new Product { Id = 3, Stock = 6, HasStock = true },
                new Product { Id = 4, Stock = 0, HasStock = false },
            });
            repoOrder.Setup(it => it.CreateOrder(It.IsAny<Order>()));

            var response = api.OrderProduct(data);

            repoProduct.Verify(dac => dac.GetAllProducts(), Times.Once);
            repoProduct.VerifyNoOtherCalls();
            Func<Order, bool> func = o =>
            {
                o.Id.Should().NotBeNullOrEmpty();
                o.Username.Should().NotBeNullOrEmpty();
                o.OrderedProducts.Any();
                o.ReferenceCode.Should().NotBeNullOrEmpty();
                return true;
            };
            repoOrder.Verify(dac => dac.CreateOrder(It.Is<Order>(o => func(o))), Times.Once);
            repoOrder.VerifyNoOtherCalls();
            response.ReferenceCode.Should().NotBeNull();
            response.Message.Should().Be(expectedMessage);
        }
    }
}