using AutoMapper;

using Basket.API.Entities;
using Basket.API.gRPCServices;
using Basket.API.Repositories;

using EventBus.Messages.Events;

using MassTransit;

using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository repository,IPublishEndpoint publishEndpoint, DiscountGrpcService discountGrpcService, IMapper mapper )
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
        }




        //[HttpPost(Name = "CreateBasket")]
        //[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<ShoppingCart>> CreateBasket([FromBody] ShoppingCart cart)
        //{
        //    var basket = await _repository.Create(cart);
        //    return Ok(basket);
        //}


        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);

            //await _publishEndpoint.Publish(new BasketCheckoutEvent
            //{
            //    UserName = "Dante",
            //    TotalPrice = 10,
            //    FirstName = "dante",
            //    LastName = "sparda",
            //    EmailAddress = "dante@test.com",
            //    AddressLine = "redgrave",
            //    Country = "???",
            //    State = "???",
            //    ZipCode = "???",
            //    CardName = "Demon Red",
            //    CardNumber = "666 666 777 777",
            //    Expiration = "67-98",
            //    CVV = "879",
            //    PaymentMethod = 2

            //});


            return Ok(basket ?? new ShoppingCart(userName));
        }


        [HttpPost(Name ="UpdateBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {

            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _repository.UpdateBasket(basket));
        }


        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }


        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price 
            // Create basketCheckoutEvent -- Set TotalPrice on basketCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the basket

            // get existing basket with total price
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // send checkout event to rabbitmq
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            // remove the basket
            await _repository.DeleteBasket(basket.UserName);

            return Accepted();
        }

    }
}
