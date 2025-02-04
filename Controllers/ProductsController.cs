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
using Mini_ERP.Repositories;

namespace Mini_ERP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _context;
        private readonly ICategoriesRepository _categoryRepository;

        public ProductsController(IProductsRepository context, ICategoriesRepository categoriesRepository)
        {
            _context = context;
            _categoryRepository = categoriesRepository;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetProducts());
        }

        // GET: Products/Details/5
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

        // GET: Products/Create
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

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                await _context.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(createProductRequest);
        }

        // GET: Products/Edit/5
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
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Quantity,minimumQuantity,UnitPrice,CreatedAt,UpdatedAt")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateProduct(id ,product);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.GetProduct(id);
            if (product != null)
            {
                await _context.DeleteProduct(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductExists(Guid id)
        {
            return await _context.ProductExists(id);
        }
    }
}
