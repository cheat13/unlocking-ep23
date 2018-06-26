using System;
using System.Collections.Generic;
using System.Security.Authentication;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace mix_coffeeshop_web.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        void UpdateProduct(Product data);
        void CreateNewProduct(Product data);
    }

    public class ProductRepository : IProductRepository
    {
        private IMongoDatabase database;

        public ProductRepository()
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

        public IEnumerable<Product> GetAllProducts()
        {
            var productCollection = database.GetCollection<Product>("products");
            var products = productCollection.Find(it => true).ToList();
            return products;
        }

        public void UpdateProduct(Product data)
        {
            var productCollection = database.GetCollection<Product>("products");
            productCollection.ReplaceOne(it=>it.Id == data.Id, data);
        }

        public void CreateNewProduct(Product data)
        {
            var productCollection = database.GetCollection<Product>("products");
            productCollection.InsertOne(data);
        }
    }
}