using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Api.Data;
using ProEventos.Api.Models;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class EventoController : ControllerBase
    {


        private readonly ILogger<EventoController> _logger;
        private readonly DataContext _context;

        
        public EventoController(ILogger<EventoController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _context.Eventos;
        }

        [HttpGet("{id}")]
        public IEnumerable<Evento> EventoById(int id)
        {
            return _context.Eventos.Where(evento => evento.EventoId == id);
        }
    }
}