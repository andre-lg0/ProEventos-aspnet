using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Contratos;
using System;
using ProEventos.Domain;
using System.Linq;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {

        private  readonly ILotePersist _lotePersist;
        private readonly IMapper _mapper;
        private readonly IGeralPersist _geralPersist;

        public LoteService(ILotePersist lotePersist, IMapper mapper, IGeralPersist geralPersist)
        {
            _lotePersist = lotePersist;

            _mapper = mapper;
            _geralPersist = geralPersist;
        }

        public async  Task<bool> DeleteLote(int eventoId, int id)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdsAsync(eventoId,id);
                if (lote == null)
                    return false;

                _geralPersist.Delete(lote);
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
        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null) return null;

            foreach (var model in models)
            {
             if( model.Id == 0){
                await addLotes(eventoId, model);

             }
             else{
                var lote  = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                _mapper.Map(model,lote);
                _geralPersist.Update(lote);
                await _geralPersist.SaveChangesAsync();

             }   
            }

            var loteDto =  await _lotePersist.GetLotesByEventoIdAsync(eventoId);
            return _mapper.Map<LoteDto[]>(loteDto);

        }

        public  async Task<LoteDto> GetLoteByIdAsync(int eventoId, int id)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdsAsync(eventoId,id);
                if(lote == null) 
                    return null;

                return _mapper.Map<LoteDto>(lote);
            
            }
            catch (Exception ex )
            {
                
                throw new Exception(message: ex.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
              try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                if(lotes== null) 
                    return null;

                return _mapper.Map<LoteDto[]>(lotes);
            
            }
            catch (Exception ex )
            {
                
                throw new Exception(message: ex.Message);
            }
        }


         public async Task addLotes(int eventoId, LoteDto model)
        {
            try
            {
                var lote  = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;

                _geralPersist.add(lote);
                 await _geralPersist.SaveChangesAsync();
    
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}