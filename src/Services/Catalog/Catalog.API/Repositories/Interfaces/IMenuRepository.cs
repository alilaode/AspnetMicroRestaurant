using Catalog.API.Entities;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        Task<IEnumerable<Menu>> GetMenus();
        Task<Menu> GetMenu(string id);
        Task<IEnumerable<Menu>> GetMenuByName(string name);
        Task<IEnumerable<Menu>> GetMenuByCategory(string categoryName);

        Task CreateMenu(Menu menu);
        Task<bool> UpdateMenu(Menu menu);
        Task<bool> DeleteMenu(string id);
    }
}
