using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace mix_coffeeshop_web.Models
{
    public class Order
    {
        [BsonId]
        public int Id { get; set; }
        public IEnumerable<string> ProductIds { get; set; }
        public string Username { get; set; }
    }
}