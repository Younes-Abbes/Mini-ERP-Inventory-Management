
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mini_ERP.Models;
using Mini_ERP.Repositories;
[Authorize("Admin")]
public class PurchaseOrdersController : Controller
{
    

    private readonly IPurchaseOrderRepository _orderService;
    private readonly ISuppliersRepository _supplierRepository;
    private readonly IProductsRepository _productRepository;

    public PurchaseOrdersController(
        IPurchaseOrderRepository orderService,
        ISuppliersRepository supplierRepository,
        IProductsRepository productRepository)
    {
        _orderService = orderService;
        _supplierRepository = supplierRepository;
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetPurchaseOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> Create()
    {
        var suppliers = await _supplierRepository.GetSuppliersAsync();
        var products = await _productRepository.GetProductsAsync();

        ViewBag.Suppliers = suppliers.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(), // Use the ID as the value
            Text = s.CompanyName // Use the name as the display text
        }).ToList();
        ViewBag.Products = products;

        return View(new PurchaseOrder {
            OrderItems = new List<OrderItem> { new OrderItem() },
            supplier = new Supplier()


        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(PurchaseOrder order)
    {
        if (ModelState.IsValid)
        {
            order.Id = Guid.NewGuid();
            await _orderService.AddPurchaseOrderAsync(order);
            return RedirectToAction(nameof(Index));
        }

        var suppliers = await _supplierRepository.GetSuppliersAsync();
        var products = await _productRepository.GetProductsAsync();

        
        ViewBag.Suppliers = suppliers.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(), // Use the ID as the value
            Text = s.CompanyName // Use the name as the display text
        }).ToList();
        ViewBag.Products = products;

        return View(order);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var order = await _orderService.GetPurchaseOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        var suppliers = await _supplierRepository.GetSuppliersAsync();
        var products = await _productRepository.GetProductsAsync();

        ViewBag.Suppliers = suppliers.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(), // Use the ID as the value
            Text = s.CompanyName // Use the name as the display text
        }).ToList();
        ViewBag.Products = products;

        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PurchaseOrder order)
    {
        if (ModelState.IsValid)
        {
            await _orderService.UpdatePurchaseOrderAsync(order);
            return RedirectToAction(nameof(Index));
        }

        var suppliers = await _supplierRepository.GetSuppliersAsync();
        var products = await _productRepository.GetProductsAsync();

        ViewBag.Suppliers = suppliers.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(), // Use the ID as the value
            Text = s.CompanyName // Use the name as the display text
        }).ToList();
        ViewBag.Products = products;

        return View(order);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var order = await _orderService.GetPurchaseOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _orderService.DeletePurchaseOrderAsync(id);
        return RedirectToAction(nameof(Index));
    }
}