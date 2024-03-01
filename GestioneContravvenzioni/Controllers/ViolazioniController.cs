using GestioneContravvenzioni.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace GestioneContravvenzioni.Controllers
{
    public class ViolazioniController : Controller
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connStringDb"].ConnectionString;

        // GET: Violazioni
        public ActionResult Index()
        {
            //crea una lista di violazioni vuota
            List<TipoViolazione> violazioni = new List<TipoViolazione>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //crea una query per selezionare tutte le violazioni
                string sql = "SELECT * FROM TipoViolazione";
                SqlCommand cmd = new SqlCommand(sql, conn);

                //apre la connessione e leggi i dati
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //crea un oggetto TipoViolazione per ogni riga letta e lo aggiunge alla lista
                        violazioni.Add(new TipoViolazione
                        {
                            IDviolazione = Convert.ToInt32(reader["IDviolazione"]),
                            Descrizione = reader["Descrizione"].ToString()
                        });
                    }
                }
                //gestione eccez
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return View(violazioni);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(TipoViolazione violazione)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //crea una query per inserire una nuova violazione
                string sql = "INSERT INTO TipoViolazione (Descrizione) VALUES (@Descrizione)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Descrizione", violazione.Descrizione);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View();
                }
            }
        }

        // GET: Violazioni/Edit/5
        public ActionResult Edit(int id)
        {
            TipoViolazione violazione = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // query per selezionare la violazione con l'ID passato come parametro
                string sql = "SELECT * FROM TipoViolazione WHERE IDviolazione = @IDviolazione";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IDviolazione", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        violazione = new TipoViolazione
                        {
                            IDviolazione = Convert.ToInt32(reader["IDviolazione"]),
                            Descrizione = reader["Descrizione"].ToString()
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }

            if (violazione == null)
            {
                return HttpNotFound();
            }

            return View(violazione);
        }

        // POST: Violazioni/Edit/5
        [HttpPost]
        public ActionResult Edit(TipoViolazione violazione)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // query per aggiornare la violazione con l'ID passato come parametro
                    string sql = "UPDATE TipoViolazione SET Descrizione = @Descrizione WHERE IDviolazione = @IDviolazione";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@IDviolazione", violazione.IDviolazione);
                    cmd.Parameters.AddWithValue("@Descrizione", violazione.Descrizione);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                        return View(violazione);
                    }
                }

                return RedirectToAction("Index");
            }


            return View(violazione);
        }


    }
}