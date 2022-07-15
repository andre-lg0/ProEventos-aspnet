

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ProEventos.Application.Dtos
{
    public class EventoDto {
        public int Id { get; set; }
        public string Local { get; set; }

        public string  DataEvento { get; set; }
        
        [Required(ErrorMessage ="O {0} é obrigatório")]
        public string Tema { get; set; }
        
        [Range(1,int.MaxValue)]
        public int QtdPessoas { get; set; }
        
        [Required(ErrorMessage ="O {0} é obrigatório")]
        [Phone]
        public string Telefone { get; set; }
        
        [Display(Name ="e-mail")]
        [EmailAddress]
        public string Email { get; set; }
        
        [RegularExpression(pattern:@".*\.(jpe?g|png|bmp|gif)$")]
        public string ImageUrl { get; set; }

        public int UserId { get; set; }

        public UserDto User { get; set; }
        
        public IEnumerable<LoteDto> Lotes { get; set; }
        
         public IEnumerable<RedeSocialDto> RedesSocials { get; set; }
        
    
         public IEnumerable<PalestranteDto> Palestrantes { get; set; }
    }
}