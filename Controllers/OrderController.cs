using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using WepApi.Dto;
using WepApi.InterFaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersInterface _orderInterface;
        private readonly IMapper _mapper;

        public OrdersController(IOrdersInterface orderInterface,IMapper mapper)
        {
            _orderInterface = orderInterface;
            _mapper = mapper;
        }

        // GET: api/Orders
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Orders>))]
        public IActionResult GetOrder()
        {
            var orders = _mapper.Map<List<Orders>>(_orderInterface.GetOrders());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(orders);

        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Orders))]
        public IActionResult GetOrder(int id)
        {
            var orders = _mapper.Map<Orders>(_orderInterface.GetOrders(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(orders);

        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult PostOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto == null)
                return BadRequest("Invalid customer data.");

            if (!_orderInterface.CostumerExist(orderDto.CustomerId) || !_orderInterface.ProductExist(orderDto.ProductId))
                return BadRequest(orderDto);

            Orders newOrder = new Orders()
            {
               CustomerId = orderDto.CustomerId,
               ProductId = orderDto.ProductId,
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (!_orderInterface.CreateOrder(newOrder))
                return StatusCode(500, "Something went wrong while saving.");

            return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
        }



    }
}
