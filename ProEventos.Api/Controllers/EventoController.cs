using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EventoController(IEventoService eventoService,IWebHostEnvironment hostEnvironment)
        {
            _eventoService = eventoService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(),true);
                if (eventos == null) return NoContent();

                return Ok(eventos);

            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro{e.Message}");
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EventoById(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(), id, true);
                if (evento == null)
                {
                    return NoContent();
                }

                return Ok(evento);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar evento. Erro{e.Message}");
            }
        }
        
        [HttpGet("tema/{tema}")]
        public async Task<IActionResult> EventoByTema(string tema)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosbyTemaAsync(User.GetUserId(),tema, true);
                if (eventos == null)
                {
                    return NoContent();
                }

                return Ok(eventos);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar evento. Erro{e.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _eventoService.addEventos(User.GetUserId(),model);
                if (evento == null)
                    return BadRequest("Erro ao tentar adicionar evento");
                return Ok(evento);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar evento. Erro{e.Message}");
            }
            
        }
        [HttpPost("image/{eventoId}")]
         public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(),eventoId);
                if (evento == null)
                    return NoContent();

                var file  = Request.Form.Files[0];
                if(file.Length > 0){
                    DeleteImage(evento.ImageUrl);
                    evento.ImageUrl = await SaveImage(file);
                }
                var EventoRetorn  = await _eventoService.UpdateEvento(User.GetUserId(),eventoId,evento);

                return Ok(evento);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar imagem. Erro{e.Message}");
            }
            
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _eventoService.UpdateEvento(User.GetUserId(),id, model);
                if (evento == null)
                    return BadRequest("Erro ao Tentar modificar o evento");
                return Ok(evento);

            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar modificar evento. Erro{e.Message}");
            }
            
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                EventoDto evento = await  _eventoService.GetEventosByIdAsync(User.GetUserId(),id,true);
                if(evento == null) return NoContent();

                if (await _eventoService.DeleteEvento(User.GetUserId(),id))
                {
                    DeleteImage(evento.ImageUrl);
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

        [NonAction]
        public void DeleteImage(string imageName){
            var imagePath  = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);
            if(System.IO.File.Exists(imagePath)){
                System.IO.File.Delete(imagePath);
            }

        }

         [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile){
           var imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                            .Take(15).ToArray()
                                            ).Replace(' ', '-');

           imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

           var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);
           using(var fileStream = new FileStream(imagePath,FileMode.Create)){
                await imageFile.CopyToAsync(fileStream);
           }
            return imageName;

        }

    }
}