using GestioneContravvenzioni.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace GestioneContravvenzioni.Controllers
{
    public class HomeController : Controller
    {

        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connStringDb"].ConnectionString;

        public ActionResult Index()
        {
            //  crea una connessione al database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  apre la connessione e leggi i dati
                conn.Open();

                // Totale Verbali
                string sql = "SELECT COUNT(*) FROM Verbale";
                SqlCommand cmd = new SqlCommand(sql, conn);
                ViewBag.TotalVerbali = (int)cmd.ExecuteScalar();

                // Totale Punti Decurtati
                sql = "SELECT SUM(DecurtamentoPunti) FROM Verbale";
                cmd = new SqlCommand(sql, conn);
                ViewBag.TotalPuntiDecurtati = (int?)cmd.ExecuteScalar() ?? 0;

                // Verbali con > 10 Punti Decurtati
                sql = "SELECT COUNT(*) FROM Verbale WHERE DecurtamentoPunti > 10";
                cmd = new SqlCommand(sql, conn);
                ViewBag.VerbaliOltre10Punti = (int)cmd.ExecuteScalar();

                // Verbali con Importo > 400 Euro
                sql = "SELECT COUNT(*) FROM Verbale WHERE Importo > 400";
                cmd = new SqlCommand(sql, conn);
                ViewBag.VerbaliOltre400Euro = (int)cmd.ExecuteScalar();
            }
            //ritorna vista
            return View();
        }

        public ActionResult Verbali()
        {
            //crea una lista di verbali vuota
            List<Anagrafica> anagrafiche = new List<Anagrafica>();

            //crea una connessione al database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //crea una query per selezionare tutte le anagrafiche
                //e il totale dei verbali
                string sql = @"
                SELECT A.IDanagrafica, A.Cognome, A.Nome, COUNT(V.IDverbale) AS TotVerbali
                FROM Anagrafica A
                JOIN  Verbale V ON A.IDanagrafica = V.IDanagrafica_FK
                GROUP BY A.IDanagrafica, A.Cognome, A.Nome";

                SqlCommand cmd = new SqlCommand(sql, conn);

                //apre la connessione e leggi i dati    
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
                            TotVerbali = Convert.ToInt32(reader["TotVerbali"])
                        });

                    }
                }
                //gestione eccezioni 
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }

            return View(anagrafiche);
        }

        public ActionResult Punti()
        {
            //  crea una lista vuota
            List<Anagrafica> anagrafiche = new List<Anagrafica>();

            //  crea una connessione al database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  crea una query per selezionare tutte le anagrafiche
                //  e il totale dei punti decurtati
                string sql = @"
                SELECT A.IDanagrafica, A.Cognome, A.Nome, SUM(V.DecurtamentoPunti) AS TotPunti
                FROM Anagrafica A
                JOIN Verbale V ON A.IDanagrafica = V.IDanagrafica_FK
                GROUP BY A.IDanagrafica, A.Cognome, A.Nome";

                SqlCommand cmd = new SqlCommand(sql, conn);

                //  apre la connessione e leggi i dati
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
                            TotVerbali = Convert.ToInt32(reader["TotPunti"])
                        });
                    }
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }


                return View(anagrafiche);
            }
        }


        public ActionResult Verbali_Oltre10Punti()
        {
            //  crea una lista vuota
            List<Verbale> verbali = new List<Verbale>();

            //  crea una connessione al database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  crea una query per selezionare i verbali con più di 10 punti decurtati
                string sql = @"
                SELECT V.IDverbale, A.Cognome, A.Nome, V.DataViolazione, V.Importo, V.DecurtamentoPunti
                FROM Verbale V
                JOIN Anagrafica A ON V.IDanagrafica_FK = A.IDanagrafica
                WHERE V.DecurtamentoPunti > 10";

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
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                            Importo = Convert.ToDecimal(reader["Importo"]),
                            DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"])
                        });
                    }
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }


                return View(verbali);
            }
        }

        public ActionResult Verbali_Oltre400Euro()
        {
            //  crea una lista vuota
            List<Verbale> verbali = new List<Verbale>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //  crea una query per selezionare i verbali con importo superiore a 400 euro
                string sql = @"
                    SELECT V.IDverbale, A.Cognome, A.Nome, V.DataViolazione, V.Importo, V.DecurtamentoPunti
                    FROM Verbale V
                    JOIN Anagrafica A ON V.IDanagrafica_FK = A.IDanagrafica
                    WHERE  V.Importo > 400";

                SqlCommand cmd = new SqlCommand(sql, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        verbali.Add(new Verbale
                        {
                            IDverbale = Convert.ToInt32(reader["IDverbale"]),
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                            Importo = Convert.ToDecimal(reader["Importo"]),
                            DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"])
                        });
                    }
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }


            return View(verbali);
        }
    }
}
