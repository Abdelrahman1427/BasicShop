using BasicShop.Data;
using BasicShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ShopContext _context;

        public CartItemsController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/CartItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            return await _context.CartItems.ToListAsync();
        }

        // GET: api/CartItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> GetCartItem(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            return cartItem;
        }

        // POST: api/CartItems
        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItem(CartItem cartItem)
        {
            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            // Check if there is enough quantity
            if (product.Qty < cartItem.Quantity)
            {
                return BadRequest("Not enough product quantity available.");
            }

            _context.CartItems.Add(cartItem);
            product.Qty -= cartItem.Quantity; // Reduce the product quantity

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartItem", new { id = cartItem.Id }, cartItem);
        }

        // PUT: api/CartItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return BadRequest();
            }
            var existingCartItem = await _context.CartItems.FindAsync(id);
            if (existingCartItem == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            int quantityDifference = cartItem.Quantity - existingCartItem.Quantity;

            if (product.Qty < quantityDifference)
            {
                return BadRequest("Not enough product quantity available.");
            }

            // Update cart item
            _context.Entry(existingCartItem).State = EntityState.Modified;
            existingCartItem.Quantity = cartItem.Quantity; // Update quantity

            // Update product quantity
            product.Qty -= quantityDifference; // Reduce the product quantity
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/CartItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
    var cartItem = await _context.CartItems.FindAsync(id);
    if (cartItem == null)
    {
        return NotFound();
    }

    var product = await _context.Products.FindAsync(cartItem.ProductId);
    if (product == null)
    {
        return NotFound("Product not found.");
    }

    // Increase product quantity
    product.Qty += cartItem.Quantity;

    _context.CartItems.Remove(cartItem);
    await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
