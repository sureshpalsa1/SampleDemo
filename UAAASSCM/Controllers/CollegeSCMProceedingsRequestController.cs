//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using UAAAS.Models;

//using DotNetOpenAuth.Messaging;
//using UAAAS.Controllers.Admin;

using UAAASSCM.Models;
using System.Threading.Tasks;
using UAAASSCM.Controllers;

namespace UAAAS.Controllers.College
{
    public class CollegeSCMProceedingsRequestController : Controller
    {
        private SCMEntities db = new SCMEntities();
        private string serverURL;
        //[Authorize(Roles = "College")]
        //[HttpGet]
        //public ActionResult CollegeScmProceedingsRequest()
        //{
        //    ScmProceedingsRequest scmProceedings=new ScmProceedingsRequest();
        //    string clgCode;
        //    var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
        //    var userCollegeId = db.jntuh_college_users.Where(u => u.userID == userId).Select(u => u.collegeID).FirstOrDefault();
        //    var firstOrDefault = db.jntuh_college.FirstOrDefault(a => a.id == userCollegeId);
        //    var specs = new List<DistinctSpecializations>();
        //    var depts = new List<DistinctDepartments>();
        //    var degrees = db.jntuh_degree.AsNoTracking().ToList();
        //    var specializations = db.jntuh_specialization.AsNoTracking().ToList();
        //    var departments = db.jntuh_department.AsNoTracking().ToList();
        //    //int[] collegespecs = new int[];
        //    List<int> collegespecs=new List<int>();
        //    collegespecs.AddRange(
        //        db.jntuh_college_intake_existing.Where(i => i.collegeId == userCollegeId)
        //            .Select(i => i.specializationId)
        //            .Distinct()
        //            .ToArray());

        //    //int[] degreeIds=(from a in db.jntuh_specialization join b in db.jntuh_department on a.departmentId equals b.id
        //    //               join c in db.jntuh_degree on b.degreeId equals c.id where collegespecs.Contains(a.id) select c.id).Distinct().ToArray();
        //    //if (degreeIds.Contains(4))
        //    //{
        //    //   var humanitesSpeci = new[] {37,48,42,31};
        //    //   collegespecs.AddRange(humanitesSpeci);
        //    //}



        //    foreach (var s in collegespecs)
        //    {
        //        var specid = specializations.FirstOrDefault(i => i.id == s);

        //        if (specid != null)
        //        {
        //            var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
        //            var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));
        //            if (degree != null)
        //                specs.Add(new DistinctSpecializations { SpecializationId = specid.id, SpecializationName = degree.degree + " - " + specid.specializationName, DepartmentId = specid.departmentId });
        //        }
        //    }


        //  //  if(specs.Contains())

        //    ViewBag.departments = specs.OrderBy(i => i.DepartmentId);

        //    var collegescmrequestslist = db.jntuh_scmproceedingsrequests.AsNoTracking().Where(i => i.CollegeId == userCollegeId).ToList();

        //    var proceedingsRequests = new List<ScmProceedingsRequest>();
        //    scmProceedings.ScmProceedingsRequestslist=new List<ScmProceedingsRequest>();
        //    foreach (var s in collegescmrequestslist)
        //    {

        //        //string cretedDate = string.Empty;
        //        //if (!string.IsNullOrEmpty(s.CreatedOn.ToString(CultureInfo.InvariantCulture)))
        //        //{
        //        //    cretedDate = UAAAS.Models.Utilities.MMDDYY2DDMMYY(cretedDate);
        //        //}
        //        var specid = specializations.FirstOrDefault(i => i.id == s.SpecializationId);

        //        if (specid != null && firstOrDefault !=null)
        //        {
        //            var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
        //            var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));
        //            if (degree != null)
        //               // if (s.RequiredProfessor != null)
        //                proceedingsRequests.Add(new ScmProceedingsRequest
        //                    {
        //                        CollegeName = firstOrDefault.collegeCode + " - " + firstOrDefault.collegeName,
        //                        CollegeCode = firstOrDefault.collegeCode,
        //                        ProfessorVacancies = s.ProfessorsCount.ToString(),
        //                        AssociateProfessorVacancies = s.AssociateProfessorsCount.ToString(),
        //                        AssistantProfessorVacancies = s.AssistantProfessorsCount.ToString(),
        //                        SpecializationName = degree.degree + " - " + specid.specializationName,
        //                        SpecializationId = specid.id,
        //                        CollegeId = firstOrDefault.id,
        //                        DepartmentId = specid.departmentId,
        //                        ScmNotificationpath = s.SCMNotification,
        //                        Id = s.ID,
        //                        RequiredProfessorVacancies = s.RequiredProfessor.ToString(),
        //                        RequiredAssistantProfessorVacancies = s.RequiredAssistantProfessor.ToString(),
        //                        RequiredAssociateProfessorVacancies = s.RequiredAssociateProfessor.ToString(),
        //                        CreatedDate = s.CreatedOn,
        //                        Checked = false
        //                    });
        //        }
        //    }
        //    scmProceedings.ScmProceedingsRequestslist.AddRange(proceedingsRequests.OrderByDescending(e=>e.CreatedDate).Select(e=>e).ToList());
        //  // ViewBag.collegescmrequestslist = proceedingsRequests;
        //    ViewBag.collegescmrequestslist = scmProceedings.ScmProceedingsRequestslist;


        //   // scmProceedings.ScmProceedingsRequestslist.AddRange(proceedingsRequests);
        //    return View(scmProceedings);
        //}



