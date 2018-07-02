using DeltaX.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Linq;


namespace DeltaX.Controllers
{
    [CustomError]  
    [RoutePrefix("Movie")]
    public class MovieController : Controller
    {
        private List<KeyValuePair<string, string>> _modelState;
        public clsMovie objBLL = null;
        public clsProducer objProducerBLL = null;
        public clsActor objActorBLL = null;       
        public IEnumerable<clsMovie> objView = null;
        public IEnumerable<clsProducer> objPrdView = null;
        public IEnumerable<clsActor> objActorView = null;
        public clsMovie objModel = null;
        public clsProducer objProducerModel = null;
        public clsActor objActorModel = null;
        DataSet ds = null;

        private DataTable dtActorList = new DataTable("ActorList");
        
        //
        // GET: /Movie/
        //[Route("Index")]
        public ActionResult Index()
        {
            var objAuth = new clsMovie();
            objBLL = new clsMovie();
            objActorBLL = new clsActor();           
            objView = objBLL.GetMovieList();
            objPrdView = objBLL.GetProducerList();
            objActorView = objActorBLL.GetActorList();          

            objModel = new clsMovie
            {
                lstMovie = objView,
                lstProducer = objPrdView,
                lstActor = objActorView,
                CollectionclsMovie = null,
                CollectionActor = null
            };
           
            return View(objModel);
        }

        #region FOR ADD/UPDATE MOVIE
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult Index(clsMovie objForm = null, string SubmitButton = null, HttpPostedFileBase Photo = null)
        {
            
                Boolean s_IsValid = true;
                Nullable<DateTime> releaseDt = null;
                string strActorName = string.Empty;
                string strActorId = string.Empty;
                string destinationPath = string.Empty;

                _modelState = new List<KeyValuePair<string, string>>();

                DataSet dsActorList = new DataSet();

                //ADD COLUMN TO A TABLE            
                DataColumn ActorId = new DataColumn("ActorId", typeof(System.Int32));
                dtActorList.Columns.Add(ActorId);

                if ((string.Compare(SubmitButton, "Add", false) == 0) || (string.Compare(SubmitButton, "Update", false) == 0))
                {

                    decimal retval = 0;

                    #region VALIDATION FOR ADD/EDIT
                    if (objForm.CollectionclsMovie.strMovieName == null)
                    {
                        s_IsValid = false;
                        TempData["ErrorMsg"] = "Please enter movie name";
                    }
                    else if (objForm.CollectionclsMovie.ProducerId == null)
                    {
                        s_IsValid = false;
                        TempData["ErrorMsg"] = "Please select producer";
                    }
                    else if (objForm.CollectionActor.strActorId == null)
                    {
                        s_IsValid = false;
                        TempData["ErrorMsg"] = "Please select actor";
                    }
                    else if (objForm.CollectionclsMovie.strReleaseDate == null)
                    {
                        s_IsValid = false;
                        TempData["ErrorMsg"] = "Please select movie release date";
                    }
                    else if (objForm.CollectionclsMovie.strMoviePlot == null)
                    {
                        s_IsValid = false;
                        TempData["ErrorMsg"] = "Please enter movie release plot";
                    }
                    else if (string.Compare(SubmitButton, "Add", false) == 0)
                    {
                        if (Photo == null)
                        {
                            s_IsValid = false;
                            TempData["ErrorMsg"] = "Please select movie poster";
                        }
                    }

                    if ((objForm.CollectionActor.strActorId != null))
                    {
                        strActorId = Convert.ToString(objForm.CollectionActor.strActorId);
                    }
                    if (objForm.CollectionclsMovie.strReleaseDate != null)
                    {
                        releaseDt = Convert.ToDateTime(DateTime.ParseExact(objForm.CollectionclsMovie.strReleaseDate.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture));
                    }
                    #endregion

                    if (!s_IsValid)
                    {
                        foreach (var items in _modelState)
                        {
                            ModelState.AddModelError(items.Key, items.Value);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strActorId))
                        {
                            string[] arrActor = strActorId.Split(',');
                            for (int j = 0; j < arrActor.Length; j++)
                            {
                                if (arrActor[j] != "")
                                {
                                    ActorList(Convert.ToInt32(arrActor[j]));
                                }
                            }
                        }

                        if (dtActorList != null)
                        {
                            dsActorList.Tables.Add(dtActorList);
                        }


                        objBLL = new clsMovie();
                        objBLL.strMovieName = objForm.CollectionclsMovie.strMovieName.Trim();
                        objBLL.ReleaseYear = releaseDt;
                        objBLL.ProducerId = objForm.CollectionclsMovie.ProducerId;
                        objBLL.strMoviePlot = objForm.CollectionclsMovie.strMoviePlot;
                        objBLL._dsActorList = dsActorList;

                        if (string.Compare(SubmitButton, "Add", false) == 0)
                        {
                            objBLL.strMoviePoster = Photo.FileName.Replace(" ", "_");
                            retval = objBLL.MovieAdd();
                            if (retval > 0)
                            {
                                if (Photo != null)
                                {
                                    destinationPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(WebConfigurationManager.AppSettings.Get("Movie_Poster_FilePath")), Photo.FileName.Replace(" ", "_"));
                                    Photo.SaveAs(destinationPath);
                                }
                                TempData["ErrorMsg"] = "Movie has been added successfully";
                                return RedirectToAction("Index");
                            }
                            else if (retval == -1)
                            {
                                TempData["ErrorMsg"] = "Movie name with same release year already exists";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["ErrorMsg"] = "System Error";
                                return RedirectToAction("Index");
                            }
                        }
                        else if (string.Compare(SubmitButton, "Update", false) == 0)
                        {
                            objBLL.MovieId = objForm.CollectionclsMovie.MovieId;
                            if (Photo != null)
                            {
                                objBLL.strMoviePoster = Photo.FileName.Replace(" ", "_");
                            }
                            else
                            {
                                objBLL.strMoviePoster = objForm.CollectionclsMovie.strMoviePoster;
                            }
                            retval = objBLL.MovieUpdate();

                            if (retval == 1)
                            {
                                if (Photo != null)
                                {
                                    destinationPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(WebConfigurationManager.AppSettings.Get("Movie_Poster_FilePath")), Photo.FileName.Replace(" ", "_"));
                                    Photo.SaveAs(destinationPath);
                                }
                                TempData["ErrorMsg"] = "Movie has been updated successfully";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["ErrorMsg"] = "System Error";
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }

                var objAuth = new clsMovie();
                objBLL = new clsMovie();
                objActorBLL = new clsActor();
                objView = objBLL.GetMovieList();
                objPrdView = objBLL.GetProducerList();
                objActorView = objActorBLL.GetActorList();

                objModel = new clsMovie
                {
                    lstMovie = objView,
                    lstProducer = objPrdView,
                    lstActor = objActorView,
                    //CollectionclsMovie = objAuth
                    CollectionclsMovie = null,
                    CollectionActor = null
                };
                return View(objModel);
            
        }

