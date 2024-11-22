using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GsMS
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumoController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public ConsumoController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Consumo consumo)
        {
            try
            {
                await _mongoDBService.CreateAsync(consumo);
                return CreatedAtAction(nameof(GetById), new { id = consumo.Id }, consumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var consumos = await _mongoDBService.GetAsync();
                if (consumos == null || consumos.Count == 0)
                {
                    return NotFound("No energy consumption data found.");
                }
                return Ok(consumos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var consumos = await _mongoDBService.GetAsync();
                var consumo = consumos.Find(c => c.Id == id);
                if (consumo == null)
                {
                    return NotFound();
                }
                return Ok(consumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}