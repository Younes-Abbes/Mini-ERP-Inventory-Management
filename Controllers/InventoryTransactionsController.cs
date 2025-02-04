using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mini_ERP.Data;
using Mini_ERP.Models;
using Mini_ERP.Models.Requests.Create;
using Mini_ERP.Models.Requests.Edit;
using Mini_ERP.Repositories;

namespace Mini_ERP.Controllers
{
    public class InventoryTransactionsController : Controller
    {
        private readonly IInventoryTransactionsRepository _context;
        private readonly IProductsRepository _productRepository;

        public InventoryTransactionsController(IInventoryTransactionsRepository context, IProductsRepository productsRepository)
        {
            _context = context;
            _productRepository = productsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetInventoryTransactions());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryTransaction = await _context.GetInventoryTransaction(id.Value);
            if (inventoryTransaction == null)
            {
                return NotFound();
            }

            return View(inventoryTransaction);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var products = await _productRepository.GetProducts();
            var viewModel = new CreateInventoryTransactionRequest
            {
                products = products.Select(x => new SelectListItem(x.Name, x.Id.ToString())),
                Timestamp = DateTime.UtcNow,
                TransactionType = TransactionType.Refill,
                Product = null,
                Notes = string.Empty,
                Quantity = 0,
                ReferenceId = Guid.Empty
            };
            return View(viewModel);
        }

        
        [HttpPost]
        
        public async Task<IActionResult> Create(CreateInventoryTransactionRequest createInventoryTransactionRequest)
        {
            if (ModelState.IsValid)
            {
                var product = await _productRepository.GetProduct(Guid.Parse(createInventoryTransactionRequest.Product));
                var inventoryTransaction = new InventoryTransaction
                {
                    Id = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow,
                    ReferenceId = createInventoryTransactionRequest.ReferenceId,
                    TransactionType = createInventoryTransactionRequest.TransactionType,
                    Quantity = createInventoryTransactionRequest.Quantity,
                    Notes = createInventoryTransactionRequest.Notes,
                    Product = product,
                };
                await _context.AddInventoryTransaction(inventoryTransaction);
                return RedirectToAction(nameof(Index));
            }
            return View(createInventoryTransactionRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var inventoryTransaction = await _context.GetInventoryTransaction(id.Value);
            if (inventoryTransaction == null)
            {
                return NotFound();
            }
            var viewModel = new EditInventoryTransactionRequest
            {
                Id = inventoryTransaction.Id,
                Timestamp = inventoryTransaction.Timestamp,
                Notes = inventoryTransaction.Notes,
                TransactionType = inventoryTransaction.TransactionType,
                Product = inventoryTransaction.Product,
                Quantity = inventoryTransaction.Quantity,
                ReferenceId = inventoryTransaction.ReferenceId,
            };
            
            
            return View(viewModel);
        }

        
        [HttpPost]
        
        public async Task<IActionResult> Edit(Guid id, EditInventoryTransactionRequest editInventoryTransactionRequest)
        {
            if (id != editInventoryTransactionRequest.Id)
            {
                return NotFound();
            }
            var inventoryTransaction = await _context.GetInventoryTransaction(id);
            if (inventoryTransaction == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    inventoryTransaction.Quantity = editInventoryTransactionRequest.Quantity;
                    inventoryTransaction.Notes = editInventoryTransactionRequest.Notes;
                    await _context.UpdateInventoryTransaction(inventoryTransaction.Id, inventoryTransaction);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await InventoryTransactionExists(editInventoryTransactionRequest.Id))
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

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryTransaction = await _context.GetInventoryTransaction(id.Value);
            if (inventoryTransaction == null)
            {
                return NotFound();
            }

            return View(inventoryTransaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var inventoryTransaction = await _context.GetInventoryTransaction(id);
            if (inventoryTransaction != null)
            {
                await _context.DeleteInventoryTransaction(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> InventoryTransactionExists(Guid id)
        {
            return await _context.InventoryTransactionExists(id);
        }
    }
}
