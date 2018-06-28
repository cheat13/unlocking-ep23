using System;
using System.Collections.Generic;
using System.Security.Authentication;
using mix_coffeeshop_web.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace mix_coffeeshop_web.Repositories
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        IMongoCollection<Order> Collection { get; set; }

        public OrderRepository(DatabaseConfigurations config)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(config.MongoDBConnection));
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };
            var mongoClient = new MongoClient(settings);
            var database = mongoClient.GetDatabase(config.DatabaseName);
            Collection = database.GetCollection<Order>("orders");
        }

        public void CreateOrder(Order order)
        {
            Collection.InsertOne(order);
        }
    }
}