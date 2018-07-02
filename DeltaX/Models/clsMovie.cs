using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DeltaX.Models
{
    public class clsMovie : clsProducer
    {
        #region DECLARE VARIABLE

        private Nullable<int> _iMovieId = null;
        private string _strMovieName = null;
        private string _strMoviePlot = null;
        private string _strMoviePoster = null;
        private string _strReleaseYearView = null;
        private string _strReleaseDate = null;
        private Nullable<DateTime> _ReleaseYear = null;

        public decimal RowNum { get; set; }
        public DataSet _dsActorList { get; set; }

        //private string _strProducerName = null;

        public IEnumerable<clsMovie> lstMovie { get; set; }
        //public IEnumerable<clsProducer> lstProducer { get; set; }

        //public int ActorId { get; set; }
        public string strActorId { get; set; }
        public string strActorName { get; set; }
        public IEnumerable<clsActor> lstActor { get; set; }

        public clsMovie CollectionclsMovie { get; set; }
        public clsMovie CollectionActor { get; set; }

        #endregion

        #region DECLARATION OF PROPERTY

        public Nullable<int> MovieId
        {
            get { return this._iMovieId; }
            set { this._iMovieId = value; }
        }
        public string strMoviePlot
        {
            get { return this._strMoviePlot; }
            set { this._strMoviePlot = value; }
        }
        public string strMoviePoster
        {
            get { return this._strMoviePoster; }
            set { this._strMoviePoster = value; }
        }

        public string strMovieName
        {
            get { return this._strMovieName; }
            set { this._strMovieName = value; }
        }
        public string strReleaseYearView
        {
            get { return this._strReleaseYearView; }
            set { this._strReleaseYearView = value; }
        }
        public string strReleaseDate
        {
            get { return this._strReleaseDate; }
            set { this._strReleaseDate = value; }
        }

        public Nullable<DateTime> ReleaseYear
        {
            get { return this._ReleaseYear; }
            set { this._ReleaseYear = value; }
        }
        
        //public string strProducerName
        //{
        //    get { return this._strProducerName; }
        //    set { this._strProducerName = value; }
        //}
        #endregion

        #region ADD MOVIE
        public decimal MovieAdd()
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
                
                SqlParameter[] arParam = new SqlParameter[6];

                arParam[0] = new SqlParameter("@pMovieName", SqlDbType.VarChar, 75);
                arParam[0].Value = _strMovieName;

                arParam[1] = new SqlParameter("@pPlot", SqlDbType.NVarChar);
                arParam[1].Value = _strMoviePlot;

                arParam[2] = new SqlParameter("@pPoster", SqlDbType.VarChar, 100);
                arParam[2].Value = _strMoviePoster;

                arParam[3] = new SqlParameter("@pReleaseYear", SqlDbType.DateTime);
                arParam[3].Value = _ReleaseYear;

                arParam[4] = new SqlParameter("@pProducerId", SqlDbType.BigInt);
                arParam[4].Value = ProducerId;

                arParam[5] = new SqlParameter("@pActorList", SqlDbType.VarChar, 3000);
                arParam[5].Value = _dsActorList.GetXml();   

                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                // We pass in database connection string, command type, stored procedure name and SqlParameter          
                retval = Convert.ToDecimal(SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, "USP_AddMovie", arParam));
                
            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                ErrorLog.WriteError("--clsMovie.cs - MovieAdd-" + errMessage.ToString());
            }

            return retval;
        }
        #endregion

        #region UPDATE MOVIE
        public decimal MovieUpdate()
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

                SqlParameter[] arParam = new SqlParameter[7];

                arParam[0] = new SqlParameter("@pMovieName", SqlDbType.VarChar, 75);
                arParam[0].Value = _strMovieName;

                arParam[1] = new SqlParameter("@pPlot", SqlDbType.NVarChar);
                arParam[1].Value = _strMoviePlot;

                arParam[2] = new SqlParameter("@pPoster", SqlDbType.VarChar, 100);
                arParam[2].Value = _strMoviePoster;

                arParam[3] = new SqlParameter("@pReleaseYear", SqlDbType.DateTime);
                arParam[3].Value = _ReleaseYear;

                arParam[4] = new SqlParameter("@pProducerId", SqlDbType.BigInt);
                arParam[4].Value = ProducerId;

                arParam[5] = new SqlParameter("@pActorList", SqlDbType.VarChar, 3000);
                arParam[5].Value = _dsActorList.GetXml();

                arParam[6] = new SqlParameter("@pMovieId", SqlDbType.BigInt);
                arParam[6].Value = _iMovieId;



                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                // We pass in database connection string, command type, stored procedure name and SqlParameter          
                retval = Convert.ToDecimal(SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, "USP_UpdateMovie", arParam));

            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                ErrorLog.WriteError("--clsMovie.cs - MovieUpdate-" + errMessage.ToString());
            }

            return retval;
        }
        #endregion

        #region DISPLAY MOVIE LIST
        public IEnumerable<clsMovie> GetMovieList()
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

                arParam[0] = new SqlParameter("@pMovieId", SqlDbType.BigInt);
                arParam[0].Value = _iMovieId;
                
                // Call ExecuteDataset static method of SqlHelper class that returns a Dataset
                // We pass in database connection string, command type, stored procedure name and SqlParameter          
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_GetMovieList", arParam);
                dt = ds.Tables[0];

                List<clsMovie> listObj = new List<clsMovie>();

                foreach (DataRow dr in dt.Rows)
                {
                    listObj.Add(new clsMovie
                    {
                        MovieId = Convert.ToInt32(dr["MovieId"]),
                        strMovieName = Convert.ToString(dr["MovieName"]),
                        strReleaseYearView = Convert.ToString(dr["MovieReleaseYear"]),
                        strReleaseDate = Convert.ToString(dr["ReleaseYear"]),                        
                        strMoviePlot = Convert.ToString(dr["Plot"]),
                        strMoviePoster = Convert.ToString(dr["Poster"]),
                        ProducerId = Convert.ToInt32(dr["ProducerId"]),
                        strProducerName = Convert.ToString(dr["ProducerName"]),
                        strActorId = Convert.ToString(dr["strActorId"]),  
                        RowNum = Convert.ToInt32(dr["RowNum"]),    
                    });
                }

                lstMovie = (IEnumerable<clsMovie>)listObj;

            }
            catch (Exception ex)
            {
                string errMessage = "";
                for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
                {
                    errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
                }
                ErrorLog.WriteError("- clsMovie.cs -- GetMovieList -- " + errMessage);
                
            }
            return lstMovie;
        }
        #endregion
    }
}