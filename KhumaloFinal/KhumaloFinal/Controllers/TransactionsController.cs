using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KhumaloFinal.Data;
using KhumaloFinal.Models;

namespace KhumaloFinal.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.Identity.Name; // Or another way to get the user ID
            var cartItems = await _context.Transaction
               
                .Where(t => t.UserId == userId)
                .ToListAsync();

            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Index");
            }

            // Create processors for each cart item
            foreach (var item in cartItems)
            {
                var processor = new Processor
                {
                    UserId = item.UserId,
                    TransactionId = item.TransactionId,
                    Quantity = item.Quantity,
                    ProcessingDate = DateTime.Now
                };

                _context.Processor.Add(processor);
            }

            // Remove cart items after checkout
            //_context.Transaction.RemoveRange(cartItems);

            await _context.SaveChangesAsync();
           // _context.Transaction.RemoveRange(cartItems);

            return RedirectToAction("Index", "Processors");
        }


        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transaction.Include(t => t.Products);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Products)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,UserId,ProductId,Quantity,TransactionDate")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", transaction.ProductId);
            return View(transaction);
        }

        [HttpPost]
        public IActionResult AddToCart(int ProductId)
        {
            // Assuming you have a method to handle adding items to the cart
            var product = _context.Product.FirstOrDefault(p => p.ProductId == ProductId);
            if (product != null && product.Availability == true)
            {
                var transaction = new Transaction
                {
                    ProductId = product.ProductId,
                    Quantity = 1, 
                    TransactionDate = DateTime.Now,
                    UserId = User.Identity.Name 
                };

                _context.Transaction.Add(transaction);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Transactions"); // Assuming TransactionsController handles transactions listing
        }


        // GET: Transactions/Cart
        public IActionResult Cart()
        {
            

            return View();
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", transaction.ProductId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,UserId,ProductId,Quantity,TransactionDate")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", transaction.ProductId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Products)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                _context.Transaction.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionId == id);
        }
    }
}
