using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Controllers
{
    [Authorize("Admin")]
    public class ShipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shipments
        public async Task<IActionResult> Index()
        {
            return View(await _context.shipments.ToListAsync());
        }

        // GET: Shipments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.shipments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        // GET: Shipments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shipments/Create
        // Only the fields provided by the form are bound. The shipment is always created as Pending,
        // and the dates are left null until updated by the system.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShippedDate,DeliveryDate,Status")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                shipment.Id = Guid.NewGuid();
                // Force new shipments to be Pending with no dates set.
                shipment.Status = ShipmentStatus.Pending;
                shipment.ShippedDate = null;
                shipment.DeliveryDate = null;

                _context.Add(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shipment);
        }

        // GET: Shipments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }
            return View(shipment);
        }

        // POST: Shipments/Edit/5
        // Only bind Id and Status since the dates should be managed internally.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Status")] Shipment updatedShipment)
        {
            if (id != updatedShipment.Id)
            {
                return NotFound();
            }

            // Retrieve the current shipment record from the database.
            var shipment = await _context.shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }

            // Prevent any changes if the shipment is already Delivered or Cancelled.
            if (shipment.Status == ShipmentStatus.Delivered || shipment.Status == ShipmentStatus.Cancelled)
            {
                ModelState.AddModelError("", "Cannot modify a Delivered or Cancelled shipment.");
                return View(shipment);
            }

            // Process the status change if the status is different.
            if (shipment.Status != updatedShipment.Status)
            {
                switch (shipment.Status)
                {
                    case ShipmentStatus.Pending:
                        // From Pending, allowed transitions: Shipped or Cancelled.
                        if (updatedShipment.Status == ShipmentStatus.Shipped)
                        {
                            shipment.Status = ShipmentStatus.Shipped;
                            shipment.ShippedDate = DateTime.UtcNow;
                        }
                        else if (updatedShipment.Status == ShipmentStatus.Cancelled)
                        {
                            shipment.Status = ShipmentStatus.Cancelled;
                        }
                        else
                        {
                            ModelState.AddModelError("", "From Pending, allowed transitions are only to Shipped or Cancelled.");
                            return View(shipment);
                        }
                        break;

                    case ShipmentStatus.Shipped:
                        // From Shipped, allowed transitions: Delivered or Cancelled.
                        if (updatedShipment.Status == ShipmentStatus.Delivered)
                        {
                            shipment.Status = ShipmentStatus.Delivered;
                            shipment.DeliveryDate = DateTime.UtcNow;
                        }
                        else if (updatedShipment.Status == ShipmentStatus.Cancelled)
                        {
                            shipment.Status = ShipmentStatus.Cancelled;
                        }
                        else
                        {
                            ModelState.AddModelError("", "From Shipped, allowed transitions are only to Delivered or Cancelled.");
                            return View(shipment);
                        }
                        break;

                    default:
                        ModelState.AddModelError("", "Invalid status transition.");
                        return View(shipment);
                }
            }
            else
            {
                // If no status change is detected, nothing is updated.
                ModelState.AddModelError("", "No changes detected in the status.");
                return View(shipment);
            }

            try
            {
                _context.Update(shipment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentExists(shipment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Shipments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.shipments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var shipment = await _context.shipments.FindAsync(id);
            if (shipment != null)
            {
                _context.shipments.Remove(shipment);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShipmentExists(Guid id)
        {
            return _context.shipments.Any(e => e.Id == id);
        }
    }
}
