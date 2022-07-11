using BookStore.Models.DataModels;
using BookStore.Models.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BookStore.Api.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly CartRepository _cartRepository = new();

        [HttpGet]
        [Route("list2")]
        public IActionResult GetCartItems(string? keyword)
        {
            List<Cart> carts = _cartRepository.GetCartItems(keyword);
            IEnumerable<CartModel> cartModels = carts.Select(c => new CartModel(c));
            return Ok(cartModels);
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResponse<CartModelResponse>), (int)HttpStatusCode.OK)]
        public IActionResult GetCartItem2(int UserId)
        {


            var cartitem = _cartRepository.GetCartListall(UserId);
            ListResponse<CartModelResponse> listResponce = new()
            {
                records = cartitem.records.Select(c => new CartModelResponse(c)),
                totalRecords = cartitem.totalRecords,
            };

            return Ok(listResponce);
        }

        [HttpPost("add")]
        public IActionResult AddCart(CartModel model)
        {
            if (model == null)
                return BadRequest();

            Cart cart = new()
            {
                Id = model.Id,
                Quantity = 1,
                Bookid = model.BookId,
                Userid = model.UserId,
        };
            cart = _cartRepository.AddCart(cart);
            if (cart == null)
            {
                return StatusCode(500);
            }

            return Ok(new CartModel(cart));
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateCart(CartModel model)
        {
            if (model == null)
                return BadRequest();

            Cart cart = new()
            {
                Id = model.Id,
                Quantity = model.Quantity,
                Bookid = model.BookId,
                Userid = model.UserId
            };
            cart = _cartRepository.UpdateCart(cart);

            return Ok(new CartModel(cart));
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteCart(int id)
        {
            if (id == 0)
                return BadRequest();

            bool response = _cartRepository.DeleteCart(id);
            return Ok(response);
        }
    }
}
