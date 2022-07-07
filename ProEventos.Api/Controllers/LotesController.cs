using System;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _loteService;




        public LotesController(ILoteService loteService)
        {
            _loteService = loteService;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar os lotes. Erro{e.Message}");
            }
        }

        
        

     
        
        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes  = await _loteService.SaveLotes(eventoId,models);
                if (lotes == null)
                    return BadRequest("Erro ao Tentar modificar o lotes");
                return Ok(lotes);

            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar modificar lotes. Erro{e.Message}");
            }
            
        }
        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> Delete(int eventoId, int id)
        {
            try
            {
        
                if (await _loteService.DeleteLote(eventoId,id))
                {
                    return Ok("Deletado");
                }else
                    return BadRequest("evento nao deletado");
                
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar remover evento. Erro{e.Message}");
            }
            
        }
    }
}