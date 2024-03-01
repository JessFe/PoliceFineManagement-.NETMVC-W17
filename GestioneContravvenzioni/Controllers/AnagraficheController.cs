using GestioneContravvenzioni.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace GestioneContravvenzioni.Controllers
{
    public class AnagraficheController : Controller
    {
        //connetti al database
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connStringDb"].ConnectionString;

        // GET: Anagrafiche
        public ActionResult Index()
        {
            //crea una lista di anagrafiche vuota
            List<Anagrafica> anagrafiche = new List<Anagrafica>();

            //crea una connessione al database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //crea una query per selezionare tutte le anagrafiche
                string sql = "SELECT * FROM Anagrafica";
                SqlCommand cmd = new SqlCommand(sql, conn);

                //apre la connessione e leggi i dati
                //crea un oggetto Anagrafica per ogni riga letta e lo aggiunge alla lista
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        anagrafiche.Add(new Anagrafica
                        {
                            IDanagrafica = Convert.ToInt32(reader["IDanagrafica"]),
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Indirizzo = reader["Indirizzo"].ToString(),
                            Citta = reader["Citta"].ToString(),
                            CAP = reader["CAP"].ToString(),
                            Cod_Fisc = reader["Cod_Fisc"].ToString()
                        });
                    }
                }
                //gestisce eventuali eccezioni
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            //ritorna la lista di anagrafiche alla vista
            return View(anagrafiche);
        }


        public ActionResult Create()
        {
            return View();
        }


        // POST: Anagrafiche/Create
        [HttpPost]

        public ActionResult Create(Anagrafica anagrafica)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //crea una query per inserire una nuova anagrafica
                string sql = "INSERT INTO Anagrafica (Cognome, Nome, Indirizzo, Citta, CAP, Cod_Fisc) VALUES (@Cognome, @Nome, @Indirizzo, @Citta, @CAP, @Cod_Fisc)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);
                cmd.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                cmd.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo);
                cmd.Parameters.AddWithValue("@Citta", anagrafica.Citta);
                cmd.Parameters.AddWithValue("@CAP", anagrafica.CAP);
                cmd.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc);

                //apre la connessione e esegue la query
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
                //gestisce eventuali eccezioni
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View();
                }
            }
        }

        // GET: Anagrafiche/Edit/5
        public ActionResult Edit(int id)
        {
            //crea un oggetto Anagrafica vuoto
            Anagrafica anagrafica = null;
            //crea una connessione al database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //crea una query per selezionare l'anagrafica con l'ID specificato
                string sql = "SELECT * FROM Anagrafica WHERE IDanagrafica = @IDanagrafica";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IDanagrafica", id);

                //apre la connessione e legge i dati
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        //crea un oggetto Anagrafica con i dati letti
                        anagrafica = new Anagrafica
                        {
                            IDanagrafica = Convert.ToInt32(reader["IDanagrafica"]),
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Indirizzo = reader["Indirizzo"].ToString(),
                            Citta = reader["Citta"].ToString(),
                            CAP = reader["CAP"].ToString(),
                            Cod_Fisc = reader["Cod_Fisc"].ToString()
                        };
                    }
                    reader.Close();
                }
                //gestisce eventuali eccezioni
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Gestire l'eccezione, ad esempio mostrare un messaggio di errore
                }
            }

            if (anagrafica == null)
            {
                return HttpNotFound();
            }

            return View(anagrafica);
        }

        // POST: Anagrafiche/Edit/5
        [HttpPost]
        public ActionResult Edit(Anagrafica anagrafica)
        {
            //controlla che i dati siano validi
            if (ModelState.IsValid)
            {
                //crea una connessione al database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //crea una query per aggiornare l'anagrafica
                    string sql = "UPDATE Anagrafica SET Cognome = @Cognome, Nome = @Nome, Indirizzo = @Indirizzo, Citta = @Citta, CAP = @CAP, Cod_Fisc = @Cod_Fisc WHERE IDanagrafica = @IDanagrafica";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@IDanagrafica", anagrafica.IDanagrafica);
                    cmd.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);
                    cmd.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                    cmd.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo);
                    cmd.Parameters.AddWithValue("@Citta", anagrafica.Citta);
                    cmd.Parameters.AddWithValue("@CAP", anagrafica.CAP);
                    cmd.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc);

                    //apre la connessione e esegue la query
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    //Gestione eccezioni
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return View(anagrafica);
                    }
                }

                return RedirectToAction("Index");
            }

            return View(anagrafica);
        }


    }
}