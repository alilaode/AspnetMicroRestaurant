using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data.Interfaces
{
    public interface ICatalogContext
    {
        IMongoCollection<Menu> Menus { get; }
    }
}
