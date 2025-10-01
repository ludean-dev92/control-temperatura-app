using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormatosController : ControllerBase
    {
        private readonly IFormatoService _service;

        public FormatosController(IFormatoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var formatos = await _service.GetAllAsync();
            return Ok(formatos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Formato formato)
        {
            if (formato == null) return BadRequest();
            var created = await _service.CreateAsync(formato);
            return Ok(created);
        }
    }
}
