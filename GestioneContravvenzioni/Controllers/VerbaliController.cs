using GestioneContravvenzioni.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace GestioneContravvenzioni.Controllers
{
    public class VerbaliController : Controller
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connStringDb"].ConnectionString;

        // GET: Verbali
        public ActionResult Index()
        {
            //  crea una lista di verbali vuota
            List<Verbale> verbali = new List<Verbale>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  crea una query per selezionare tutti i verbali
                //  e le descrizioni delle violazioni
                string sql = @"
                SELECT V.*, TV.Descrizione AS DescrizioneViolazione
                FROM Verbale V
                INNER JOIN TipoViolazione TV ON V.IDviolazione_FK = TV.IDviolazione
                ORDER BY V.DataViolazione DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);

                //  apre la connessione e leggi i dati
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //  crea un oggetto Verbale per ogni riga letta e lo aggiunge alla lista
                        verbali.Add(new Verbale
                        {
                            IDverbale = Convert.ToInt32(reader["IDverbale"]),
                            DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                            IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                            Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                            DataTrascrizioneVerbale = Convert.ToDateTime(reader["DataTrascrizioneVerbale"]),
                            Importo = Convert.ToDecimal(reader["Importo"]),
                            DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]),
                            IDanagrafica_FK = Convert.ToInt32(reader["IDanagrafica_FK"]),
                            IDviolazione_FK = Convert.ToInt32(reader["IDviolazione_FK"]),

                            DescrizioneViolazione = reader["DescrizioneViolazione"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }


            return View(verbali);
        }

        public ActionResult Create()
        {
            // Recupero l'elenco delle persone per il menu a discesa
            List<SelectListItem> anagrafiche = new List<SelectListItem>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  Query per selezionare tutte le anagrafiche
                string sql = "SELECT IDanagrafica, Cognome, Nome FROM Anagrafica";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //  Aggiunge alla lista per ogni anagrafica
                    anagrafiche.Add(new SelectListItem
                    {
                        Value = reader["IDanagrafica"].ToString(),
                        Text = reader["IDanagrafica"] + ": " + reader["Cognome"] + " " + reader["Nome"] // Concatenazione per visualizzare cognome e nome
                    });
                }
            }

            ViewBag.Anagrafiche = anagrafiche;


            // Recupero l'elenco dei tipi di violazione per il menu a discesa
            List<SelectListItem> tipiViolazione = new List<SelectListItem>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  Query per selezionare tutti i tipi di violazione
                string sql = "SELECT IDviolazione, Descrizione FROM TipoViolazione";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //  Aggiunge alla lista per ogni tipo di violazione
                    tipiViolazione.Add(new SelectListItem
                    {
                        Value = reader["IDviolazione"].ToString(),
                        Text = reader["IDviolazione"] + ": " + reader["Descrizione"].ToString()
                    });
                }
            }
            ViewBag.TipiViolazione = tipiViolazione;

            return View();
        }

        [HttpPost]

        public ActionResult Create(Verbale verbale)
        {
            verbale.DataTrascrizioneVerbale = DateTime.Now;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  Query per inserire un nuovo verbale
                string sql = "INSERT INTO Verbale (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, IDanagrafica_FK, IDviolazione_FK) VALUES (@DataViolazione, @IndirizzoViolazione, @Nominativo_Agente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @IDanagrafica_FK, @IDviolazione_FK)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.Nominativo_Agente);
                cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                cmd.Parameters.AddWithValue("@Importo", verbale.Importo);
                cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti);
                cmd.Parameters.AddWithValue("@IDanagrafica_FK", verbale.IDanagrafica_FK);
                cmd.Parameters.AddWithValue("@IDviolazione_FK", verbale.IDviolazione_FK);

                //  Apre la connessione e esegue la query
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
                //  Gestione dell'eccezione
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View();
                }
            }
        }

        // GET: Verbali/Edit/5
        public ActionResult Edit(int id)
        {
            Verbale verbale = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  Query per selezionare un verbale per ID
                string sql = "SELECT * FROM Verbale WHERE IDverbale = @IDverbale";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IDverbale", id);

                //  Apre la connessione e legge i dati
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        verbale = new Verbale
                        {
                            IDverbale = Convert.ToInt32(reader["IDverbale"]),
                            DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                            IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                            Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                            DataTrascrizioneVerbale = Convert.ToDateTime(reader["DataTrascrizioneVerbale"]),
                            Importo = Convert.ToInt32(reader["Importo"]),
                            DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]),
                            IDanagrafica_FK = Convert.ToInt32(reader["IDanagrafica_FK"]),
                            IDviolazione_FK = Convert.ToInt32(reader["IDviolazione_FK"])
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Gestione dell'eccezione
                }
            }

            if (verbale == null)
            {
                return HttpNotFound();
            }

            // Preparazione dei dati per i dropdowns
            List<SelectListItem> anagrafiche = new List<SelectListItem>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  Query per selezionare tutte le anagrafiche
                string sql = "SELECT IDanagrafica, Cognome, Nome FROM Anagrafica";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    anagrafiche.Add(new SelectListItem
                    {
                        Value = reader["IDanagrafica"].ToString(),
                        Text = reader["IDanagrafica"] + ": " + reader["Cognome"] + " " + reader["Nome"]
                    });
                }
            }
            ViewBag.Anagrafiche = anagrafiche;

            List<SelectListItem> tipiViolazione = new List<SelectListItem>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  Query per selezionare tutti i tipi di violazione
                string sql = "SELECT IDviolazione, Descrizione FROM TipoViolazione";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tipiViolazione.Add(new SelectListItem
                    {
                        Value = reader["IDviolazione"].ToString(),
                        Text = reader["IDviolazione"] + ": " + reader["Descrizione"].ToString()
                    });
                }
            }
            ViewBag.TipiViolazione = tipiViolazione;


            return View(verbale);
        }


        // POST: Verbali/Edit/5
        [HttpPost]

        public ActionResult Edit(Verbale verbale)
        {
            //  Verifica la validità del modello
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        //  Query per aggiornare un verbale
                        string sql = "UPDATE Verbale SET DataViolazione = @DataViolazione, IndirizzoViolazione = @IndirizzoViolazione, Nominativo_Agente = @Nominativo_Agente, DataTrascrizioneVerbale = @DataTrascrizioneVerbale, Importo = @Importo, DecurtamentoPunti = @DecurtamentoPunti, IDanagrafica_FK = @IDanagrafica_FK, IDviolazione_FK = @IDviolazione_FK WHERE IDverbale = @IDverbale";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        cmd.Parameters.AddWithValue("@IDverbale", verbale.IDverbale);
                        cmd.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                        cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                        cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.Nominativo_Agente);
                        cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                        cmd.Parameters.AddWithValue("@Importo", verbale.Importo);
                        cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti);
                        cmd.Parameters.AddWithValue("@IDanagrafica_FK", verbale.IDanagrafica_FK);
                        cmd.Parameters.AddWithValue("@IDviolazione_FK", verbale.IDviolazione_FK);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return RedirectToAction("Index");
                    }
                }
            }
            // Gestione eccezioni
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }


            return View(verbale);
        }



    }
}