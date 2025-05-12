using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager orderManager;
        public OrderController(IOrderManager orderManager)
        {
            this.orderManager = orderManager;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrderFromCart()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var orders = await orderManager.PlaceOrderFromCart(userId);

                if (orders == null || orders.Count == 0)
                {
                    return NotFound(new { Success = false, Message = "No items in the cart to place an order." });
                }

                return Ok(new { Success = true, Message = "Order placed successfully.", Data = orders });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while placing the order.", Error = ex.Message });
            }
        }
    }
}
