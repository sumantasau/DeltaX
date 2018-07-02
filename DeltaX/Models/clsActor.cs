using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DeltaX.Models
{
    public class clsActor
    {
        #region DECLARE VARIABLE

        private Nullable<int> _iActorId = null;
        private string _strActorName = null;
        private string _strActorSex = null;
        private string _strActorBio = null;
        private string _strActorDOBView = null;
        private Nullable<DateTime> _ActorDOB = null;



        public IEnumerable<clsActor> lstActor { get; set; }
        public clsActor CollectionclsActor { get; set; }

        #endregion

        #region DECLARATION OF PROPERTY

        public Nullable<int> ActorId
        {
            get { return this._iActorId; }
            set { this._iActorId = value; }
        }
        public string strActorName
        {
            get { return this._strActorName; }
            set { this._strActorName = value; }
        }
        public string strActorSex
        {
            get { return this._strActorSex; }
            set { this._strActorSex = value; }
        }

        public string strActorBio
        {
            get { return this._strActorBio; }
            set { this._strActorBio = value; }
        }
        public string strActorDOBView
        {
            get { return this._strActorDOBView; }
            set { this._strActorDOBView = value; }
        }

        public Nullable<DateTime> ActorDOB
        {
            get { return this._ActorDOB; }
            set { this._ActorDOB = value; }
        }

        #endregion

        #region ADD ACTOR
        public decimal ActorAdd()
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

                arParam[0] = new SqlParameter("@pActorName", SqlDbType.VarChar, 75);
                arParam[0].Value = _strActorName;

                arParam[1] = new SqlParameter("@pSEX", SqlDbType.Char, 1);
                arParam[1].Value = _strActorSex;

                arParam[2] = new SqlParameter("@pDOB", SqlDbType.DateTime);
                arParam[2].Value = _ActorDOB;

                arParam[3] = new SqlParameter("@pBio", SqlDbType.NVarChar);
                arParam[3].Value = _strActorBio;



                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                // We pass in database connection string, command type, stored procedure name and SqlParameter          
                retval = Convert.ToDecimal(SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, "USP_AddActor", arParam));

            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                ErrorLog.WriteError("--clsActor.cs - ProducerAdd-" + errMessage.ToString());
            }

            return retval;
        }
        #endregion

        #region DISPLAY ACTOR LIST
        public List<clsActor> GetActorList()
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

                arParam[0] = new SqlParameter("@pActorId", SqlDbType.BigInt);
                arParam[0].Value = _iActorId;

                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                // We pass in database connection string, command type, stored procedure name and SqlParameter          
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_GetActorList", arParam);
                dt = ds.Tables[0];

                List<clsActor> listObj = new List<clsActor>();

                foreach (DataRow dr in dt.Rows)
                {
                    listObj.Add(new clsActor
                    {
                        ActorId = Convert.ToInt32(dr["ActorId"]),
                        strActorName = Convert.ToString(dr["ActorName"]),
                        strActorSex = Convert.ToString(dr["Sex"]),
                        strActorDOBView = Convert.ToString(dr["DOB"]),
                        strActorBio = Convert.ToString(dr["Bio"]),
                    });
                }

                lstActor = (IEnumerable<clsActor>)listObj;

            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                ErrorLog.WriteError("- clsActor.cs -- GetActorList -- " + errMessage);

            }
            return lstActor.ToList();
        }
        #endregion
    }
}