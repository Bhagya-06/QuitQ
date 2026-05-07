using Microsoft.EntityFrameworkCore;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;

namespace QuitQ.API.Services
{
    public class OfferService : IOfferService
    {
        private readonly QuitQDbContext _context;

        public OfferService(QuitQDbContext context)
        {
            _context = context;
        }

        public async Task<Offer> CreateOffer(Offer offer)
        {
            if (offer == null)
                throw new Exception("Offer cannot be null");

            if (offer.DiscountPercentage <= 0 || offer.DiscountPercentage > 100)
                throw new Exception("Invalid discount");

            if (offer.StartDate > offer.EndDate)
                throw new Exception("Start date cannot be after end date");

            var newOffer = new Offer
            {
                Title = offer.Title,
                DiscountPercentage = offer.DiscountPercentage,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                IsActive = offer.IsActive ?? true
            };

            _context.Offers.Add(newOffer);
            await _context.SaveChangesAsync();

            return newOffer;
        }

        public async Task<List<Offer>> GetAllOffers()
            => await _context.Offers.ToListAsync();

        public async Task<Offer> GetOfferById(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
                throw new Exception("Offer not found");

            return offer;
        }

        public async Task UpdateOffer(int id, Offer updated)
        {
            var existing = await GetOfferById(id);

            if (updated.DiscountPercentage <= 0 || updated.DiscountPercentage > 100)
                throw new Exception("Invalid discount");

            if (updated.StartDate > updated.EndDate)
                throw new Exception("Invalid date range");

            existing.Title = updated.Title ?? existing.Title;
            existing.DiscountPercentage = updated.DiscountPercentage;
            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;
            existing.IsActive = updated.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOffer(int id)
        {
            var offer = await GetOfferById(id);
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
        }

        public bool ValidateOffer(Offer offer)
        {
            var now = DateTime.UtcNow;

            return offer.IsActive == true &&
                   offer.StartDate <= now &&
                   offer.EndDate >= now;
        }

        public async Task<decimal> ApplyOfferToProduct(int productId, decimal price)
        {
            var offer = await _context.Offers.Where(x => x.IsActive == true &&
            x.StartDate <= DateTime.UtcNow && x.EndDate >= DateTime.UtcNow)
            .OrderByDescending(x => x.DiscountPercentage).FirstOrDefaultAsync();

            if (offer == null || !ValidateOffer(offer))
                return price;

            var discount = (offer.DiscountPercentage ?? 0) / 100;

            return price - (price * discount);
        }
    }
}