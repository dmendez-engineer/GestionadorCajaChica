using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
namespace GestionadorCajaChica.DB
{
    public class CConexcion
    {
        NpgsqlConnection connection = new NpgsqlConnection();
        static String servidor="localhost";
        static String db= "CajaChicaDB";
        static String usuario= "postgres";
        static String password="admin123";
        static String port = "5432";

        String cadenaConexion = "server=" + servidor + ";" + "port=" + port + ";" +
            "user id=" + usuario + ";" + "password=" + password + ";" 
            + "database=" + db + ";";
        public NpgsqlConnection establecerConexcion()
        {
            try
            {
                connection.ConnectionString=cadenaConexion;
                connection.Open();
               // MessageBox.Show("Connection Succesfully");
            }catch(NpgsqlException ex)
            {
                MessageBox.Show("Failed in the connection: "+ex.Message);
            }
            return connection;
        }
    }
}
