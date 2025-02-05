
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mini_ERP.Models;
using Mini_ERP.Repositories;
[Authorize("Admin")]
public class SalesOrdersController : Controller
{

    private readonly ISalesOrderRepository _orderService;
    private readonly ICustomersRepository _customerRepository;
    private readonly IProductsRepository _productRepository;

    public SalesOrdersController(
        ISalesOrderRepository orderService,
        ICustomersRepository customerRepository,
        IProductsRepository productRepository)
    {
        _orderService = orderService;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetSalesOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> Create()
    {
        var customers = await _customerRepository.GetCustomersAsync();
        var products = await _productRepository.GetProductsAsync();

        ViewBag.Customers = customers.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(), // Use the ID as the value
            Text = c.Name // Use the name as the display text
        }).ToList();
        ViewBag.Products = products;

        return View(new SalesOrder { OrderItems = new List<OrderItem> { new OrderItem() } });
    }

    [HttpPost]
    public async Task<IActionResult> Create(SalesOrder order)
    {
        if (ModelState.IsValid)
        {
            await _orderService.AddSalesOrderAsync(order);
            return RedirectToAction(nameof(Index));
        }

        var customers = await _customerRepository.GetCustomersAsync();
        var products = await _productRepository.GetProductsAsync();

        ViewBag.Customers = customers.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(), // Use the ID as the value
            Text = c.Name // Use the name as the display text
        }).ToList();
        ViewBag.Products = products;

        return View(order);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var order = await _orderService.GetSalesOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        var customers = await _customerRepository.GetCustomersAsync();
        var products = await _productRepository.GetProductsAsync();

        ViewBag.Customers = customers.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(), // Use the ID as the value
            Text = c.Name // Use the name as the display text
        }).ToList();
        ViewBag.Products = products;

        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(SalesOrder order)
    {
        if (ModelState.IsValid)
        {
            await _orderService.UpdateSalesOrderAsync(order);
            return RedirectToAction(nameof(Index));
        }

        var customers = await _customerRepository.GetCustomersAsync();
        var products = await _productRepository.GetProductsAsync();

        ViewBag.Customers = customers.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(), // Use the ID as the value
            Text = c.Name // Use the name as the display text
        }).ToList();
        ViewBag.Products = products;

        return View(order);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var order = await _orderService.GetSalesOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _orderService.DeleteSalesOrderAsync(id);
        return RedirectToAction(nameof(Index));
    }
}