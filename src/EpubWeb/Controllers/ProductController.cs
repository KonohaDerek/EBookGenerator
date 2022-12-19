using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EpubWeb.Models.Product;

namespace EpubWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetProductByIdRequest query)
        {
            var result = await _mediator.Send(query);

            if (result is null) return NotFound();
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddProductRequest command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}