          [AuthorizedUserAccess("Admin")]
        public ActionResult CollegeScmProceedingsRequestView(int id)
        {
            //var userCollegeId = 0;
            //if (Roles.IsUserInRole("Admin"))
            //{
            //    if (id != null)
            //    {
            //        userCollegeId = Convert.ToInt32(UAAAS.Models.Utilities.DecryptString(id, WebConfigurationManager.AppSettings["CryptoKey"]));
            //    }
            //}
            if (User.Identity.IsAuthenticated)
          //  if (User.Identity.Name == "admin")
            {
                if (id != 0)
                {
                    var firstOrDefault = db.jntuh_college.FirstOrDefault(a => a.id == id);
                    var specs = new List<DistinctSpecializations>();
                    var degrees = db.jntuh_degree.AsNoTracking().ToList();
                    var specializations = db.jntuh_specialization.AsNoTracking().ToList();
                    var departments = db.jntuh_department.AsNoTracking().ToList();
                    var collegespecs =
                        db.jntuh_college_intake_existing.Where(i => i.collegeId == id)
                            .Select(i => i.specializationId)
                            .Distinct()
                            .ToArray();
                    foreach (var s in collegespecs)
                    {
                        var specid = specializations.FirstOrDefault(i => i.id == s);

                        if (specid != null)
                        {
                            var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
                            var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));
                            if (degree != null)
                                specs.Add(new DistinctSpecializations
                                {
                                    SpecializationId = specid.id,
                                    SpecializationName = degree.degree + " - " + specid.specializationName,
                                    DepartmentId = specid.departmentId
                                });
                        }
                    }
                    ViewBag.departments = specs.OrderBy(i => i.DepartmentId);
                    var collegescmrequestslist = db.jntuh_scmproceedingsrequests.AsNoTracking().Where(i => i.CollegeId == id && i.RequestSubmittedDate != null).ToList();

                    var nomineeAssignedScmIds = (from SCM in db.jntuh_scmproceedingsrequests
                                                 join AUDA in db.jntuh_auditor_assigned on SCM.ID equals AUDA.ScmId
                                                 where
                                                     SCM.CollegeId == id && SCM.SpecializationId != 0 && SCM.DEpartmentId != 0 &&
                                                     SCM.DegreeId != 0
                                                 select SCM.ID).Distinct().ToArray();


                    var jntuh_auditor_assigned = db.jntuh_auditor_assigned.AsNoTracking().ToList();
                    var jntuh_auditors_dataentry = db.jntuh_auditors_dataentry.AsNoTracking().ToList();
                    var proceedingsRequests = new List<ScmProceedingsRequest>();



                    var SplittedSCMIds =db.jntuh_scmproceedingsrequests.Where(e => e.ISActive == true && e.OldSCMId != null).Select(e => e.OldSCMId).ToArray();



                    //If Any Empty SCM Request Check
                    var AddFacultySCMIds = db.jntuh_scmproceedingrequest_addfaculty.Where(e=>e.FacultyType!=1).Select(e => e.ScmProceedingId).Distinct().ToArray();
                    foreach (var s in collegescmrequestslist.Where(e => !nomineeAssignedScmIds.Contains(e.ID)).Select(e => e).ToList())
                    {
                        if (AddFacultySCMIds.Contains(s.ID))
                        {

                            bool Isauditor = false;
                            bool IsSplited = false;
                            bool localIsAuditorVerified = false;
                            var specid = specializations.FirstOrDefault(i => i.id == s.SpecializationId);

                            if (specid != null && firstOrDefault != null)
                            {
                                var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
                                var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));
                                var AuditorAssigneddata = jntuh_auditor_assigned.Where(e => e.ScmId == s.ID).Select(e => e.Id).FirstOrDefault();
                                var Auditorverifieddata = jntuh_auditors_dataentry.Where(e => e.ScmProceedingId == s.ID).Select(e => e.Id).FirstOrDefault();
                                if (AuditorAssigneddata != 0)
                                {
                                    Isauditor = true;
                                }
                                if (Auditorverifieddata != 0)
                                {
                                    localIsAuditorVerified = true;
                                }
                                if (SplittedSCMIds.Contains(s.ID))
                                {
                                    IsSplited = true;
                                }

                                if (degree != null)
                                    proceedingsRequests.Add(new ScmProceedingsRequest
                                    {
                                        CollegeName = firstOrDefault.collegeCode + " - " + firstOrDefault.collegeName,
                                        CollegeId = firstOrDefault.id,
                                        SpecializationId = s.SpecializationId,
                                        ProfessorVacancies = s.ProfessorsCount.ToString(),
                                        AssociateProfessorVacancies = s.AssociateProfessorsCount.ToString(),
                                        AssistantProfessorVacancies = s.AssistantProfessorsCount.ToString(),
                                        SpecializationName = degree.degree + " - " + specid.specializationName,
                                        DepartmentId = specid.departmentId,
                                        DepartmentName = deptment.departmentName,
                                        CreatedDate = (DateTime)s.RequestSubmittedDate,
                                        ScmNotificationpath = s.SCMNotification,
                                        RequiredProfessorVacancies = s.RequiredProfessor.ToString(),
                                        RequiredAssociateProfessorVacancies = s.RequiredAssociateProfessor.ToString(),
                                        RequiredAssistantProfessorVacancies = s.RequiredAssistantProfessor.ToString(),
                                        Id = s.ID,
                                        IsSplited = IsSplited,
                                        IsAuditorAssigned = Isauditor,
                                        IsAuditorVerified = localIsAuditorVerified
                                    });
                            }
                        }
                    }
                    ViewBag.collegescmrequestslist = proceedingsRequests.OrderByDescending(e => e.CreatedDate).Select(e => e).ToList();
                    return View(proceedingsRequests.OrderBy(e => e.DepartmentName).ThenBy(e => e.SpecializationId).ThenBy(e => e.DegreeId).Select(e => e).ToList());//OrderByDescending(e => e.CreatedDate)
                }
                else
                {
                    return RedirectToAction("SCMRequestsList");
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }

        }


        public ActionResult CollegeScmProceedingsRequestViewList(List<ScmProceedingsRequest> scmcheckeddata)
        {
            if (scmcheckeddata.Count != 0)
            {
                var selecteddata = scmcheckeddata.Where(e => e.Checked == true).Select(e => e.Id).ToArray();
            }
            return RedirectToAction("SCMRequestsList");
        }

         [AuthorizedUserAccess("Admin")]
        public ActionResult CollegeScmProceedingsRequestNomineeAssignedview(int collegeId)
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                if (collegeId != 0)
                {
                    var nomineeAssignedScmIds = (from SCM in db.jntuh_scmproceedingsrequests
                                                 join AUDA in db.jntuh_auditor_assigned on SCM.ID equals AUDA.ScmId
                                                 where
                                                     SCM.CollegeId == collegeId && SCM.SpecializationId != 0 && SCM.DEpartmentId != 0 &&
                                                     SCM.DegreeId != 0
                                                 select SCM.ID).Distinct().ToArray();
                    var nomineeassignRequest = (from SCM in db.jntuh_scmproceedingsrequests
                                                join AUDA in db.jntuh_auditor_assigned on SCM.ID equals AUDA.ScmId
                                                join Nominee in db.jntuh_ffc_auditor on AUDA.AuditorId equals Nominee.id
                                                join SPEC in db.jntuh_specialization on SCM.SpecializationId equals SPEC.id
                                                join DEPT in db.jntuh_department on SCM.DEpartmentId equals DEPT.id
                                                join DEG in db.jntuh_degree on SCM.DegreeId equals DEG.id
                                                join CLG in db.jntuh_college on SCM.CollegeId equals CLG.id
                                                where nomineeAssignedScmIds.Contains(SCM.ID)
                                                select new NomineeAssignSCMRequests
                                                {
                                                    SCMId = SCM.ID,
                                                    CollegeCode = CLG.collegeCode,
                                                    CollegeName = CLG.collegeName,
                                                    Department = DEG.degree + " " + SPEC.specializationName,
                                                    AuditorName = Nominee.auditorName,
                                                    AuditorAssignDate = AUDA.CreatedOn,
                                                    ScmRequestDate = (DateTime)SCM.RequestSubmittedDate,
                                                }).OrderByDescending(e => e.AuditorAssignDate).ToList();

                    return View(nomineeassignRequest);
                }
                return RedirectToAction("SCMRequestsList", "CollegeSCMProceedingsRequest");
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }

        }



        //[HttpPost]
        //public ActionResult CollegeScmProceedingsRequest(ScmProceedingsRequest scmrequest)
        //{
        //    var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
        //    var userCollegeId = db.jntuh_college_users.Where(u => u.userID == userId).Select(u => u.collegeID).FirstOrDefault();


        //        if (ModelState.IsValid)
        //        {
        //            var fileName = string.Empty;
        //            var filepath = string.Empty;
        //            var collegescmrequests = new jntuh_scmproceedingsrequests();
        //            const string scmnotificationpath = "~/Content/Upload/SCMPROCEEDINGSREQUEST/ScmNotificationDocuments";
        //            if (scmrequest.ScmNotificationSupportDoc != null)
        //            {
        //                if (!Directory.Exists(Server.MapPath(scmnotificationpath)))
        //                {
        //                    Directory.CreateDirectory(Server.MapPath(scmnotificationpath));
        //                }

        //                var ext = Path.GetExtension(scmrequest.ScmNotificationSupportDoc.FileName);
        //                if (ext != null && ext.ToUpper().Equals(".PDF"))
        //                {
        //                    var scmfileName =
        //                        db.jntuh_college.Where(c => c.id == userCollegeId)
        //                            .Select(c => c.id)
        //                            .FirstOrDefault() + "_" + "ScmNotofication" + "_" +
        //                        DateTime.Now.ToString("yyyMMddHHmmss");
        //                    scmrequest.ScmNotificationSupportDoc.SaveAs(string.Format("{0}/{1}{2}",
        //                        Server.MapPath(scmnotificationpath), scmfileName, ext));
        //                    collegescmrequests.SCMNotification = scmfileName + ext;
        //                }
        //                IUserMailer mailer = new UserMailer();
        //                collegescmrequests.CollegeId = userCollegeId;
        //                collegescmrequests.SpecializationId = scmrequest.SpecializationId;
        //                var specialization =
        //                    db.jntuh_specialization.AsNoTracking()
        //                        .FirstOrDefault(i => i.id == scmrequest.SpecializationId);
        //                var department =
        //                    db.jntuh_department.AsNoTracking().FirstOrDefault(i => i.id == specialization.departmentId);
        //                collegescmrequests.DEpartmentId = specialization != null ? specialization.departmentId : 0;
        //                collegescmrequests.DegreeId = department != null ? department.degreeId : 0;
        //                collegescmrequests.ProfessorsCount = Convert.ToInt16(scmrequest.ProfessorVacancies);
        //                collegescmrequests.AssociateProfessorsCount =Convert.ToInt16(scmrequest.AssociateProfessorVacancies);
        //                collegescmrequests.AssistantProfessorsCount =Convert.ToInt16(scmrequest.AssistantProfessorVacancies);
        //                collegescmrequests.RequiredProfessor = Convert.ToInt16(scmrequest.RequiredProfessorVacancies);
        //                collegescmrequests.RequiredAssistantProfessor = Convert.ToInt16(scmrequest.RequiredAssistantProfessorVacancies);
        //                collegescmrequests.RequiredAssociateProfessor = Convert.ToInt16(scmrequest.RequiredAssociateProfessorVacancies);
        //                if (scmrequest.NotificationDate!=null)
        //                    collegescmrequests.Notificationdate = UAAAS.Models.Utilities.DDMMYY2MMDDYY(scmrequest.NotificationDate);
        //                collegescmrequests.Remarks = scmrequest.Remarks;
        //                collegescmrequests.CreatedBy = 1;
        //                collegescmrequests.CreatedOn = DateTime.Now;
        //                collegescmrequests.ISActive = true;
        //                db.jntuh_scmproceedingsrequests.Add(collegescmrequests);
        //                try
        //                {
        //                    db.SaveChanges();

        //                    var attachments = new List<Attachment>();
        //                    if (scmrequest.ScmNotificationSupportDoc != null)
        //                    {

        //                        fileName = Path.GetFileName(scmrequest.ScmNotificationSupportDoc.FileName);
        //                        filepath = Path.Combine(Server.MapPath("~/Content/Attachments"), fileName);
        //                        scmrequest.ScmNotificationSupportDoc.SaveAs(filepath);
        //                        attachments.Add(new Attachment(filepath));
        //                        mailer.SendAttachmentToAllColleges("sureshpalsa1@gmail.com", "", "",
        //                            "SCM PROCEEDINGS REQUEST", "Scm Requests", attachments).SendAsync();
        //                        TempData["Success"] = "Your request has been proccessed successfully..";
        //                    }

        //                }
        //                catch (DbEntityValidationException dbEx)
        //                {
        //                    foreach (var validationErrors in dbEx.EntityValidationErrors)
        //                    {
        //                        foreach (var validationError in validationErrors.ValidationErrors)
        //                        {
        //                            Trace.TraceInformation("Property: {0} Error: {1}",
        //                                validationError.PropertyName,
        //                                validationError.ErrorMessage);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                TempData["Error"] = "Please Fill All Mandatory Fields..";
        //            }
        //        }


        //    return RedirectToAction("CollegeScmProceedingsRequest");
        //}

        [AuthorizedUserAccess("Admin")]
        [HttpGet]
        public ActionResult CollegeScmProceedingsRequestScreen()
        {
            var colleges = db.jntuh_college.AsNoTracking().ToList();
            var collegescmrequestslist = db.jntuh_scmproceedingsrequests.AsNoTracking().ToList();
            var proceedingsRequests = new List<ScmProceedingsRequest>();
            foreach (var s in collegescmrequestslist.GroupBy(i => i.CollegeId))
            {
                var colldetails = colleges.FirstOrDefault(i => i.id == s.Key);
                proceedingsRequests.Add(new ScmProceedingsRequest
                {
                    CollegeId = s.Key,
                    CollegeName = colldetails != null ? colldetails.collegeCode + " - " + colldetails.collegeName : "",
                });
            }
            ViewBag.collegescmrequestslist = proceedingsRequests;
            return View();
        }

        #region Pdf download

        //public ActionResult CollegeScmPrint(ScmProceedingsRequest scmlist)
        //{
        //    if (scmlist.ScmProceedingsRequestslist.Count() != 0)
        //    {
        //        serverURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
        //        var checkedscmlistdata = scmlist.ScmProceedingsRequestslist.Where(e => e.Checked == true).ToList();
        //        if (checkedscmlistdata.Count() != 0)
        //        {
        //            var pdfPath = string.Empty;
        //            int preview = 0;
        //            if (preview == 0)
        //            {
        //                pdfPath = SaveFacultyDataPdf(preview, checkedscmlistdata);
        //                pdfPath = pdfPath.Replace("/", "\\");
        //            }
        //            return File(pdfPath, "application/pdf", scmlist.ScmProceedingsRequestslist[0].CollegeCode + "- Scm Request File - " + DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf");

        //        }
        //        else
        //        {
        //            TempData["Error"] = "Please select any one checkbox for the print";
        //            return RedirectToAction("CollegeScmProceedingsRequest");
        //        }
        //    }
        //    return RedirectToAction("CollegeScmProceedingsRequest");
        //}

        //public string SaveFacultyDataPdf(int preview,List<ScmProceedingsRequest> scmProceedings)
        //{
        //    string fullPath = string.Empty;
        //    //Set page size as A4
        //    Document pdfDoc = new Document(PageSize.A4, 60, 50, 60, 60);
        //    string path = Server.MapPath("~/Content/PDFReports/SCMRequestDownload");
        //    if (!Directory.Exists(Server.MapPath("~/Content/PDFReports/SCMRequestDownload")))
        //    {
        //        Directory.CreateDirectory(Server.MapPath("~/Content/PDFReports/SCMRequestDownload"));
        //    }

        //    if (preview == 0)
        //    {
        //        fullPath = path + "/" + scmProceedings[0].CollegeCode + "- SCM Request Download -" + DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf";//
        //        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(fullPath, FileMode.Create));
        //        ITextEvents iTextEvents = new ITextEvents();
        //        iTextEvents.CollegeCode = scmProceedings[0].CollegeCode;
        //        iTextEvents.CollegeName = scmProceedings[0].CollegeName;
        //        iTextEvents.formType = "Scm Request Download";
        //        pdfWriter.PageEvent = iTextEvents;
        //    }
        //    //Open PDF Document to write data
        //    pdfDoc.Open();

        //    //Assign Html content in a string to write in PDF
        //    string contents = string.Empty;

        //    StreamReader sr;

        //    //Read file from server path
        //    sr = System.IO.File.OpenText(Server.MapPath("~/Content/SCMRequestDownload.html"));
        //    //store content in the variable
        //    contents = sr.ReadToEnd();
        //    sr.Close();

        //    contents = contents.Replace("##SERVERURL##", serverURL);


        //    contents = GetSCMRequestData(scmProceedings, contents);
        //    //  contents = affiliationType(collegeId, contents);

        //    //Read string contents using stream reader and convert html to parsed conent
        //    var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(contents), null);
        //    //Get each array values from parsed elements and add to the PDF document
        //    bool pageRotated = false;
        //    int count = 0;
        //    foreach (var htmlElement in parsedHtmlElements)
        //    {
        //        count++;
        //        if (count == 100)
        //        {

        //        }
        //        if (htmlElement.Equals("<textarea>"))
        //        {
        //            pdfDoc.NewPage();
        //        }

        //        if (htmlElement.Chunks.Count >= 3)
        //        {
        //            if (htmlElement.Chunks.Count == 4)
        //            {
        //                pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
        //                pdfDoc.SetMargins(60, 50, 60, 60);
        //                pageRotated = true;
        //            }
        //            else
        //            {
        //                if (pageRotated == true)
        //                {
        //                    pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4);
        //                    pdfDoc.SetMargins(60, 50, 60, 60);
        //                    pageRotated = false;
        //                }
        //            }

        //            pdfDoc.NewPage();

        //        }
        //        else
        //        {
        //            pdfDoc.Add(htmlElement as IElement);
        //        }
        //    }

        //    //Close your PDF
        //    pdfDoc.Close();
        //    if (pdfDoc.IsOpen())
        //    {
        //        pdfDoc.Close();
        //    }

        //    string returnPath = string.Empty;
        //    returnPath = fullPath;
        //    return returnPath;
        //}

        //public string GetSCMRequestData(List<ScmProceedingsRequest> scmProceedings, string contents)
        //{
        //    string contentdata = string.Empty;
        //    int[] scmrequestIds = scmProceedings.Select(e => e.Id).ToArray();
        //    var jntuh_scmproceeding_add_faculty =
        //        db.jntuh_scmproceedingrequest_addfaculty.Where(e => scmrequestIds.Contains(e.ScmProceedingId))
        //            .Select(e => e.ScmProceedingId)
        //            .Distinct()
        //            .ToArray();
        //    var scmdetails = (from a in db.jntuh_scmproceedingsrequests
        //        join b in db.jntuh_college on a.CollegeId equals b.id into abdata
        //        from ab in abdata.DefaultIfEmpty()
        //        join c in db.jntuh_specialization on a.SpecializationId equals c.id into abcdata
        //        from abc in abcdata.DefaultIfEmpty()
        //        join d in db.jntuh_department on a.DEpartmentId equals d.id into abcddata
        //        from abcd in abcdata.DefaultIfEmpty()
        //        join e in db.jntuh_degree on a.DegreeId equals e.id into abcdedata
        //        from abcde in abcdedata.DefaultIfEmpty()
        //        where scmrequestIds.Contains(a.ID)
        //        select new {

        //            CollegeCode=ab.collegeCode,
        //            CollegeName = ab.collegeName,
        //            SpecializationId = abc.id,
        //            SpecializationName = abc.specializationName,
        //            //DepartmentId = abcd.id,
        //            //DepartmentName = abcd.departmentName,
        //            DegreeId=abcde.id,
        //            DegreeName=abcde.degree,
        //            Professors=a.ProfessorsCount,
        //            AssociateProfessors=a.AssociateProfessorsCount,
        //            AssistantProfessors=a.AssistantProfessorsCount,
        //            RequiredProfessors = a.RequiredProfessor,
        //            RequiredAssociateProfessors = a.RequiredAssociateProfessor,
        //            RequiredAssistantProfessors = a.RequiredAssistantProfessor,

        //        }).ToList();

        //    contentdata += "<br/><p style='text-align:center;font-size:12px'><b> Staff Selection Committee Request </b></p><br/>";
        //    contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
        //    contentdata += "<tr><td style='text-align:left'><b>College Name : " + scmdetails[0].CollegeName + "</b></td>";
        //    contentdata += "<td style='text-align:left'><b>College Code : " + scmdetails[0].CollegeCode + "</b></td></tr>";
        //    contentdata += "</table>";
        //    contentdata += "<br/><table border='1'cellspacing='0' cellpadding='4' width='100%'>";
        //    contentdata += "<tr><td style='text-align:left' width='8%'>S.No</td><td width='28%'>Specialization</td><td width='10%'>Available Prof</td><td width='12%'>Available Assoc.Prof</td><td width='10%'>Available Asst.Prof</td><td width='10%'>Required Prof</td><td width='12%'>Required  Assoc.Prof</td><td width='10%'>Required  Asst.Prof</td></tr>";

        //    for (int i = 0; i < scmdetails.Count(); i++)
        //    {
        //        contentdata += "<tr>";
        //        contentdata += "<td width='8%'>" + (i + 1) + "</td>";
        //        contentdata += "<td width='28%'>" + scmdetails[i].DegreeName + "-" + scmdetails[i].SpecializationName + "</td>";
        //        contentdata += "<td width='10%'>" + scmdetails[i].Professors + "</td>";
        //        contentdata += "<td width='12%'>" + scmdetails[i].AssociateProfessors + "</td>";
        //        contentdata += "<td width='10%'>" + scmdetails[i].AssistantProfessors + "</td>";
        //        contentdata += "<td width='10%'>" + scmdetails[i].RequiredProfessors + "</td>";
        //        contentdata += "<td width='12%'>" + scmdetails[i].RequiredAssociateProfessors + "</td>";
        //        contentdata += "<td width='10%'>" + scmdetails[i].RequiredAssistantProfessors + "</td>";
        //        contentdata += "</tr>";
        //    }
        //    contentdata += "</table>";

        //    contents = contents.Replace("##SCMDOWLOAD##", contentdata);
        //    //******* Display the Added Faculty Details *********//

        //    string FacultyData = string.Empty;
        //    List<ScmProceedingsRequestAddReg> scmaddedfaculty=new List<ScmProceedingsRequestAddReg>();
        //   scmaddedfaculty = (from SPR in db.jntuh_scmproceedingsrequests join SPRF in db.jntuh_scmproceedingrequest_addfaculty on SPR.ID equals SPRF.ScmProceedingId 
        //                          join RF in db.jntuh_registered_faculty on SPRF.RegistrationNumber equals  RF.RegistrationNumber
        //                          join D in db.jntuh_department on SPR.DEpartmentId equals  D.id
        //                          join S in db.jntuh_specialization on SPR.SpecializationId equals  S.id
        //                          join DG in db.jntuh_degree on SPR.DegreeId equals DG.id
        //                          where scmrequestIds.Contains(SPR.ID )
        //                       select new ScmProceedingsRequestAddReg
        //                       {
        //                           Id = (int)SPRF.Id,
        //                           SpecializationId = SPR.SpecializationId,
        //                           SpecializationName = S.specializationName,//abcde.degree + "-" + abcd.specializationName,
        //                           Regno = RF.RegistrationNumber,
        //                           RegName = RF.FirstName + " " + RF.LastName,
        //                           DegreeName = DG.degree,
        //                           ScmId = SPRF.ScmProceedingId
        //                       }).ToList();


        //    var specializationIds = scmaddedfaculty.Select(e => e.ScmId).Distinct().ToArray();
        //    if (specializationIds.Count() > 0)
        //    {
        //        foreach (var speid in specializationIds )
        //        {
        //            FacultyData += PrintingDepartmentwiseFaculty(scmaddedfaculty.Where(e => e.ScmId == speid).ToList());
        //        }
        //    }


        //    contents = contents.Replace("##FACULTYDATA##", FacultyData);

        //    return contents;
        //}

        //private string PrintingDepartmentwiseFaculty(List<ScmProceedingsRequestAddReg> scmfacultylist)
        //{
        //    int count = 1;
        //    string contentdata = string.Empty;
        //    if (scmfacultylist!=null)
        //    {
        //        contentdata += "<strong><u>" + scmfacultylist[0].DegreeName+"-"+scmfacultylist[0].SpecializationName + "</u></strong> <br /> <br />";
        //        contentdata += "<table border='1' cellspacing='0' cellpadding='5' style='font-size: 9px;' width='100%'>";
        //        contentdata += "<tbody>";
        //        contentdata += "<tr>";
        //        contentdata += "<td width='8%'><p align='left'>SNo</p></td>";
        //        contentdata += "<td width='20%'><p align='left'>Registration No</p></td>";
        //        contentdata += "<td width='27%'><p align='left'>Name</p></td>";
        //        contentdata += "<td width='15%'><p align='left'>Degree</p></td>";
        //        contentdata += "<td width='30%'><p align='left'>Specilization</p></td>";
        //        contentdata += "</tr>";
        //        foreach (var item in scmfacultylist)
        //        {
        //            contentdata += "<tr>";
        //            contentdata += "<td width='8%'><p align='left'>" + count + "</p></td>";
        //            contentdata += "<td width='20%'><p align='left'>" + item.Regno + "</p></td>";
        //            contentdata += "<td width='27%'><p align='left'>" + item.RegName + "</p></td>";
        //            contentdata += "<td width='15%'><p align='left'>" + item.DegreeName + "</p></td>";
        //            contentdata += "<td width='30%'><p align='left'>" + item.SpecializationName + "</p></td>";
        //            contentdata += "</tr>";
        //            count++;
        //        }
        //        contentdata += "</tbody></table>";
        //        return contentdata;
        //    }

        //    return contentdata;
        //}

        #endregion


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult ScmFacultyVerfication(int? collegeId)
        {

            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                var jntuh_scmproceedingsrequests =
                    db.jntuh_scmproceedingsrequests.Where(e => e.ISActive == true).Select(e => e).ToList();
                var collegeIds =
                    jntuh_scmproceedingsrequests.Where(e => e.ISActive == true)
                        .Select(e => e.CollegeId)
                        .Distinct()
                        .ToArray();
                ViewBag.Colleges =
                    db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id))
                        .Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName })
                        .OrderBy(e => e.collegeName)
                        .ToList();
                var jntuh_department = db.jntuh_department.ToList();
                var jntuh_specialization = db.jntuh_specialization.ToList();
                var jntuh_degree = db.jntuh_degree.ToList();
                List<FacultyRegistrationDetails> teachingFaculty = new List<FacultyRegistrationDetails>();

                if (collegeId != null)
                {
                    var jntuh_scmproceedingrequest_addfaculty =
                        db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking().ToList();
                    string[] strRegNoS = (from a in jntuh_scmproceedingsrequests
                                          join b in jntuh_scmproceedingrequest_addfaculty on a.ID equals b.ScmProceedingId
                                          where a.CollegeId == collegeId
                                          select b.RegistrationNumber).Distinct().ToArray();


                    int[] SCMProccedingIds =
                        jntuh_scmproceedingsrequests.Where(e => e.CollegeId == collegeId)
                            .Select(e => e.ID)
                            .Distinct()
                            .ToArray();

                    List<jntuh_registered_faculty> jntuh_registered_faculty = new List<jntuh_registered_faculty>();

                    jntuh_registered_faculty =
                        db.jntuh_registered_faculty.Where(
                            rf => strRegNoS.Contains(rf.RegistrationNumber) && rf.Notin116 != true).ToList();

                    var jntuhScmproceedingrequestAddfaculty =
                        jntuh_scmproceedingrequest_addfaculty.Where(e => SCMProccedingIds.Contains(e.ScmProceedingId))
                            .Select(e => e)
                            .ToList();



                    string RegNumber = "";
                    int? Specializationid = 0;
                    int SCMId = 0;
                    int DeptId = 0;
                    int degId = 0;
                    foreach (var a in jntuh_registered_faculty)
                    {
                        string Reason = String.Empty;
                        SCMId =
                            jntuhScmproceedingrequestAddfaculty.Where(
                                C => C.RegistrationNumber.Trim() == a.RegistrationNumber.Trim())
                                .Select(C => C.ScmProceedingId)
                                .FirstOrDefault();
                        Specializationid =
                            jntuh_scmproceedingsrequests.Where(C => C.ID == SCMId)
                                .Select(C => C.SpecializationId)
                                .FirstOrDefault();
                        DeptId =
                            jntuh_scmproceedingsrequests.Where(C => C.ID == SCMId)
                                .Select(C => C.DEpartmentId)
                                .FirstOrDefault();
                        degId =
                            jntuh_scmproceedingsrequests.Where(C => C.ID == SCMId)
                                .Select(C => C.DegreeId)
                                .FirstOrDefault();
                        var faculty = new FacultyRegistrationDetails();
                        faculty.id = a.id;
                        faculty.ScmproceedingId = SCMId;
                        faculty.RegistrationNumber = a.RegistrationNumber;
                        faculty.FirstName = a.FirstName;
                        faculty.MiddleName = a.MiddleName;
                        faculty.LastName = a.LastName;
                        faculty.isActive = a.isActive;
                        faculty.FacultyAddId =
                            jntuhScmproceedingrequestAddfaculty.Where(
                                e => e.RegistrationNumber.Trim() == a.RegistrationNumber.Trim())
                                .Select(e => e.Id)
                                .FirstOrDefault();
                        faculty.DeactivationReason =
                            jntuhScmproceedingrequestAddfaculty.Where(
                                e => e.RegistrationNumber.Trim() == a.RegistrationNumber.Trim())
                                .Select(e => e.DeactiviationReason)
                                .FirstOrDefault();
                        faculty.isApproved =
                            jntuhScmproceedingrequestAddfaculty.Where(
                                e => e.RegistrationNumber.Trim() == a.RegistrationNumber.Trim())
                                .Select(e => e.IsApproved)
                                .FirstOrDefault();
                        faculty.department =
                            jntuh_department.Where(d => d.id == DeptId).Select(d => d.departmentName).FirstOrDefault();
                        faculty.specialization =
                            jntuh_specialization.Where(d => d.id == Specializationid)
                                .Select(d => d.specializationName)
                                .FirstOrDefault();
                        faculty.degree = jntuh_degree.Where(d => d.id == degId).Select(d => d.degree).FirstOrDefault();
                        faculty.SCMRequestDate =
                            UAAAS.Models.Utilities.MMDDYY2DDMMYY(
                                jntuh_scmproceedingsrequests.Where(C => C.ID == SCMId)
                                    .Select(C => C.Notificationdate)
                                    .FirstOrDefault()
                                    .ToString());
                        teachingFaculty.Add(faculty);
                    }
                    teachingFaculty =
                        teachingFaculty.Where(m => m.isActive == true)
                            .OrderByDescending(e => e.ScmproceedingId)
                            .ToList();
                    return View(teachingFaculty);
                }

                return View(teachingFaculty);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpGet]
        public ActionResult AddRegistrationNumber(int id)
        {
            ScmProceedingsRequestAddReg scmDetails = new ScmProceedingsRequestAddReg();
            scmDetails = (from a in db.jntuh_scmproceedingsrequests
                          join b in db.jntuh_college on a.CollegeId equals b.id into abdata
                          from ab in abdata.DefaultIfEmpty()
                          join c in db.jntuh_specialization on a.SpecializationId equals c.id into abcdata
                          from abc in abcdata.DefaultIfEmpty()
                          join d in db.jntuh_department on a.DEpartmentId equals d.id into abcddata
                          from abcd in abcdata.DefaultIfEmpty()
                          join e in db.jntuh_degree on a.DegreeId equals e.id into abcdedata
                          from abcde in abcdedata.DefaultIfEmpty()
                          where a.ID == id
                          select new ScmProceedingsRequestAddReg
                          {
                              CollegeCode = ab.collegeCode,
                              CollegeName = ab.collegeName,

                              SpecializationId = abc.id,
                              SpecializationName = abc.specializationName,
                              DepartmentId = abcd.id,
                              DepartmentName = abc.jntuh_department.departmentName,
                              DegreeId = abcde.id,
                              DegreeName = abcde.degree,
                              Professors = (int)a.ProfessorsCount,
                              AssociateProfessors = (int)a.AssociateProfessorsCount,
                              AssistantProfessors = (int)a.AssistantProfessorsCount,
                              Id = a.ID
                          }).FirstOrDefault();

            var jntuh_college = db.jntuh_college.Where(e => e.isActive == true).Select(e => e).ToList();
            //var jntuh_college1 = db.jntuh_college.Where(e => e.isActive == true).Select(e => e).ToList();
            jntuh_college.Add(new jntuh_college() { id = 0, collegeCode = "Not Working" });

            ViewBag.Colleges =
                jntuh_college.Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName })
                    .OrderBy(e => e.collegeId)
                    .ToList();

            //var data = jntuh_college1.Select(e => new SelectListItem() { Value = e.id.ToString(), Text = e.collegeCode + "-" + e.collegeName }).ToList();
            //data.Insert(00, new SelectListItem() { Value = "00", Text = "Not Working", Selected = true });
            //data.Insert(0, new SelectListItem() {  Text = "---Select---",Selected = false});
            //ViewBag.Collegess = data;

            ViewBag.Designations =
                db.jntuh_designation.Where(e => e.isActive == true && (e.id == 1 || e.id == 2 || e.id == 3))
                    .Select(e => new { Id = e.id, Name = e.designation })
                    .OrderBy(e => e.Id)
                    .ToList();


            return PartialView("_AddRegistrationNumber", scmDetails);
        }

        //[HttpPost]
        //public ActionResult AddRegistrationNumber(ScmProceedingsRequestAddReg reg)
        //{
        //    TempData["Error"] = null;
        //    int userID = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
        //  //  int userCollegeID = db.jntuh_college_users.Where(collegeUser => collegeUser.userID == userID).Select(collegeUser => collegeUser.collegeID).FirstOrDefault();
        //    if (ModelState.IsValid)
        //    {
        //        if (reg != null)
        //        {
        //            jntuh_scmproceedingrequest_addfaculty addfaculty=new jntuh_scmproceedingrequest_addfaculty();
        //            addfaculty.ScmProceedingId = reg.Id;
        //            addfaculty.RegistrationNumber = reg.RegistrationNo;
        //            addfaculty.FacultyType = reg.FacultyId;
        //            addfaculty.PreviousCollegeId = reg.PreviousCollegeId;
        //            addfaculty.Createdby = userID;
        //            addfaculty.CreatedOn = DateTime.Now;
        //            addfaculty.Isactive = true;
        //            db.jntuh_scmproceedingrequest_addfaculty.Add(addfaculty);
        //            db.SaveChanges();
        //            TempData["Success"] = "Faculty Add Successfully";
        //            return RedirectToAction("CollegeScmProceedingsRequest", "CollegeSCMProceedingsRequest");
        //        }
        //    }
        //    return RedirectToAction("CollegeScmProceedingsRequest", "CollegeSCMProceedingsRequest");
        //}

        [HttpPost]
        public JsonResult CheckRegistrationNumber(string RegistrationNo)
        {
            string CheckingReg =
                db.jntuh_registered_faculty.Where(f => f.RegistrationNumber.Trim() == RegistrationNo.Trim())
                    .Select(f => f.RegistrationNumber)
                    .FirstOrDefault();
            if (!string.IsNullOrEmpty(CheckingReg))
            {
                if (CheckingReg.Trim() == RegistrationNo.Trim())
                    return Json(true);
                else
                    return Json("This Registration Number doesn't Exist", JsonRequestBehavior.AllowGet);
            }
            else
                return Json("This Registration Number doesn't Exist", JsonRequestBehavior.AllowGet);

        }

        public ActionResult ViewFaculty(int scmid)
        {
            if (scmid != 0)
            {
                List<ScmProceedingsRequestAddReg> addFacultyDetails = new List<ScmProceedingsRequestAddReg>();

                addFacultyDetails = (from a in db.jntuh_scmproceedingsrequests
                                     join b in db.jntuh_scmproceedingrequest_addfaculty on a.ID equals b.ScmProceedingId into abdata
                                     from ab in abdata.DefaultIfEmpty()
                                     join c in db.jntuh_registered_faculty on ab.RegistrationNumber.Trim() equals
                                         c.RegistrationNumber.Trim() into abcdata
                                     from abc in abcdata.DefaultIfEmpty()
                                     join d in db.jntuh_specialization on a.SpecializationId equals d.id into abcddata
                                     from abcd in abcddata.DefaultIfEmpty()
                                     join e in db.jntuh_degree on a.DegreeId equals e.id into abcdedata
                                     from abcde in abcdedata.DefaultIfEmpty()
                                     where ab.ScmProceedingId == scmid
                                     select new ScmProceedingsRequestAddReg
                                     {
                                         Id = ab.Id,
                                         SpecializationId = a.SpecializationId,
                                         SpecializationName = abcde.degree + "-" + abcd.specializationName,
                                         Regno = abc.RegistrationNumber,
                                         RegName = abc.FirstName + " " + abc.LastName,
                                         ScmId = ab.ScmProceedingId,
                                         FacultyId = abc.id

                                     }).ToList();
                return View(addFacultyDetails);
            }
            return RedirectToAction("CollegeScmProceedingsRequest");
        }


        public ActionResult DeleteRegistrationNumber(int id, int scmId)
        {
            if (id != 0 && scmId != 0)
            {
                var faculydata =
                    db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == id).Select(e => e).FirstOrDefault();
                if (faculydata != null)
                {
                    db.jntuh_scmproceedingrequest_addfaculty.Remove(faculydata);
                    db.SaveChanges();
                    TempData["Success"] = "Faculty Deleted Successfully";
                    return RedirectToAction("ViewFaculty", "CollegeSCMProceedingsRequest", new { scmid = scmId });
                }
            }
            return RedirectToAction("CollegeScmProceedingsRequest");
        }

     [AuthorizedUserAccess("Admin")]
        public ActionResult SCMRequestsList()
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                var jntuh_scmproceedingsrequests =
                    db.jntuh_scmproceedingsrequests.Where(e => e.ISActive == true).Select(e => e).ToList();
                var AddFacultyScmIds =
                    db.jntuh_scmproceedingrequest_addfaculty.Select(e => e.ScmProceedingId).Distinct().ToArray();


                var jntuh_auditor_assign =
                    db.jntuh_auditor_assigned.Where(e => e.IsActive == true).Select(e => e).ToList();


                //SCM Ids with Requests
                var SCMRequests =
                    jntuh_scmproceedingsrequests.Where(
                        e => e.ISActive == true && e.SpecializationId != 0 && e.DEpartmentId != 0 && e.DegreeId != 0)
                        .Select(e => e.ID)
                        .Distinct()
                        .ToArray();
                var AddFacultySCMRequests =
                    AddFacultyScmIds.Where(e => SCMRequests.Contains(e)).Select(e => e).Distinct().ToArray();


                var collegeIds =
                    jntuh_scmproceedingsrequests.Where(
                        e =>
                            e.ISActive == true && e.SpecializationId != 0 && e.DEpartmentId != 0 && e.DegreeId != 0 &&
                            AddFacultySCMRequests.Contains(e.ID)).Select(e => e.CollegeId).Distinct().ToArray();


                List<jntuh_college> colleges =
                    db.jntuh_college.Where(e => collegeIds.Contains(e.id)).Select(e => e).Distinct().ToList();


                ScmRequestList scmdata = new ScmRequestList();
                scmdata.SCmRequestList = new List<ScmRequestList>();
                List<ScmRequestList> scmRequestLists = new List<ScmRequestList>();
                foreach (var item in colleges)
                {
                    bool Ismatch = false;
                    var scmrequestIds =
                        jntuh_scmproceedingsrequests.Where(
                            e =>
                                e.CollegeId == item.id && e.SpecializationId != 0 && e.DEpartmentId != 0 &&
                                e.DegreeId != 0 && e.RequestSubmittedDate != null)
                            .Select(e => e.ID)
                            .Distinct()
                            .ToArray();
                    var scmrequestsaddfaculty =
                        AddFacultyScmIds.Where(e => scmrequestIds.Contains(e)).Select(e => e).Distinct().ToArray();
                    var scmrequestassigncount =
                        jntuh_auditor_assign.Where(e => scmrequestsaddfaculty.Contains(e.ScmId))
                            .Select(e => e.ScmId)
                            .Distinct()
                            .Count();
                    if (scmrequestassigncount == scmrequestsaddfaculty.Count())
                    {
                        Ismatch = true;
                    }
                    scmRequestLists.Add(new ScmRequestList()
                    {
                        Id = item.id,
                        CollegeCode = item.collegeCode,
                        CollegeName = item.collegeName,
                        IsAuditorAssigned = Ismatch,
                        Checked = false
                    });
                }
                scmdata.SCmRequestList.AddRange(scmRequestLists.OrderBy(e => e.CollegeName).Select(e => e).ToList());
                return View(scmdata);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }

        //SCM College Wise Report List
        public ActionResult CollegeScmPrint(ScmRequestList scmlist)
        {

            if (scmlist.SCmRequestList.Count() != 0)
            {

                var checkedscmlistdata = scmlist.SCmRequestList.Where(e => e.Checked == true).ToList();
                if (checkedscmlistdata.Count() != 0)
                {
                    ////////////////////Word Code
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=SCM Request Data Report.doc");
                    Response.ContentType = "application/vnd.ms-word ";
                    Response.Charset = string.Empty;
                    StringBuilder str = new StringBuilder();
                    str.Append(GetSCMRequestData(checkedscmlistdata));
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 60, 50, 60, 60);

                    pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    pdfDoc.SetMargins(60, 50, 60, 60);

                    string path = Server.MapPath("~/Content/PDFReports/SCMRequestToAll/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = path + "SCm Requests Data" + "-" + DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf";
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.Create));

                    pdfDoc.Open();

                    List<IElement> parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(str.ToString()), null);

                    foreach (var htmlElement in parsedHtmlElements)
                    {
                        pdfDoc.Add((IElement)htmlElement);
                    }

                    pdfDoc.Close();

                    Response.Output.Write(str.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    TempData["Error"] = "Please select any one checkbox for the print";
                    return RedirectToAction("SCMRequestsList");
                }
            }
            return RedirectToAction("SCMRequestsList");
        }

        public string GetSCMRequestData(List<ScmRequestList> scmProceedings)
        {
            var collegeIds = scmProceedings.Where(e => e.Checked == true).Select(e => e.Id).Distinct().ToArray();



            var scmaddfacultyData = db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking().ToList();

            var NomineeassignSCMIds = db.jntuh_auditor_assigned.Select(e => e.ScmId).Distinct().ToArray();

            //   var jntuh_departments = db.jntuh_department.AsNoTracking().ToList();

            var ScmAddFacultySCMIds = scmaddfacultyData.Select(e => e.ScmProceedingId).Distinct().ToArray();

            var SCMData = (from SCMREQ in db.jntuh_scmproceedingsrequests
                           join CLG in db.jntuh_college on SCMREQ.CollegeId equals CLG.id
                           join SPEC in db.jntuh_specialization on SCMREQ.SpecializationId equals SPEC.id
                           join DPET in db.jntuh_department on SCMREQ.DEpartmentId equals DPET.id
                           join DEG in db.jntuh_degree on SCMREQ.DegreeId equals DEG.id
                           join SCMADD in db.jntuh_scmproceedingrequest_addfaculty on SCMREQ.ID equals SCMADD.ScmProceedingId
                           where collegeIds.Contains(SCMREQ.CollegeId) && ScmAddFacultySCMIds.Contains(SCMREQ.ID) &&
                               !NomineeassignSCMIds.Contains(SCMREQ.ID) && SCMREQ.RequestSubmittedDate != null && SCMADD.FacultyType != 1
                           select new
                           {
                               CollegeId = SCMREQ.CollegeId,
                               CollegeCode = CLG.collegeCode,
                               CollegeName = CLG.collegeName,
                               Department = DEG.degree + "-" + SPEC.specializationName,
                               SCMId = SCMREQ.ID,
                               DepartmentId = SCMREQ.DEpartmentId,
                               DepartmentName = DPET.departmentName,
                               DegreeName = DEG.degree,
                               //  Reqprof = SCMREQ.RequiredProfessor,
                               Reqassociprof = SCMREQ.RequiredAssociateProfessor,
                               ReqAssistprof = SCMREQ.RequiredAssistantProfessor,
                               //   Availprof = SCMREQ.ProfessorsCount,
                               Availassociprof = SCMREQ.AssociateProfessorsCount,
                               AvailAssistprof = SCMREQ.AssistantProfessorsCount,
                               //  FacultyType = SCMADD.FacultyType
                           }).Distinct().ToList();


            //var SCMData = (from element in SCMDATA1
            //    group element by element.SCMId
            //    into groups
            //    let firstOrDefault = groups.FirstOrDefault()
            //    where firstOrDefault != null
            //    select new
            //    {
            //        CollegeId = firstOrDefault.CollegeId,
            //        CollegeCode = firstOrDefault.CollegeCode,
            //        CollegeName = firstOrDefault.CollegeName,
            //        Department = firstOrDefault.Department,
            //        SCMId = firstOrDefault.SCMId,
            //        DepartmentId = firstOrDefault.DepartmentId,
            //        DepartmentName = firstOrDefault.DepartmentName,
            //        DegreeName = firstOrDefault.DegreeName,
            //        Reqassociprof = firstOrDefault.Reqassociprof,
            //        ReqAssistprof = firstOrDefault.ReqAssistprof,
            //        Availassociprof = firstOrDefault.Availassociprof,
            //        AvailAssistprof = firstOrDefault.AvailAssistprof,
            //        FacultyType = firstOrDefault.FacultyType
            //    }).ToList();


            ;



            string contextstring = string.Empty;
            contextstring += "<table border='1' style='width:100%'>";
            contextstring += "<tr>";
            contextstring += "<td width='5%'>S.No</td>";
            contextstring += "<td width='20%'>College Name</td>";
            contextstring += "<td width='25%'>Department Name</td>";
            contextstring += "<td width='50%'>University Nominee</td>";
            contextstring += "</tr>";
            int count = 1;
            //foreach (var item in SCMData)
            //{
            //    var rowspancount = SCMData.Where(e => e.CollegeId == item.CollegeId).Select(e => e.CollegeId).Count();
            //    contextstring += "<tr>";
            //  //  contextstring += "<td>" + count + "</td>";
            //    contextstring += "<td rowspan='" + rowspancount + "'>" + item.CollegeName + "</td>";
            //    contextstring += "<td rowspan='" + rowspancount + "'>" + item.Department + "</td>";
            //    contextstring += "<td rowspan='" + rowspancount + "'>" + " " + "</td>";
            //    contextstring += "</tr>";
            //    count++;
            //}

            var jntuh_address = (from CLGADDRESS in db.jntuh_address
                                 join DIST in db.jntuh_district on CLGADDRESS.districtId equals DIST.id
                                 where CLGADDRESS.addressTye == "COLLEGE"
                                 select new
                                 {
                                     CollegeId = CLGADDRESS.collegeId,
                                     Address = CLGADDRESS.address,
                                     Mandal = CLGADDRESS.mandal,
                                     District = DIST.districtName,
                                     Town = CLGADDRESS.townOrCity,
                                     PINcode = CLGADDRESS.pincode
                                 }).ToList();

            //db.jntuh_address.Where(e => e.addressTye == "COLLEGE").Select(e => e).ToList();


            // SCMData = SCMData.Where(e => e.FacultyType != 1).Select(e => e).ToList();


            foreach (var item in SCMData.Select(e => e.CollegeId).Distinct().ToArray())
            {
                var CollegeAddress = jntuh_address.Where(e => e.CollegeId == item).Select(e => e).FirstOrDefault();
                var SCMDatabycollegeId =
                    SCMData.Where(e => e.CollegeId == item).Select(e => e).OrderBy(e => e.DepartmentName).ToList();

                var SCMDataByDepartmentId =
                    SCMData.Where(e => e.CollegeId == item)
                        .Select(e => e)
                        .Select(e => e)
                        .OrderBy(e => e.DepartmentName)
                        .Distinct()
                        .ToList();

                var SCMDataByDepartmentId1 =
                    SCMData.Where(e => e.CollegeId == item)
                        .Select(e => e)
                        .Select(e => e.DepartmentId)
                        .Distinct()
                        .ToArray();

                contextstring += "<tr>";
                contextstring += "<td width='5%' style='vertical-align:top'>" + count + "</td>";
                contextstring += "<td width='20%' style='vertical-align:top'>" + SCMDatabycollegeId[0].CollegeName +
                                 "<br/> " + CollegeAddress.Address + "<br/>" + CollegeAddress.Town + "(T), " +
                                 CollegeAddress.Mandal + "(M), <br/>" + CollegeAddress.District + "(D), PIN - " +
                                 CollegeAddress.PINcode + " <b>(" + SCMDatabycollegeId[0].CollegeCode + ")</b></td>";
                contextstring +=
                    "<td width='75%' colspan='2' style='vertical-align:top'><table border='1' width='100%'>";

                //sample 
                int deeploop = 1;

                foreach (var DeptId in SCMDataByDepartmentId)
                {
                    //  var totalcount =SCMDatabycollegeId.Where(e =>e.DepartmentName == DeptId.DepartmentName &&(e.DegreeName == "M.Tech" || e.DegreeName == "B.Tech")).Distinct().Count();
                    //var DeptName =jntuh_departments.Where(e => e.id == DeptId).Select(e => e.departmentName).FirstOrDefault();
                    //var DeptIds =jntuh_departments.Where(e => e.departmentName == DeptName).Select(e => e.id).Distinct().ToArray();
                    int indexof = SCMDataByDepartmentId.IndexOf(DeptId);
                    if (indexof > 0 &&
                        SCMDataByDepartmentId[indexof].DepartmentName !=
                        SCMDataByDepartmentId[indexof - 1].DepartmentName)
                    {
                        deeploop = 1;
                    }
                    if (deeploop == 1)
                    {

                        contextstring += "<tr><td width='33%'>";
                        foreach (
                            var data in
                                SCMDatabycollegeId.Where(e => e.DepartmentName == DeptId.DepartmentName)
                                    .Select(e => e)
                                    .ToList()) //e.DepartmentId==DeptId.DepartmentId
                        {

                            //  var professor =scmaddfacultyData.Where(e => e.ScmProceedingId == data.SCMId && e.FacultyType == 1).Select(e => e.ScmProceedingId).Count();
                            var Associateprofessor =
                                scmaddfacultyData.Where(e => e.ScmProceedingId == data.SCMId && e.FacultyType == 2)
                                    .Select(e => e.ScmProceedingId)
                                    .Count();
                            var Assistantprofessor =
                                scmaddfacultyData.Where(e => e.ScmProceedingId == data.SCMId && e.FacultyType == 3)
                                    .Select(e => e.ScmProceedingId)
                                    .Count();

                            //  var Reqprofessor = data.Reqprof;
                            var ReqAssociateprofessor = data.Reqassociprof;
                            var ReqAssistantprofessor = data.ReqAssistprof;

                            // var Availprofessor = data.Availprof;
                            var AvailAssociateprofessor = data.Availassociprof;
                            var AvailAssistantprofessor = data.AvailAssistprof;
                            contextstring += data.Department + " (" + Associateprofessor + "/" + ReqAssociateprofessor +
                                             "/" + AvailAssociateprofessor + "+" + Assistantprofessor + "/" +
                                             ReqAssistantprofessor + "/" + AvailAssistantprofessor + ")<br/>";
                            //+ professor + "/" + Reqprofessor + "/" + Availprofessor + "+"
                        }
                        contextstring += "</td><td width='67%'></td></tr>";
                        deeploop++;
                    }
                }
                contextstring += "</table></td>";
                //  contextstring += "<td>" + " " + "</td>";
                contextstring += "</tr>";
                count++;
            }
            contextstring += "</table>";
            return contextstring;
            //contents = contents.Replace("##SCMREPORT##", contextstring);
            //return contents;
        }

        //End SCM College Wise Report List



        //SCM College Wise Report List With Merge Option
        public string GetSCMRequestDataWithMerge(List<ScmRequestList> scmProceedings)
        {
            var collegeIds = scmProceedings.Where(e => e.Checked == true).Select(e => e.Id).Distinct().ToArray();

            var SCMData = (from SCMREQ in db.jntuh_scmproceedingsrequests
                           join CLG in db.jntuh_college on SCMREQ.CollegeId equals CLG.id
                           join SPEC in db.jntuh_specialization on SCMREQ.SpecializationId equals SPEC.id
                           join DPET in db.jntuh_specialization on SCMREQ.DEpartmentId equals DPET.id
                           join DEG in db.jntuh_degree on SCMREQ.DegreeId equals DEG.id
                           where collegeIds.Contains(SCMREQ.CollegeId)
                           select new
                           {
                               CollegeId = SCMREQ.CollegeId,
                               CollegeName = CLG.collegeName,
                               Department = DEG.degree + "-" + SPEC.specializationName
                           }).ToList();


            string contextstring = string.Empty;
            contextstring += "<table border='1' style='width:100%'>";
            contextstring += "<tr>";
            contextstring += "<td>S.No</td>";
            contextstring += "<td>College Name</td>";
            contextstring += "<td>Department Name</td>";
            contextstring += "<td>University Nominee</td>";
            contextstring += "</tr>";
            int count = 1;
            foreach (var item in SCMData)
            {
                var rowspancount = SCMData.Where(e => e.CollegeId == item.CollegeId).Select(e => e.CollegeId).Count();
                contextstring += "<tr>";
                contextstring += "<td>" + count + "</td>";
                contextstring += "<td>" + item.CollegeName + "</td>";
                contextstring += "<td>" + item.Department + "</td>";
                contextstring += "<td>" + " " + "</td>";
                contextstring += "</tr>";
                count++;
            }
            contextstring += "</table>";
            return contextstring;
            //contents = contents.Replace("##SCMREPORT##", contextstring);
            //return contents;
        }

        //End--SCM College Wise Report List With Merge Option






        // SCM Upload

        public ActionResult SCMUpload(int? collegeId)
        {
            ViewBag.Colleges =
                db.jntuh_college.Where(e => e.isActive == true)
                    .Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName })
                    .OrderBy(e => e.collegeName)
                    .ToList();

            if (collegeId != null)
            {
                // var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
                // var userCollegeId =db.jntuh_college_users.Where(u => u.userID == userId).Select(u => u.collegeID).FirstOrDefault();
                var firstOrDefault = db.jntuh_college.FirstOrDefault(a => a.id == collegeId);
                var specs = new List<DistinctSpecializations>();
                var depts = new List<DistinctDepartments>();
                var degrees = db.jntuh_degree.AsNoTracking().ToList();
                var specializations = db.jntuh_specialization.AsNoTracking().ToList();
                var departments = db.jntuh_department.AsNoTracking().ToList();
                //int[] collegespecs = new int[];
                List<int> collegespecs = new List<int>();
                collegespecs.AddRange(
                    db.jntuh_college_intake_existing.Where(i => i.collegeId == collegeId)
                        .Select(i => i.specializationId)
                        .Distinct()
                        .ToArray());
                foreach (var s in collegespecs)
                {
                    var specid = specializations.FirstOrDefault(i => i.id == s);

                    if (specid != null)
                    {
                        var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
                        var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));
                        if (degree != null)
                            specs.Add(new DistinctSpecializations
                            {
                                SpecializationId = specid.id,
                                SpecializationName = degree.degree + " - " + specid.specializationName,
                                DepartmentId = specid.departmentId
                            });
                    }
                }

                ViewBag.departments = specs.OrderBy(i => i.DepartmentId);

                ViewBag.Designations =
                    db.jntuh_designation.Where(e => e.isActive == true && (e.id == 1 || e.id == 2 || e.id == 3))
                        .Select(e => new { Id = e.id, Name = e.designation })
                        .OrderBy(e => e.Id)
                        .ToList();
                //List<Scmuploads> scmuploadeddata = (from SCMUPL in db.jntuh_scmupload
                //    join SPE in db.jntuh_specialization on SCMUPL.SpecializationId equals SPE.id
                //    join DEPT in db.jntuh_department on SCMUPL.DepartmentId equals DEPT.id
                //    join DEG in db.jntuh_degree on SCMUPL.DegreeId equals DEG.id
                //     where SCMUPL.IsActive == true && SCMUPL.CollegeId == collegeId
                //    select new Scmuploads()
                //    {
                //        Id = SCMUPL.Id,
                //        CollegeId = SCMUPL.CollegeId,
                //        SpecializationId = SCMUPL.SpecializationId,
                //        DegreeId = SCMUPL.DegreeId,
                //        DepartmentId = SCMUPL.DepartmentId,
                //        SpecializationName = DEG.degree + "-" + SPE.specializationName,
                //        Department = DEPT.departmentName,
                //        Degree = DEG.degree,
                //        ScmDateView = SCMUPL.SCMdate,
                //        ProfessorDocumentView = SCMUPL.ProfDocument,
                //        AssociateProfessorDocumentView = SCMUPL.AssocDocument,
                //        AssistantProfessorDocumentView = SCMUPL.AssistDocument
                //    }).ToList();

                //ViewBag.SCMUPLOADEDDATA = scmuploadeddata;
                return View();
            }
            return View();
        }


        //[Authorize(Roles = "College")]
        [HttpPost]
        public ActionResult SCMUpload(Scmuploads scmdata)
        {
            // var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
            //  var userCollegeId = db.jntuh_college_users.Where(u => u.userID == userId).Select(u => u.collegeID).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var jntuh_college = db.jntuh_college.Where(e => e.isActive == true).Select(e => e).ToList();
                var jntuh_specialization = db.jntuh_specialization.AsNoTracking().ToList();
                var jntuh_department = db.jntuh_department.AsNoTracking().ToList();

                const string scmnotificationpath = "~/Content/Upload/SCMUploads";
                var jntuh_scmupload = new UAAASSCM.Models.jntuh_scmupload();

                #region Saving Pdf data if Upload SCM File

                //Professor's Document Saving
                if (scmdata.ProfessorDocument != null)
                {
                    if (!Directory.Exists(Server.MapPath(scmnotificationpath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(scmnotificationpath));
                    }

                    var ext = Path.GetExtension(scmdata.ProfessorDocument.FileName);
                    if (ext != null && ext.ToUpper().Equals(".PDF"))
                    {
                        var professorScmFileName =
                            jntuh_college.Where(c => c.id == scmdata.CollegeId)
                                .Select(c => c.collegeCode)
                                .FirstOrDefault() + "_" + "Professors" + "_" + DateTime.Now.ToString("yyyMMddHHmmss");
                        scmdata.ProfessorDocument.SaveAs(string.Format("{0}/{1}{2}",
                            Server.MapPath(scmnotificationpath), professorScmFileName, ext));
                        scmdata.ProfessorDocumentView = professorScmFileName + ext;
                    }
                }

                //Associate Professor's Document Saving
                if (scmdata.AssociateProfessorDocument != null)
                {
                    if (!Directory.Exists(Server.MapPath(scmnotificationpath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(scmnotificationpath));
                    }

                    var ext = Path.GetExtension(scmdata.AssociateProfessorDocument.FileName);
                    if (ext != null && ext.ToUpper().Equals(".PDF"))
                    {
                        var associateprofessorScmFileName =
                            jntuh_college.Where(c => c.id == scmdata.CollegeId)
                                .Select(c => c.collegeCode)
                                .FirstOrDefault() +
                            "_" + "AssociateProfessors" + "_" + DateTime.Now.ToString("yyyMMddHHmmss");
                        scmdata.AssociateProfessorDocument.SaveAs(string.Format("{0}/{1}{2}",
                            Server.MapPath(scmnotificationpath), associateprofessorScmFileName, ext));
                        scmdata.AssociateProfessorDocumentView = associateprofessorScmFileName + ext;
                    }
                }

                //Assistant Professor's Document Saving
                if (scmdata.AssistantProfessorDocument != null)
                {
                    if (!Directory.Exists(Server.MapPath(scmnotificationpath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(scmnotificationpath));
                    }

                    var ext = Path.GetExtension(scmdata.AssistantProfessorDocument.FileName);
                    if (ext != null && ext.ToUpper().Equals(".PDF"))
                    {
                        var assistantprofessorScmFileName =
                            jntuh_college.Where(c => c.id == scmdata.CollegeId)
                                .Select(c => c.collegeCode)
                                .FirstOrDefault() +
                            "_" + "AssistantProfessors" + "_" + DateTime.Now.ToString("yyyMMddHHmmss");
                        scmdata.AssistantProfessorDocument.SaveAs(string.Format("{0}/{1}{2}",
                            Server.MapPath(scmnotificationpath), assistantprofessorScmFileName, ext));
                        scmdata.AssistantProfessorDocumentView = assistantprofessorScmFileName + ext;
                    }
                }

                #endregion

                jntuh_scmupload.CollegeId = scmdata.CollegeId;
                jntuh_scmupload.SpecializationId = scmdata.SpecializationId;
                var specialization = jntuh_specialization.FirstOrDefault(i => i.id == scmdata.SpecializationId);
                var department = jntuh_department.FirstOrDefault(i => i.id == specialization.departmentId);
                jntuh_scmupload.DepartmentId = specialization != null ? specialization.departmentId : 0;
                jntuh_scmupload.DegreeId = department != null ? department.degreeId : 0;
                jntuh_scmupload.ProfDocument = scmdata.ProfessorDocumentView ?? "";
                jntuh_scmupload.AssocDocument = scmdata.AssociateProfessorDocumentView ?? "";
                jntuh_scmupload.AssistDocument = scmdata.AssistantProfessorDocumentView ?? "";
                if (scmdata.ScmDate != null)
                    jntuh_scmupload.SCMdate = UAAAS.Models.Utilities.DDMMYY2MMDDYY(scmdata.ScmDate);
                jntuh_scmupload.IsActive = true;
                jntuh_scmupload.CreatedBy = 1;
                jntuh_scmupload.CreatedOn = DateTime.Now;
                db.jntuh_scmupload.Add(jntuh_scmupload);
                db.SaveChanges();
                TempData["Success"] = "SCM Data Uploaded Successfully......";
            }
            else
            {
                TempData["Error"] = "Enter Data Mandatory Fields";
            }
            return RedirectToAction("SCMUpload");
        }



        [HttpGet]
        public ActionResult SCMUploadView(int? collegeId)
        {

            if (User.Identity.IsAuthenticated)
            {
                var jntuh_scmupload = db.jntuh_scmupload.Where(e => e.IsActive == true).Select(e => e).ToList();
                var collegeIds =
                    jntuh_scmupload.Where(e => e.IsActive == true).Select(e => e.CollegeId).Distinct().ToArray();
                ViewBag.Colleges =
                    db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id))
                        .Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName })
                        .OrderBy(e => e.collegeName)
                        .ToList();
                //Get the Deprtments Based on the CollegeId
                if (collegeId != null)
                {


                    //List<Scmdates> SCMdatesList = new List<Scmdates>();
                    //SCMdatesList.AddRange(db.jntuh_scmupload.Where(e => e.IsActive == true && e.CollegeId == collegeId && e.SpecializationId == departmentId).Select(e => new Scmdates() {SCMDATE = e.SCMdate, SCMDateId = e.Id}).ToList());

                    //ViewBag.SCMDates =SCMdatesList.Select(e =>new Scmdates(){SCMDATEview = UAAAS.Models.Utilities.MMDDYY2DDMMYY(e.SCMDATE.ToString()),SCMDateId = e.SCMDateId}).ToList();

                    //var specialization =db.jntuh_specialization.Where(e => e.isActive == true && e.id == departmentId).Select(e => e).ToList();

                    //ViewBag.MenuData = specialization.Select(e => new jntuh_specialization()
                    //{
                    //    specializationName = e.jntuh_department.jntuh_degree.degree + "-" + e.specializationName,
                    //    specializationDescription = e.jntuh_department.departmentName

                    //}).FirstOrDefault();

                    var jntuh_scmupload1 =
                        jntuh_scmupload.Where(
                            e => e.IsActive == true && e.CollegeId == collegeId && e.SpecializationId != 0 &&
                                 e.DepartmentId != 0 && e.DegreeId != 0).Select(e => e).ToList();

                    var allscmuploadeddata = (from SCMUPL in jntuh_scmupload1
                                              join SPEC in db.jntuh_specialization on SCMUPL.SpecializationId equals SPEC.id
                                              join DEPT in db.jntuh_department on SCMUPL.DepartmentId equals DEPT.id
                                              join DEG in db.jntuh_degree on SCMUPL.DegreeId equals DEG.id

                                              select new Scmuploads()
                                              {
                                                  ScmDate = UAAAS.Models.Utilities.MMDDYY2DDMMYY(SCMUPL.SCMdate.ToString()),
                                                  SpecializationName = DEG.degree + "-" + SPEC.specializationName,
                                                  ProfessorDocumentView = SCMUPL.ProfDocument,
                                                  AssociateProfessorDocumentView = SCMUPL.AssocDocument,
                                                  AssistantProfessorDocumentView = SCMUPL.AssistDocument,
                                                  CreatedDate = SCMUPL.CreatedOn
                                              }).OrderByDescending(e => e.CreatedDate).ToList();

                    ViewBag.SCMData = allscmuploadeddata.ToList();
                }


                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }


        [HttpGet]
        public ActionResult PrincipalSCMUploadView(int? collegeId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var jntuh_scmupload = db.jntuh_scmupload.Where(e => e.IsActive == true).Select(e => e).ToList();
                var collegeIds = jntuh_scmupload.Where(e => e.IsActive == true && e.SpecializationId == 0 &&
                                                            e.DepartmentId == 0 && e.DegreeId == 0)
                    .Select(e => e.CollegeId)
                    .Distinct()
                    .ToArray();
                ViewBag.Colleges =
                    db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id))
                        .Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName })
                        .OrderBy(e => e.collegeName)
                        .ToList();

                if (collegeId != null)
                {

                    var jntuh_scmupload1 =
                        jntuh_scmupload.Where(
                            e => e.IsActive == true && e.CollegeId == collegeId && e.SpecializationId == 0 &&
                                 e.DepartmentId == 0 && e.DegreeId == 0).Select(e => e).ToList();

                    var allscmuploadeddata = (from SCMUPL in jntuh_scmupload1
                                              where
                                                  SCMUPL.IsActive == true && SCMUPL.CollegeId == collegeId && SCMUPL.SpecializationId == 0 &&
                                                  SCMUPL.DepartmentId == 0 && SCMUPL.DegreeId == 0
                                              select new Scmuploads()
                                              {
                                                  ScmDate = UAAAS.Models.Utilities.MMDDYY2DDMMYY(SCMUPL.SCMdate.ToString()),
                                                  //  SpecializationName = DEG.degree + "-" + SPEC.specializationName,
                                                  ProfessorDocumentView = SCMUPL.ProfDocument,
                                                  CreatedDate = SCMUPL.CreatedOn
                                              }).OrderByDescending(e => e.CreatedDate).ToList();

                    ViewBag.SCMData = allscmuploadeddata.ToList();
                }


                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }


        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //public ActionResult TempFacultyVerification()
        //{

        //    List<FacultyRegistration> teachingFaculty = new List<FacultyRegistration>();
        //    // List<jntuh_registered_faculty> jntuh_registered_faculty = new List<jntuh_registered_faculty>();
        //    List<jntuh_registered_faculty_log> jntuh_registered_faculty_log = new List<jntuh_registered_faculty_log>();

        //    //string[] FacultyRegNumbers = db.jntuh_college_faculty_registered.Where(C => C.collegeId == collegeid).Select(C => C.RegistrationNumber).ToArray();
        //    //if (collegeid != null && collegeid != 0)
        //    jntuh_registered_faculty_log = db.jntuh_registered_faculty_log.Where(c=>c.isActive == true  && (c.FacultyApprovedStatus == 0 || c.FacultyApprovedStatus == 3)).ToList();//c => FacultyRegNumbers.Contains(c.RegistrationNumber)
        //    //else
        //    //    jntuh_registered_faculty_log = db.jntuh_registered_faculty_log.Where(c => c.isActive == true && c.FacultyApprovedStatus == 0).Take(50).ToList();
        //    var data = jntuh_registered_faculty_log.Select(a => new FacultyRegistration
        //    {

        //        id = db.jntuh_registered_faculty.Where(f => f.UserId == a.UserId).Select(f => f.id).FirstOrDefault(),
        //        Type = a.type,
        //        RegistrationNumber = a.RegistrationNumber,
        //        UniqueID = a.UniqueID,
        //        FirstName = a.FirstName,
        //        MiddleName = a.MiddleName,
        //        LastName = a.LastName,
        //        GenderId = a.GenderId,
        //        Email = a.Email,
        //        facultyPhoto = a.Photo,
        //        Mobile = a.Mobile,
        //        PANNumber = a.PANNumber,
        //        AadhaarNumber = a.AadhaarNumber,
        //        isActive = a.isActive,
        //        isApproved = a.isApproved,
        //        SamePANNumberCount = 1,
        //        SameAadhaarNumberCount = 2,
        //        FIsApproved = a.FacultyApprovedStatus


        //    });
        //    teachingFaculty.AddRange(data);
        //    return View(teachingFaculty);
        //}

        [HttpGet]
        [AuthorizedUserAccess("Admin", "FacultyVerification")]
        public ActionResult ApprovedFaculty(int facultyAddId, int collegeId)
        {
            if (User.Identity.IsAuthenticated)
          //  if (User.Identity.Name == "admin" || (User.Identity.Name == "v01") || (User.Identity.Name == "v02") || (User.Identity.Name == "v03") || (User.Identity.Name == "v04") || (User.Identity.Name == "v05") || (User.Identity.Name == "v06") || (User.Identity.Name == "v07") || (User.Identity.Name == "v08") || (User.Identity.Name == "v09") || (User.Identity.Name == "v10"))
            {
                //var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
                if (facultyAddId != 0 && collegeId != 0)
                {
                    var data = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == facultyAddId && e.FacultyType!=1).Select(e => e).FirstOrDefault();
                    var UserId = db.jntuh_registration.Where(e => e.Email == User.Identity.Name).Select(e => e.Id).FirstOrDefault();
                    if (data != null && UserId != 0)
                    {
                        data.IsApproved = true;
                        data.Updatedby = UserId;
                        data.UpdatedOn = DateTime.Now;
                        data.Isactive = true;
                        db.Entry(data).State = EntityState.Modified;
                        db.SaveChanges();

                        //var registraredFacultydata =
                        //    db.jntuh_registered_faculty.Where(
                        //        e => e.RegistrationNumber.Trim() == data.RegistrationNumber.Trim())
                        //        .Select(e => e)
                        //        .FirstOrDefault();
                        //if (registraredFacultydata != null)
                        //{
                        //    registraredFacultydata.NoSCM = false;
                        //    registraredFacultydata.updatedBy = 1;
                        //    registraredFacultydata.updatedOn = DateTime.Now;
                        //    db.Entry(registraredFacultydata).State = EntityState.Modified;
                        //    db.SaveChanges();
                        //}
                        //else
                        //{
                        //    TempData["Error"] = "Faculty Approved Failed";
                        //    // return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest",
                        //    return RedirectToAction("CollegewiseDetails", "Admin",
                        //        new { collegeId = collegeId });
                        //}
                        TempData["Success"] = "Faculty Approved Successfully";
                    }
                    
                    // return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest",
                    return RedirectToAction("CollegewiseDetails", "Admin",
                        new { collegeId = collegeId });
                }
                // return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest");
                return RedirectToAction("CollegewiseDetails", "Admin");
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }


        [HttpGet]
        [AuthorizedUserAccess("Admin", "FacultyVerification")]
        public ActionResult NotApproveFaculty(int facultyAddId, int collegeId)
        {
            if (User.Identity.IsAuthenticated)
          //  if (User.Identity.Name == "admin" || (User.Identity.Name == "v01") || (User.Identity.Name == "v02") || (User.Identity.Name == "v03") || (User.Identity.Name == "v04") || (User.Identity.Name == "v05") || (User.Identity.Name == "v06") || (User.Identity.Name == "v07") || (User.Identity.Name == "v08") || (User.Identity.Name == "v09") || (User.Identity.Name == "v10"))
            {
                Facultynotapproved data = new Facultynotapproved();
                if (facultyAddId != 0 && collegeId != 0)
                {
                    data.FacultyAddId = facultyAddId;
                    data.CollegeId = collegeId;
                    // data.DeactivationReason = "";
                }
                return PartialView("_NotApproveFaculty", data);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }


        [HttpPost]
        [AuthorizedUserAccess("Admin", "FacultyVerification")]
        public ActionResult NotApproveFaculty(Facultynotapproved facultynotapproved)
        {
            if (User.Identity.IsAuthenticated)
          //  if (User.Identity.Name == "admin" || (User.Identity.Name == "v01") || (User.Identity.Name == "v02") || (User.Identity.Name == "v03") || (User.Identity.Name == "v04") || (User.Identity.Name == "v05") || (User.Identity.Name == "v06") || (User.Identity.Name == "v07") || (User.Identity.Name == "v08") || (User.Identity.Name == "v09") || (User.Identity.Name == "v10"))
            {
                TempData["Error"] = null;
                //  int userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
                if (ModelState.IsValid)
                {
                    if (facultynotapproved != null)
                    {
                        UAAASSCM.Models.jntuh_scmproceedingrequest_addfaculty addfaculty = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == facultynotapproved.FacultyAddId && e.FacultyType != 1).Select(e => e).FirstOrDefault();
                        var UserId = db.jntuh_registration.Where(e => e.Email == User.Identity.Name).Select(e => e.Id).FirstOrDefault();
                        if (addfaculty != null && UserId!=0)
                        {
                            addfaculty.DeactiviationReason = facultynotapproved.DeactivationReason;
                            addfaculty.Updatedby = UserId;
                            addfaculty.UpdatedOn = DateTime.Now;
                            addfaculty.Isactive = true;
                            addfaculty.IsApproved = false;
                            db.Entry(addfaculty).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["Success"] = "Faculty Not Approved Successfully";
                        }
                        //  return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest",

                        return RedirectToAction("CollegewiseDetails", "Admin",
                            new { collegeId = facultynotapproved.CollegeId });
                    }
                }
                //  return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest");
                return RedirectToAction("CollegewiseDetails", "Admin");
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult CollegeScmProceedingsPrincipalRequest(int? CollegeId)
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                //  ViewBag.Colleges =db.jntuh_college.Where(e => e.isActive == true).Select(e => new {collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName}).OrderBy(e => e.collegeName).ToList();
                var jntuh_college = db.jntuh_college.Where(e => e.isActive == true).Select(e => e).ToList();
                ViewBag.Colleges =
                    jntuh_college.Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName })
                        .OrderBy(e => e.collegeId)
                        .ToList();

                jntuh_college.Add(new jntuh_college() { id = 0, collegeCode = "Not Working" });
                jntuh_college.Add(new jntuh_college() { id = -1, collegeCode = "Others" });
                ViewBag.PreviousColleges =
                    jntuh_college.Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName })
                        .OrderBy(e => e.collegeId)
                        .ToList();

                if (CollegeId != 0)
                {
                    List<SCMPrincipal> scmPrincipaldata = new List<SCMPrincipal>();
                    scmPrincipaldata = (from SCMREQ in db.jntuh_scmproceedingsrequests
                                        join SCMADDFLY in db.jntuh_scmproceedingrequest_addfaculty on SCMREQ.ID equals
                                            SCMADDFLY.ScmProceedingId
                                        join REGFLY in db.jntuh_registered_faculty on SCMADDFLY.RegistrationNumber equals
                                            REGFLY.RegistrationNumber
                                        where
                                            SCMREQ.CollegeId == CollegeId && SCMREQ.SpecializationId == 0 && SCMREQ.DegreeId == 0 &&
                                            SCMREQ.DEpartmentId == 0 && SCMADDFLY.FacultyType == 0
                                        select new SCMPrincipal
                                        {
                                            RegistrationNo = SCMADDFLY.RegistrationNumber,
                                            FirstName = REGFLY.FirstName + "-" + REGFLY.LastName,
                                            scmnotificationdocview = SCMREQ.SCMNotification,
                                            createdDate = SCMREQ.CreatedOn,
                                            FacultyId = REGFLY.id
                                        }).ToList();
                    ViewBag.SCMPrincipal = scmPrincipaldata;
                    ViewBag.SCMPrincipalcount = scmPrincipaldata.Count();
                }
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }


        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public ActionResult CollegeScmProceedingsPrincipalRequest(SCMPrincipal scmprincipaldata)
        {
            if (User.Identity.IsAuthenticated)
            //if (User.Identity.Name == "admin")
            {
                //  var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
                // var userCollegeId =db.jntuh_college_users.Where(u => u.userID == userId).Select(u => u.collegeID).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    var jntuh_college = db.jntuh_college.Where(e => e.isActive == true).Select(e => e).ToList();
                    int SCmOrder = 1;
                    var collegescmrequests = new jntuh_scmproceedingsrequests();
                    //  const string scmnotificationpath = "~/Content/Upload/SCMPROCEEDINGSREQUEST/ScmNotificationDocuments";
                    //if (scmprincipaldata.ScmNotificationSupportDoc != null)
                    //{
                    //    if (!Directory.Exists(Server.MapPath(scmnotificationpath)))
                    //    {
                    //        Directory.CreateDirectory(Server.MapPath(scmnotificationpath));
                    //    }

                    //    var ext = Path.GetExtension(scmprincipaldata.ScmNotificationSupportDoc.FileName);
                    //    if (ext != null && ext.ToUpper().Equals(".PDF"))
                    //    {
                    //        var scmfileName =
                    //            db.jntuh_college.Where(c => c.id == userCollegeId).Select(c => c.id).FirstOrDefault() +
                    //            "_" + "ScmNotofication" + "_" + DateTime.Now.ToString("yyyMMddHHmmss");
                    //        scmprincipaldata.ScmNotificationSupportDoc.SaveAs(string.Format("{0}/{1}{2}",
                    //            Server.MapPath(scmnotificationpath), scmfileName, ext));
                    //        collegescmrequests.SCMNotification = scmfileName + ext;
                    //    }
                    //    IUserMailer mailer = new UserMailer();
                    collegescmrequests.CollegeId = scmprincipaldata.CollegeId;
                    collegescmrequests.SpecializationId = 0;
                    collegescmrequests.DEpartmentId = 0;
                    collegescmrequests.DegreeId = 0;
                    collegescmrequests.ProfessorsCount = 0;
                    collegescmrequests.AssociateProfessorsCount = 0;
                    collegescmrequests.AssistantProfessorsCount = 0;
                    collegescmrequests.RequiredProfessor = 0;
                    collegescmrequests.RequiredAssistantProfessor = 0;
                    collegescmrequests.RequiredAssociateProfessor = 0;
                    if (scmprincipaldata.NotificationDate != null)
                        collegescmrequests.Notificationdate =
                            UAAAS.Models.Utilities.DDMMYY2MMDDYY(scmprincipaldata.NotificationDate);
                    collegescmrequests.CreatedBy = 1;
                    collegescmrequests.SCMNotification = "admin";
                    collegescmrequests.TotalNoofFacultyRequired = 0;
                    collegescmrequests.CreatedOn = DateTime.Now;
                    collegescmrequests.ISActive = true;


                    //Checking SCM Order Id
                    var scmdata =
                        db.jntuh_scmproceedingsrequests.Where(
                            e =>
                                e.ISActive == true && e.CollegeId == scmprincipaldata.CollegeId &&
                                e.SpecializationId == 0 &&
                                e.DEpartmentId == 0 && e.DegreeId == 0)
                            .OrderByDescending(e => e.ID)
                            .Select(e => e)
                            .FirstOrDefault();

                    if (scmdata != null)
                    {
                        var assigneddata =
                            db.jntuh_auditor_assigned.Where(e => e.ScmId == scmdata.ID)
                                .Select(e => e.Id)
                                .FirstOrDefault();
                        if (assigneddata != 0)
                        {
                            SCmOrder = scmdata.SCMOrder + 1;
                        }
                        else
                        {
                            SCmOrder = scmdata.SCMOrder;
                        }
                    }
                    collegescmrequests.SCMOrder = SCmOrder;

                    db.jntuh_scmproceedingsrequests.Add(collegescmrequests);
                    try
                    {
                        db.SaveChanges();
                        int scmId = collegescmrequests.ID;

                        if (scmId != 0)
                        {
                            jntuh_scmproceedingrequest_addfaculty addfaculty =
                                new jntuh_scmproceedingrequest_addfaculty();
                            addfaculty.ScmProceedingId = scmId;
                            addfaculty.RegistrationNumber = scmprincipaldata.RegistrationNo;
                            addfaculty.PreviousCollegeId = scmprincipaldata.PreviousCollegeId.ToString() == "-1"
                                ? scmprincipaldata.PreviousCollegeName
                                : scmprincipaldata.PreviousCollegeId != 0
                                    ? jntuh_college.Where(e => e.id == scmprincipaldata.PreviousCollegeId)
                                        .Select(e => e.collegeName)
                                        .FirstOrDefault()
                                    : scmprincipaldata.PreviousCollegeId.ToString();
                            ;
                            addfaculty.FacultyType = 0;
                            addfaculty.Createdby = 1;
                            addfaculty.CreatedOn = DateTime.Now;
                            addfaculty.Isactive = true;
                            db.jntuh_scmproceedingrequest_addfaculty.Add(addfaculty);
                            db.SaveChanges();
                            TempData["Success"] = "Your request has been proccessed successfully..";
                        }
                        //var attachments = new List<Attachment>();
                        //if (scmprincipaldata.ScmNotificationSupportDoc != null)
                        //{

                        //    fileName = Path.GetFileName(scmprincipaldata.ScmNotificationSupportDoc.FileName);
                        //    filepath = Path.Combine(Server.MapPath("~/Content/Attachments"), fileName);
                        //    scmprincipaldata.ScmNotificationSupportDoc.SaveAs(filepath);
                        //    attachments.Add(new Attachment(filepath));
                        //    mailer.SendAttachmentToAllColleges("sureshpalsa1@gmail.com", "", "",
                        //        "SCM PROCEEDINGS REQUEST", "Scm Requests", attachments).SendAsync();
                        //    TempData["Success"] = "Your request has been proccessed successfully..";
                        //}

                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                    validationError.PropertyName,
                                    validationError.ErrorMessage);
                            }
                        }
                    }
                    //}
                    //else
                    //{
                    //    TempData["Error"] = "Please Fill All Mandatory Fields..";
                    //}

                    return RedirectToAction("CollegeScmProceedingsPrincipalRequest",
                        new { CollegeId = scmprincipaldata.CollegeId });
                }
                return RedirectToAction("CollegeScmProceedingsPrincipalRequest");
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }


         [AuthorizedUserAccess("Admin")]
        public ActionResult ProfessorsUploadedCollegs()
        {
            if (User.Identity.IsAuthenticated)
            //if (User.Identity.Name == "admin")
            {
                var jntuh_scmproceedingsrequests = db.jntuh_scmproceedingsrequests.Where(e => e.ISActive == true).Select(e => e).ToList();
                var AddFacultyScmIds = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.FacultyType == 1).Select(e => e.ScmProceedingId).Distinct().ToArray();


                //  var jntuh_auditor_assign = db.jntuh_auditor_assigned.Where(e => e.IsActive == true).Select(e => e).ToList();


                //SCM Ids with Requests
                var SCMRequests = jntuh_scmproceedingsrequests.Where(e => e.ISActive == true && e.SpecializationId != 0 && e.DEpartmentId != 0 && e.DegreeId != 0).Select(e => e.ID).Distinct().ToArray();
                //   var AddFacultySCMRequests =AddFacultyScmIds.Where(e => SCMRequests.Contains(e)).Select(e => e).Distinct().ToArray();


                var collegeIds = jntuh_scmproceedingsrequests.Where(e => e.ISActive == true && e.SpecializationId != 0 && e.DEpartmentId != 0 && e.DegreeId != 0 &&
                            AddFacultyScmIds.Contains(e.ID) && e.RequestSubmittedDate != null).Select(e => e.CollegeId).Distinct().ToArray();
                //&& e.RequestSubmittedDate!=null

                List<jntuh_college> colleges = db.jntuh_college.Where(e => collegeIds.Contains(e.id)).Select(e => e).Distinct().ToList();


                ScmRequestList scmdata = new ScmRequestList();
                scmdata.SCmRequestList = new List<ScmRequestList>();
                List<ScmRequestList> scmRequestLists = new List<ScmRequestList>();
                foreach (var item in colleges)
                {
                    //bool Ismatch = false;
                    //var scmrequestIds = jntuh_scmproceedingsrequests.Where(e => e.CollegeId == item.id && e.SpecializationId != 0 && e.DEpartmentId != 0 && e.DegreeId != 0 && e.RequestSubmittedDate != null).Select(e => e.ID).Distinct().ToArray();
                    //var scmrequestsaddfaculty = AddFacultyScmIds.Where(e => scmrequestIds.Contains(e)).Select(e => e).Distinct().ToArray();
                    //var scmrequestassigncount = jntuh_auditor_assign.Where(e => scmrequestsaddfaculty.Contains(e.ScmId)).Select(e => e.ScmId).Distinct().Count();
                    //if (scmrequestassigncount == scmrequestsaddfaculty.Count())
                    //{
                    //    Ismatch = true;
                    //}
                    scmRequestLists.Add(new ScmRequestList()
                    {
                        Id = item.id,
                        CollegeCode = item.collegeCode,
                        CollegeName = item.collegeName,
                        //  IsAuditorAssigned = Ismatch,
                        Checked = false
                    });
                }
                scmdata.SCmRequestList.AddRange(scmRequestLists.OrderBy(e => e.CollegeName).Select(e => e).ToList());
                return View(scmdata);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }

         [AuthorizedUserAccess("Admin")]
        public ActionResult ProfessorsScmProceedingsRequestView(string CollegeId)
        {

          
          //  if (User.Identity.Name == "admin" && !string.IsNullOrEmpty(CollegeId))
            if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(CollegeId))
            {
                int collegeId = Convert.ToInt32(UAAAS.Models.Utilities.DecryptString(CollegeId, WebConfigurationManager.AppSettings["CryptoKey"]));
                if (collegeId != 0)
                {
                    var proceedingsRequests = new List<ScmProceedingsRequest>();
                    var firstOrDefault = db.jntuh_college.FirstOrDefault(a => a.id == collegeId);
                    var degrees = db.jntuh_degree.AsNoTracking().ToList();
                    var specializations = db.jntuh_specialization.AsNoTracking().ToList();
                    var departments = db.jntuh_department.AsNoTracking().ToList();

                    var ProfessorsSCMIds = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.FacultyType == 1).Select(e => e.ScmProceedingId).Distinct().ToArray();

                    var collegescmrequestslist = db.jntuh_scmproceedingsrequests.Where(i => i.CollegeId == collegeId && ProfessorsSCMIds.Contains(i.ID) && i.RequestSubmittedDate != null).Select(e => e).ToList();
                    //i.RequestSubmittedDate != null &&
                    foreach (var scm in collegescmrequestslist)
                    {
                        var specid = specializations.FirstOrDefault(i => i.id == scm.SpecializationId);
                        if (specid != null && firstOrDefault != null)
                        {
                            var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
                            var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));

                            if (degree != null)
                                proceedingsRequests.Add(new ScmProceedingsRequest
                                {

                                    CollegeName = firstOrDefault.collegeCode + " - " + firstOrDefault.collegeName,
                                    CollegeId = firstOrDefault.id,
                                    SpecializationId = scm.SpecializationId,
                                    ProfessorVacancies = scm.ProfessorsCount.ToString(),
                                    // AssociateProfessorVacancies = scm.AssociateProfessorsCount.ToString(),
                                    // AssistantProfessorVacancies = scm.AssistantProfessorsCount.ToString(),
                                    SpecializationName = degree.degree + " - " + specid.specializationName,
                                    DepartmentId = specid.departmentId,
                                    CreatedDate = scm.RequestSubmittedDate,
                                    ScmNotificationpath = scm.SCMNotification,
                                    RequiredProfessorVacancies = scm.RequiredProfessor.ToString(),
                                    // RequiredAssociateProfessorVacancies = scm.RequiredAssociateProfessor.ToString(),
                                    //    RequiredAssistantProfessorVacancies = scm.RequiredAssistantProfessor.ToString(),
                                    Id = scm.ID,


                                });
                        }
                    }
                    ViewBag.Professorsscmrequestslist = proceedingsRequests.OrderByDescending(e => e.CreatedDate).Select(e => e).ToList();
                    return View(proceedingsRequests.OrderByDescending(e => e.CreatedDate).Select(e => e).ToList());
                }
                else
                {
                    return RedirectToAction("ProfessorsUploadedCollegs");
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }


        public ActionResult ProfessorsScmProceedingRequestPrint(ScmRequestList scmlist)
        {
            if (scmlist.SCmRequestList.Count() != 0)
            {

                var checkedscmlistdata = scmlist.SCmRequestList.Where(e => e.Checked == true).ToList();
                if (checkedscmlistdata.Count() != 0)
                {
                    ////////////////////Word Code
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=Professor SCM Request Data Report.doc");
                    Response.ContentType = "application/vnd.ms-word ";
                    Response.Charset = string.Empty;
                    StringBuilder str = new StringBuilder();
                    str.Append(GetProfessorSCMRequestData(checkedscmlistdata));
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 60, 50, 60, 60);

                    pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    pdfDoc.SetMargins(60, 50, 60, 60);

                    string path = Server.MapPath("~/Content/PDFReports/SCMRequestToAll/Professors");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = path + "Professors SCm Requests Data" + "-" + DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf";
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.Create));

                    pdfDoc.Open();

                    List<IElement> parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(str.ToString()), null);

                    foreach (var htmlElement in parsedHtmlElements)
                    {
                        pdfDoc.Add((IElement)htmlElement);
                    }

                    pdfDoc.Close();

                    Response.Output.Write(str.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    TempData["Error"] = "Please select any one checkbox for the print";
                    return RedirectToAction("ProfessorsUploadedCollegs");
                }
            }
            return RedirectToAction("ProfessorsUploadedCollegs");
        }


        public string GetProfessorSCMRequestData(List<ScmRequestList> scmProceedings)
        {
            var collegeIds = scmProceedings.Where(e => e.Checked == true).Select(e => e.Id).Distinct().ToArray();

            var scmaddfacultyData = db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking().ToList();

            var NomineeassignSCMIds = db.jntuh_auditor_assigned.Select(e => e.ScmId).Distinct().ToArray();



            var ScmAddFacultySCMIds = scmaddfacultyData.Select(e => e.ScmProceedingId).Distinct().ToArray();

            var SCMData = (from SCMREQ in db.jntuh_scmproceedingsrequests
                           join CLG in db.jntuh_college on SCMREQ.CollegeId equals CLG.id
                           join SPEC in db.jntuh_specialization on SCMREQ.SpecializationId equals SPEC.id
                           join DPET in db.jntuh_department on SCMREQ.DEpartmentId equals DPET.id
                           join DEG in db.jntuh_degree on SCMREQ.DegreeId equals DEG.id
                           join SCMADD in db.jntuh_scmproceedingrequest_addfaculty on SCMREQ.ID equals SCMADD.ScmProceedingId
                           where collegeIds.Contains(SCMREQ.CollegeId) && ScmAddFacultySCMIds.Contains(SCMREQ.ID) && SCMREQ.RequestSubmittedDate != null
                           //&& SCMADD.FacultyType==1
                           //&& !NomineeassignSCMIds.Contains(SCMREQ.ID) && SCMREQ.RequestSubmittedDate != null
                           select new
                           {
                               CollegeId = SCMREQ.CollegeId,
                               CollegeCode = CLG.collegeCode,
                               CollegeName = CLG.collegeName,
                               Department = DEG.degree + "-" + SPEC.specializationName,
                               SCMId = SCMREQ.ID,
                               DepartmentId = SCMREQ.DEpartmentId,
                               DepartmentName = DPET.departmentName,
                               DegreeName = DEG.degree,
                               Reqprof = SCMREQ.RequiredProfessor,
                               // Reqassociprof = SCMREQ.RequiredAssociateProfessor,
                               // ReqAssistprof = SCMREQ.RequiredAssistantProfessor,
                               Availprof = SCMREQ.ProfessorsCount,
                               // Availassociprof = SCMREQ.AssociateProfessorsCount,
                               //  AvailAssistprof = SCMREQ.AssistantProfessorsCount,
                               FacultyType = SCMADD.FacultyType
                           }).ToList();


            string contextstring = string.Empty;
            contextstring += "<table border='1' style='width:100%'>";
            contextstring += "<tr>";
            contextstring += "<td width='5%'>S.No</td>";
            contextstring += "<td width='20%'>College Name</td>";
            contextstring += "<td width='25%'>Department Name</td>";
            contextstring += "<td width='50%'>University Nominee</td>";
            contextstring += "</tr>";
            int count = 1;


            var jntuh_address = (from CLGADDRESS in db.jntuh_address
                                 join DIST in db.jntuh_district on CLGADDRESS.districtId equals DIST.id
                                 where CLGADDRESS.addressTye == "COLLEGE"
                                 select new
                                 {
                                     CollegeId = CLGADDRESS.collegeId,
                                     Address = CLGADDRESS.address,
                                     Mandal = CLGADDRESS.mandal,
                                     District = DIST.districtName,
                                     Town = CLGADDRESS.townOrCity,
                                     PINcode = CLGADDRESS.pincode
                                 }).ToList();

            //db.jntuh_address.Where(e => e.addressTye == "COLLEGE").Select(e => e).ToList();


            SCMData = SCMData.Where(e => e.FacultyType == 1).Select(e => e).Distinct().ToList();


            foreach (var item in SCMData.Select(e => e.CollegeId).Distinct().ToArray())
            {
                var CollegeAddress = jntuh_address.Where(e => e.CollegeId == item).Select(e => e).FirstOrDefault();
                var SCMDatabycollegeId = SCMData.Where(e => e.CollegeId == item).Select(e => e).OrderBy(e => e.DepartmentName).ToList();

                var SCMDataByDepartmentId = SCMData.Where(e => e.CollegeId == item).Select(e => e).Select(e => e).OrderBy(e => e.DepartmentName).Distinct().ToList();

                //   var SCMDataByDepartmentId1 =SCMData.Where(e => e.CollegeId == item).Select(e => e).Select(e => e.DepartmentId).Distinct().ToArray();

                contextstring += "<tr>";
                contextstring += "<td width='5%' style='vertical-align:top'>" + count + "</td>";
                contextstring += "<td width='20%' style='vertical-align:top'>" + SCMDatabycollegeId[0].CollegeName +
                                 "<br/> " + CollegeAddress.Address + "<br/>" + CollegeAddress.Town + "(T), " +
                                 CollegeAddress.Mandal + "(M), <br/>" + CollegeAddress.District + "(D), PIN - " +
                                 CollegeAddress.PINcode + " <b>(" + SCMDatabycollegeId[0].CollegeCode + ")</b></td>";
                contextstring += "<td width='75%' colspan='2' style='vertical-align:top'><table border='1' width='100%'>";

                //sample 
                int deeploop = 1;

                foreach (var DeptId in SCMDataByDepartmentId)
                {
                    //  var totalcount =SCMDatabycollegeId.Where(e =>e.DepartmentName == DeptId.DepartmentName &&(e.DegreeName == "M.Tech" || e.DegreeName == "B.Tech")).Distinct().Count();
                    //var DeptName =jntuh_departments.Where(e => e.id == DeptId).Select(e => e.departmentName).FirstOrDefault();
                    //var DeptIds =jntuh_departments.Where(e => e.departmentName == DeptName).Select(e => e.id).Distinct().ToArray();
                    int indexof = SCMDataByDepartmentId.IndexOf(DeptId);
                    if (indexof > 0 &&
                        SCMDataByDepartmentId[indexof].DepartmentName !=
                        SCMDataByDepartmentId[indexof - 1].DepartmentName)
                    {
                        deeploop = 1;
                    }
                    if (deeploop == 1)
                    {

                        contextstring += "<tr><td width='33%'>";
                        foreach (var data in SCMDatabycollegeId.Where(e => e.DepartmentName == DeptId.DepartmentName).Select(e => e).ToList()) //e.DepartmentId==DeptId.DepartmentId
                        {

                            var professor = scmaddfacultyData.Where(e => e.ScmProceedingId == data.SCMId && e.FacultyType == 1).Select(e => e.ScmProceedingId).Count();
                            // var Associateprofessor =scmaddfacultyData.Where(e => e.ScmProceedingId == data.SCMId && e.FacultyType == 2).Select(e => e.ScmProceedingId).Count();
                            //  var Assistantprofessor =scmaddfacultyData.Where(e => e.ScmProceedingId == data.SCMId && e.FacultyType == 3).Select(e => e.ScmProceedingId).Count();

                            var Reqprofessor = data.Reqprof;
                            // var ReqAssociateprofessor = data.Reqassociprof;
                            // var ReqAssistantprofessor = data.ReqAssistprof;

                            var Availprofessor = data.Availprof;
                            //  var AvailAssociateprofessor = data.Availassociprof;
                            //  var AvailAssistantprofessor = data.AvailAssistprof;
                            contextstring += data.Department + " (" + professor + "/" + Reqprofessor + "/" + Availprofessor + ")<br/>";
                            // "+"
                            //+ Associateprofessor + "/" + ReqAssociateprofessor +"/" + AvailAssociateprofessor + "+" + Assistantprofessor + "/" +ReqAssistantprofessor + "/" + AvailAssistantprofessor + 
                        }
                        contextstring += "</td><td width='67%'></td></tr>";
                        deeploop++;
                    }
                }
                contextstring += "</table></td>";
                //  contextstring += "<td>" + " " + "</td>";
                contextstring += "</tr>";
                count++;
            }
            contextstring += "</table>";
            return contextstring;
        }

         [AuthorizedUserAccess("Admin")]
        public ActionResult FacultyVerificationForProfessors(int? collegeId)
        {
            if (User.Identity.IsAuthenticated)
             //if (User.Identity.Name == "admin")
            {
               

                var SCMRequestCollegesIds = (from SCM in db.jntuh_scmproceedingsrequests
                                             join SCMADD in db.jntuh_scmproceedingrequest_addfaculty on SCM.ID equals SCMADD.ScmProceedingId
                                             join CLG in db.jntuh_college on SCM.CollegeId equals CLG.id
                                             where SCMADD.FacultyType == 1 && SCM.RequestSubmittedDate!=null
                                             select new { collegeId = CLG.id, collegeName = CLG.collegeCode + "-" + CLG.collegeName }).ToList();


                var data = SCMRequestCollegesIds.GroupBy(e => e.collegeId).Select(e => new { collegeId = e.Key, collegeName = e.FirstOrDefault().collegeName }).OrderBy(e => e.collegeName).ToList();
                ViewBag.Colleges = data;

                // ViewBag.Colleges =db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id)).Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName }).OrderBy(e => e.collegeName).ToList();

                List<FacultyRegistrationDetails> teachingFaculty = new List<FacultyRegistrationDetails>();

                if (collegeId != null)
                {
                    //var jntuh_department = db.jntuh_department.ToList();
                    //var jntuh_specialization = db.jntuh_specialization.ToList();
                    //var jntuh_degree = db.jntuh_degree.ToList();
                   // var NotProfRegNumbers =db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.FacultyType == 2 || e.FacultyType == 3).Select(e => e.RegistrationNumber).ToArray();

                    teachingFaculty = (from SCM in db.jntuh_scmproceedingsrequests
                                       join SCMAdd in db.jntuh_scmproceedingrequest_addfaculty on SCM.ID equals SCMAdd.ScmProceedingId
                                       join REG in db.jntuh_registered_faculty on SCMAdd.RegistrationNumber equals REG.RegistrationNumber
                                       join Deg in db.jntuh_degree on SCM.DegreeId equals Deg.id
                                       join Dept in db.jntuh_department on SCM.DEpartmentId equals Dept.id
                                       join Spec in db.jntuh_specialization on SCM.SpecializationId equals Spec.id
                                       where SCM.CollegeId == collegeId && SCMAdd.FacultyType == 1 && SCM.RequestSubmittedDate != null 
                                       //&& !NotProfRegNumbers.Contains(SCMAdd.RegistrationNumber) 
                                       select new FacultyRegistrationDetails
                                       {
                                           id = REG.id,
                                           ScmproceedingId = SCMAdd.ScmProceedingId,
                                           RegistrationNumber = SCMAdd.RegistrationNumber,
                                           FirstName = REG.FirstName,
                                           MiddleName = REG.MiddleName,
                                           LastName = REG.LastName,
                                           isActive = REG.isActive,
                                           FacultyAddId = SCMAdd.Id,
                                           DeactivationReason = SCMAdd.DeactiviationReason,
                                           isApproved = SCMAdd.IsApproved,
                                           department = Dept.departmentName,
                                           specialization = Spec.specializationName,
                                           degree = Deg.degree,
                                           SCMRequestDated = SCM.RequestSubmittedDate,
                                           approvedby = (int)SCMAdd.Updatedby
                                           
                                           // UAAAS.Models.Utilities.MMDDYY2DDMMYY(SCM.RequestSubmittedDate.ToString())
                                           //jntuh_scmproceedingsrequests.Where(C => C.ID == SCMId).Select(C => C.RequestSubmittedDate).FirstOrDefault().ToString()

                                       }).ToList();



                    #region old code

                    // var jntuh_scmproceedingrequest_addfaculty =db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking().ToList();
                    // string[] strRegNoS = (from a in jntuh_scmproceedingsrequests
                    //                       join b in jntuh_scmproceedingrequest_addfaculty on a.ID equals b.ScmProceedingId
                    //                       where a.CollegeId == collegeId && b.FacultyType==1 && a.RequestSubmittedDate!=null
                    //                       select b.RegistrationNumber).Distinct().ToArray();


                    // int[]  RegnoSCMIds= (from a in jntuh_scmproceedingsrequests
                    //                       join b in jntuh_scmproceedingrequest_addfaculty on a.ID equals b.ScmProceedingId
                    //                       where a.CollegeId == collegeId && b.FacultyType == 1 && a.RequestSubmittedDate != null
                    //                       select a.ID).Distinct().ToArray();




                    // int[] SCMProccedingIds =jntuh_scmproceedingsrequests.Where(e => e.CollegeId == collegeId).Select(e => e.ID).Distinct().ToArray();

                    // List<jntuh_registered_faculty> jntuh_registered_faculty = new List<jntuh_registered_faculty>();

                    //// jntuh_registered_faculty =db.jntuh_registered_faculty.Where(rf => strRegNoS.Contains(rf.RegistrationNumber) && rf.Notin116 != true).ToList();

                    // jntuh_registered_faculty =db.jntuh_registered_faculty.Join(strRegNoS, reg => reg.RegistrationNumber, regno => regno,
                    //         (reg, regno) => new {reg = reg, regno = regno}).Select(e => e.reg).ToList();
                    //     //Where(rf => strRegNoS.Contains(rf.RegistrationNumber) && rf.Notin116 != true).ToList();





                    // var jntuhScmproceedingrequestAddfaculty =jntuh_scmproceedingrequest_addfaculty.Where(e => SCMProccedingIds.Contains(e.ScmProceedingId) && e.FacultyType==1).Select(e => e).ToList();



                    // string RegNumber = "";
                    // int? Specializationid = 0;
                    // int SCMId = 0;
                    // int DeptId = 0;
                    // int degId = 0;
                    // foreach (var a in jntuh_registered_faculty)
                    // {
                    //     string Reason = String.Empty;
                    //     SCMId = jntuhScmproceedingrequestAddfaculty.Where(C => C.RegistrationNumber.Trim() == a.RegistrationNumber.Trim() && RegnoSCMIds.Contains(C.ScmProceedingId)).Select(C => C.ScmProceedingId).FirstOrDefault();
                    //     Specializationid =jntuh_scmproceedingsrequests.Where(C => C.ID == SCMId).Select(C => C.SpecializationId).FirstOrDefault();
                    //     DeptId =jntuh_scmproceedingsrequests.Where(C => C.ID == SCMId).Select(C => C.DEpartmentId).FirstOrDefault();
                    //     degId =jntuh_scmproceedingsrequests.Where(C => C.ID == SCMId).Select(C => C.DegreeId).FirstOrDefault();
                    //     var faculty = new FacultyRegistrationDetails();
                    //     faculty.id = a.id;
                    //     faculty.ScmproceedingId = SCMId;
                    //     faculty.RegistrationNumber = a.RegistrationNumber;
                    //     faculty.FirstName = a.FirstName;
                    //     faculty.MiddleName = a.MiddleName;
                    //     faculty.LastName = a.LastName;
                    //     faculty.isActive = a.isActive;
                    //     faculty.FacultyAddId = jntuhScmproceedingrequestAddfaculty.Where(e => e.RegistrationNumber.Trim() == a.RegistrationNumber.Trim() && RegnoSCMIds.Contains(e.ScmProceedingId)).Select(e => e.Id).FirstOrDefault();
                    //     faculty.DeactivationReason = jntuhScmproceedingrequestAddfaculty.Where(e => e.RegistrationNumber.Trim() == a.RegistrationNumber.Trim() && RegnoSCMIds.Contains(e.ScmProceedingId)).Select(e => e.DeactiviationReason).FirstOrDefault();
                    //     faculty.isApproved = jntuhScmproceedingrequestAddfaculty.Where(e => e.RegistrationNumber.Trim() == a.RegistrationNumber.Trim() && RegnoSCMIds.Contains(e.ScmProceedingId)).Select(e => e.IsApproved).FirstOrDefault();
                    //     faculty.department =jntuh_department.Where(d => d.id == DeptId).Select(d => d.departmentName).FirstOrDefault();
                    //     faculty.specialization =jntuh_specialization.Where(d => d.id == Specializationid).Select(d => d.specializationName).FirstOrDefault();
                    //     faculty.degree = jntuh_degree.Where(d => d.id == degId).Select(d => d.degree).FirstOrDefault();
                    //     faculty.SCMRequestDate =UAAAS.Models.Utilities.MMDDYY2DDMMYY(jntuh_scmproceedingsrequests.Where(C => C.ID == SCMId).Select(C => C.RequestSubmittedDate).FirstOrDefault().ToString());
                    //     teachingFaculty.Add(faculty);
                    // }
                    // teachingFaculty =teachingFaculty.Where(m => m.isActive == true).OrderByDescending(e => e.ScmproceedingId).ToList();
                    #endregion
                    return View(teachingFaculty);
                }

                return View(teachingFaculty);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
         public ActionResult ApprovedProfessorFaculty(string facultyRegno, int collegeId)
        {
            //var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
            if (!string.IsNullOrEmpty(facultyRegno) && collegeId != 0)
            {
                var data = db.jntuh_scmproceedingrequest_addfaculty.Join(db.jntuh_scmproceedingsrequests, ADD => ADD.ScmProceedingId, SCM => SCM.ID, (ADD, SCM) => new { ADD = ADD, SCM = SCM }).Where(e => e.ADD.RegistrationNumber == facultyRegno && e.ADD.FacultyType == 1 &&e.SCM.RequestSubmittedDate!=null).Select(e => e.ADD).ToList();
                var UserId = db.jntuh_registration.Where(e => e.Email == User.Identity.Name).Select(e => e.Id).FirstOrDefault();
                if (data.Count() != 0 && UserId!=0)
                {
                    foreach (var approvedData in data)
                    {
                        if (approvedData != null)
                        {
                            var ApprovedFaculty =db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == approvedData.Id).Select(e => e).FirstOrDefault();
                            if (ApprovedFaculty != null)
                            {
                                ApprovedFaculty.IsApproved = true;
                                ApprovedFaculty.Updatedby = UserId;
                                ApprovedFaculty.DeactiviationReason = "";
                                ApprovedFaculty.UpdatedOn = DateTime.Now;
                                ApprovedFaculty.Isactive = true;
                                db.Entry(ApprovedFaculty).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                    }
                    #region oldcode
                    //var registraredFacultydata =
                    //    db.jntuh_registered_faculty.Where(
                    //        e => e.RegistrationNumber.Trim() == data.RegistrationNumber.Trim())
                    //        .Select(e => e)
                    //        .FirstOrDefault();
                    //if (registraredFacultydata != null)
                    //{
                    //    registraredFacultydata.NoSCM = false;
                    //    registraredFacultydata.updatedBy = 1;
                    //    registraredFacultydata.updatedOn = DateTime.Now;
                    //    db.Entry(registraredFacultydata).State = EntityState.Modified;
                    //    db.SaveChanges();
                    //}
                    //else
                    //{
                    //    TempData["Error"] = "Faculty Approved Failed";
                    //    // return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest",
                    //    return RedirectToAction("CollegewiseDetails", "Admin",
                    //        new { collegeId = collegeId });
                    //}
                    #endregion

                    TempData["Success"] = "Faculty Approved Successfully";
                }
                else
                {
                    TempData["Error"] = "Faculty Approved Failed";
                }
               
                // return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest",
                return RedirectToAction("FacultyVerificationForProfessors", "CollegeSCMProceedingsRequest",
                    new { collegeId = collegeId });
            }
            // return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest");
            return RedirectToAction("FacultyVerificationForProfessors", "CollegeSCMProceedingsRequest");
        }


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult NotApproveProfessorFaculty(string facultyRegno, int collegeId)
        {
            Facultynotapproved data = new Facultynotapproved();
            if (!string.IsNullOrEmpty(facultyRegno) && collegeId != 0)
            {
                data.FacultyRegno = facultyRegno;
                data.CollegeId = collegeId;
                // data.DeactivationReason = "";
            }
            return PartialView("_NotApproveProfessorFaculty", data);
        }

        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public ActionResult NotApproveProfessorFaculty(Facultynotapproved facultynotapproved)
        {
            TempData["Error"] = null;
            //  int userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
            if (ModelState.IsValid)
            {
                if (facultynotapproved != null)
                {
                   // var addfaculty = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.RegistrationNumber == facultynotapproved.FacultyRegno && e.FacultyType == 1).Select(e => e).ToList();

                    var addfaculty = db.jntuh_scmproceedingrequest_addfaculty.Join(db.jntuh_scmproceedingsrequests, ADD => ADD.ScmProceedingId, SCM => SCM.ID, (ADD, SCM) => new { ADD = ADD, SCM = SCM }).Where(e => e.ADD.RegistrationNumber == facultynotapproved.FacultyRegno && e.ADD.FacultyType == 1 && e.SCM.RequestSubmittedDate != null).Select(e => e.ADD).ToList();
                    var UserId = db.jntuh_registration.Where(e => e.Email == User.Identity.Name).Select(e => e.Id).FirstOrDefault();
                    if (addfaculty.Count() != 0 && UserId!=0)
                    {
                        foreach (var approvedData in addfaculty)
                        {
                            if (approvedData != null)
                            {
                                var ApprovedFaculty = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == approvedData.Id).Select(e => e).FirstOrDefault();
                                if (ApprovedFaculty != null)
                                {
                                    ApprovedFaculty.DeactiviationReason = facultynotapproved.DeactivationReason;
                                    ApprovedFaculty.Updatedby = UserId;
                                    ApprovedFaculty.UpdatedOn = DateTime.Now;
                                    ApprovedFaculty.Isactive = true;
                                    ApprovedFaculty.IsApproved = false;
                                    db.Entry(ApprovedFaculty).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }

                        }
                        
                        TempData["Success"] = "Faculty Not Approved Successfully";
                    }
                    else
                    {
                        TempData["Error"] = "Faculty Not Approved Failed";
                    }
                    //  return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest",

                    return RedirectToAction("FacultyVerificationForProfessors", "CollegeSCMProceedingsRequest",
                        new { collegeId = facultynotapproved.CollegeId });
                }
            }
            //  return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest");
            return RedirectToAction("FacultyVerificationForProfessors", "CollegeSCMProceedingsRequest");
        }


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult NomineeReassignView(int? collegeId)
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                List<Nomineereassign> NomineeAssigneddata = new List<Nomineereassign>();

                List<Nomineereassign> NomineeAssignedFinaldata = new List<Nomineereassign>();

                List<Nomineereassign> NomineeAssignedFinaldata1 = new List<Nomineereassign>();

                var SCMIds = (from SCM in db.jntuh_scmproceedingsrequests join auditAssign in db.jntuh_auditor_assigned on SCM.ID equals auditAssign.ScmId select SCM.ID).ToArray();
                var DataEntrySCMIds = db.jntuh_auditors_dataentry.GroupBy(e => e.ScmProceedingId).Select(e => e.Key).ToArray();
                var NomineeAssignSCMIds = SCMIds.Where(e => !DataEntrySCMIds.Contains(e)).Select(e => e).Distinct().ToArray();
                var Collegesdata = (from SCM in db.jntuh_scmproceedingsrequests
                                    join CLG in db.jntuh_college on SCM.CollegeId equals CLG.id
                                    where NomineeAssignSCMIds.Contains(SCM.ID)
                                    select new
                                    {
                                        CollegeId = CLG.id,
                                        CollegeName = CLG.collegeCode + "-" + CLG.collegeName
                                    }).ToList();
                ViewBag.Colleges = Collegesdata.GroupBy(e => e.CollegeId).Select(e => new { CollegeId = e.Key, CollegeName = e.FirstOrDefault().CollegeName }).OrderBy(e => e.CollegeName).ToList();

                if (collegeId != 0 && collegeId != null)
                {

                    //checking purpose unused
                    var NomineeAssigndIds = (from SCM in db.jntuh_scmproceedingsrequests
                                             join AUDASSIGN in db.jntuh_auditor_assigned on SCM.ID equals AUDASSIGN.ScmId
                                             where NomineeAssignSCMIds.Contains(SCM.ID) && SCM.CollegeId == collegeId && SCM.SpecializationId != 0
                                             select new
                                             {
                                                 SCMId = SCM.ID,
                                                 AUDId = AUDASSIGN.AuditorId
                                             }).ToList();

                    //unused end



                    NomineeAssigneddata = (from SCM in db.jntuh_scmproceedingsrequests
                                           join AUDASSIGN in db.jntuh_auditor_assigned on SCM.ID equals AUDASSIGN.ScmId
                                           join SPEC in db.jntuh_specialization on SCM.SpecializationId equals SPEC.id
                                           join DEPT in db.jntuh_department on SCM.DEpartmentId equals DEPT.id
                                           join DEG in db.jntuh_degree on SCM.DegreeId equals DEG.id
                                           join FF in db.jntuh_ffc_auditor on AUDASSIGN.AuditorId equals FF.id
                                           where NomineeAssignSCMIds.Contains(SCM.ID) && SCM.CollegeId == collegeId && SCM.SpecializationId != 0
                                           select new Nomineereassign
                                             {
                                                 SCMId = SCM.ID,
                                                 NomineeId = AUDASSIGN.AuditorId,
                                                 CollegeId = SCM.CollegeId,
                                                 DeparttmentId = DEPT.id,
                                                 DegreeId = DEG.id,
                                                 SPEcializationId = SPEC.id,
                                                 Degree = DEG.degree,
                                                 DepartmentName = DEPT.departmentName,
                                                 SpecializationName = SPEC.specializationName,
                                                 NomineeName = FF.auditorName,
                                                 AuditorAssignedDate = AUDASSIGN.CreatedOn,
                                                 Aid = FF.id,
                                                 ReassignedDate = AUDASSIGN.NomineeReassignDate,

                                             }).ToList();


                    List<Nomineereassign> NomineeAssigneddatawithDate = new List<Nomineereassign>();

                    foreach (var data in NomineeAssigneddata)
                    {

                        if (data != null)
                        {
                            data.viewAuditorAssignedDate = UAAAS.Models.Utilities.MMDDYY2DDMMYY(data.AuditorAssignedDate.ToString());
                            data.viewReassignedDate = data.ReassignedDate != null ? UAAAS.Models.Utilities.MMDDYY2DDMMYY(data.ReassignedDate.ToString()) : "---";
                            NomineeAssigneddatawithDate.Add(data);
                        }

                    }


                    var Dates = NomineeAssigneddatawithDate.GroupBy(e => e.viewAuditorAssignedDate).Select(e => e.Key).Distinct().ToArray();

                    foreach (var item in Dates)
                    {
                        var NomineDetailsDate = NomineeAssigneddatawithDate.Where(e => e.viewAuditorAssignedDate == item && e.CollegeId == collegeId).Select(e => e).ToList();
                        if (NomineDetailsDate.Count != 0)
                        {
                            int[] AuditorIds = NomineDetailsDate.GroupBy(e => e.NomineeId).Select(e => e.Key).ToArray();

                            foreach (var AId in AuditorIds)
                            {
                                NomineeAssignedFinaldata.AddRange(NomineDetailsDate.Where(e => e.NomineeId == AId).Select(e => e).ToList());
                            }
                        }

                    }


                    var AssignedSCMIds = NomineeAssignedFinaldata.GroupBy(e => e.SCMId).Select(e => e.Key).ToArray();

                    foreach (var scmId in AssignedSCMIds)
                    {
                        string AuditorAssignIds = string.Empty;
                        string AuditorNames = string.Empty;
                        var AuditorsData = NomineeAssignedFinaldata.Where(e => e.SCMId == scmId).Select(e => e).ToList();
                        foreach (var ids in AuditorsData)
                        {
                            if (string.IsNullOrEmpty(AuditorAssignIds))
                            {
                                AuditorAssignIds += ids.Aid + " ,";
                            }
                            else
                            {
                                AuditorAssignIds += ids.Aid + ",";
                            }




                            if (string.IsNullOrEmpty(AuditorNames))
                            {
                                AuditorNames += ids.NomineeName + " ,";
                            }
                            else
                            {
                                AuditorNames += ids.NomineeName + ",";
                            }

                        }

                        AuditorAssignIds = AuditorAssignIds.Substring(0, AuditorAssignIds.Length - 1);
                        AuditorNames = AuditorNames.Substring(0, AuditorNames.Length - 1);

                        Nomineereassign dataNomineereassign = new Nomineereassign();
                        var data = NomineeAssignedFinaldata.Where(e => e.SCMId == scmId).Select(e => e).FirstOrDefault();

                        if (data != null)
                        {
                            dataNomineereassign.Aid = data.Aid;
                            dataNomineereassign.Degree = data.Degree;
                            dataNomineereassign.DepartmentName = data.DepartmentName;
                            dataNomineereassign.SpecializationName = data.SpecializationName;
                            dataNomineereassign.SCMId = data.SCMId;
                            dataNomineereassign.NomineeId = data.NomineeId;
                            dataNomineereassign.NomineeName = AuditorNames;
                            dataNomineereassign.CollegeId = data.CollegeId;
                            dataNomineereassign.SPEcializationId = data.SPEcializationId;
                            dataNomineereassign.AuditorAssignedDate = data.AuditorAssignedDate;
                            dataNomineereassign.viewAuditorAssignedDate = data.viewAuditorAssignedDate;
                            dataNomineereassign.AuditorIds = AuditorAssignIds;
                            dataNomineereassign.viewReassignedDate = data.viewReassignedDate;

                            NomineeAssignedFinaldata1.Add(dataNomineereassign);
                        }

                    }





                }
                return View(NomineeAssignedFinaldata.OrderBy(e => e.SCMId).Select(e => e).ToList());
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult AuditorReAssign(int collegeId, int SCMId, string AssignedDate, int AuditorId)
        {
            if (collegeId != 0 && SCMId != 0 && AuditorId != 0 && AssignedDate != null)
            {
                var DeptId = db.jntuh_scmproceedingsrequests.Where(e => e.ID == SCMId && e.SpecializationId != 0).Select(e => e.DEpartmentId).FirstOrDefault();
                var jntuh_departments = db.jntuh_department.ToList();


                //Pharmacy Departments
                var pharmacyDeptIds = new int[] { 26, 27, 36, 39, 61, 64 };



                int[] DepartIds = jntuh_departments.Where(e => e.degreeId == 1 && e.isActive == true).Select(e => e.id).ToArray();
                if (DepartIds.Contains(DeptId))
                {
                    var deptname = jntuh_departments.Where(e => e.id == DeptId).Select(e => e.departmentName).FirstOrDefault();
                    int BtechDeptId = jntuh_departments.Where(e => e.departmentName == deptname && e.degreeId == 4).Select(e => e.id).FirstOrDefault();
                    DeptId = BtechDeptId;
                }
                else if (pharmacyDeptIds.Contains(DeptId))//Pharmacy Nominess to any Pharmacy Dept
                {
                    DeptId = 26;
                }
                else if (DeptId == 35)
                {
                    DeptId = 3;
                }
                else if (DeptId == 6)
                {
                    DeptId = 15;
                }
                else if (DeptId == 4)
                {
                    DeptId = 3;
                }
                else if (DeptId == 17) //Assign MMT(48) Nominess --->Mining(17)
                {
                    DeptId = 48;
                }
                else if (DeptId == 13) //Assign ECE(2) Nominess --->ETE(13)
                {
                    DeptId = 2;
                }
                else if (DeptId == 73) //Assign Civil(5) Nominess --->Other(civil)(73)
                {
                    DeptId = 5;
                }
                else if (DeptId == 11) //Assign CSE(3) Nominess --->ECM(11)
                {
                    DeptId = 3;
                }

                var OthersDeptIds = new int[] { 65, 66, 67, 68 };



                ReassignAuditorsCheck Check = new ReassignAuditorsCheck();
                Check.Auditors = new List<ReassignAuditors>();

                // List<ReassignAuditors> aditors = new List<ReassignAuditors>();

                if (OthersDeptIds.Contains(DeptId))
                {
                    int?[] DepIds = new int?[5];
                    if (DeptId == 65)
                    {
                        DepIds = new int?[] { 3 };
                    }
                    else if (DeptId == 66)
                    {
                        DepIds = new int?[] { 5, 15 };
                    }
                    else if (DeptId == 67)
                    {
                        DepIds = new int?[] { 1, 2 };
                    }
                    else if (DeptId == 68)
                    {
                        DepIds = new int?[] { 28, 31 };
                    }

                    Check.Auditors = (from Adts in db.jntuh_ffc_auditor
                                      join Desg in db.jntuh_designation on Adts.auditorDesignationID equals Desg.id
                                      where DepIds.Contains(Adts.auditorDepartmentID) && Adts.isActive == true
                                      select new ReassignAuditors()
                                      {
                                          //   SCMIds = scmIds,
                                          SCMRequestId = SCMId,
                                          AditorId = Adts.id,
                                          AditorName = Adts.auditorName,
                                          Designation = Desg.designation,
                                          DesignationId = Desg.id,
                                          DepartmentId = Adts.auditorDepartmentID ?? 0,
                                          Checke = false,
                                          CollegeId = collegeId,
                                          AssignedAuditorId = AuditorId,
                                          AssignedDate = AssignedDate
                                      }).Distinct().ToList();

                }
                else
                {
                    Check.Auditors = (from Adts in db.jntuh_ffc_auditor
                                      join Desg in db.jntuh_designation on Adts.auditorDesignationID equals Desg.id
                                      where Adts.auditorDepartmentID == DeptId && Adts.isActive == true
                                      select new ReassignAuditors()
                                      {
                                          // SCMIds = scmIds,
                                          AditorId = Adts.id,
                                          AditorName = Adts.auditorName,
                                          Designation = Desg.designation,
                                          DesignationId = Desg.id,
                                          DepartmentId = Adts.auditorDepartmentID ?? 0,
                                          Checke = false,
                                          CollegeId = collegeId,
                                          AssignedAuditorId = AuditorId,
                                          AssignedDate = AssignedDate
                                      }).Distinct().ToList();
                }

                List<ScmProceedingsRequest> scmdata = new List<ScmProceedingsRequest>();

                var scmIds = db.jntuh_auditor_assigned.Where(e => e.CollegeId == collegeId && e.AuditorId == AuditorId).Select(e => e.ScmId).ToArray();



                scmdata = (from SCMReq in db.jntuh_scmproceedingsrequests
                           join CLG in db.jntuh_college on SCMReq.CollegeId equals CLG.id
                           join DEG in db.jntuh_degree on SCMReq.DegreeId equals DEG.id
                           join DEPT in db.jntuh_department on SCMReq.DEpartmentId equals DEPT.id
                           join SPEC in db.jntuh_specialization on SCMReq.SpecializationId equals SPEC.id
                           join Assign in db.jntuh_auditor_assigned on SCMReq.ID equals Assign.ScmId
                           where scmIds.Contains(SCMReq.ID)
                           select new ScmProceedingsRequest
                           {

                               CollegeCode = CLG.collegeCode,
                               CollegeName = CLG.collegeName,
                               DepartmentName = DEG.degree + " " + SPEC.specializationName,
                               CreatedDate = (DateTime)Assign.CreatedOn,
                               Id = Assign.AuditorId,
                               CollegeId = SCMReq.CollegeId
                           }).ToList();


                foreach (var scmdatas in scmdata)
                {
                    if (scmdatas.CreatedDate != null)
                    {
                        scmdatas.NotificationDate = UAAAS.Models.Utilities.MMDDYY2DDMMYY(scmdatas.CreatedDate.ToString());
                    }
                }


                scmdata = scmdata.Where(e => e.Id == AuditorId && e.CollegeId == collegeId && e.NotificationDate == AssignedDate).Select(e => e).ToList();



                ViewBag.SelectedSCMRequest = scmdata;
                return PartialView("_AuditorReAssign", Check);
            }
            return RedirectToAction("NomineeReassignView", "CollegeSCMProceedingsRequest");
        }

        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public async Task<ActionResult> AuditorReAssign(ReassignAuditorsCheck reassignAuditors)
        {
            List<ReassignAuditors> aditorsdata = reassignAuditors.Auditors;
            if (aditorsdata.Count() != 0)
            {
                string DepartmentText = string.Empty;
                string OldAuditorIds = string.Empty;
                bool status = false;
                IEnumerable<string> CollegeEmailIds;

                var details = aditorsdata.Where(e => e.Checke == true).Select(e => e).ToList();
                if (details.Count() != 0)
                {
                    if (details.Count() >= 1 && details.Count() < 2)
                    {
                        IEnumerable<jntuh_auditor_assigned> jntuh_auditor_assigneds = db.jntuh_auditor_assigned.AsNoTracking().ToList();
                        List<AuditorAssignedDatalist> auditorAssignedDatalists = new List<AuditorAssignedDatalist>();
                        var NomineeAssignedRequests = jntuh_auditor_assigneds.Where(e => e.CollegeId == details[0].CollegeId).Select(e => e).ToList();
                        if (NomineeAssignedRequests.Count() != 0)
                        {

                            foreach (var Auditors in NomineeAssignedRequests)
                            {
                                if (Auditors != null)
                                {
                                    AuditorAssignedDatalist auditorAssignedData = new AuditorAssignedDatalist();
                                    auditorAssignedData.Id = Auditors.Id;
                                    auditorAssignedData.ScmId = Auditors.ScmId;
                                    auditorAssignedData.CollegeId = Auditors.CollegeId;
                                    auditorAssignedData.AuditorId = Auditors.AuditorId;
                                    auditorAssignedData.OldAuditorIds = Auditors.OldAuditorIds;
                                    auditorAssignedData.SCMListPath = Auditors.SCMListPath;
                                    auditorAssignedData.RequestSubmittedDate = Auditors.RequestSubmittedDate;
                                    auditorAssignedData.NomineeReassignDate = Auditors.NomineeReassignDate;
                                    auditorAssignedData.IsActive = Auditors.IsActive;
                                    auditorAssignedData.CreatedBy = Auditors.CreatedBy;
                                    auditorAssignedData.CreatedOn = Auditors.CreatedOn;
                                    auditorAssignedData.UpdatedBy = Auditors.UpdatedBy;
                                    auditorAssignedData.UpdatedOn = Auditors.UpdatedOn;
                                    auditorAssignedData.viewAssignedDate =
                                        UAAAS.Models.Utilities.MMDDYY2DDMMYY(Auditors.CreatedOn.ToString());
                                    auditorAssignedDatalists.Add(auditorAssignedData);
                                }
                            }
                        }
                        var ScmIdss =
                            auditorAssignedDatalists.Where(
                                e =>
                                    e.AuditorId == details[0].AssignedAuditorId && e.CollegeId == details[0].CollegeId &&
                                    e.viewAssignedDate == details[0].AssignedDate).Select(e => e.ScmId).ToArray();

                        var PreviousAuditorIds =
                            auditorAssignedDatalists.Where(
                                e =>
                                    e.CollegeId == details[0].CollegeId && e.viewAssignedDate == details[0].AssignedDate &&
                                    ScmIdss.Contains(e.ScmId)).Select(e => e.AuditorId).ToArray();
                        if (PreviousAuditorIds.Count() != 0)
                        {
                            foreach (var AuditorId in PreviousAuditorIds)
                            {
                                if (!string.IsNullOrEmpty(OldAuditorIds))
                                {
                                    if (!OldAuditorIds.Contains(AuditorId.ToString()))
                                        OldAuditorIds += "," + AuditorId.ToString();
                                }
                                else
                                {
                                    OldAuditorIds = AuditorId.ToString();
                                }
                            }
                        }

                        int CollegeId = (int)details[0].CollegeId;
                        //Get College Details
                        var collegedata = db.jntuh_college.Where(e => e.id == CollegeId).Select(e => e).FirstOrDefault();

                        //Get Department 
                        var collegedataDeptment = (from SCMREQ in db.jntuh_scmproceedingsrequests
                                                   join SCMADD in db.jntuh_scmproceedingrequest_addfaculty on SCMREQ.ID equals
                                                       SCMADD.ScmProceedingId
                                                   join SPEC in db.jntuh_specialization on SCMREQ.SpecializationId equals SPEC.id
                                                   join DEPT in db.jntuh_department on SCMREQ.DEpartmentId equals DEPT.id
                                                   join DEG in db.jntuh_degree on SCMREQ.DegreeId equals DEG.id
                                                   where
                                                       ScmIdss.Contains(SCMREQ.ID) && SCMADD.FacultyType != 1 &&
                                                       SCMREQ.RequestSubmittedDate != null
                                                   select new
                                                   {
                                                       Deg = DEG.degree,
                                                       Spec = SPEC.specializationName,
                                                       DeptId = DEPT.id,
                                                       SpecId = SPEC.id
                                                   }).ToList();

                        foreach (
                            var dept in
                                collegedataDeptment.GroupBy(e => e.SpecId)
                                    .Select(
                                        e =>
                                            new
                                            {
                                                Deg = e.FirstOrDefault().Deg,
                                                Spec = e.FirstOrDefault().Spec,
                                                DistinctSpecializationsId = e.Key
                                            })
                                    .ToList())
                        {
                            DepartmentText += dept.Deg + "-" + dept.Spec + ",";
                        }
                        DepartmentText = DepartmentText.Substring(0, DepartmentText.Length - 1);




                        int AuditorId1 = (int)details[0].AditorId;

                        List<jntuh_ffc_auditor> aditorslist =
                            db.jntuh_ffc_auditor.Where(e => e.isActive == true && e.id == AuditorId1)
                                .Select(e => e)
                                .ToList();
                        var nomineeRegistration = db.jntuh_registration.AsNoTracking().ToList();







                        //second Attachment Get Path of the File


                        var filepathsecond = SaveSCMReportSeconadAttachment(0, CollegeId);
                        filepathsecond = filepathsecond.Replace("/", "\\");


                        if (reassignAuditors.IsNeworOld == true)
                        {
                            //Re assign Nominee Code

                            var AuditorslistUpdate =
                                jntuh_auditor_assigneds.Where(
                                    e =>
                                        e.CollegeId == details[0].CollegeId && ScmIdss.Contains(e.ScmId) &&
                                        e.AuditorId == details[0].AssignedAuditorId).Select(e => e).ToList();
                            foreach (var Auditorsdata in AuditorslistUpdate)
                            {
                                if (Auditorsdata.Id != 0)
                                {
                                    Auditorsdata.OldAuditorIds = OldAuditorIds;
                                    Auditorsdata.NomineeReassignDate = DateTime.Now;
                                    Auditorsdata.AuditorId = details[0].AditorId;
                                    Auditorsdata.UpdatedBy = 1;
                                    Auditorsdata.UpdatedOn = DateTime.Now;
                                    db.Entry(Auditorsdata).State = EntityState.Modified;
                                }
                            }
                            db.SaveChanges();



                            //Get Co-Nominee Name Reassigned 
                            var CoNomineeName =
                                db.jntuh_auditor_assigned.Join(db.jntuh_ffc_auditor, ASS => ASS.AuditorId, FFC => FFC.id,
                                    (ASS, FFC) => new { ASS = ASS, FFC = FFC })
                                    .Where(
                                        e =>
                                            e.ASS.CollegeId == CollegeId && e.ASS.AuditorId != AuditorId1 &&
                                            ScmIdss.Contains(e.ASS.ScmId))
                                    .Select(e => new { Conominee = e.FFC.auditorName, Filepath = e.ASS.SCMListPath })
                                    .FirstOrDefault();


                            foreach (var item in aditorslist)
                            {
                                if (item != null)
                                {
                                    string UserName = string.Empty;
                                    string Password = string.Empty;
                                    var nomineeusernameandpassword =
                                        nomineeRegistration.Where(e => e.Email.Trim() == item.auditorEmail1.Trim())
                                            .Select(e => e)
                                            .FirstOrDefault();
                                    if (nomineeusernameandpassword != null)
                                    {
                                        UserName = nomineeusernameandpassword.Email;
                                        Password = nomineeusernameandpassword.Password ==item.auditorMobile1.Substring(5, 5)? nomineeusernameandpassword.Password: "***********";
                                    }

                                    //Send Mail to Nominee For SCM Request Body
                                    //
                                    string mailsendtonomineesubject =
                                        "JNTUH-DUAAC: Allotment of University Nominee for Faculty Selection:" +
                                        DepartmentText + ": Reg";
                                    string mailsendtonomineebody =
                                        "<div style='border:1px solid red;padding:10px;background:beige'><div><p>Dear Sir/Madam,</p></div>";
                                    mailsendtonomineebody +=
                                        "<p>Vice-Chancellor is pleased to appoint you as University Nominee for faculty selections in<p>";
                                    mailsendtonomineebody +=
                                        "<table  style='text-align:left;border:1px solid green;background:darksalmon'>";
                                    mailsendtonomineebody += "<tr>";
                                    mailsendtonomineebody += "<th>College Code </th><th>:</th><td>" +
                                                             collegedata.collegeCode + "</td>";
                                    mailsendtonomineebody += "</tr>";
                                    mailsendtonomineebody += "<tr>";
                                    mailsendtonomineebody += "<th>College Name </th><th>:</th><td> " +
                                                             collegedata.collegeName + "</td>";
                                    mailsendtonomineebody += "</tr>";
                                    mailsendtonomineebody += "<tr>";
                                    mailsendtonomineebody += "<th>Department </th><th>:</th><td> " + DepartmentText +
                                                             "</td>";
                                    mailsendtonomineebody += "</tr>";
                                    mailsendtonomineebody += "<tr>";
                                    mailsendtonomineebody += "<th>Your Co-nominee is </th><th>:</th><td>" +
                                                             CoNomineeName.Conominee + "</td>";
                                    mailsendtonomineebody += "</tr>";
                                    mailsendtonomineebody += "</table>";
                                    mailsendtonomineebody +=
                                        "<p> You are required to follow the procedure detailed below:</p>";
                                    mailsendtonomineebody +=
                                        "<p>1) The list of candidates to be interviewed along with their registrations Ids is herewith attached. You are required to conduct interviews only for these candidates in the list.</p>";
                                    mailsendtonomineebody +=
                                        "<p>2) Once the candidates are selected you are requested to fill the SCM Minutes in the enclosed format.</p>";
                                    mailsendtonomineebody +=
                                        "<p>3) Further, you are requested to login to the URL http://112.133.193.228:76/ with following login details.</p>";
                                    mailsendtonomineebody +=
                                        "<table style='text-align:left;border:1px solid green;background:darksalmon'>";
                                    mailsendtonomineebody += "<tr>";
                                    mailsendtonomineebody += "<th>Login </th><th>:</th><td>" + UserName + "</td>";
                                    mailsendtonomineebody += "</tr>";
                                    mailsendtonomineebody += "<tr>";
                                    mailsendtonomineebody += "<th>Password </th><th>:</th><td>" + Password + "</td>";
                                    mailsendtonomineebody += "</tr>";
                                    mailsendtonomineebody += "</table>";
                                    mailsendtonomineebody +=
                                        "<p>4) After logging please upload the scanned copy of SCM minutes duly signed by all the members. Also please indicate the selected candidates with a tick mark in the list shown in the URL.</p>";
                                    mailsendtonomineebody +=
                                        "<p><b><u>Note:</u></b> The above procedure can be performed in the college itself (where selections were done) by one of the nominees in coordination with other nominee immediately after the completion of the selection process.</p>";
                                    mailsendtonomineebody +=
                                        "<br/><p><b style='font-size:15px'>REGISTRAR,JNTUH</b></p></div>";

                                    //Send SNS to Nominee For SCM Request Message
                                    string SMSText =
                                        "Dear Sir/Madam,\n Vice-Chancellor is pleased to appoint you as University nominee for faculty selection in " +
                                        DepartmentText + " of " + collegedata.collegeName + " with College Code " +
                                        collegedata.collegeCode + ". Your Co-nominee is " + CoNomineeName.Conominee +
                                        ". Please go through your registered Mail Id for further details.\n REGISTRAR,JNTUH";

                                    //***sending Emails to Nominees//              
                                    var message = new MailMessage();
                                    message.To.Add(item.auditorEmail1.Trim());

                                    message.Subject = mailsendtonomineesubject;
                                    message.Body = mailsendtonomineebody;
                                    message.IsBodyHtml = true;
                                    string path = Server.MapPath("~/Content/PDFReports/SCMReportDownload");
                                    string Filelocationpath = path + "/" + CoNomineeName.Filepath;
                                    message.Attachments.Add(new Attachment(Filelocationpath));
                                    message.Attachments.Add(new Attachment(filepathsecond));
                                    var smtp = new SmtpClient();
                                    smtp.Credentials = new NetworkCredential("staffselection3@jntuh.ac.in", "jntu@123");
                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Port = 587;
                                    smtp.EnableSsl = true;
                                    await smtp.SendMailAsync(message);

                                    status = UAAAS.Models.Utilities.SendSMS(item.auditorMobile1, SMSText);
                                }
                            }
                            //  TempData["Success"] = "Nominee ReAssinged Successfully.";
                            // return RedirectToAction("NomineeReassignView", "CollegeSCMProceedingsRequest", new { collegeId = details[0].CollegeId });





                            var OldauditorId =
                                db.jntuh_auditor_assigned.Where(
                                    e => e.CollegeId == CollegeId && ScmIdss.Contains(e.ScmId) && e.AuditorId != AuditorId1)
                                    .Select(e => e.AuditorId)
                                    .Distinct()
                                    .ToArray();
                            var NewauditorId = AuditorId1;
                            var PresentauditorId =
                                jntuh_auditor_assigneds.Where(
                                    e => e.CollegeId == CollegeId && ScmIdss.Contains(e.ScmId) && e.AuditorId != AuditorId1)
                                    .Select(e => e.AuditorId)
                                    .FirstOrDefault();
                            //Replace Nominee Details
                            string ReplacedNomineeName = string.Empty;

                            List<int> ReplacedAuditorId = new List<int>();
                            var ReplacedNomineeDetails =
                                db.jntuh_auditor_assigned.Where(
                                    e => e.CollegeId == CollegeId && ScmIdss.Contains(e.ScmId) && e.AuditorId == AuditorId1)
                                    .Select(e => e.OldAuditorIds)
                                    .FirstOrDefault();
                            if (ReplacedNomineeDetails != null)
                            {
                                var ReplacedNomineeId = ReplacedNomineeDetails.Split(',');
                                foreach (var Id in ReplacedNomineeId)
                                {
                                    if (!string.IsNullOrEmpty(Id))
                                    {
                                        ReplacedAuditorId.Add(Convert.ToInt32(Id));
                                    }
                                }
                            }

                            if (ReplacedAuditorId.Count != 0)
                            {
                                ReplacedNomineeName =
                                    db.jntuh_ffc_auditor.Where(
                                        e => e.id != PresentauditorId && ReplacedAuditorId.Contains(e.id))
                                        .Select(e => e.auditorName)
                                        .FirstOrDefault();
                            }


                            //Get Co-Nominee Name For Old Nominee
                            var CoNomineeNameold =
                                db.jntuh_auditor_assigned.Join(db.jntuh_ffc_auditor, ASS => ASS.AuditorId, FFC => FFC.id,
                                    (ASS, FFC) => new { ASS = ASS, FFC = FFC })
                                    .Where(
                                        e =>
                                            e.ASS.CollegeId == CollegeId && e.ASS.AuditorId == AuditorId1 &&
                                            ScmIdss.Contains(e.ASS.ScmId))
                                    .Select(e => new { Conominee = e.FFC.auditorName, Filepath = e.ASS.SCMListPath })
                                    .FirstOrDefault();




                            //College Mail Ids
                            CollegeEmailIds =
                                db.jntuh_address.Where(e => e.collegeId == CollegeId).Select(e => e.email).ToList();
                            var princiaplEmail =
                                db.jntuh_college_principal_director.Where(
                                    e => e.collegeId == CollegeId && e.type == "PRINCIPAL")
                                    .Select(e => e.email)
                                    .FirstOrDefault();
                            CollegeEmailIds = CollegeEmailIds.Concat(new string[] { princiaplEmail });



                            var aditorslist1 =
                                db.jntuh_ffc_auditor.Where(e => e.id == PresentauditorId || e.id == NewauditorId)
                                    .Select(e => e)
                                    .ToList();


                            List<jntuh_ffc_auditor> aditorslist2 =
                                db.jntuh_ffc_auditor.Where(e => e.isActive == true && e.id == PresentauditorId)
                                    .Select(e => e)
                                    .ToList();

                            if (OldauditorId.Contains(PresentauditorId))
                            {

                                foreach (var item in aditorslist2)
                                {
                                    if (item != null)
                                    {
                                        string UserName = string.Empty;
                                        string Password = string.Empty;
                                        var nomineeusernameandpassword =
                                            nomineeRegistration.Where(e => e.Email.Trim() == item.auditorEmail1.Trim())
                                                .Select(e => e)
                                                .FirstOrDefault();
                                        if (nomineeusernameandpassword != null)
                                        {
                                            UserName = nomineeusernameandpassword.Email;
                                            Password = nomineeusernameandpassword.Password ==
                                                       item.auditorMobile1.Substring(5, 5)
                                                ? nomineeusernameandpassword.Password
                                                : "***********";
                                        }

                                        //Send Mail to Nominee For SCM Request Body
                                        //
                                        string mailsendtonomineesubject =
                                            "JNTUH-DUAAC: Allotment of University Nominee for Faculty Selection:" +
                                            DepartmentText + ": Reg";
                                        string mailsendtonomineebody =
                                            "<div style='border:1px solid red;padding:10px;background:beige'><div><p>Dear Sir/Madam,</p></div>";
                                        mailsendtonomineebody +=
                                            "<p>In partial modifications to the previous order, Your Co-nominee is : " +
                                            CoNomineeNameold.Conominee + " ,in place of " + ReplacedNomineeName + "<p>";
                                        mailsendtonomineebody +=
                                            "<table  style='text-align:left;border:1px solid green;background:darksalmon'>";
                                        mailsendtonomineebody += "<tr>";
                                        mailsendtonomineebody += "<th>College Code </th><th>:</th><td>" +
                                                                 collegedata.collegeCode + "</td>";
                                        mailsendtonomineebody += "</tr>";
                                        mailsendtonomineebody += "<tr>";
                                        mailsendtonomineebody += "<th>College Name </th><th>:</th><td> " +
                                                                 collegedata.collegeName + "</td>";
                                        mailsendtonomineebody += "</tr>";
                                        mailsendtonomineebody += "<tr>";
                                        mailsendtonomineebody += "<th>Department </th><th>:</th><td> " + DepartmentText +
                                                                 "</td>";
                                        mailsendtonomineebody += "</tr>";
                                        mailsendtonomineebody += "<tr>";
                                        mailsendtonomineebody += "<th>Your Co-nominee is </th><th>:</th><td>" +
                                                                 CoNomineeNameold.Conominee + " ,in place of " +
                                                                 ReplacedNomineeName + "</td>";
                                        mailsendtonomineebody += "</tr>";
                                        mailsendtonomineebody += "</table>";
                                        mailsendtonomineebody +=
                                            "<p> You are required to follow the procedure detailed below:</p>";
                                        mailsendtonomineebody +=
                                            "<p>1) The list of candidates to be interviewed along with their registrations Ids is herewith attached. You are required to conduct interviews only for these candidates in the list.</p>";
                                        mailsendtonomineebody +=
                                            "<p>2) Once the candidates are selected you are requested to fill the SCM Minutes in the enclosed format.</p>";
                                        mailsendtonomineebody +=
                                            "<p>3) Further, you are requested to login to the URL http://112.133.193.228:76/ with following login details.</p>";
                                        mailsendtonomineebody +=
                                            "<table style='text-align:left;border:1px solid green;background:darksalmon'>";
                                        mailsendtonomineebody += "<tr>";
                                        mailsendtonomineebody += "<th>Login </th><th>:</th><td>" + UserName + "</td>";
                                        mailsendtonomineebody += "</tr>";
                                        mailsendtonomineebody += "<tr>";
                                        mailsendtonomineebody += "<th>Password </th><th>:</th><td>" + Password + "</td>";
                                        mailsendtonomineebody += "</tr>";
                                        mailsendtonomineebody += "</table>";
                                        mailsendtonomineebody +=
                                            "<p>4) After logging please upload the scanned copy of SCM minutes duly signed by all the members. Also please indicate the selected candidates with a tick mark in the list shown in the URL.</p>";
                                        mailsendtonomineebody +=
                                            "<p><b><u>Note:</u></b> The above procedure can be performed in the college itself (where selections were done) by one of the nominees in coordination with other nominee immediately after the completion of the selection process.</p>";
                                        mailsendtonomineebody +=
                                            "<br/><p><b style='font-size:15px'>REGISTRAR,JNTUH</b></p></div>";

                                        //Send SNS to Nominee For SCM Request Message
                                        string SMSText =
                                            "Dear Sir/Madam,\n In partial modifications to the previous order " +
                                            DepartmentText + " of " + collegedata.collegeName + " with College Code " +
                                            collegedata.collegeCode + ". Your Co-nominee is " + CoNomineeNameold.Conominee +
                                            ",In place of " + ReplacedNomineeName +
                                            ". Please go through your registered Mail Id for further details.\n REGISTRAR,JNTUH";

                                        //***sending Emails to Nominees//              
                                        var message = new MailMessage();
                                        message.To.Add(item.auditorEmail1.Trim());

                                        message.Subject = mailsendtonomineesubject;
                                        message.Body = mailsendtonomineebody;
                                        message.IsBodyHtml = true;
                                        string path = Server.MapPath("~/Content/PDFReports/SCMReportDownload");
                                        string Filelocationpath = path + "/" + CoNomineeNameold.Filepath;
                                        message.Attachments.Add(new Attachment(Filelocationpath));
                                        message.Attachments.Add(new Attachment(filepathsecond));
                                        var smtp = new SmtpClient();
                                        smtp.Credentials = new NetworkCredential("staffselection3@jntuh.ac.in", "jntu@123");
                                        smtp.Host = "smtp.gmail.com";
                                        smtp.Port = 587;
                                        smtp.EnableSsl = true;
                                        await smtp.SendMailAsync(message);

                                        status = UAAAS.Models.Utilities.SendSMS(item.auditorMobile1, SMSText);
                                    }

                                }

                                var collegedata1 = (from SCMREQ in db.jntuh_scmproceedingsrequests
                                                    join CLG in db.jntuh_college on SCMREQ.CollegeId equals CLG.id
                                                    where ScmIdss.Contains(SCMREQ.ID)
                                                    select new
                                                    {
                                                        CollegeCode = CLG.collegeCode,
                                                        CollegeName = CLG.collegeName,
                                                        //  DepartmentName = DEG.degree + "-" + SPEC.specializationName,
                                                        ScmRequestDate = SCMREQ.RequestSubmittedDate
                                                    }).FirstOrDefault();






                                //Send Mail to College For SCM Request Body Text
                                string CollegeMailText = string.Empty;
                                string CollegeMailSubject =
                                    "JNTUH-DUAAC: Allotment of University Nominee for Faculty Selection:" + DepartmentText +
                                    ": Reg";
                                CollegeMailText =
                                    "<div style='border:1px solid red;padding:10px;background:beige'><div><p>Dear Sir/Madam,</p></div>";
                                CollegeMailText += "<p><b>College Name: " + collegedata.collegeCode + " - " +
                                                   collegedata.collegeName + "</b></p>";
                                CollegeMailText += "<p>For your request, dated:<b style='color:red'>" +
                                                   UAAAS.Models.Utilities.MMDDYY2DDMMYY(
                                                       collegedata1.ScmRequestDate.ToString()) +
                                                   " (requested candidate's list is attached)</b> , ";
                                CollegeMailText += "the following University Nominee <br/><b style='color:red'>" +
                                                   CoNomineeNameold.Conominee + "</b><br/>is allotted in place of <b>" +ReplacedNomineeName +"</b> for Faculty Selection in <b style='color:red'>" + DepartmentText +
                                                   "</b> Department  at your college.</p>";
                                CollegeMailText +=
                                    "<p><b>You are required to complete the selection process and upload the SCM Minuties duly signed by the Selection Committee Members.</b></p>";
                                CollegeMailText += "<p><b>Nominee Details: </b></p><br/>";
                                CollegeMailText += "<table border='1'>";
                                CollegeMailText += "<tr>";
                                CollegeMailText += "<th>Name</th><th>Email</th><th>Mobile</th><th>Designation</th>";
                                CollegeMailText += "</tr>";
                                if (aditorslist1.Count() != 0)
                                {
                                    foreach (var auditor in aditorslist1)
                                    {
                                        if (auditor != null)
                                        {
                                            CollegeMailText += "<tr>";
                                            CollegeMailText += "<td>" + auditor.auditorName + "</td>";
                                            CollegeMailText += "<td>" + auditor.auditorEmail1 + "</td>";
                                            CollegeMailText += "<td>" + auditor.auditorMobile1 + "</td>";
                                            CollegeMailText += "<td>" + auditor.jntuh_designation.designation + "</td>";
                                            CollegeMailText += "</tr>";
                                        }
                                    }
                                }
                                CollegeMailText += "</table>";
                                CollegeMailText += "<br/><p><b style='font-size:15px'>REGISTRAR,JNTUH</b></p></div>";

                                //Send Mail to College For SCM Request



                                string CollegeemailsSeparate = string.Empty;


                                foreach (var collegeEmailId in CollegeEmailIds)
                                {
                                    if (!string.IsNullOrEmpty(CollegeemailsSeparate))
                                    {
                                        CollegeemailsSeparate += "," + collegeEmailId.ToString();
                                    }
                                    else
                                    {
                                        CollegeemailsSeparate = collegeEmailId.ToString();
                                    }
                                }

                                var collegemessage = new MailMessage();
                                collegemessage.To.Add(CollegeemailsSeparate);

                                collegemessage.Subject = CollegeMailSubject;
                                collegemessage.Body = CollegeMailText;
                                collegemessage.IsBodyHtml = true;
                                string collegepath = Server.MapPath("~/Content/PDFReports/SCMReportDownload");
                                string collegeFilelocationpath = collegepath + "/" + CoNomineeNameold.Filepath;
                                collegemessage.Attachments.Add(new Attachment(collegeFilelocationpath));
                                var collegesmtp = new SmtpClient();
                                collegesmtp.Credentials = new NetworkCredential("staffselection3@jntuh.ac.in", "jntu@123");
                                collegesmtp.Host = "smtp.gmail.com";
                                collegesmtp.Port = 587;
                                collegesmtp.EnableSsl = true;
                                await collegesmtp.SendMailAsync(collegemessage);

                                TempData["Success"] = "Nominee Re-Assinged Successfully.";
                                return RedirectToAction("NomineeReassignView", "CollegeSCMProceedingsRequest",
                                    new { collegeId = CollegeId });

                            }
                            else
                            {
                                TempData["Error"] = "Nominee Does not Exist.";
                                return RedirectToAction("NomineeReassignView", "CollegeSCMProceedingsRequest",
                                    new { collegeId = details[0].CollegeId });
                            }
                        }
                        else
                        {
                            //Old Nominee Code

                            #region Old Nominee Mail Sending

                            // var OldauditorId =jntuh_auditor_assigneds.Where(e =>e.CollegeId == CollegeId && ScmIdss.Contains(e.ScmId) &&e.AuditorId == details[0].AssignedAuditorId).Select(e => e.AuditorId).Distinct().ToArray();
                            // var NewauditorId =jntuh_auditor_assigneds.Where(e =>e.CollegeId == CollegeId && ScmIdss.Contains(e.ScmId) &&e.AuditorId != details[0].AssignedAuditorId).Select(e => e.AuditorId).FirstOrDefault();
                            //  var PresentauditorId=jntuh_auditor_assigneds.Where(e => e.CollegeId == CollegeId && ScmIdss.Contains(e.ScmId) && e.AuditorId == details[0].AssignedAuditorId).Select(e => e.AuditorId).FirstOrDefault();
                            ////Replace Nominee Details
                            // string ReplacedNomineeName = string.Empty;

                            // List<int> ReplacedAuditorId = new List<int>();
                            // var ReplacedNomineeDetails = jntuh_auditor_assigneds.Where(e => e.CollegeId == CollegeId && ScmIdss.Contains(e.ScmId) && e.AuditorId != details[0].AssignedAuditorId).Select(e => e.OldAuditorIds).FirstOrDefault();
                            // if (ReplacedNomineeDetails!=null)
                            // {
                            //     var ReplacedNomineeId = ReplacedNomineeDetails.Split(',');
                            //     foreach (var Id in ReplacedNomineeId)
                            //     {
                            //         if (!string.IsNullOrEmpty(Id))
                            //         {
                            //             ReplacedAuditorId.Add(Convert.ToInt32(Id));
                            //         }
                            //     }
                            // }

                            // if (ReplacedAuditorId.Count!=0)
                            // {
                            //     ReplacedNomineeName =db.jntuh_ffc_auditor.Where(e => e.id != AuditorId1 && ReplacedAuditorId.Contains(e.id)).Select(e => e.auditorName).FirstOrDefault();
                            // }

                            // //College Mail Ids
                            // CollegeEmailIds = db.jntuh_address.Where(e => e.collegeId == CollegeId).Select(e => e.email).ToList();
                            // var princiaplEmail = db.jntuh_college_principal_director.Where(e => e.collegeId == CollegeId && e.type == "PRINCIPAL").Select(e => e.email).FirstOrDefault();
                            // CollegeEmailIds = CollegeEmailIds.Concat(new string[] { princiaplEmail });



                            // var aditorslist1 = db.jntuh_ffc_auditor.Where(e => e.id == PresentauditorId || e.id == NewauditorId).Select(e => e).ToList();




                            // if (OldauditorId.Contains(AuditorId1))
                            // {

                            //     foreach (var item in aditorslist)
                            //     {
                            //         if (item != null)
                            //         {
                            //             string UserName = string.Empty;
                            //             string Password = string.Empty;
                            //             var nomineeusernameandpassword = nomineeRegistration.Where(e => e.Email.Trim() == item.auditorEmail1.Trim()).Select(e => e).FirstOrDefault();
                            //             if (nomineeusernameandpassword != null)
                            //             {
                            //                 UserName = nomineeusernameandpassword.Email;
                            //                 Password = nomineeusernameandpassword.Password == item.auditorMobile1.Substring(5, 5) ? nomineeusernameandpassword.Password : "***********";
                            //             }

                            //             //Send Mail to Nominee For SCM Request Body
                            //             //
                            //             string mailsendtonomineesubject = "JNTUH-DUAAC: Allotment of University Nominee for Faculty Selection:" + DepartmentText + ": Reg";
                            //             string mailsendtonomineebody = "<div><p>Dear Sir/Madam,</p></div>";
                            //             mailsendtonomineebody += "<p>In partial modifications to the previous order, Your Co-nominee is : " + CoNomineeName.Conominee + " ,in place of " + ReplacedNomineeName + "<p>";
                            //             mailsendtonomineebody += "<table  style='text-align:left;border:1px solid green;background:darksalmon'>";
                            //             mailsendtonomineebody += "<tr>";
                            //             mailsendtonomineebody += "<th>College Code </th><th>:</th><td>" + collegedata.collegeCode + "</td>";
                            //             mailsendtonomineebody += "</tr>";
                            //             mailsendtonomineebody += "<tr>";
                            //             mailsendtonomineebody += "<th>College Name </th><th>:</th><td> " + collegedata.collegeName + "</td>";
                            //             mailsendtonomineebody += "</tr>";
                            //             mailsendtonomineebody += "<tr>";
                            //             mailsendtonomineebody += "<th>Department </th><th>:</th><td> " + DepartmentText + "</td>";
                            //             mailsendtonomineebody += "</tr>";
                            //             mailsendtonomineebody += "<tr>";
                            //             mailsendtonomineebody += "<th>Your Co-nominee is </th><th>:</th><td>" + CoNomineeName.Conominee + " ,in place of " + ReplacedNomineeName + "</td>";
                            //             mailsendtonomineebody += "</tr>";
                            //             mailsendtonomineebody += "</table>";
                            //             mailsendtonomineebody += "<p> You are required to follow the procedure detailed below:</p>";
                            //             mailsendtonomineebody += "<p>1) The list of candidates to be interviewed along with their registrations Ids is herewith attached. You are required to conduct interviews only for these candidates in the list.</p>";
                            //             mailsendtonomineebody += "<p>2) Once the candidates are selected you are requested to fill the SCM Minutes in the enclosed format.</p>";
                            //             mailsendtonomineebody += "<p>3) Further, you are requested to login to the URL http://112.133.193.228:76/ with following login details.</p>";
                            //             mailsendtonomineebody += "<table style='text-align:left;border:1px solid green;background:darksalmon'>";
                            //             mailsendtonomineebody += "<tr>";
                            //             mailsendtonomineebody += "<th>Login </th><th>:</th><td>" + UserName + "</td>";
                            //             mailsendtonomineebody += "</tr>";
                            //             mailsendtonomineebody += "<tr>";
                            //             mailsendtonomineebody += "<th>Password </th><th>:</th><td>" + Password + "</td>";
                            //             mailsendtonomineebody += "</tr>";
                            //             mailsendtonomineebody += "</table>";
                            //             mailsendtonomineebody += "<p>4) After logging please upload the scanned copy of SCM minutes duly signed by all the members. Also please indicate the selected candidates with a tick mark in the list shown in the URL.</p>";
                            //             mailsendtonomineebody += "<p><b><u>Note:</u></b> The above procedure can be performed in the college itself (where selections were done) by one of the nominees in coordination with other nominee immediately after the completion of the selection process.</p>";
                            //             mailsendtonomineebody += "<br/><p><b style='font-size:15px'>REGISTRAR,JNTUH</b></p>";

                            //             //Send SNS to Nominee For SCM Request Message
                            //             string SMSText = "Dear Sir/Madam,\n In partial modifications to the previous order " + DepartmentText + " of " + collegedata.collegeName + " with College Code " + collegedata.collegeCode + ". Your Co-nominee is " + CoNomineeName.Conominee + ",In place of " + ReplacedNomineeName + ". Please go through your registered Mail Id for further details.\n REGISTRAR,JNTUH";

                            //             //***sending Emails to Nominees//              
                            //             var message = new MailMessage();
                            //             message.To.Add(item.auditorEmail1.Trim());

                            //             message.Subject = mailsendtonomineesubject;
                            //             message.Body = mailsendtonomineebody;
                            //             message.IsBodyHtml = true;
                            //             string path = Server.MapPath("~/Content/PDFReports/SCMReportDownload");
                            //             string Filelocationpath = path + "/" + CoNomineeName.Filepath;
                            //             message.Attachments.Add(new Attachment(Filelocationpath));
                            //             message.Attachments.Add(new Attachment(filepathsecond));
                            //             var smtp = new SmtpClient();
                            //             smtp.Credentials = new NetworkCredential("staffselection@jntuh.ac.in", "jntu@123");
                            //             smtp.Host = "smtp.gmail.com";
                            //             smtp.Port = 587;
                            //             smtp.EnableSsl = true;
                            //             await smtp.SendMailAsync(message);

                            //             status = UAAAS.Models.Utilities.SendSMS(item.auditorMobile1, SMSText);
                            //         }

                            //     }

                            //     var collegedata1 = (from SCMREQ in db.jntuh_scmproceedingsrequests
                            //                        join CLG in db.jntuh_college on SCMREQ.CollegeId equals CLG.id
                            //                        where ScmIdss.Contains(SCMREQ.ID)
                            //                        select new
                            //                        {
                            //                            CollegeCode = CLG.collegeCode,
                            //                            CollegeName = CLG.collegeName,
                            //                            //  DepartmentName = DEG.degree + "-" + SPEC.specializationName,
                            //                            ScmRequestDate = SCMREQ.RequestSubmittedDate
                            //                        }).FirstOrDefault();






                            //     //Send Mail to College For SCM Request Body Text
                            //     string CollegeMailText = string.Empty;
                            //     string CollegeMailSubject = "JNTUH-DUAAC: Allotment of University Nominee for Faculty Selection:" + DepartmentText + ": Reg";
                            //     CollegeMailText = "<div><p>Dear Sir/Madam,</p></div>";
                            //     CollegeMailText += "<p><b>College Name: " + collegedata.collegeCode + " - " + collegedata.collegeName + "</b></p>";
                            //     CollegeMailText += "<p>For your request, dated:<b style='color:red'>" + UAAAS.Models.Utilities.MMDDYY2DDMMYY(collegedata1.ScmRequestDate.ToString()) + " (requested candidate's list is attached)</b> , ";
                            //     CollegeMailText += "the following University Nominee <br/><b style='color:red'>" + CoNomineeName.Conominee + "</b><br/>is allotted in place of <b>" + ReplacedNomineeName + "</b> for Faculty Selection in <b style='color:red'>" + DepartmentText + "</b> Department  at your college.</p>";
                            //     CollegeMailText += "<p><b>You are required to complete the selection process and upload the SCM Minuties duly signed by the Selection Committee Members within 5 days from the date of receipt of this mail.</b></p>";
                            //     CollegeMailText += "<p><b>Nominee Details: </b></p><br/>";
                            //     CollegeMailText += "<table border='1'>";
                            //     CollegeMailText += "<tr>";
                            //     CollegeMailText += "<th>Name</th><th>Email</th><th>Mobile</th><th>Designation</th>";
                            //     CollegeMailText += "</tr>";
                            //     if (aditorslist1.Count() != 0)
                            //     {
                            //         foreach (var auditor in aditorslist1)
                            //         {
                            //             if (auditor != null)
                            //             {
                            //                 CollegeMailText += "<tr>";
                            //                 CollegeMailText += "<td>" + auditor.auditorName + "</td>";
                            //                 CollegeMailText += "<td>" + auditor.auditorEmail1 + "</td>";
                            //                 CollegeMailText += "<td>" + auditor.auditorMobile1 + "</td>";
                            //                 CollegeMailText += "<td>" + auditor.jntuh_designation.designation + "</td>";
                            //                 CollegeMailText += "</tr>";
                            //             }
                            //         }
                            //     }
                            //     CollegeMailText += "</table>";
                            //     CollegeMailText += "<br/><p><b style='font-size:15px'>REGISTRAR,JNTUH</b></p>";

                            //     //Send Mail to College For SCM Request

                            //     foreach (var collegeEmailId in CollegeEmailIds)
                            //     {
                            //         var collegemessage = new MailMessage();
                            //         collegemessage.To.Add(collegeEmailId);

                            //         collegemessage.Subject = CollegeMailSubject;
                            //         collegemessage.Body = CollegeMailText;
                            //         collegemessage.IsBodyHtml = true;
                            //         var collegesmtp = new SmtpClient();
                            //         collegesmtp.Credentials = new NetworkCredential("staffselection@jntuh.ac.in", "jntu@123");
                            //         collegesmtp.Host = "smtp.gmail.com";
                            //         collegesmtp.Port = 587;
                            //         collegesmtp.EnableSsl = true;
                            //         await collegesmtp.SendMailAsync(collegemessage);
                            //     }

                            //     TempData["Success"] = "Send Co-Nominee Details Successfully.";
                            //     return RedirectToAction("NomineeReassignView", "CollegeSCMProceedingsRequest", new { collegeId = CollegeId });

                            // }
                            // else
                            // {
                            //     TempData["Error"] = "Nominee Does not Exist.";
                            //     return RedirectToAction("NomineeReassignView", "CollegeSCMProceedingsRequest",new {collegeId = details[0].CollegeId});
                            // }

                            #endregion



                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Please Select One Nominee Only.";
                    return RedirectToAction("NomineeReassignView", "CollegeSCMProceedingsRequest");
                }
            }
            return RedirectToAction("NomineeReassignView", "CollegeSCMProceedingsRequest");
        }

        public string SaveSCMReportSeconadAttachment(int preview, int collegeId)
        {
            string fullPath = string.Empty;
            Document pdfDoc = new Document(PageSize.A4, 60, 50, 60, 60);
            var collegedata = db.jntuh_college.Where(e => e.isActive == true && e.id == collegeId).Select(e => e).FirstOrDefault();
            string path = Server.MapPath("~/Content/PDFReports/SCMReportDownloadSeconadAttach");
            if (!Directory.Exists(Server.MapPath("~/Content/PDFReports/SCMReportDownloadSeconadAttach")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Content/PDFReports/SCMReportDownloadSeconadAttach"));
            }
            if (preview == 0)
            {
                fullPath = path + "/" + collegedata.collegeCode + "- SCM Report Download Seconad Attchment-" + DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf";
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(fullPath, FileMode.Create));
                ITextEvents iTextEvents = new ITextEvents();
                iTextEvents.CollegeCode = collegedata.collegeCode;
                iTextEvents.CollegeName = collegedata.collegeName;
                iTextEvents.formType = "Scm Report Download";
                pdfWriter.PageEvent = iTextEvents;
            }
            //Open PDF Document to write data
            pdfDoc.Open();

            //Assign Html content in a string to write in PDF
            string contentses = string.Empty;

            StreamReader sr;

            //Read file from server path
            sr = System.IO.File.OpenText(Server.MapPath("~/Content/SCMReportSendtoNominee.html"));
            //store content in the variable
            contentses = sr.ReadToEnd();
            sr.Close();
            //  contents = contents.Replace("##SERVERURL##", serverURL);

            contentses = GetScmReportDataSecondAttachment(contentses, collegeId);

            //Read string contents using stream reader and convert html to parsed conent
            var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(contentses), null);
            //Get each array values from parsed elements and add to the PDF document
            bool pageRotated = false;
            int count = 0;
            foreach (var htmlElement in parsedHtmlElements)
            {
                count++;
                if (count == 100)
                {

                }
                if (htmlElement.Equals("<textarea>"))
                {
                    pdfDoc.NewPage();
                }

                if (htmlElement.Chunks.Count >= 3)
                {
                    if (htmlElement.Chunks.Count == 4)
                    {
                        pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        pdfDoc.SetMargins(60, 50, 60, 60);
                        pageRotated = true;
                    }
                    else
                    {
                        if (pageRotated == true)
                        {
                            pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4);
                            pdfDoc.SetMargins(60, 50, 60, 60);
                            pageRotated = false;
                        }
                    }

                    pdfDoc.NewPage();

                }
                else
                {
                    pdfDoc.Add(htmlElement as IElement);
                }
            }

            //Close your PDF
            pdfDoc.Close();
            if (pdfDoc.IsOpen())
            {
                pdfDoc.Close();
            }

            string returnPath = string.Empty;
            returnPath = fullPath;
            return returnPath;


        }

        public string GetScmReportDataSecondAttachment(string contents, int collegeId)
        {
            string contentdata = string.Empty;

            contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left' width='90%'><b>College Name : </b>----------------------------------------------------------------</td>";
            contentdata += "<td style='text-align:left' width='20%'><b>Code : </b>---------</td></tr>";
            contentdata += "</table>";

            contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left' width='70%'><b>Proceeding No : </b></td>";
            contentdata += "<td style='text-align:left' width='30%'><b>Date : </b></td></tr>";
            contentdata += "</table>";

            contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:center'><b>Affiliated to</b></td></tr>";
            contentdata += "<tr><td style='text-align:center'><b>(JAWAHARLAL NEHRU TECHNOLOGICAL UNIVERSITY HYDERABAD KUKATPALLY, HYDERABAD)</b></td></tr>";
            contentdata += "</table>";

            contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left'>Minutes of the Selection Committee Meeting held on--------------------- at ------------------------</td></tr></table>";


            contentdata += "<br/><table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left' width='30%'><b>Post</b></td><td  width='5%'>:</td><td  style='text-align:left'></td></tr>";
            contentdata += "<tr><td style='text-align:left' width='30%'><b>Department</b></td><td  width='5%'>:</td><td  style='text-align:left'></td></tr>";
            contentdata += "<tr><td style='text-align:left' width='30%'><b>Scale of Pay</b></td><td  width='5%'>:</td><td  style='text-align:left'></td></tr></table>";

            contentdata += "<table border='1'cellspacing='0' cellpadding='10' width='100%' >";
            contentdata += "<tr><td style='text-align:left' width='8%'><b>Sno</b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b>Name of the Faculty</b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b>JNTUH Faculty Portal Reg. No</b></td>";
            contentdata += "<td style='text-align:left' width='10%'><b>PAN No</b></td>";
            contentdata += "<td style='text-align:left' width='8%'><b>Aadhar No</b></td>";
            contentdata += "<td style='text-align:left' width='8%'><b>UG Branch</b></td>";
            contentdata += "<td style='text-align:left' width='8%'><b>PG Branch</b></td>";
            contentdata += "<td style='text-align:left' width='8%'><b>Ph.D</b></td>";
            contentdata += "<td style='text-align:left' width='10%'><b>Years of Experience</b></td>";
            // contentdata += "<td style='text-align:left' width='10%'><b>Previous College</b></td>";
            contentdata += "</tr>";
            for (int i = 0; i < 12; i++)//scmregdata.Count() > 12 ? i < scmregdata.Count() :
            {
                contentdata += "<tr>";
                contentdata += "<tr><td style='text-align:left' width='8%'></td>";
                contentdata += "<td style='text-align:left' width='20%'></td>";
                contentdata += "<td style='text-align:left' width='20%'></td>";
                contentdata += "<td style='text-align:left' width='10%'></td>";
                contentdata += "<td style='text-align:left' width='8%'></td>";
                //if (scmregdata.Count()>i)
                //{
                //    contentdata += "<td style='text-align:left' width='14%'>" + scmregdata[i].PANNo + "</td>";
                //}
                //else
                //{
                //    contentdata += "<td style='text-align:left' width='14%'></td>";
                //}
                contentdata += "<td style='text-align:left' width='8%'></td>";
                contentdata += "<td style='text-align:left' width='8%'></td>";
                contentdata += "<td style='text-align:left' width='8%'></td>";
                contentdata += "<td style='text-align:left' width='10%'></td>";
                contentdata += "</tr>";
            }
            contentdata += "</table><br>";

            contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left' width='10%'><b>Sno</b></td>";
            contentdata += "<td style='text-align:left' width='30%' colspan='2'><b>Role</b></td>";
            contentdata += "<td style='text-align:left' width='40%'><b>Name</b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b>Signature</b></td></tr>";

            contentdata += "<tr><td style='text-align:left' width='10%'><b>1</b></td>";
            contentdata += "<td style='text-align:left' width='25%'><b>Chairperson</b></td>";
            contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            contentdata += "<tr><td style='text-align:left' width='10%'><b>2</b></td>";
            contentdata += "<td style='text-align:left' width='25%'><b>Principal</b></td>";
            contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            contentdata += "<tr><td style='text-align:left' width='10%'><b>3</b></td>";
            contentdata += "<td style='text-align:left' width='25%'><b>University Nominee 1</b></td>";
            contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            contentdata += "<tr><td style='text-align:left' width='10%'><b>4</b></td>";
            contentdata += "<td style='text-align:left' width='25%'><b>University Nominee 2</b></td>";
            contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            contentdata += "<tr><td style='text-align:left' width='10%'><b>5</b></td>";
            contentdata += "<td style='text-align:left' width='25%'><b>Expert 1</b></td>";
            contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            contentdata += "<tr><td style='text-align:left' width='10%'><b>6</b></td>";
            contentdata += "<td style='text-align:left' width='25%'><b>Expert 2</b></td>";
            contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            contentdata += "<tr><td style='text-align:left' width='10%'><b>7</b></td>";
            contentdata += "<td style='text-align:left' width='25%'><b>SC/ST/OBC/Women/<br/>Differently Abled if any</b></td>";
            contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";
            contentdata += "</table>";

            contents = contents.Replace("##SCMREPORTALLColleges##", contentdata);
            return contents;
        }

        [AuthorizedUserAccess("Admin")]
        public ActionResult ProfessorSCMRequestsList()
        {
            if (User.Identity.IsAuthenticated)
            // if (User.Identity.Name == "admin")
            {
                var jntuh_scmproceedingsrequests =db.jntuh_scmproceedingsrequests.Where(e => e.ISActive == true &&e.RequestSubmittedDate!=null).Select(e => e).ToList();
             //   var AddFacultyScmIds =db.jntuh_scmproceedingrequest_addfaculty.Select(e => e.ScmProceedingId).Distinct().ToArray();
                List<jntuh_scmproceedingrequest_addfaculty> jntuh_scmaddfaculty = db.jntuh_scmproceedingrequest_addfaculty.Select(e => e).ToList();

                var collegeIds =
                    jntuh_scmproceedingsrequests.Join(jntuh_scmaddfaculty, SCM => SCM.ID, ADD => ADD.ScmProceedingId,
                        (SCM, ADD) => new {SCM = SCM, ADD = ADD}).Where(e =>e.ADD.FacultyType == 1 &&!jntuh_scmaddfaculty.Where(f => f.FacultyType == 2 || f.FacultyType == 3)
                .Select(f => f.RegistrationNumber).ToArray().Contains(e.ADD.RegistrationNumber)).Select(e => e.SCM.CollegeId).Distinct().ToArray();


                //var jntuh_auditor_assign =db.jntuh_auditor_assigned.Where(e => e.IsActive == true).Select(e => e).ToList();


                ////SCM Ids with Requests
                //var SCMRequests =jntuh_scmproceedingsrequests.Where(e => e.ISActive == true && e.SpecializationId != 0 && e.DEpartmentId != 0 && e.DegreeId != 0)
                //        .Select(e => e.ID).Distinct().ToArray();
                //var AddFacultySCMRequests =AddFacultyScmIds.Where(e => SCMRequests.Contains(e)).Select(e => e).Distinct().ToArray();


                //var collegeIds =jntuh_scmproceedingsrequests.Where(e =>e.ISActive == true && e.SpecializationId != 0 && e.DEpartmentId != 0 && e.DegreeId != 0 &&
                //            AddFacultySCMRequests.Contains(e.ID)).Select(e => e.CollegeId).Distinct().ToArray();


                List<jntuh_college> colleges =db.jntuh_college.Where(e => collegeIds.Contains(e.id)).Select(e => e).Distinct().ToList();


                ScmRequestList scmdata = new ScmRequestList();
                scmdata.SCmRequestList = new List<ScmRequestList>();
                List<ScmRequestList> scmRequestLists = new List<ScmRequestList>();
                foreach (var item in colleges)
                {
                    //bool Ismatch = false;
                    //var scmrequestIds =jntuh_scmproceedingsrequests.Where(e =>e.CollegeId == item.id && e.SpecializationId != 0 && e.DEpartmentId != 0 &&e.DegreeId != 0 && e.RequestSubmittedDate != null)
                    //        .Select(e => e.ID).Distinct().ToArray();
                    //var scmrequestsaddfaculty =AddFacultyScmIds.Where(e => scmrequestIds.Contains(e)).Select(e => e).Distinct().ToArray();
                    //var scmrequestassigncount =jntuh_auditor_assign.Where(e => scmrequestsaddfaculty.Contains(e.ScmId)).Select(e => e.ScmId).Distinct().Count();
                    //if (scmrequestassigncount == scmrequestsaddfaculty.Count())
                    //{
                    //    Ismatch = true;
                    //}
                    scmRequestLists.Add(new ScmRequestList()
                    {
                        Id = item.id,
                        CollegeCode = item.collegeCode,
                        CollegeName = item.collegeName,
                       // IsAuditorAssigned = Ismatch,
                        Checked = false
                    });
                }
                scmdata.SCmRequestList.AddRange(scmRequestLists.OrderBy(e => e.CollegeName).Select(e => e).ToList());
                return View(scmdata);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }

        [AuthorizedUserAccess("Admin")]
        public ActionResult ProfessorCollegeScmProceedingsRequestView(int id)
        {
           
            if (User.Identity.IsAuthenticated)
            //  if (User.Identity.Name == "admin")
            {
                if (id != 0)
                {
                    var firstOrDefault = db.jntuh_college.FirstOrDefault(a => a.id == id);
                    var specs = new List<DistinctSpecializations>();
                    var degrees = db.jntuh_degree.AsNoTracking().ToList();
                    var specializations = db.jntuh_specialization.AsNoTracking().ToList();
                    var departments = db.jntuh_department.AsNoTracking().ToList();
                    var collegespecs =db.jntuh_college_intake_existing.Where(i => i.collegeId == id).Select(i => i.specializationId).Distinct().ToArray();
                    foreach (var s in collegespecs)
                    {
                        var specid = specializations.FirstOrDefault(i => i.id == s);

                        if (specid != null)
                        {
                            var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
                            var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));
                            if (degree != null)
                                specs.Add(new DistinctSpecializations
                                {
                                    SpecializationId = specid.id,
                                    SpecializationName = degree.degree + " - " + specid.specializationName,
                                    DepartmentId = specid.departmentId
                                });
                        }
                    }
                    ViewBag.departments = specs.OrderBy(i => i.DepartmentId);
                    var collegescmrequestslist = db.jntuh_scmproceedingsrequests.AsNoTracking().Where(i => i.CollegeId == id && i.RequestSubmittedDate != null).ToList();

                    var nomineeAssignedScmIds = (from SCM in db.jntuh_scmproceedingsrequests
                                                 join AUDA in db.jntuh_auditor_assigned on SCM.ID equals AUDA.ScmId
                                                 where
                                                     SCM.CollegeId == id && SCM.SpecializationId != 0 && SCM.DEpartmentId != 0 &&
                                                     SCM.DegreeId != 0
                                                 select SCM.ID).Distinct().ToArray();


                    var jntuh_auditor_assigned = db.jntuh_auditor_assigned.AsNoTracking().ToList();
                    var jntuh_auditors_dataentry = db.jntuh_auditors_dataentry.AsNoTracking().ToList();
                    var proceedingsRequests = new List<ScmProceedingsRequest>();



                    var SplittedSCMIds = db.jntuh_scmproceedingsrequests.Where(e => e.ISActive == true && e.OldSCMId != null).Select(e => e.OldSCMId).ToArray();



                    //If Any Empty SCM Request Check
                    var AddFacultySCMIds = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.FacultyType != 1).Select(e => e.ScmProceedingId).Distinct().ToArray();
                    foreach (var s in collegescmrequestslist.Where(e => !nomineeAssignedScmIds.Contains(e.ID)).Select(e => e).ToList())
                    {
                        if (AddFacultySCMIds.Contains(s.ID))
                        {

                            bool Isauditor = false;
                            bool IsSplited = false;
                            bool localIsAuditorVerified = false;
                            var specid = specializations.FirstOrDefault(i => i.id == s.SpecializationId);

                            if (specid != null && firstOrDefault != null)
                            {
                                var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
                                var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));
                                var AuditorAssigneddata = jntuh_auditor_assigned.Where(e => e.ScmId == s.ID).Select(e => e.Id).FirstOrDefault();
                                var Auditorverifieddata = jntuh_auditors_dataentry.Where(e => e.ScmProceedingId == s.ID).Select(e => e.Id).FirstOrDefault();
                                if (AuditorAssigneddata != 0)
                                {
                                    Isauditor = true;
                                }
                                if (Auditorverifieddata != 0)
                                {
                                    localIsAuditorVerified = true;
                                }
                                if (SplittedSCMIds.Contains(s.ID))
                                {
                                    IsSplited = true;
                                }

                                if (degree != null)
                                    proceedingsRequests.Add(new ScmProceedingsRequest
                                    {
                                        CollegeName = firstOrDefault.collegeCode + " - " + firstOrDefault.collegeName,
                                        CollegeId = firstOrDefault.id,
                                        SpecializationId = s.SpecializationId,
                                        ProfessorVacancies = s.ProfessorsCount.ToString(),
                                        AssociateProfessorVacancies = s.AssociateProfessorsCount.ToString(),
                                        AssistantProfessorVacancies = s.AssistantProfessorsCount.ToString(),
                                        SpecializationName = degree.degree + " - " + specid.specializationName,
                                        DepartmentId = specid.departmentId,
                                        DepartmentName = deptment.departmentName,
                                        CreatedDate = (DateTime)s.RequestSubmittedDate,
                                        ScmNotificationpath = s.SCMNotification,
                                        RequiredProfessorVacancies = s.RequiredProfessor.ToString(),
                                        RequiredAssociateProfessorVacancies = s.RequiredAssociateProfessor.ToString(),
                                        RequiredAssistantProfessorVacancies = s.RequiredAssistantProfessor.ToString(),
                                        Id = s.ID,
                                        IsSplited = IsSplited,
                                        IsAuditorAssigned = Isauditor,
                                        IsAuditorVerified = localIsAuditorVerified
                                    });
                            }
                        }
                    }
                    ViewBag.collegescmrequestslist = proceedingsRequests.OrderByDescending(e => e.CreatedDate).Select(e => e).ToList();
                    return View(proceedingsRequests.OrderBy(e => e.DepartmentName).ThenBy(e => e.SpecializationId).ThenBy(e => e.DegreeId).Select(e => e).ToList());//OrderByDescending(e => e.CreatedDate)
                }
                else
                {
                    return RedirectToAction("SCMRequestsList");
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }

        }


        public ActionResult AllCollegeAddress()
        {

            
                    ////////////////////Word Code
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=SCM Request Data Report.doc");
                    Response.ContentType = "application/vnd.ms-word ";
                    Response.Charset = string.Empty;
                    StringBuilder str = new StringBuilder();
                    str.Append(GetCollegeAddress());
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 60, 50, 60, 60);

                    pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    pdfDoc.SetMargins(60, 50, 60, 60);

                    string path = Server.MapPath("~/Content/PDFReports/SCMRequestToAll/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = path + "SCm Requests Data" + "-" + DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf";
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.Create));

                    pdfDoc.Open();

                    List<IElement> parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(str.ToString()), null);

                    foreach (var htmlElement in parsedHtmlElements)
                    {
                        pdfDoc.Add((IElement)htmlElement);
                    }

                    pdfDoc.Close();

                    Response.Output.Write(str.ToString());
                    Response.Flush();
                    Response.End();
               
            return RedirectToAction("SCMRequestsList");
        }

        public string GetCollegeAddress()
        {

            var CollegeData = (from c in db.jntuh_college
                join a in db.jntuh_address on c.id equals a.collegeId
                join d in db.jntuh_district on a.districtId equals d.id
                               where c.isActive == true && a.addressTye == "COLLEGE"
                select new
                {
                    CollegeCode = c.collegeCode,
                    CollegeName=c.collegeName,
                    Address=a.address,
                    Town=a.townOrCity,
                    Mandal=a.mandal,
                    District=d.districtName,
                    Pincode=a.pincode
                }).ToList();

            string contextstring = string.Empty;
            contextstring += "<table border='1' style='width:100%' cellspacing='10'>";
           
            foreach (var item in CollegeData)
            {
                contextstring += "<tr>";
                contextstring += "<td style='border-radius:10px'>To<br/>The Principal,<br/>" + item.CollegeName + "("+item.CollegeCode+"),<br/>" + item.Address + ",<br/>" + item.Town + "," + item.Mandal + ",<br/>" + item.District + "-" + item.Pincode + ",<br/>Telangana.<br/></td>";
                contextstring += "<td style='border-radius:10px'>To<br/>The Chairman/Secretary,<br/>" + item.CollegeName + ",<br/>" + item.Address + ",<br/>" + item.Town + "," + item.Mandal + ",<br/>" + item.District + "-" + item.Pincode + ",<br/>Telangana.<br/></td>";
                contextstring += "</tr>";
            }
            contextstring += "</table>";




            return contextstring;
        }
         [AuthorizedUserAccess("Admin")]
        public ActionResult ResetFaculty(int facultyAddId, int collegeId)
        {
            if (User.Identity.IsAuthenticated)
            {
               
                if (facultyAddId != 0 && collegeId != 0)
                {
                    var data = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == facultyAddId && e.FacultyType != 1).Select(e => e).FirstOrDefault();
                    var UserId = db.jntuh_registration.Where(e => e.Email == User.Identity.Name).Select(e => e.Id).FirstOrDefault();
                    if (data != null && UserId != 0)
                    {
                        data.IsApproved = null;
                        data.DeactiviationReason = null;
                        data.Updatedby = UserId;
                        data.UpdatedOn = DateTime.Now;
                        data.Isactive = true;
                        db.Entry(data).State = EntityState.Modified;
                        db.SaveChanges();

                       
                        TempData["Success"] = "Faculty Reset Successfully";
                    }

                    return RedirectToAction("CollegeWiseApprovedReport", "Admin", new { collegeId = collegeId });
                }
                return RedirectToAction("CollegeWiseApprovedReport", "Admin");
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }


         [HttpGet]
         [AuthorizedUserAccess("Admin")]
         public ActionResult ApprovedFacultyAss(int facultyAddId, int collegeId)
         {
             if (User.Identity.IsAuthenticated)
             //  if (User.Identity.Name == "admin" || (User.Identity.Name == "v01") || (User.Identity.Name == "v02") || (User.Identity.Name == "v03") || (User.Identity.Name == "v04") || (User.Identity.Name == "v05") || (User.Identity.Name == "v06") || (User.Identity.Name == "v07") || (User.Identity.Name == "v08") || (User.Identity.Name == "v09") || (User.Identity.Name == "v10"))
             {
                 //var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
                 if (facultyAddId != 0 && collegeId != 0)
                 {
                     var data = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == facultyAddId && e.FacultyType != 1).Select(e => e).FirstOrDefault();
                     var UserId = db.jntuh_registration.Where(e => e.Email == User.Identity.Name).Select(e => e.Id).FirstOrDefault();
                     if (data != null && UserId != 0)
                     {
                         data.IsApproved = true;
                         data.Updatedby = UserId;
                         data.UpdatedOn = DateTime.Now;
                         data.Isactive = true;
                         db.Entry(data).State = EntityState.Modified;
                         db.SaveChanges();

                        
                         TempData["Success"] = "Faculty Approved Successfully";
                     }

                     // return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest",
                     return RedirectToAction("CollegeWiseApprovedReport", "Admin",
                         new { collegeId = collegeId });
                 }
                 // return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest");
                 return RedirectToAction("CollegeWiseApprovedReport", "Admin");
             }
             else
             {
                 FormsAuthentication.SignOut();
                 return RedirectToAction("Login", "Admin");
             }
         }


         [HttpGet]
         [AuthorizedUserAccess("Admin")]
         public ActionResult NotApproveFacultyAss(int facultyAddId, int collegeId)
         {
             if (User.Identity.IsAuthenticated)
             //  if (User.Identity.Name == "admin" || (User.Identity.Name == "v01") || (User.Identity.Name == "v02") || (User.Identity.Name == "v03") || (User.Identity.Name == "v04") || (User.Identity.Name == "v05") || (User.Identity.Name == "v06") || (User.Identity.Name == "v07") || (User.Identity.Name == "v08") || (User.Identity.Name == "v09") || (User.Identity.Name == "v10"))
             {
                 Facultynotapproved data = new Facultynotapproved();
                 if (facultyAddId != 0 && collegeId != 0)
                 {
                     data.FacultyAddId = facultyAddId;
                     data.CollegeId = collegeId;
                     // data.DeactivationReason = "";
                 }
                 return PartialView("__NotApproveFacultyAss", data);
             }
             else
             {
                 FormsAuthentication.SignOut();
                 return RedirectToAction("Login", "Admin");
             }
         }


         [HttpPost]
         [AuthorizedUserAccess("Admin")]
         public ActionResult NotApproveFacultyAss(Facultynotapproved facultynotapproved)
         {
             if (User.Identity.IsAuthenticated)
             //  if (User.Identity.Name == "admin" || (User.Identity.Name == "v01") || (User.Identity.Name == "v02") || (User.Identity.Name == "v03") || (User.Identity.Name == "v04") || (User.Identity.Name == "v05") || (User.Identity.Name == "v06") || (User.Identity.Name == "v07") || (User.Identity.Name == "v08") || (User.Identity.Name == "v09") || (User.Identity.Name == "v10"))
             {
                 TempData["Error"] = null;
                 //  int userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
                 if (ModelState.IsValid)
                 {
                     if (facultynotapproved != null)
                     {
                         UAAASSCM.Models.jntuh_scmproceedingrequest_addfaculty addfaculty = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == facultynotapproved.FacultyAddId && e.FacultyType != 1).Select(e => e).FirstOrDefault();
                         var UserId = db.jntuh_registration.Where(e => e.Email == User.Identity.Name).Select(e => e.Id).FirstOrDefault();
                         if (addfaculty != null && UserId != 0)
                         {
                             addfaculty.DeactiviationReason = facultynotapproved.DeactivationReason;
                             addfaculty.Updatedby = UserId;
                             addfaculty.UpdatedOn = DateTime.Now;
                             addfaculty.Isactive = true;
                             addfaculty.IsApproved = false;
                             db.Entry(addfaculty).State = EntityState.Modified;
                             db.SaveChanges();
                             TempData["Success"] = "Faculty Not Approved Successfully";
                         }
                         //  return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest",

                         return RedirectToAction("CollegeWiseApprovedReport", "Admin",
                             new { collegeId = facultynotapproved.CollegeId });
                     }
                 }
                 //  return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest");
                 return RedirectToAction("CollegeWiseApprovedReport", "Admin");
             }
             else
             {
                 FormsAuthentication.SignOut();
                 return RedirectToAction("Login", "Admin");
             }
         }





    }


    #region Model class



    public class DistinctSpecializations
    {
        public int DepartmentId { get; set; }
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }
        public string DepartmentName { get; set; }
    }

    public class DistinctDepartments
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    public class ScmProceedingsRequest
    {
        public string CollegeName { get; set; }
        public string CollegeCode { get; set; }
        public int CollegeId { get; set; }
        public int DepartmentId { get; set; }
        public int SpecializationId { get; set; }

        public int Professors { get; set; }
        public int AssociateProfessors { get; set; }
        public int AssistantProfessors { get; set; }




        [Required(ErrorMessage = "*")]
        [StringLength(2, ErrorMessage = "Max 2 characters")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Number")]
        [Display(Name = "Professors")]
        public string ProfessorVacancies { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(2, ErrorMessage = "Max 2 characters")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Number")]
        [Display(Name = "Associate Professors")]
        public string AssociateProfessorVacancies { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(2, ErrorMessage = "Max 2 characters")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Number")]
        [Display(Name = "Assistant Professors")]
        public string AssistantProfessorVacancies { get; set; }

        public string Remarks { get; set; }
        public HttpPostedFileBase ScmNotificationSupportDoc { get; set; }
        public string ScmNotificationpath { get; set; }
        public string SpecializationName { get; set; }

        public string DepartmentName { get; set; }
        public int DegreeId { get; set; }
        public string DegreeName { get; set; }

        public bool Checked { get; set; }
        public int Id { get; set; }

        public string NotificationDate { get; set; }
        public DateTime? NotificationDateView { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(2, ErrorMessage = "Max 2 characters")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Number")]
        [Display(Name = "Professors")]
        public string RequiredProfessorVacancies { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(2, ErrorMessage = "Max 2 characters")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Number")]
        [Display(Name = "Associate Professors")]
        public string RequiredAssociateProfessorVacancies { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(2, ErrorMessage = "Max 2 characters")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Number")]
        [Display(Name = "Assistant Professors")]
        public string RequiredAssistantProfessorVacancies { get; set; }

        public DateTime? CreatedDate { get; set; }


        public bool IsEdit { get; set; }
        public bool IsAuditorAssigned { get; set; }
        public bool IsAuditorVerified { get; set; }
        public bool IsSplited { get; set; }

        public List<ScmProceedingsRequest> ScmProceedingsRequestslist { get; set; }
    }


    public class ScmProceedingsRequestAddReg
    {
        public int Id { get; set; }
        public int ScmId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeCode { get; set; }

        public int CollegeId { get; set; }
        public int DepartmentId { get; set; }
        public int SpecializationId { get; set; }

        public string SpecializationName { get; set; }

        public string DepartmentName { get; set; }
        public int DegreeId { get; set; }
        public string DegreeName { get; set; }


        public int Professors { get; set; }
        public int AssociateProfessors { get; set; }
        public int AssistantProfessors { get; set; }

        [Required(ErrorMessage = "Registration Number is Required")]
        [Remote("CheckRegistrationNumber", "CollegeSCMProceedingsRequest", HttpMethod = "POST",
            ErrorMessage = "Registration Number doesn't Exist")]
        public string RegistrationNo { get; set; }

        public string Regno { get; set; }
        public string RegName { get; set; }

        public int FacultyId { get; set; }

        [Required]
        public int PreviousCollegeId { get; set; }
    }

    public class Scmdates
    {
        public DateTime SCMDATE { get; set; }
        public string SCMDATEview { get; set; }
        public int SCMDateId { get; set; }
    }

    public class Scmuploads
    {
        public int Id { get; set; }
        public string CollegeCode { get; set; }
        public string CollegeName { get; set; }
        public int CollegeId { get; set; }

        [Required(ErrorMessage = "*")]
        public int SpecializationId { get; set; }

        public string SpecializationName { get; set; }
        public int DegreeId { get; set; }
        public string Degree { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }

        [Required(ErrorMessage = "*")]
        public string ScmDate { get; set; }


        [Required(ErrorMessage = "*")]
        public int Designation { get; set; }

        public HttpPostedFileBase ProfessorDocument { get; set; }
        public HttpPostedFileBase AssociateProfessorDocument { get; set; }
        public HttpPostedFileBase AssistantProfessorDocument { get; set; }

        public string ProfessorDocumentView { get; set; }
        public string AssociateProfessorDocumentView { get; set; }
        public string AssistantProfessorDocumentView { get; set; }
        public DateTime ScmDateView { get; set; }

        public DateTime CreatedDate { get; set; }

    }

    public class FacultyRegistrationDetails
    {
        public FacultyRegistrationDetails()
        {
            this.jntuh_registered_faculty_education = new HashSet<jntuh_registered_faculty_education>();
            //this.jntuh_registered_faculty_education_log=new HashSet<jntuh_registered_faculty_education_log>();
        }

        public int id { get; set; }
        public string Type { get; set; }
        public int? CollegeId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeCode { get; set; }
        public string RegistrationNumber { get; set; }
        public string UniqueID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? GenderId { get; set; }
        public string FatherOrhusbandName { get; set; }
        public string MotherName { get; set; }
        public string Principal { get; set; }
        public string Email { get; set; }
        public string facultyPhoto { get; set; }
        public string Mobile { get; set; }
        public string PANNumber { get; set; }
        public string AadhaarNumber { get; set; }
        public bool isActive { get; set; }
        public bool? isApproved { get; set; }
        public string department { get; set; }
        public int? SamePANNumberCount { get; set; }
        public int? SameAadhaarNumberCount { get; set; }
        public string SpecializationIdentfiedFor { get; set; }
        public string DeactivationReason { get; set; }
        public string IdentfiedFor { get; set; }
        public int FacultyAddId { get; set; }

        public string specialization { get; set; }
        public string degree { get; set; }
        public int ScmproceedingId { get; set; }
        public string SCMRequestDate { get; set; }
        public DateTime? SCMRequestDated { get; set; }

        public int? approvedby { get; set; }


        //public int DegreeId { get; set; }
        //public int Eid { get; set; }
        //public string PanVerificationStatus { get; set; }
        //public string PanDeactivationReasion { get; set; }
        //public string PanStatusAfterDE { get; set; }
        //public int? FIsApproved { get; set; }
        ////[Required(ErrorMessage = "*")]
        //public string UserName { get; set; }
        //public string AdjunctDesignation { get; set; }
        //public string AdjunctDepartment { get; set; }

        //public DateTime? DateOfBirth { get; set; }
        //public string facultyDateOfBirth { get; set; }
        //public string OrganizationName { get; set; }
        //public int? DepartmentId { get; set; }
        //public int? DesignationId { get; set; }
        //public string designation { get; set; }
        //public DateTime? DateOfAppointment { get; set; }
        //public string facultyDateOfAppointment { get; set; }
        //public string ProceedingsNo { get; set; }
        //public string SelectionCommitteeProcedings { get; set; }
        //public string AICTEFacultyId { get; set; }
        //public string GrossSalary { get; set; }
        //public int? TotalExperience { get; set; }
        //public int? TotalExperiencePresentCollege { get; set; }
        //public string EditPANNumber { get; set; }
        //public HttpPostedFileBase PANCardDocument { get; set; }
        //public string facultyPANCardDocument { get; set; }
        //public string EditEmail { get; set; }
        //public string National { get; set; }
        //public string InterNational { get; set; }
        //public string Citation { get; set; }
        //public string Awards { get; set; }
        //public HttpPostedFileBase Photo { get; set; }
        //public string facultyAadhaarCardDocument { get; set; }
        //public bool? isView { get; set; }
        //public Nullable<System.DateTime> createdOn { get; set; }
        //public Nullable<int> createdBy { get; set; }
        //public Nullable<System.DateTime> updatedOn { get; set; }
        //public Nullable<int> updatedBy { get; set; }
        //public bool? isFacultyRatifiedByJNTU { get; set; }
        //public DateTime? DateOfRatification { get; set; }
        //public string facultyDateOfRatification { get; set; }
        //public bool? WorkingStatus { get; set; }
        //public string OtherDepartment { get; set; }
        //public string OtherDesignation { get; set; }
        //public bool isVerified { get; set; }
        //public bool isValid { get; set; }
        //public virtual my_aspnet_users my_aspnet_users { get; set; }
        //public virtual my_aspnet_users my_aspnet_users1 { get; set; }
        //public virtual jntuh_designation jntuh_designation { get; set; }
        //public virtual jntuh_department jntuh_department { get; set; }
        public virtual ICollection<jntuh_registered_faculty_education> jntuh_registered_faculty_education
        {
            get;
            set;
        }

        //public virtual ICollection<jntuh_reinspection_registered_faculty_education> jntuh_reinspection_registered_faculty_education { get; set; }
        //// public virtual ICollection<jntuh_registered_faculty_education_log> jntuh_registered_faculty_education_log { get; set; }
        //public List<RegisteredFacultyEducation> FacultyEducation { get; set; }
        //public List<RegisteredfacultyExperience> RFExperience { get; set; }
        //public int? SpecializationId { get; set; }
        //public string SpecializationName { get; set; }

    }

    public class Facultynotapproved
    {
        [Required]
        public int FacultyAddId { get; set; }

        public string FacultyRegno { get; set; }

        [Required]
        public int CollegeId { get; set; }

        [Required(ErrorMessage = "Please Enter Reason")]
        public string DeactivationReason { get; set; }
    }

    public class CollegeData
    {
        public int collegeId { get; set; }
        public string collegeName { get; set; }
    }

    public class SCMPrincipal
    {
        public int SCMId { get; set; }

        [Required(ErrorMessage = "Registration Number is Required")]
        [Remote("CheckRegistrationNumber", "CollegeSCMProceedingsRequest", HttpMethod = "POST",
            ErrorMessage = "Registration Number doesn't Exist")]
        public string RegistrationNo { get; set; }

        public string NotificationDate { get; set; }

        public HttpPostedFileBase ScmNotificationSupportDoc { get; set; }

        [Required(ErrorMessage = "Select Previous Working College")]
        public int PreviousCollegeId { get; set; }

        public string PreviousCollegeName { get; set; }

        public string FirstName { get; set; }
        public string scmnotificationdocview { get; set; }
        public DateTime createdDate { get; set; }
        public int FacultyId { get; set; }

        [Required(ErrorMessage = "*")]
        public int CollegeId { get; set; }

        public string CollegeCode { get; set; }
        public string CollegeName { get; set; }
    }

    public class ScmRequestList
    {
        public int Id { get; set; }
        public string CollegeCode { get; set; }
        public string CollegeName { get; set; }
        public bool IsAuditorAssigned { get; set; }
        public bool Checked { get; set; }

        public List<ScmRequestList> SCmRequestList { get; set; }
    }

    public class NomineeAssignSCMRequests
    {
        public int SCMId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeCode { get; set; }
        public string Department { get; set; }
        public DateTime? ScmRequestDate { get; set; }
        public string AuditorName { get; set; }
        public DateTime AuditorAssignDate { get; set; }

    }

    public class Nomineereassign
    {
        public string Degree { get; set; }
        public string DepartmentName { get; set; }
        public string SpecializationName { get; set; }
        public int SCMId { get; set; }
        public int NomineeId { get; set; }
        public string NomineeName { get; set; }
        public int CollegeId { get; set; }
        public int DeparttmentId { get; set; }
        public int DegreeId { get; set; }
        public int SPEcializationId { get; set; }
        public DateTime AuditorAssignedDate { get; set; }
        public string viewAuditorAssignedDate { get; set; }
        public int Aid { get; set; }
        public string AuditorIds { get; set; }
        public DateTime? ReassignedDate { get; set; }
        public string viewReassignedDate { get; set; }


        public List<Nomineereassign> NomineeAssignedData { get; set; }
    }


    public class ReassignAuditors
    {
        public int SCMRequestId { get; set; }
        public int AditorId { get; set; }
        public string AditorName { get; set; }
        public string Designation { get; set; }
        public int DesignationId { get; set; }
        public string Department { get; set; }
        public int DepartmentId { get; set; }
        public int SpecializationId { get; set; }
        public string Specialization { get; set; }
        public bool Checke { get; set; }
        //  public string SCMIds { get; set; }
        public int CollegeId { get; set; }
        public string AssignedDate { get; set; }
        public int AssignedAuditorId { get; set; }

    }


    public class ReassignAuditorsCheck
    {
        public bool IsNeworOld { get; set; }
        public List<ReassignAuditors> Auditors { get; set; }
    }


    public class AuditorAssignedDatalist
    {
        public int Id { get; set; }
        public int ScmId { get; set; }
        public int? CollegeId { get; set; }
        public int AuditorId { get; set; }
        public string OldAuditorIds { get; set; }
        public string SCMListPath { get; set; }
        public DateTime? RequestSubmittedDate { get; set; }
        public DateTime? NomineeReassignDate { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string viewAssignedDate { get; set; }
    }



#endregion
}