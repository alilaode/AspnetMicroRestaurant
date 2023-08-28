using Cart.API.Entities;

namespace Cart.API.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Bill> GetCart(string userName);
        Task<Bill> UpdateCart(Bill bill);
        Task DeleteCart(string userName);
    }
}
