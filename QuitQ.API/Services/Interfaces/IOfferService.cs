using QuitQ.API.Models;

namespace QuitQ.API.Services.Interfaces
{
    public interface IOfferService
    {
        Task<Offer> CreateOffer(Offer offer);
        Task<List<Offer>> GetAllOffers();
        Task<Offer> GetOfferById(int id);
        Task UpdateOffer(int id, Offer offer);
        Task DeleteOffer(int id);
        Task<decimal> ApplyOfferToProduct(int productId, decimal price);
        bool ValidateOffer(Offer offer);
    }
}
