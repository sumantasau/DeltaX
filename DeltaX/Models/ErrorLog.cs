using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace DeltaX.Models
{
    public class ErrorLog
    {
        #region WRITE ERROR LOG IN DB
        public static DataSet WriteError(string strMessage)
        {

            // SqlConnection that will be used to execute the sql commands
            SqlConnection connection = null;
            DataSet ds = null;
            try
            {
                try
                {
                    connection = SqlHelper.GetConnection(0);
                }
                catch
                {
                    throw new Exception("The connection with the database can´t be established");
                }

                // Set up parameter 
                SqlParameter[] arParms = new SqlParameter[1];


                arParms[0] = new SqlParameter("@pMessage", SqlDbType.VarChar, 1500);
                arParms[0].Value = strMessage;


                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                // We pass in database connection string, command type, stored procedure name and SqlParameter          
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_AddExceptionErrorLogData", arParms);


            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                ErrorLog.WriteErrorLog("ErrorLog.cs - WriteError() - " + errMessage.ToString());
                throw new Exception("ErrorLog.cs - WriteError() - " + errMessage.ToString());
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }

            return ds;
        }
        #endregion

        #region WRITE ERROR LOG IN FILE ONLY FOR ABOVE WriteError() METHOD EXCEPTION
        private static void WriteErrorLog(string errorMessage)
        {
            try
            {
                string dirroot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFolder");

                if (!Directory.Exists(dirroot + "/Error"))  // if it doesn't exist, create
                    Directory.CreateDirectory(dirroot + "/Error");

                string filepath = "Error/" + DateTime.Today.ToString("dd-MM-yy") + ".Config";
                if (!File.Exists(Path.Combine(dirroot, filepath)))
                {
                    File.Create(Path.Combine(dirroot, filepath)).Close();
                }

                using (StreamWriter w = File.AppendText(Path.Combine(dirroot, filepath)))
                {
                    w.WriteLine("\r\nLog Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error in: " + Path.Combine(dirroot, filepath) +
                                  ". Error Message:" + errorMessage;
                    w.WriteLine(err);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();

                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteError(ex.Message);
            }
        }
        #endregion
    }
}