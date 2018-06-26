using System;
using System.Collections.Generic;
using System.Security.Authentication;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace mix_coffeeshop_web.Models
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private IMongoDatabase database;

        public OrderRepository()
        {
            string connectionString = @"mongodb://unlocking:3kYTyyRKbVaSMKFdMo9VdnrTAh5YQSv5pxOUOy4WSM9PWX0hRqlOshAwb9eSed5A3wUtmKmUxENs5YjpWyM31Q==@unlocking.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };
            var mongoClient = new MongoClient(settings);
            database = mongoClient.GetDatabase("unlocking");
        }

        public void CreateOrder(Order order)
        {
            var Collection = database.GetCollection<Order>("orders");
            Collection.InsertOne(order);
        }
    }
}