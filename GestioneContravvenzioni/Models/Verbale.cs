using System;
using System.ComponentModel.DataAnnotations;

namespace GestioneContravvenzioni.Models
{
    public class Verbale
    {
        [Display(Name = "ID Verbale")]
        public int IDverbale { get; set; }

        [Display(Name = "Data Violazione")]
        [Required(ErrorMessage = "La data della violazione è obbligatoria")]
        public DateTime DataViolazione { get; set; }

        [Display(Name = "Indirizzo Violazione")]
        [Required(ErrorMessage = "L'indirizzo è obbligatorio")]
        public string IndirizzoViolazione { get; set; }

        [Display(Name = "Nominativo Agente")]
        [Required(ErrorMessage = "Il nominativo dell'agente è obbligatorio")]
        public string Nominativo_Agente { get; set; }

        [Display(Name = "Data Trascrizione Verbale")]
        [Required(ErrorMessage = "La data di trascrizione del verbale è obbligatoria")]
        public DateTime DataTrascrizioneVerbale { get; set; }

        [Required(ErrorMessage = "L'importo è obbligatorio")]
        public decimal Importo { get; set; }

        [Display(Name = "Punti Decurtati")]
        [Required(ErrorMessage = "I punti decurtati sono obbligatori")]
        public int DecurtamentoPunti { get; set; }

        [Display(Name = "ID Anagrafica")]
        public int IDanagrafica_FK { get; set; }

        [Display(Name = "ID Violazione")]
        public int IDviolazione_FK { get; set; }

        [Display(Name = "Descr. Violazione")]
        public string DescrizioneViolazione { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
    }
}