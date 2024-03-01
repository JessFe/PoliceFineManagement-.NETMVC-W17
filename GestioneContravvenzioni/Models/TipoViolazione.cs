using System.ComponentModel.DataAnnotations;

namespace GestioneContravvenzioni.Models
{
    public class TipoViolazione
    {
        [Display(Name = "ID Violazione")]
        public int IDviolazione { get; set; }

        [Required(ErrorMessage = "Inserire una descrizione")]
        public string Descrizione { get; set; }
    }
}