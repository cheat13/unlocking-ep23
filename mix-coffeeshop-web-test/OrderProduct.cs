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
            new object[] { new Order { ProductIds = null, Username = "john-doe@gmail.com", }, "ไม่พบเมนูที่จะสั่ง" },
            new object[] { new Order { ProductIds = { }, Username = "john-doe@gmail.com", }, "ไม่พบเมนูที่จะสั่ง" },
        };
        [Theory(DisplayName = "ไม่มีข้อมูล หรือไม่เลือกสินค้าที่จะสั่ง ให้แจ้งกลับว่า 'ไม่พบเมนูที่จะสั่ง' และไม่บันทึกข้อมูล")]
        [MemberData(nameof(OrderNoneProductsData))]
        public void OrderNoneProduct(Order data, string expectedMessage)
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
        /*
        ไม่มีข้อมูล หรือไม่เลือกสินค้าที่จะสั่ง ให้แจ้งกลับว่า 'ไม่พบเมนูที่จะสั่ง' และไม่บันทึกข้อมูล
		สินค้าบางรายการ (หรือทุกรายการ) หมด หรือมีไม่พอให้สั่ง ให้แจ้งกลับว่า 'สินค้าบางรายการมีไม่พอ กรุณาสั่งใหม่อีกครั้ง' และไม่บันทึกข้อมูล
		ไม่พบสินค้าบางรายการ (หรือทุกรายการ) ให้แจ้งกลับว่า 'ไม่พบสินค้าบางรายการ กรุณาสั่งใหม่อีกครั้ง' และไม่บันทึกข้อมูล
		สั่งเมนูสำเร็จ ให้แจ้งกลับว่า 'สั่งเมนูสำเร็จ กรุณาชำระเงินที่เค้าเตอร์'
         */
    }
}