        #endregion

        #region EDIT MOVIE
        public ActionResult Edit(string id)
        {
            try
            {
                objModel = new clsMovie();
                objBLL = new clsMovie();
                objActorBLL = new clsActor(); 

                objView = objBLL.GetMovieList();

                objPrdView = objBLL.GetProducerList();
                objActorView = objActorBLL.GetActorList();      

                var objAuth = new clsMovie();
                if (id != null)
                {
                    objAuth = objView.Where(c => c.MovieId == Convert.ToInt32(id)).FirstOrDefault();
                }

                objModel = new clsMovie
                {                  
                    lstMovie = objView,
                    lstProducer = objPrdView,
                    lstActor = objActorView,
                    CollectionclsMovie = objAuth,
                    CollectionActor = objAuth

                     //CollectionclsMovie = null,
                    //CollectionActor = null
                };
                
                return View("Index", objModel);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region ADD PRODUCER
        [HttpPost]
        public ActionResult AddProducer(string ProducerName,  string Sex, string DOB, string Bio)
        {
            decimal retval = 0;
            objProducerBLL = new clsProducer();
            objProducerBLL.strProducerName = ProducerName;
            objProducerBLL.strSex = Sex;
            objProducerBLL.strBio = Bio;
            if (DOB != null)
            {
                objProducerBLL.DOB = Convert.ToDateTime(DateTime.ParseExact(DOB.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture));
            }

            retval = objProducerBLL.ProducerAdd();

            objPrdView = objProducerBLL.GetProducerList();

            objProducerModel = new clsProducer
            {               
                lstProducer = objPrdView              
            };

            return Json(objProducerModel);
        }
        #endregion

        #region ADD ACTOR
        [HttpPost]
        public ActionResult AddActor(string ActorName, string Sex, string DOB, string Bio)
        {
            decimal retval = 0;
            objActorBLL = new clsActor();
            objActorBLL.strActorName = ActorName;
            objActorBLL.strActorSex = Sex;
            objActorBLL.strActorBio = Bio;
            if (DOB != null)
            {
                objActorBLL.ActorDOB = Convert.ToDateTime(DateTime.ParseExact(DOB.Replace("/", "-"), "dd-MM-yyyy", CultureInfo.InvariantCulture));
            }

            retval = objActorBLL.ActorAdd();

            objActorView = objActorBLL.GetActorList();

            objActorModel = new clsActor
            {
                lstActor = objActorView
            };

            return Json(objActorModel);
        }
        #endregion


        #region CREATE A TABLE FOR MULTIPLE ACTOR ID ENTRY
        private void ActorList(int ActorId)
        {
            DataRow dr = dtActorList.NewRow();
            dr["ActorId"] = ActorId;           
            dtActorList.Rows.Add(dr);
        }
        #endregion
	}


    
}