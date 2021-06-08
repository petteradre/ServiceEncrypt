using EncryptSiemens;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using System.Text.Json;

namespace ServiceEncrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            // constante de conexion para la base de datos
            const string CONNECTION = "Server=162.241.62.197;Database=mrchamba_serac; Uid=mrchamba_serac;Pwd=Ed,#0.MXNPA^;";
            //Instanciar la nueva conexion
            MySqlConnection dbConnection = new MySqlConnection(CONNECTION);
            try
            {
                // abrir conexion
                dbConnection.Open();
                //Queri para traer informacion de la base de datos
                string sqlCommand = "SELECT * FROM serac_usuarios";
                // inicializar mysqladaptter para la consulta a la base de datos
                MySqlDataAdapter adpt = new MySqlDataAdapter(sqlCommand, dbConnection);
                //convertir a texto e comando para que sea reconocido por la base de datos
                adpt.SelectCommand.CommandType = CommandType.Text;
                // instanciar la consulta a la base de datos y donde se guarda la informacion de la consulta
                DataTable dt= new DataTable();
                adpt.Fill(dt);
                //hacer un ciclo para la consulta de tabla especifica
                foreach (DataRow userData in dt.Rows)
                {

                    Console.WriteLine(userData["nombre_usuario"].ToString());
                }
                // Cierre de conexion con la base de datos
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                // si la conexion no es exitosa manda exepcion 
                Console.WriteLine(ex.Message);
            }
            var datosJson = new Siemens()
            { CustomerNumberSAP = "0041003430", ProductoId = "GSL2000", PartNumber = "A7B10001013558", NetExistence = 5, StoreLocation = "NL", StoreName = "GSL", DateExtraction = "2021-03-26" };
            
            string json = JsonSerializer.Serialize(datosJson);
            string Concatenado = "[" + json + "]";
            Console.WriteLine(Concatenado);

            byte[] bytKey = { 100, 177, 198, 142, 169, 119, 213, 236, 197, 202, 250, 7, 31, 2, 220, 36, 118, 210, 234, 46, 29, 173, 131, 214, 133, 239, 11, 99, 93, 239, 252, 189 };

            byte[] bytIV = { 49, 114, 220, 162, 44, 197, 112, 102, 228, 169, 68, 227, 178, 250, 26, 40 };

            byte[] bytData = Encoding.ASCII.GetBytes(json);

            byte[] MyEncryptedData = Encryption.EncryptToBytes_AesCustomer(bytData, bytKey, bytIV);

            //Console.WriteLine(Encoding.Default.GetString(MyEncryptedData));
            // var EncriiptedString = Encoding.Default.GetString(MyEncryptedData);
            var baseCadena64 = System.Convert.ToBase64String(MyEncryptedData);
            Console.WriteLine(baseCadena64);
        }

        public class Siemens
        {
            public string CustomerNumberSAP { get; set; }
            public string ProductoId { get; set; }
            public string PartNumber { get; set; }
            public int NetExistence { get; set; }
            public string StoreLocation { get; set; }
            public string StoreName { get; set; }
            public string DateExtraction { get; set; }
        }
    }
    
}
