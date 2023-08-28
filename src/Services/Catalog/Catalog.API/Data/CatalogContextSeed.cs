using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Menu> menuCollection)
        {
            bool existMenu = menuCollection.Find(p => true).Any();
            if (!existMenu)
            {
                menuCollection.InsertManyAsync(GetPreconfiguredMenus());
            }
        }

        private static IEnumerable<Menu> GetPreconfiguredMenus()
        {
            return new List<Menu>()
            {
                new Menu()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "Palumara",
                    Price = 50.00M,
                    Category = "Food"
                },
                new Menu()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Juice Apel",
                    Price = 5.00M,
                    Category = "Drink"
                },

            };
        }
    }
}
