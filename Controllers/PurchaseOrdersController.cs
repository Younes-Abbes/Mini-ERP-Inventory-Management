
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mini_ERP.Models;
using Mini_ERP.Models.Requests.Create;
using Mini_ERP.Models.Requests.Edit;
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
        var viewModel = new CreatePurchaseOrderRequest
        {
            OrderItems = new List<CreateOrderItemRequest> { },
            BillingAddress = string.Empty,
            DeliveryDate = DateTime.Now,
            PaymentMethod = PaymentMethod.CreditCard,
            ShippingAddress = string.Empty,
            ShippingMethod = ShippingMethod.Standard,
            SubTotal = 0,
            Total = 0,
            supplierId = string.Empty,
            suppliers = suppliers.Select(suppliers => new SelectListItem
            {
                Value = suppliers.Id.ToString(),
                Text = suppliers.CompanyName
            }),
        };
        
        var products = await _productRepository.GetProductsAsync();

       
        ViewBag.Products = products;

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePurchaseOrderRequest createPurchaseOrderRequest)
    {
        if (ModelState.IsValid)
        {
            var orderItems = (await Task.WhenAll(createPurchaseOrderRequest.OrderItems.Select(async x => new OrderItem
            {
                Id = Guid.NewGuid(),
                Product = await _productRepository.GetByIdAsync(x.ProductId),
                Quantity = x.Quantity,
                Total = x.Total
            }))).ToList();

            var shipment = new Shipment
            {
               DeliveryDate = null,
               ShippedDate = null,
                Id = Guid.NewGuid(),
                Status = ShipmentStatus.Pending
            };

            var purchaseOrder = new PurchaseOrder
            {
                createdAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Shipment = shipment,
                Status = OrderStatus.Pending,
                OrderItems = orderItems,
                BillingAddress = createPurchaseOrderRequest.BillingAddress,
                DeliveryDate = createPurchaseOrderRequest.DeliveryDate,
                PaymentMethod = createPurchaseOrderRequest.PaymentMethod,
                ShippingAddress = createPurchaseOrderRequest.ShippingAddress,
                ShippingMethod = createPurchaseOrderRequest.ShippingMethod,
                SubTotal = createPurchaseOrderRequest.SubTotal,
                Total = createPurchaseOrderRequest.Total,
                supplier = await _supplierRepository.GetSupplier(Guid.Parse(createPurchaseOrderRequest.supplierId)),
            };

            await _orderService.AddPurchaseOrderAsync(purchaseOrder);
            return RedirectToAction(nameof(Index));
        }

        var suppliers = await _supplierRepository.GetSuppliersAsync();
        var products = await _productRepository.GetProductsAsync();

        ViewBag.Suppliers = suppliers.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = s.CompanyName
        }).ToList();
        ViewBag.Products = products;

        return View(createPurchaseOrderRequest);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var order = await _orderService.GetPurchaseOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        var suppliers = await _supplierRepository.GetSuppliersAsync();
        var purchaseOrder = await _orderService.GetPurchaseOrderByIdAsync(id);
        if (purchaseOrder == null) {
            return NotFound();
        }
        purchaseOrder.OrderItems = purchaseOrder.OrderItems;
        purchaseOrder.BillingAddress = purchaseOrder.BillingAddress;
        purchaseOrder.DeliveryDate = purchaseOrder.DeliveryDate;
        purchaseOrder.PaymentMethod = purchaseOrder.PaymentMethod;
        purchaseOrder.ShippingAddress = purchaseOrder.ShippingAddress;
        purchaseOrder.ShippingMethod = purchaseOrder.ShippingMethod;
        purchaseOrder.SubTotal = purchaseOrder.SubTotal;
        purchaseOrder.Total = purchaseOrder.Total;
        purchaseOrder.supplier = purchaseOrder.supplier;
        await _orderService.UpdatePurchaseOrderAsync( purchaseOrder);

        var products = await _productRepository.GetProductsAsync();

        ViewBag.Suppliers = suppliers.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = s.CompanyName
        }).ToList();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, CreatePurchaseOrderRequest createPurchaseOrderRequest)
    {
        if (ModelState.IsValid)
        {
            var orderItems = (await Task.WhenAll(createPurchaseOrderRequest.OrderItems.Select(async x => new OrderItem
            {
                Id = Guid.NewGuid(),
                Product = await _productRepository.GetByIdAsync(x.ProductId),
                Quantity = x.Quantity,
                Total = x.Total
            }))).ToList();

            var purchaseOrder = await _orderService.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            purchaseOrder.OrderItems = orderItems;
            purchaseOrder.BillingAddress = createPurchaseOrderRequest.BillingAddress;
            purchaseOrder.DeliveryDate = createPurchaseOrderRequest.DeliveryDate;
            purchaseOrder.PaymentMethod = createPurchaseOrderRequest.PaymentMethod;
            purchaseOrder.ShippingAddress = createPurchaseOrderRequest.ShippingAddress;
            purchaseOrder.ShippingMethod = createPurchaseOrderRequest.ShippingMethod;
            purchaseOrder.SubTotal = createPurchaseOrderRequest.SubTotal;
            purchaseOrder.Total = createPurchaseOrderRequest.Total;
            purchaseOrder.supplier = await _supplierRepository.GetSupplier(Guid.Parse(createPurchaseOrderRequest.supplierId));

            await _orderService.UpdatePurchaseOrderAsync(purchaseOrder);
            return RedirectToAction(nameof(Index));
        }

        var suppliers = await _supplierRepository.GetSuppliersAsync();
        var products = await _productRepository.GetProductsAsync();

        ViewBag.Suppliers = suppliers.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = s.CompanyName
        }).ToList();
        ViewBag.Products = products;

        return View(createPurchaseOrderRequest);
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