using System.ComponentModel.DataAnnotations;

namespace GestioneContravvenzioni.Models
{
    public class Anagrafica
    {
        [Display(Name = "ID Anagrafica")]
        public int IDanagrafica { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio.")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "L'indirizzo è obbligatorio.")]
        public string Indirizzo { get; set; }

        [Display(Name = "Città")]
        [Required(ErrorMessage = "La città è obbligatoria.")]
        public string Citta { get; set; }

        [Required(ErrorMessage = "Il CAP è obbligatorio.")]
        public string CAP { get; set; }

        [Display(Name = "Cod. Fiscale")]
        [Required(ErrorMessage = "Il codice fiscale è obbligatorio.")]
        public string Cod_Fisc { get; set; }


        public int TotVerbali { get; set; } // Proprietà aggiuntiva per il conteggio dei verbali
    }
}