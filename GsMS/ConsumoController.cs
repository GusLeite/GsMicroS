using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GsMS
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumoController : ControllerBase
    {
        private static readonly List<Consumo> consumos = new List<Consumo>();

        /// <summary>
        /// Consulta dados de consumo energético.
        /// </summary>
        /// <returns>Lista de consumos.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            if(consumos.Count == 0)
            {
                return NotFound(new { message = "Nenhum dado de consumo encontrado no nosso banco de dados"});
            }

            return Ok(consumos);
        }

        /// <summary>
        /// Consulta dados de consumo energético por Id.
        /// </summary>
        /// <param name="id">Id do consumo.</param>
        /// <returns>Dados do consumo.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var consumo = consumos.FirstOrDefault(c => c.Id == id);

            if (consumo == null)
            {
                return NotFound(new { message = "Consumption data not found." });
            }

            return Ok(new { message = "Consumption data retrieved successfully.", data = consumo });
        }

        /// <summary>
        /// Registra dados de consumo energético.
        /// </summary>
        /// <param name="consumo">Dados do consumo.</param>
        /// <returns>Status da operação.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Consumo consumo)
        {
            if (consumo.Nome == null|| consumo.local == null)
            {
                return BadRequest(new { message = "Prencha todos os campos para fazer um registro valido!" });
            }
            if (consumo.ValorEmKwh == 0)
            {
                return BadRequest(new { message = "O valorEmKwh deve ser maior do que 0!" });
            }
            
           
            Random random = new Random();
            int randomInt = random.Next(1, 10000);
            consumo.Id = randomInt;
            consumos.Add(consumo);
            return CreatedAtAction(nameof(Get), new { id = consumo.Id }, consumo);
        }
    }
}