using System;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist _eventoPersist;
        private readonly IGeralPersist _geralPersist;
        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
        }


        public async Task<Evento> addEventos(Evento model)
        {
            try
            {
                _geralPersist.add(model);
                if (await _geralPersist.SaveChangesAsync())
                {
                    return await _eventoPersist.GetEventosByIdASync(model.Id, false);
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventosByIdASync(eventoId,false);
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

        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventosByIdASync(eventoId, false);
                if (evento == null) 
                    return null;
                
                model.Id = evento.Id;
                
                _geralPersist.Update(model);
                if (await _geralPersist.SaveChangesAsync())
                {
                    return await _eventoPersist.GetEventosByIdASync(model.Id, false);
                }

                return null;
            }
            catch (Exception e)
            {

                throw new Exception(message: e.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrante = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrante);
                return eventos;
            }
            catch (Exception e)
            {
                
                throw new Exception(message:e.Message);
            }
        }

        public async  Task<Evento[]> GetAllEventosbyTemaAsync(string tema, bool includePalestrante = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaASync(tema, includePalestrante);
                return eventos;

            }
            catch (Exception e)
            {
                throw new Exception(message: e.Message);
            }
        }

        public async Task<Evento> GetEventosByIdAsync(int id, bool includePalestrante = false)
        {
            try
            {
                return await _eventoPersist.GetEventosByIdASync(id, includePalestrante);
            }
            catch (Exception e)
            {
                throw new Exception(message: e.Message);
            }
        }
    }
}