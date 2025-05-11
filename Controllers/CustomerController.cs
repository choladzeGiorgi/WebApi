using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using WebApi.Data;
using WebApi.Models;
using WepApi;
using WepApi.Dto;
using WepApi.InterFaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController :  ControllerBase
    {
        private readonly ICustomerInterface _CustomerInterface;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerInterface CustomerInterface, IMapper mapper)
        {
            _CustomerInterface = CustomerInterface;
            _mapper = mapper;
        }




        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CustomerDto>))]
        public IActionResult GetCustomer()
        {
            var Customers = _mapper.Map<List<CustomerDto>>(_CustomerInterface.GetCustomers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(Customers);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        public IActionResult GetCustomer(int id)
        {
            var Customer = _mapper.Map<Customer>(_CustomerInterface.GetCustomer(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(Customer);

        }

        [HttpGet("product/{Customerid}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetCustomerProducts(int Customerid)
        {
            var products = _mapper.Map<List<Product>>(_CustomerInterface.GetCustomersProducts(Customerid));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(products);

        }


     


        [HttpPost]
        [ProducesResponseType(201)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(409)]
        public IActionResult PostCustomer([FromBody] CustomerDto  Customer)
        {
            if (Customer == null)
                return BadRequest("Invalid customer data.");

            if (_CustomerInterface.CustomerExists(Customer.Email))
                return BadRequest("Customer already exists."); // 409 Conflict

            Customer newCustomer = new Customer()
            {
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                PhoneNumber = Customer.PhoneNumber,
                Email = Customer.Email,
                Password = Customer.Password
            };

            if (!_CustomerInterface.CreateCustomer(newCustomer))
                return StatusCode(500, "Something went wrong while saving.");

            return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.Id }, newCustomer);
        }




    }
}
