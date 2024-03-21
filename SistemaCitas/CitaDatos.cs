using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SistemaCitas
{
    class CitaDatos
    {

        public int Id_Cita { get; set; } //0
        public string Nombre { get; set; }//1
        public string Apellido { get; set; }//2
        public string Cedula { get; set; }//3
        public string Detalle { get; set; }//4
        public DateTime Fecha { get; set; }//5
        public int Id_User { get; set; }//6


        SqlConnection connect = new SqlConnection(@"Data Source=ELIAN-D-;Initial Catalog=Sistema_Citas;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        public List<CitaDatos> ListarCitas() 
        {

            List<CitaDatos> listadata = new List<CitaDatos>();

            if (connect.State != ConnectionState.Open) 
            {

                try 
                {

                    connect.Open();

                    string selecData = "SELECT * FROM Citas";

                    using(SqlCommand cmd = new SqlCommand(selecData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read()) 
                        { 
                            CitaDatos cd = new CitaDatos();

                            cd.Id_Cita = (int)reader["Id_Cita"];
                            cd.Nombre = reader["Nombre"].ToString();
                            cd.Apellido = reader["Apellido"].ToString();
                            cd.Cedula = reader["Cedula"].ToString();
                            cd.Detalle = reader["Detalle_Cita"].ToString();
                            cd.Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha"));
                            cd.Id_User = (int)reader["Id_User"];

                            listadata.Add(cd);
                            
                        }
                    }

                }
                catch(Exception ex) 
                {
                    Console.WriteLine("Error:" + ex);
                }
                finally 
                { 
                    connect.Close(); 
                }

                
            }

            return listadata;
        }
    }
}
