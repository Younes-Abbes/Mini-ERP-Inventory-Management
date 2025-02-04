using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _context;
        private readonly ICategoriesRepository _categoryRepository;
        private readonly IInventoryTransactionsRepository _inventoryTransactionsRepository;

        public ProductsController(IProductsRepository context, ICategoriesRepository categoriesRepository, IInventoryTransactionsRepository inventoryTransactionsRepository)
        {
            _context = context;
            _categoryRepository = categoriesRepository;
            _inventoryTransactionsRepository = inventoryTransactionsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return View(await _context.GetProductsByName(search));
            }
            
            return View(await _context.GetProducts());
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.GetProduct(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetCategories();
            var viewModel = new CreateProductRequest
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }),
                Name = string.Empty,
                Description = string.Empty,
                Quantity = 0,
                minimumQuantity = 0,
                UnitPrice = 0,
                UpdatedAt = DateTime.Now
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest createProductRequest)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryRepository.GetCategory(Guid.Parse(createProductRequest.category));
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = createProductRequest.Name,
                    Description = createProductRequest.Description,
                    Quantity = createProductRequest.Quantity,
                    minimumQuantity = createProductRequest.minimumQuantity,
                    UnitPrice = createProductRequest.UnitPrice,
                    UpdatedAt = DateTime.UtcNow,
                    category = category,
                    CreatedAt = DateTime.UtcNow,
                    
                };
                var result = await _context.AddProduct(product);
                if (result != null)
                {
                    var inventoryTransaction = new InventoryTransaction
                    {
                        Id = Guid.NewGuid(),
                        Notes = String.Empty,
                        Product = product,
                        Quantity = product.Quantity,
                        TransactionType = TransactionType.Refill,
                        ReferenceId = product.Id,
                        Timestamp = DateTime.UtcNow,
                    };
                    await _inventoryTransactionsRepository.AddInventoryTransaction(inventoryTransaction);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(createProductRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.GetProduct(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetCategories();
            var viewModel = new EditProductRequest
            {
                
                categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }),
                createdAt = product.CreatedAt,
                Name = product.Name,
                Id = product.Id,
                Description = product.Description,
                Quantity = product.Quantity,
                minimumQuantity = product.minimumQuantity,
                UnitPrice = product.UnitPrice,
                UpdatedAt = product.UpdatedAt,
                category = product.category.Id.ToString(),
                
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, EditProductRequest createProductRequest)
        {
            if (id != createProductRequest.Id)
            {
                return NotFound();
            }
            
            var exists = await _context.ProductExists(id);
            if (!exists)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                
                try
                {
                    var category = await _categoryRepository.GetCategory(Guid.Parse(createProductRequest.category));
                    var product = await _context.GetProduct(id);
                    product.Name = createProductRequest.Name;
                    product.Description = createProductRequest.Description;
                    product.Quantity = createProductRequest.Quantity;
                    product.UnitPrice = createProductRequest.UnitPrice;
                    product.minimumQuantity = createProductRequest.minimumQuantity;
                    product.UpdatedAt = DateTime.UtcNow;
                    product.category = category;
                        
                    
                    var result = await _context.UpdateProduct(id , product);
                    if (result != null)
                    {
                        var inventoryTransaction = new InventoryTransaction
                        {
                            Id = Guid.NewGuid(),
                            Notes = String.Empty,
                            Product = product,
                            Quantity = product.Quantity,
                            TransactionType = TransactionType.Adjustment,
                            ReferenceId = product.Id,
                            Timestamp = DateTime.UtcNow,
                        };
                        await _inventoryTransactionsRepository.AddInventoryTransaction(inventoryTransaction);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductExists(createProductRequest.Id))
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
            return View(createProductRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.GetProduct(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.GetProduct(id);
            if (product != null)
            {
                var result = await _context.DeleteProduct(id);
                var inventoryTransaction = new InventoryTransaction
                {
                    Id = Guid.NewGuid(),
                    Notes = String.Empty,
                    Product = product,
                    Quantity = product.Quantity,
                    TransactionType = TransactionType.WriteOff,
                    ReferenceId = product.Id,
                    Timestamp = DateTime.UtcNow,
                };
                await _inventoryTransactionsRepository.AddInventoryTransaction(inventoryTransaction);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductExists(Guid id)
        {
            return await _context.ProductExists(id);
        }
    }
}
