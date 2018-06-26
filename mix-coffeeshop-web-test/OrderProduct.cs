using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;
using mix_coffeeshop_web.Models;
using Moq;
using Xunit;
using System.Linq;

namespace mix_coffeeshop_web_test
{
    public class OrderProduct
    {
        /*
        ไม่มีข้อมูล หรือไม่เลือกสินค้าที่จะสั่ง ให้แจ้งกลับว่า "ไม่พบเมนูที่จะสั่ง" และไม่บันทึกข้อมูล
		สินค้าบางรายการ (หรือทุกรายการ) หมด หรือมีไม่พอให้สั่ง ให้แจ้งกลับว่า "สินค้าบางรายการมีไม่พอ กรุณาสั่งใหม่อีกครั้ง" และไม่บันทึกข้อมูล
		ไม่พบสินค้าบางรายการ (หรือทุกรายการ) ให้แจ้งกลับว่า "ไม่พบสินค้าบางรายการ กรุณาสั่งใหม่อีกครั้ง" และไม่บันทึกข้อมูล
		สั่งเมนูสำเร็จ ให้แจ้งกลับว่า "สั่งเมนูสำเร็จ กรุณาชำระเงินที่เค้าเตอร์"
         */
    }
}