using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Menu> Menus { get; }

        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Menus = database.GetCollection<Menu>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            CatalogContextSeed.SeedData(Menus);
        }

    }
}
