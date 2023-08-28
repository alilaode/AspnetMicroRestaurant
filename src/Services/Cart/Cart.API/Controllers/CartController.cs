using AutoMapper;
using Cart.API.Entities;
using Cart.API.Repositories.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Cart.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;
        //private readonly DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public CartController(ICartRepository repository, //DiscountGrpcService discountGrpcService, 
            IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            //_discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{userName}", Name = "GetCart")]
        [ProducesResponseType(typeof(Bill), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bill>> GetCart(string userName)
        {
            var basket = await _repository.GetCart(userName);
            return Ok(basket ?? new Bill(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Bill), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bill>> UpdateCart([FromBody] Bill cart)
        {
            return Ok(await _repository.UpdateCart(cart));
        }

        [HttpDelete("{userName}", Name = "DeleteCart")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCart(string userName)
        {
            await _repository.DeleteCart(userName);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] CartCheckout cartCheckout)
        {
            // get existing cart with total price            
            // Set TotalPrice on cartCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the cart

            // get existing basket with total price
            var cart = await _repository.GetCart(cartCheckout.UserName);
            if (cart == null)
            {
                return BadRequest();
            }

            // send checkout event to rabbitmq
            var eventMessage = _mapper.Map<CartCheckoutEvent>(cartCheckout);
            eventMessage.TotalPrice = cart.TotalPrice;
            await _publishEndpoint.Publish<CartCheckoutEvent>(eventMessage);

            // remove the cart
            await _repository.DeleteCart(cart.UserName);

            return Accepted();
        }
    }
}
