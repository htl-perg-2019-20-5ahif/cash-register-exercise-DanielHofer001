using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CashRegister.Shared;
using System.Net;

namespace CashRegister.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly DataContext _context;

        public ReceiptsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostReceiptLines([FromBody] List<ReceiptLine> receiptLines)
        {

            if (receiptLines == null || receiptLines.Count == 0)
            {
                return BadRequest("Missing lines");
            }

            foreach (var rl in receiptLines)
            {
                rl.Product = await _context.Products.FirstOrDefaultAsync(p => p.Id == rl.ProductId);
                if (rl.Product == null)
                {
                    return BadRequest($"Unknown product ID {rl.Id}");
                }

            }
            var newReceipt = new Receipt
            {
                TimeStamp = DateTime.UtcNow,
                ReceiptLines = receiptLines.Select(rl => new ReceiptLine
                {
                    Id = 0,
                    Amount = rl.Amount,
                    TotalPrice = rl.Amount * rl.Product.Price,
                    Product=rl.Product
                }).ToList()
            };
            newReceipt.TotalPrice = newReceipt.ReceiptLines.Sum(rl => rl.TotalPrice);
            _context.Receipts.Add(newReceipt);      
            await _context.SaveChangesAsync();

            return StatusCode((int)HttpStatusCode.Created, newReceipt);
        }

    }
}
