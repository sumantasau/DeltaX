using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DeltaX.Models
{
    public class clsProducer 
    {
        #region DECLARE VARIABLE

        private Nullable<int> _iProducerId = null;
        private string _strProducerName = null;
        private string _strSex = null;
        private string _strBio = null;
        private string _strDOBView = null;
        private Nullable<DateTime> _DOB = null;



        public IEnumerable<clsProducer> lstProducer { get; set; }
        public clsProducer CollectionclsProducer { get; set; }

        #endregion

        #region DECLARATION OF PROPERTY

        public Nullable<int> ProducerId
        {
            get { return this._iProducerId; }
            set { this._iProducerId = value; }
        }
        public string strProducerName
        {
            get { return this._strProducerName; }
            set { this._strProducerName = value; }
        }
        public string strSex
        {
            get { return this._strSex; }
            set { this._strSex = value; }
        }

        public string strBio
        {
            get { return this._strBio; }
            set { this._strBio = value; }
        }
        public string strDOBView
        {
            get { return this._strDOBView; }
            set { this._strDOBView = value; }
        }

        public Nullable<DateTime> DOB
        {
            get { return this._DOB; }
            set { this._DOB = value; }
        }
        
        #endregion

        #region ADD PRODUCER
        public decimal ProducerAdd()
        {
            SqlConnection connection = null;
            decimal retval = 0;
            try
            {

                // SqlConnection that will be used to execute the sql commands    
                try
                {
                    connection = SqlHelper.GetConnection();
                }
                catch
                {
                    throw new Exception("The connection with the database can´t be established");
                }

                SqlParameter[] arParam = new SqlParameter[4];

                arParam[0] = new SqlParameter("@pProducerName", SqlDbType.VarChar, 75);
                arParam[0].Value = _strProducerName;

                arParam[1] = new SqlParameter("@pSEX", SqlDbType.Char, 1);
                arParam[1].Value = _strSex;

                arParam[2] = new SqlParameter("@pDOB", SqlDbType.DateTime);
                arParam[2].Value = _DOB;

                arParam[3] = new SqlParameter("@pBio", SqlDbType.NVarChar);
                arParam[3].Value = _strBio;             

               

                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                // We pass in database connection string, command type, stored procedure name and SqlParameter          
                retval = Convert.ToDecimal(SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, "USP_AddProducer", arParam));

            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                ErrorLog.WriteError("--clsProducer.cs - ProducerAdd-" + errMessage.ToString());
            }

            return retval;
        }
        #endregion

        #region DISPLAY PRODUCER LIST
        public List<clsProducer> GetProducerList()
        {
            DataSet ds = null;
            DataTable dt = null;
            SqlConnection connection = null;

            try
            {

                // SqlConnection that will be used to execute the sql commands    
                try
                {
                    connection = SqlHelper.GetConnection();
                }
                catch
                {
                    throw new Exception("The connection with the database can´t be established");
                }

                SqlParameter[] arParam = new SqlParameter[1];

                arParam[0] = new SqlParameter("@pProducerId", SqlDbType.BigInt);
                arParam[0].Value = _iProducerId;

                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                // We pass in database connection string, command type, stored procedure name and SqlParameter          
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_GetProducerList", arParam);
                dt = ds.Tables[0];

                List<clsProducer> listObj = new List<clsProducer>();

                foreach (DataRow dr in dt.Rows)
                {
                    listObj.Add(new clsProducer
                    {
                        ProducerId = Convert.ToInt32(dr["ProducerId"]),
                        strProducerName = Convert.ToString(dr["ProducerName"]),
                        strSex = Convert.ToString(dr["Sex"]),
                        strDOBView = Convert.ToString(dr["DOB"]),
                        strBio = Convert.ToString(dr["Bio"]),
                    });
                }

                lstProducer = (IEnumerable<clsProducer>)listObj;

            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                ErrorLog.WriteError("- clsProducer.cs -- GetProducerList -- " + errMessage);

            }
            return lstProducer.ToList();
        }
        #endregion
    }
}