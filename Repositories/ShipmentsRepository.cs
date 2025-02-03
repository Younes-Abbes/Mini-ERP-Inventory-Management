using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Repositories
{
    public class ShipmentsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Shipment> _shipments;
        public ShipmentsRepository(ApplicationDbContext context)
        {
            _context = context;
            _shipments = context.shipments;
        }
        public async Task<IEnumerable<Shipment>> GetShipments()
        {
            return await _shipments.ToListAsync();
        }
        public async Task<Shipment?> GetShipment(Guid id)
        {
            return await _shipments.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Shipment> AddShipment(Shipment shipment)
        {
            var entry = await _shipments.AddAsync(shipment);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<Shipment?> UpdateShipment(Guid id, Shipment shipment)
        {
            var existingShipment = await _shipments.FindAsync(id);
            if (existingShipment == null)
            {
                return null;
            }
            existingShipment.ShippedDate = shipment.ShippedDate;
            existingShipment.DeliveryDate = shipment.DeliveryDate;
            existingShipment.Status = shipment.Status;
            await _context.SaveChangesAsync();
            return existingShipment;
        }
        public async Task<Shipment?> DeleteShipment(Guid id)
        {
            var existingShipment = await _shipments.FindAsync(id);
            if (existingShipment == null)
            {
                return null;
            }
            _shipments.Remove(existingShipment);
            await _context.SaveChangesAsync();
            return existingShipment;
        }
        public async void SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
