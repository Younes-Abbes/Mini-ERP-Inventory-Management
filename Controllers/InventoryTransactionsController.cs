using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;

namespace Mini_ERP.Controllers
{
    public class InventoryTransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryTransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InventoryTransactions
        public async Task<IActionResult> Index()
        {
            return View(await _context.InventoryTransactions.ToListAsync());
        }

        // GET: InventoryTransactions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryTransaction = await _context.InventoryTransactions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryTransaction == null)
            {
                return NotFound();
            }

            return View(inventoryTransaction);
        }

        // GET: InventoryTransactions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InventoryTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Timestamp,TransactionType,Quantity,ProductId,Notes,ReferenceId")] InventoryTransaction inventoryTransaction)
        {
            if (ModelState.IsValid)
            {
                inventoryTransaction.Id = Guid.NewGuid();
                _context.Add(inventoryTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryTransaction);
        }

        // GET: InventoryTransactions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryTransaction = await _context.InventoryTransactions.FindAsync(id);
            if (inventoryTransaction == null)
            {
                return NotFound();
            }
            return View(inventoryTransaction);
        }

        // POST: InventoryTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Timestamp,TransactionType,Quantity,ProductId,Notes,ReferenceId")] InventoryTransaction inventoryTransaction)
        {
            if (id != inventoryTransaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryTransactionExists(inventoryTransaction.Id))
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
            return View(inventoryTransaction);
        }

        // GET: InventoryTransactions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryTransaction = await _context.InventoryTransactions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryTransaction == null)
            {
                return NotFound();
            }

            return View(inventoryTransaction);
        }

        // POST: InventoryTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var inventoryTransaction = await _context.InventoryTransactions.FindAsync(id);
            if (inventoryTransaction != null)
            {
                _context.InventoryTransactions.Remove(inventoryTransaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryTransactionExists(Guid id)
        {
            return _context.InventoryTransactions.Any(e => e.Id == id);
        }
    }
}
