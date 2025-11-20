using BlogPersonal.Application.Commands.Etiquetas;
using BlogPersonal.Application.DTOs;
using BlogPersonal.Application.Queries.Etiquetas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogPersonal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtiquetasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EtiquetasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<EtiquetaDto>>> GetEtiquetas()
        {
            var etiquetas = await _mediator.Send(new GetEtiquetasQuery());
            return Ok(etiquetas);
        }

        [HttpPost]
        public async Task<ActionResult<EtiquetaDto>> CreateEtiqueta([FromBody] CreateEtiquetaCommand command)
        {
            var etiqueta = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetEtiquetas), new { id = etiqueta.Id }, etiqueta);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EtiquetaDto>> UpdateEtiqueta(int id, [FromBody] UpdateEtiquetaCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var etiqueta = await _mediator.Send(command);

            if (etiqueta == null)
            {
                return NotFound();
            }

            return Ok(etiqueta);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEtiqueta(int id)
        {
            var result = await _mediator.Send(new DeleteEtiquetaCommand { Id = id });

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
