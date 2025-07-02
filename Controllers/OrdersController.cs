using Microsoft.AspNetCore.Mvc;
using apidappermvcproje.Data;
using apidappermvcproje.Models;

namespace apidappermvcproje.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderRepository _orderRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
        public OrdersController(OrderRepository orderRepository, CustomerRepository customerRepository, ProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _customerRepository.GetAllAsync();
            ViewBag.Products = await _productRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _customerRepository.GetAllAsync();
                ViewBag.Products = await _productRepository.GetAllAsync();
                return View(order);
            }
            // Ürün fiyatını çekip toplam fiyatı hesapla
            var product = await _productRepository.GetByIdAsync(order.ProductId);
            if (product == null) return BadRequest();
            order.TotalPrice = product.Price * order.Quantity;
            order.OrderDate = DateTime.Now;
            await _orderRepository.CreateAsync(order);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();
            ViewBag.Customers = await _customerRepository.GetAllAsync();
            ViewBag.Products = await _productRepository.GetAllAsync();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _customerRepository.GetAllAsync();
                ViewBag.Products = await _productRepository.GetAllAsync();
                return View(order);
            }
            var product = await _productRepository.GetByIdAsync(order.ProductId);
            if (product == null) return BadRequest();
            order.TotalPrice = product.Price * order.Quantity;
            await _orderRepository.UpdateAsync(order);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
} 