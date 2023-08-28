using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ICatalogContext _context;

        public MenuRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Menu>> GetMenus()
        {
            return await _context
                            .Menus
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Menu> GetMenu(string id)
        {
            return await _context
                           .Menus
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Menu>> GetMenuByName(string name)
        {
            FilterDefinition<Menu> filter = Builders<Menu>.Filter.ElemMatch(p => p.Name, name);

            return await _context
                            .Menus
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Menu>> GetMenuByCategory(string categoryName)
        {
            FilterDefinition<Menu> filter = Builders<Menu>.Filter.Eq(p => p.Category, categoryName);

            return await _context
                            .Menus
                            .Find(filter)
                            .ToListAsync();
        }


        public async Task CreateMenu(Menu menu)
        {
            await _context.Menus.InsertOneAsync(menu);
        }

        public async Task<bool> UpdateMenu(Menu menu)
        {
            var updateResult = await _context
                                        .Menus
                                        .ReplaceOneAsync(filter: g => g.Id == menu.Id, replacement: menu);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteMenu(string id)
        {
            FilterDefinition<Menu> filter = Builders<Menu>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Menus
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
