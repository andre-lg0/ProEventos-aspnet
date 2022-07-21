using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Helpers;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist _eventoPersist;
        private readonly IMapper _mapper;
        private readonly IGeralPersist _geralPersist;
        

        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
            _mapper = mapper;
        }


        public async Task<EventoDto> addEventos(int userId, EventoDto model)
        {
            try
            {
                var evento  = _mapper.Map<Evento>(model);
                evento.UserId = userId;
                _geralPersist.add(evento);
                if (await _geralPersist.SaveChangesAsync())
                {
                    var result = await _eventoPersist.GetEventosByIdAsync(userId,evento.Id, false);
                    return _mapper.Map<EventoDto>(result);
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteEvento(int userId,int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventosByIdAsync(userId,eventoId,false);
                if (evento == null)
                    return false;
                _geralPersist.Delete(evento);
                if (await _geralPersist.SaveChangesAsync())
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
               
                throw new Exception(message:e.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventosByIdAsync(userId,eventoId, false);
                if (evento == null) 
                    return null;
            
                model.Id = evento.Id;
                model.UserId = userId;
                
                _mapper.Map(model,evento);
                _geralPersist.Update(evento);
                if (await _geralPersist.SaveChangesAsync())
                {
                    var result  = await _eventoPersist.GetEventosByIdAsync(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(result);
                }

                return null;
            }
            catch (Exception e)
            {

                throw new Exception(message: e.Message);
            }
        }

        public async Task<PageList<EventoDto>> GetAllEventosAsync(int userId,PageParams pageParams,bool includePalestrante = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(userId,pageParams,includePalestrante);
                if(eventos == null) return null;
                
                var eventoList = _mapper.Map<PageList<EventoDto>>(eventos);

                eventoList.PageSize = eventos.PageSize;
                eventoList.CurrentPage = eventos.CurrentPage;
                eventoList.TotalPage = eventos.TotalPage;
                eventoList.TotalCount = eventos.TotalCount;

                return eventoList;
            }
            catch (Exception e)
            {
                
                throw new Exception(message:e.Message);
            }
        }

       

        public async Task<EventoDto> GetEventosByIdAsync(int userId,int id, bool includePalestrante = false)
        {
            try
            {
                var evento =  await _eventoPersist.GetEventosByIdAsync(userId,id, includePalestrante);
                if(evento == null) return null;

                return  _mapper.Map<EventoDto>(evento);
            }
            catch (Exception e)
            {
                throw new Exception(message: e.Message);
            }
        }
    }
}