

using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Continental.v2.Classes
{

    public class ConnectionTraza
    {
        #region Print
        public static SerialPrintModel GetInformationSerial(string serial)
        {
            serial = serial.ToUpper();
            SerialPrintModel label = new SerialPrintModel();

            string oracleConn = "Data Source= tqdb002x.tq.mx.conti.de:1521/tqtrazapdb.tq.mx.conti.de; User Id=consulta; Password= solover";

            string oracleConnNpg = "Username =logistica; Password= logistica; Host= 10.218.108.252; Port = 5432; Database = iip";

            // string NpgQuery = "select MLFB, big_box, small_box, serial,timedate from units where big_box='459535432Q9' and timedate >='2018%';";
            string NpgQuery = $"select distinct MLFB, big_box from units where  big_box = '{serial}' and timedate >= '2018%'";

            #region Connection Traza
            string query = $"SELECT MLFB, Serial from units WHERE SERIAL = '{serial}'"; //9974S432B1136138

            // string query = $"SELECT MLFB, Serial, ordernumber, datetimestamp from ETGDL.boxes WHERE SERIAL = '{serial}'"; //9974S432B1136138

            // string query2 = $"SELECT aunitsperbox * aboxperpallet FROM ETGDL.products WHERE MLFB = (Select MLFB from ETGDL.boxes WHERE SERIAL = '{serial}')";
            string delimStr = "_ -";
            char[] delimiter = delimStr.ToCharArray();
            int start = 0;

            using (NpgsqlConnection connectioNpg = new NpgsqlConnection(oracleConnNpg))
            {
                NpgsqlCommand command = new NpgsqlCommand(NpgQuery, connectioNpg);
                connectioNpg.Open();
                NpgsqlDataReader reader = command.ExecuteReader();

                Console.WriteLine(delimiter);
                while (reader.Read())
                {
                    string str = reader.GetString(0).ToString();
                    for (int i = 0; i < delimiter.Length; i++)
                    {
                        start = str.IndexOf(delimiter[i]);
                        if (start != -1)
                            break;
                    }

                    //int end = str.Length -1;
                    //int start = str.IndexOf('_');
                    if (start == -1)
                    {
                        label.MLFB = reader.GetString(0).ToString();
                        label.Serial = reader.GetString(1).ToString();
                    }
                    else
                    {
                        string h = str.Substring(0, start);
                        label.MLFB = h;
                        label.Serial = reader.GetString(1).ToString();
                    }

                }
                reader.Close();
                connectioNpg.Close();
            }
            string query2 = $"SELECT aunitsperbox * aboxperpallet FROM ETGDL.products WHERE MLFB = '{label.MLFB}'";
            using (OracleConnection connection = new OracleConnection(oracleConn))
            {

                connection.Open();
                OracleCommand command2 = new OracleCommand(query2, connection);

                OracleDataReader reader2 = command2.ExecuteReader();

                while (reader2.Read())
                {
                    //label.Quantity = reader2.GetOracleValue(0);
                    var uno = reader2.GetOracleValue(0).ToString();
                    label.Quantity = int.Parse(uno);

                }
                Console.WriteLine("Label: " + label);
                // reader.Close();
                reader2.Close();
                connection.Close();
            }

            #endregion


            #region Cambiar por la version en el server de traza
            //using (OracleConnection connection = new OracleConnection(oracleConn))
            //{
            //    OracleCommand command = new OracleCommand(queryString, connection);
            //    connection.Open();
            //    OracleDataReader reader = command.ExecuteReader();
            //    try
            //    {
            //        while (reader.Read())
            //        {
            //            label.MLFB = reader.GetValue(0).ToString();
            //            label.Order = reader.GetValue(1).ToString();
            //            label.Quantity = 80;
            //            label.Serial = reader.GetValue(2).ToString();
            //            label.PackingType = "Tarima";
            //        }
            //    }
            //    finally
            //    {
            //        // always call Close when done reading.
            //        reader.Close();
            //    } 

            //}
            #endregion

            #region Borrar
            //SerialPrintModel labelTest = new SerialPrintModel();
            //labelTest.Serial = "1234567890";
            //labelTest.MLFB = "A2C1781540095";
            //labelTest.Quantity = 100;
            //labelTest.PackingType = "Tarima";

            //return labelTest;
            #endregion

            return label;

        }
        
        #endregion
    }

    public class SerialPrintModel
    {
        public string Serial { get; set; }

        public string MLFB { get; set; }

        public int Quantity { get; set; }

        public string PackingType { get; set; }

        public string Order { get; set; }

    }

}
