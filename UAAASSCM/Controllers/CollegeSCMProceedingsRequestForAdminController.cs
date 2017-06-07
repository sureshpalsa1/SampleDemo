using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Data.Objects.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using UAAAS.Controllers.Reports;
using UAAAS.Models;

using UAAASSCM.Models;
using System.Threading.Tasks;
using WebGrease.Css.Extensions;


namespace UAAASSCM.Controllers
{
    public class CollegeSCMProceedingsRequestForAdminController : Controller
    {

        //Asking SCM Faculty Details Print 
      
        private SCMEntities db = new SCMEntities();
        //SCMReportsController scmreport = new SCMReportsController();
        private string serverURL;
        private string PdfGenerationpath = string.Empty;

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult CollegeScmProceedingsRequest(int? CollegeId)
        {
            if (User.Identity.IsAuthenticated)
            //if (User.Identity.Name == "admin")
            {
                var jntuh_college = db.jntuh_college.Where(e => e.isActive == true).Select(e => e).ToList();
                ViewBag.Colleges = jntuh_college.Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName }).OrderBy(e => e.collegeName).ToList();

                ScmProceedingsRequest scmProceedings =new ScmProceedingsRequest();
                scmProceedings.IsEdit = false;
                string clgCode;
                if (CollegeId != null)
                {
                      
                        var humanitesSpeci1 = new[] {37, 48, 42, 31, 154};

                        var DepartmentsData = (from SPEC in db.jntuh_specialization
                            join INTAKE in db.jntuh_college_intake_existing on SPEC.id equals INTAKE.specializationId into INTAKEData
                            from INTAKESPEC in INTAKEData.DefaultIfEmpty()
                            join DEPT in db.jntuh_department on SPEC.departmentId equals DEPT.id
                            join DEG in db.jntuh_degree on DEPT.degreeId equals DEG.id
                            where (INTAKESPEC.collegeId == CollegeId && INTAKESPEC.academicYearId == 8) || humanitesSpeci1.Contains(SPEC.id)
                            select new
                            {
                                SpecializationId = SPEC.id,
                                SpecializationName = DEG.degree + " - " + SPEC.specializationName,
                                DepartmentId = DEPT.id,
                                DegreeId = DEG.id
                            }).ToList();



                        if (DepartmentsData.Select(e => e.DegreeId).Distinct().ToArray().Contains(4))
                        {
                            ViewBag.departments = DepartmentsData.OrderBy(i => i.DepartmentId).ToList();
                        }
                        else
                        {

                            ViewBag.departments =DepartmentsData.Where(e => !humanitesSpeci1.Contains(e.SpecializationId)).Select(e => e).OrderBy(i => i.DepartmentId).ToList();
                        }
                       
                        scmProceedings.ScmProceedingsRequestslist = new List<ScmProceedingsRequest>();
                       
                        List<ScmProceedingsRequest> proceedingsRequests1 = db.jntuh_scmproceedingsrequests.Join(db.jntuh_specialization,SCMSPEC => SCMSPEC.SpecializationId, SPEC => SPEC.id,(SCMSPEC, SPEC) => new {SCMSPEC, SPEC})
                                .Join(db.jntuh_department, SCMDEPT => SCMDEPT.SCMSPEC.DEpartmentId, DEPT => DEPT.id,(SEPCDEPT, DEPT) => new {SEPCDEPT, DEPT})
                                .Join(db.jntuh_degree, SCMDEG => SCMDEG.SEPCDEPT.SCMSPEC.DegreeId, DEG => DEG.id,(SCMDEG, DEG) => new {SCMDEG, DEG})
                                .Where(e => e.SCMDEG.SEPCDEPT.SCMSPEC.CollegeId == CollegeId)
                                .Select(e => new ScmProceedingsRequest
                                {
                                    // CollegeName = e.SCMDEG.SEPCDEPT.SCMSPEC.CLG.collegeCode + " - " + e.SCMDEG.SEPCDEPT.SCMSPEC.CLG.collegeName,
                                    // CollegeCode = e.SCMDEG.SEPCDEPT.SCMSPEC.CLG.collegeCode,
                                    Professors = e.SCMDEG.SEPCDEPT.SCMSPEC.ProfessorsCount ?? 0,
                                    AssociateProfessors = e.SCMDEG.SEPCDEPT.SCMSPEC.AssociateProfessorsCount ?? 0,
                                    AssistantProfessors = e.SCMDEG.SEPCDEPT.SCMSPEC.AssistantProfessorsCount ?? 0,
                                    SpecializationName =e.DEG.degree + " - " + e.SCMDEG.SEPCDEPT.SPEC.specializationName,
                                    SpecializationId = e.SCMDEG.SEPCDEPT.SPEC.id,
                                    CollegeId = e.SCMDEG.SEPCDEPT.SCMSPEC.CollegeId,
                                    DepartmentId = e.SCMDEG.SEPCDEPT.SCMSPEC.DEpartmentId,
                                    ScmNotificationpath = e.SCMDEG.SEPCDEPT.SCMSPEC.SCMNotification,
                                    Id = e.SCMDEG.SEPCDEPT.SCMSPEC.ID,
                                    RequestSubmittedDate = (DateTime)e.SCMDEG.SEPCDEPT.SCMSPEC.RequestSubmittedDate,
                                    RequiredProfessors = e.SCMDEG.SEPCDEPT.SCMSPEC.RequiredProfessor ?? 0,
                                    RequiredAssistantProfessors =e.SCMDEG.SEPCDEPT.SCMSPEC.RequiredAssistantProfessor ?? 0,
                                    RequiredAssociateProfessors =e.SCMDEG.SEPCDEPT.SCMSPEC.RequiredAssociateProfessor ?? 0,
                                    Checked = false
                                }).ToList();
                       
                        scmProceedings.ScmProceedingsRequestslist.AddRange(
                            proceedingsRequests1.OrderByDescending(e => e.CreatedDate).Select(e => e).ToList());
                        ViewBag.collegescmrequestslist = scmProceedings.ScmProceedingsRequestslist;
                        scmProceedings.IsEdit = true;
                }
                return View(scmProceedings);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public ActionResult CollegeScmProceedingsRequest(ScmProceedingsRequest scmrequest)
        {
           if (ModelState.IsValid)
           {
               int SCmOrder = 1;
                var fileName = string.Empty;
                var filepath = string.Empty;
                var collegescmrequests = new UAAASSCM.Models.jntuh_scmproceedingsrequests();
                const string scmnotificationpath = "~/Content/Upload/SCMPROCEEDINGSREQUEST/ScmNotificationDocuments";
                collegescmrequests.CollegeId = scmrequest.CollegeId;
                collegescmrequests.SpecializationId = scmrequest.SpecializationId;
                var specialization = db.jntuh_specialization.AsNoTracking().FirstOrDefault(i => i.id == scmrequest.SpecializationId);
                var department = db.jntuh_department.AsNoTracking().FirstOrDefault(i => i.id == specialization.departmentId);

               collegescmrequests.DEpartmentId = specialization != null ? specialization.departmentId : 0;
                collegescmrequests.DegreeId = department != null ? department.degreeId : 0;
                collegescmrequests.ProfessorsCount = Convert.ToInt16(scmrequest.ProfessorVacancies);
                collegescmrequests.AssociateProfessorsCount = Convert.ToInt16(scmrequest.AssociateProfessorVacancies);
                collegescmrequests.AssistantProfessorsCount = Convert.ToInt16(scmrequest.AssistantProfessorVacancies);
                collegescmrequests.RequiredProfessor = Convert.ToInt16(scmrequest.RequiredProfessorVacancies);
                collegescmrequests.RequiredAssistantProfessor = Convert.ToInt16(scmrequest.RequiredAssistantProfessorVacancies);
                collegescmrequests.RequiredAssociateProfessor = Convert.ToInt16(scmrequest.RequiredAssociateProfessorVacancies);
                if (scmrequest.NotificationDate != null)
                    collegescmrequests.Notificationdate = UAAAS.Models.Utilities.DDMMYY2MMDDYY(scmrequest.NotificationDate);
                collegescmrequests.Remarks = scmrequest.Remarks;
                collegescmrequests.TotalNoofFacultyRequired = Convert.ToInt16(scmrequest.TotalFacultyRequired);
                collegescmrequests.SCMNotification = "Admin";
                collegescmrequests.CreatedBy = 1;
                collegescmrequests.CreatedOn = DateTime.Now;
                collegescmrequests.ISActive = true;


                //Checking SCM Order Id
                var scmdata = db.jntuh_scmproceedingsrequests.Where(e => e.ISActive == true && e.CollegeId == scmrequest.CollegeId && e.SpecializationId == collegescmrequests.SpecializationId &&
                            e.DEpartmentId == collegescmrequests.DEpartmentId && e.DegreeId == collegescmrequests.DegreeId).OrderByDescending(e => e.ID).Select(e => e).FirstOrDefault();

                if (scmdata != null)
                {
                    var assigneddata =db.jntuh_auditor_assigned.Where(e => e.ScmId == scmdata.ID).Select(e => e.Id).FirstOrDefault();
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
                       TempData["Success"] = "Your request has been proccessed successfully..";
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
            }
            return RedirectToAction("CollegeScmProceedingsRequest", new { CollegeId = scmrequest.CollegeId });
        }



        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult AddRegistrationNumber(int id)
        {
            ScmProceedingsRequestAddReg scmDetails = new ScmProceedingsRequestAddReg();

            var jntuh_scmproceedingsrequests = db.jntuh_scmproceedingsrequests.AsNoTracking().ToList();

            scmDetails = (from a in jntuh_scmproceedingsrequests
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
                              CollegeId = ab.id,
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

            //var jntuh_college = db.jntuh_college.Where(e => e.isActive == true).Select(e => e).ToList();
            //jntuh_college.Add(new jntuh_college() { id = 0, collegeCode = "Not Working" });
            //ViewBag.Colleges = jntuh_college.Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName }).OrderBy(e => e.collegeId).ToList();


            ViewBag.Designations = db.jntuh_designation.Where(e => e.isActive == true && (e.id == 1 || e.id == 2 || e.id == 3)).Select(e => new { Id = e.id, Name = e.designation }).OrderBy(e => e.Id).ToList();


            return PartialView("_AddRegistrationNumber", scmDetails);
        }

        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public ActionResult AddRegistrationNumber(ScmProceedingsRequestAddReg reg)
        {
            TempData["Error"] = null;
            // int userID = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
            //  int userCollegeID = db.jntuh_college_users.Where(collegeUser => collegeUser.userID == userID).Select(collegeUser => collegeUser.collegeID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (reg != null)
                {
                    UAAASSCM.Models.jntuh_scmproceedingrequest_addfaculty addfaculty = new UAAASSCM.Models.jntuh_scmproceedingrequest_addfaculty();
                    addfaculty.ScmProceedingId = reg.Id;
                    addfaculty.RegistrationNumber = reg.RegistrationNo;
                    addfaculty.FacultyType = reg.FacultyId;
                    addfaculty.PreviousCollegeId = reg.PreviousCollegeId.ToString();
                    addfaculty.Createdby = 1;
                    addfaculty.CreatedOn = DateTime.Now;
                    addfaculty.Isactive = true;
                    db.jntuh_scmproceedingrequest_addfaculty.Add(addfaculty);
                    db.SaveChanges();
                    TempData["Success"] = "Faculty Add Successfully";
                    return RedirectToAction("CollegeScmProceedingsRequest", "CollegeSCMProceedingsRequestForAdmin", new { CollegeId = reg.CollegeId });
                }
            }
            return RedirectToAction("CollegeScmProceedingsRequest", "CollegeSCMProceedingsRequestForAdmin");
        }

        [HttpPost]
        public JsonResult CheckRegistrationNumber(string RegistrationNo)
        {
            string CheckingReg = db.jntuh_registered_faculty.Where(f => f.RegistrationNumber.Trim() == RegistrationNo.Trim()).Select(f => f.RegistrationNumber).FirstOrDefault();
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

               

                var scmaddfacultydata = (from SCM in db.jntuh_scmproceedingsrequests
                    join SCMADDFLY in db.jntuh_scmproceedingrequest_addfaculty on SCM.ID equals
                        SCMADDFLY.ScmProceedingId
                        where SCM.ID==scmid
                    select new
                    {
                        RegistrationNumber=SCMADDFLY.RegistrationNumber,
                        SpecializationId=SCM.SpecializationId,
                        DegreeId=SCM.DegreeId,
                        ScmProceedingId=SCMADDFLY.ScmProceedingId,
                        Id=SCMADDFLY.Id,
                        FacultyTypeId=SCMADDFLY.FacultyType
                    }).ToList();


                var jntuh_designation = db.jntuh_designation.ToList();
                addFacultyDetails = (from a in scmaddfacultydata
                                     join c in db.jntuh_registered_faculty.AsNoTracking() on a.RegistrationNumber.Trim() equals c.RegistrationNumber.Trim() 
                                     join d in db.jntuh_specialization on a.SpecializationId equals d.id 
                                     join e in db.jntuh_degree on a.DegreeId equals e.id 
                                    // join DESG in db.jntuh_designation on a.FacultyTypeId equals DESG.id
                                     select new ScmProceedingsRequestAddReg
                                     {
                                         Id = a.Id,
                                         SpecializationId = a.SpecializationId,
                                         SpecializationName = e.degree + "-" + d.specializationName,
                                         Regno = c.RegistrationNumber,
                                         RegName = c.FirstName + " " + c.LastName,
                                         ScmId = a.ScmProceedingId,
                                         FacultyId = c.id,
                                         DesignationId=(int)a.FacultyTypeId,
                                      Designation = jntuh_designation.Where(x=>x.id==a.FacultyTypeId).Select(x=>x.designation).FirstOrDefault()
                                     }).ToList();





                
                return View(addFacultyDetails);
            }
            return RedirectToAction("CollegeScmProceedingsRequest");
        }


        public ActionResult DeleteRegistrationNumber(int id, int scmId)
        {
            if (id != 0 && scmId != 0)
            {
                var faculydata = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == id).Select(e => e).FirstOrDefault();
                if (faculydata != null)
                {
                    db.jntuh_scmproceedingrequest_addfaculty.Remove(faculydata);
                    db.SaveChanges();
                    TempData["Success"] = "Faculty Deleted Successfully";
                    return RedirectToAction("ViewFaculty", "CollegeSCMProceedingsRequestForAdmin", new { scmid = scmId });
                }
            }
            return RedirectToAction("CollegeScmProceedingsRequest");
        }


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult AddAuditors(string scmcheckeddata)
        {
            var SCMDATA = scmcheckeddata.Split(',');
            List<int> SCMIds = new List<int>();
            foreach (var SCMstr in SCMDATA)
            {
                if (!string.IsNullOrEmpty(SCMstr))
                {
                    SCMIds.Add(Convert.ToInt16(SCMstr));
                }
            }


            
                //scmcheckeddata.Where(e => e.Checked == true).Select(e => e.Id).Distinct().ToList();
            string scmIds = "";
            foreach (var ids in SCMIds)
            {
                if (!string.IsNullOrEmpty(scmIds))
                {
                    scmIds += ","+ids.ToString();
                }
                else
                {
                    scmIds =  ids.ToString();
                }
            }

            if (SCMIds.Count>0)
            {
                var jntuh_scmproceedingsrequests = db.jntuh_scmproceedingsrequests.AsNoTracking().ToList();
                var DeptId =jntuh_scmproceedingsrequests.Where(e => e.ID == SCMIds[0] && e.SpecializationId!=0).Select(e => e.DEpartmentId).FirstOrDefault();
                var jntuh_departments = db.jntuh_department.ToList();


                //Pharmacy Departments
                var pharmacyDeptIds = new int[] { 26,27,36,39,61,64 };



                int[] DepartIds = jntuh_departments.Where(e => e.degreeId == 1 && e.isActive == true).Select(e => e.id).ToArray();
                if (DepartIds.Contains(DeptId))
                {
                    if (DeptId == 69)
                    {
                        DeptId = 3;
                    }
                    else
                    {
                        var deptname = jntuh_departments.Where(e => e.id == DeptId).Select(e => e.departmentName).FirstOrDefault();
                        int BtechDeptId = jntuh_departments.Where(e => e.departmentName == deptname && e.degreeId == 4).Select(e => e.id).FirstOrDefault();
                        DeptId = BtechDeptId;
                    }
                    
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
                else if (DeptId == 73) //Assign CIVIL(5) Nominess --->Others(CIVIL)(73)
                {
                    DeptId = 5;
                }
                else if (DeptId == 74) //Assign MECH(2) Nominess --->Others(MECH)(74)
                {
                    DeptId = 15;
                }
                else if (DeptId == 75) //Assign ECE(2) Nominess --->Others(ECE)(75)
                {
                    DeptId = 2;
                }
                else if (DeptId == 76) //Assign EEE(1) Nominess --->Others(EEE)(76)
                {
                    DeptId = 1;
                }
                else if (DeptId == 77 || DeptId == 78) //Assign MBA(28) Nominess --->Others(MNGT)(77) and Others(MNGT)(78)
                {
                    DeptId = 28;
                }
                else if(DeptId == 21)
                {
                    DeptId = 15;
                }
                else if (DeptId == 11)
                {
                    DeptId = 3;
                }
                

                var OthersDeptIds = new int[] {65,66,67,68};

                List<AditorsDetails> aditors=new List<AditorsDetails>();

                if (OthersDeptIds.Contains(DeptId))
                {
                    int?[] DepIds=new int?[5];
                if (DeptId == 65)
                {
                    DepIds = new int?[] {3};
                }
                else if (DeptId == 66)
                {
                    DepIds = new int?[]{ 5, 15 };
                }
                else if (DeptId == 67)
                {
                    DepIds = new int?[] { 1, 2 };
                }
                else if (DeptId == 68)
                {
                    DepIds = new int?[] { 28, 31 };
                }
                
                aditors = (from Adts in db.jntuh_ffc_auditor
                           join Desg in db.jntuh_designation on Adts.auditorDesignationID equals Desg.id
                           where DepIds.Contains(Adts.auditorDepartmentID) && Adts.isActive == true
                           select new AditorsDetails()
                           {
                               SCMIds = scmIds,
                               AditorId = Adts.id,
                               AditorName = Adts.auditorName,
                               Designation = Desg.designation,
                               DesignationId = Desg.id,
                               DepartmentId = Adts.auditorDepartmentID ?? 0,
                               Checke = false
                           }).Distinct().ToList();

                }
                else
                {
                    aditors = (from Adts in db.jntuh_ffc_auditor
                               join Desg in db.jntuh_designation on Adts.auditorDesignationID equals Desg.id
                               where Adts.auditorDepartmentID == DeptId && Adts.isActive == true
                               select new AditorsDetails()
                               {
                                   SCMIds = scmIds,
                                  AditorId = Adts.id,
                                   AditorName = Adts.auditorName,
                                   Designation = Desg.designation,
                                   DesignationId = Desg.id,
                                   DepartmentId = Adts.auditorDepartmentID ?? 0,
                                   Checke = false
                               }).Distinct().ToList();
                }



                ViewBag.SelectedSCMRequest = (from SCMReq in db.jntuh_scmproceedingsrequests
                    join CLG in db.jntuh_college on SCMReq.CollegeId equals CLG.id
                    join DEG in db.jntuh_degree on SCMReq.DegreeId equals DEG.id
                    join DEPT in db.jntuh_department on SCMReq.DEpartmentId equals DEPT.id
                    join SPEC in db.jntuh_specialization on SCMReq.SpecializationId equals SPEC.id
                    where SCMIds.Contains(SCMReq.ID)
                    select new ScmProceedingsRequest
                    {
                        CollegeCode = CLG.collegeCode,
                        CollegeName = CLG.collegeName,
                        DepartmentName = DEG.degree+" "+SPEC.specializationName,
                        CreatedDate = (DateTime)SCMReq.RequestSubmittedDate
                    }).ToList();






              //  ViewBag.Designations = db.jntuh_designation.Where(e => e.isActive == true && (e.id == 1 || e.id == 2 || e.id == 3)).Select(e => new { Id = e.id, Name = e.designation }).OrderBy(e => e.Id).ToList();

                return PartialView("_AddAuditors", aditors);
            }
            return RedirectToAction("SCMRequestsList", "CollegeSCMProceedingsRequest");
        }

        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public async Task<ActionResult>  AddAuditors(List<AditorsDetails> aditorsdata)
        {
          
            if (aditorsdata.Count() != 0)
            {
                PdfGenerationpath = "";
                bool status = false;
                var attachments = new List<Attachment>();
                IUserMailer mailer = new UserMailer();
                var details = aditorsdata.Where(e => e.Checke == true).Select(e => e).ToList();
                if (details.Count() != 0)
                {
                    var aditorsIds = details.Select(e => e.AditorId).ToArray();

                    var SCMIDS = new List<int>();
                    var SCMDATA = details[0].SCMIds.Split(',');
                    foreach (var strscmId in SCMDATA)
                    {
                        if (!string.IsNullOrEmpty(strscmId))
                        {
                            SCMIDS.Add(Convert.ToInt16(strscmId));
                        }
                    }



                    int[] SCMId = SCMIDS.Select(e => e).Distinct().ToArray();
                    List<jntuh_scmproceedingsrequests> scmdata =db.jntuh_scmproceedingsrequests.Where(e => SCMId.Contains(e.ID)).Select(e => e).ToList();
                    if (scmdata.Count() != 0)
                    {


                        string DepartmentText = string.Empty;
                        string CollegeMailText = string.Empty;
                        IEnumerable<string> CollegeEmailIds;
                        //SCM Report Generation Code
                        var filepath = SaveSCMReportPdfDeptWise(0, scmdata);
                        filepath = filepath.Replace("/", "\\");
                        attachments.Add(new Attachment(filepath));


                        //second Attachment Get Path of the File
                        var collegeId = scmdata.GroupBy(e => e.CollegeId).Select(e => e.Key).FirstOrDefault();

                        var filepathsecond = SaveSCMReportSeconadAttachment(0, collegeId);
                        filepathsecond = filepathsecond.Replace("/", "\\");
                        attachments.Add(new Attachment(filepathsecond));





                        //Get Auditor's List 
                        List<jntuh_ffc_auditor> aditorslist = db.jntuh_ffc_auditor.Where(e => e.isActive == true && aditorsIds.Contains(e.id)).Select(e => e).ToList();

                        var collegedata = (from SCMREQ in db.jntuh_scmproceedingsrequests
                            join CLG in db.jntuh_college on SCMREQ.CollegeId equals CLG.id
                            join SPEC in db.jntuh_specialization on SCMREQ.SpecializationId equals SPEC.id
                            join DEPT in db.jntuh_department on SCMREQ.DEpartmentId equals DEPT.id
                            join DEG in db.jntuh_degree on SCMREQ.DegreeId equals DEG.id
                            where SCMId.Contains(SCMREQ.ID)
                            select new
                            {
                                CollegeCode=CLG.collegeCode,
                                CollegeName=CLG.collegeName,
                                DepartmentName=DEG.degree+"-"+SPEC.specializationName,
                                ScmRequestDate=SCMREQ.RequestSubmittedDate
                            }).FirstOrDefault();


                        //Get Department 
                        var collegedataDeptment = (from SCMREQ in db.jntuh_scmproceedingsrequests
                            join SCMADD in db.jntuh_scmproceedingrequest_addfaculty on SCMREQ.ID equals SCMADD.ScmProceedingId
                            join SPEC in db.jntuh_specialization on SCMREQ.SpecializationId equals SPEC.id
                            join DEPT in db.jntuh_department on SCMREQ.DEpartmentId equals DEPT.id
                            join DEG in db.jntuh_degree on SCMREQ.DegreeId equals DEG.id
                                                   where SCMId.Contains(SCMREQ.ID) && SCMADD.FacultyType != 1 && SCMREQ.RequestSubmittedDate!=null
                            select new
                            {
                                Deg=DEG.degree,
                               Spec= SPEC.specializationName,
                               DeptId=DEPT.id,
                               SpecId=SPEC.id
                            }).ToList();


                        var DeptIds = collegedataDeptment.GroupBy(e => e.DeptId).Select(e => e.Key).ToArray();

                        //foreach (var dept in collegedataDeptment.GroupBy(e => e.DeptId).Select(e => new { Deg = e.FirstOrDefault().Deg, Spec = e.FirstOrDefault().Spec, DeptId = e.Key }).ToList())
                        //{
                        //    DepartmentText += dept.Deg + "-" + dept.Spec + ",";
                        //}
                        //DepartmentText=DepartmentText.Substring(0, DepartmentText.Length - 1);


                        foreach (var dept in collegedataDeptment.GroupBy(e => e.SpecId).Select(e => new { Deg = e.FirstOrDefault().Deg, Spec = e.FirstOrDefault().Spec, DistinctSpecializationsId = e.Key }).ToList())
                        {
                            DepartmentText += dept.Deg + "-" + dept.Spec + ",";
                        }
                        DepartmentText = DepartmentText.Substring(0, DepartmentText.Length - 1);




                        CollegeEmailIds = db.jntuh_address.Where(e => e.collegeId == collegeId).Select(e => e.email).ToList();
                        var princiaplEmail = db.jntuh_college_principal_director.Where(e => e.collegeId == collegeId && e.type == "PRINCIPAL").Select(e => e.email).FirstOrDefault();
                        CollegeEmailIds = CollegeEmailIds.Concat(new string[] { princiaplEmail });

                        var nomineeRegistration = db.jntuh_registration.AsNoTracking().ToList();
                        foreach (var item in aditorslist)
                        {
                            if (item != null)
                            {
                                string UserName = string.Empty;
                                string Password = string.Empty;
                                var nomineeusernameandpassword =nomineeRegistration.Where(e => e.Email.Trim() == item.auditorEmail1.Trim()).Select(e => e).FirstOrDefault();
                                if (nomineeusernameandpassword != null)
                                {
                                    UserName = nomineeusernameandpassword.Email;

                                    Password = nomineeusernameandpassword.Password == item.auditorMobile1.Substring(5, 5) ? nomineeusernameandpassword.Password : "***********"; 
                                }

                                //Send Mail to Nominee For SCM Request Body
                                //
                                string mailsendtonomineesubject = "JNTUH-DUAAC: Allotment of University Nominee for Faculty Selection:" + DepartmentText + ": Reg";
                               string mailsendtonomineebody = "<div><p>Dear Sir/Madam,</p></div>";
                                mailsendtonomineebody +="<p>Vice-Chancellor is pleased to appoint you as University Nominee for faculty selections in<p>";
                                    //+ DepartmentText + " of " + collegedata.CollegeName + " with College Code " + collegedata.CollegeCode + ". Your Co-nominee is " + aditorslist.Where(e => e.id != item.id).Select(e => e.auditorName).FirstOrDefault() + ",";
                                mailsendtonomineebody += "<table  style='text-align:left;border:1px solid green;background:darksalmon'>";
                                mailsendtonomineebody += "<tr>";
                                mailsendtonomineebody += "<th>College Code </th><th>:</th><td>" + collegedata.CollegeCode + "</td>";
                                mailsendtonomineebody += "</tr>";
                                mailsendtonomineebody += "<tr>";
                                mailsendtonomineebody += "<th>College Name </th><th>:</th><td> " + collegedata.CollegeName + "</td>";
                                mailsendtonomineebody += "</tr>";
                                mailsendtonomineebody += "<tr>";
                                mailsendtonomineebody += "<th>Department </th><th>:</th><td> " + DepartmentText + "</td>";
                                mailsendtonomineebody += "</tr>";
                                mailsendtonomineebody += "<tr>";
                                mailsendtonomineebody += "<th>Your Co-nominee is </th><th>:</th><td>" + aditorslist.Where(e => e.id != item.id).Select(e => e.auditorName).FirstOrDefault() + "</td>";
                                mailsendtonomineebody += "</tr>";
                                mailsendtonomineebody += "</table>";
                                mailsendtonomineebody += "<p> You are required to follow the procedure detailed below:</p>";
                                mailsendtonomineebody += "<p>1) The list of candidates to be interviewed along with their registrations Ids is herewith attached. You are required to conduct interviews only for these candidates in the list.</p>";
                                mailsendtonomineebody += "<p>2) Once the candidates are selected you are requested to fill the SCM Minutes in the enclosed format.</p>";
                                mailsendtonomineebody += "<p>3) Further, you are requested to login to the URL http://112.133.193.228:76/ with following login details.</p>";
                                mailsendtonomineebody += "<table style='text-align:left;border:1px solid green;background:darksalmon'>";
                                mailsendtonomineebody += "<tr>";
                                mailsendtonomineebody += "<th>Login </th><th>:</th><td>" + UserName + "</td>";
                                mailsendtonomineebody += "</tr>";
                                mailsendtonomineebody += "<tr>";
                                mailsendtonomineebody += "<th>Password </th><th>:</th><td>" + Password + "</td>";
                                mailsendtonomineebody += "</tr>";
                                mailsendtonomineebody += "</table>";
                              //  mailsendtonomineebody += "<p style='text-align:left'> Login:" + UserName + "<br/>Password:" + Password + "</p>";
                                mailsendtonomineebody += "<p>4) After logging please upload the scanned copy of SCM minutes duly signed by all the members. Also please indicate the selected candidates with a tick mark in the list shown in the URL.</p>";
                                mailsendtonomineebody += "<p><b><u>Note:</u></b> The above procedure can be performed in the college itself (where selections were done) by one of the nominees in coordination with other nominee immediately after the completion of the selection process.</p>";
                                mailsendtonomineebody += "<br/><p><b style='font-size:15px'>REGISTRAR,JNTUH</b></p>";
                                //Send SNS to Nominee For SCM Request Message
                                string SMSText = "Dear Sir/Madam,\n Vice-Chancellor is pleased to appoint you as University nominee for faculty selection in " + DepartmentText + " of " + collegedata.CollegeName + " with College Code " + collegedata.CollegeCode + ". Your Co-nominee is " + aditorslist.Where(e => e.id != item.id).Select(e => e.auditorName).FirstOrDefault() + ". Please go through your registered Mail Id for further details.\n REGISTRAR,JNTUH";
                               
                //***sending Emails to Nominees//              
                            var message = new MailMessage();
                            message.To.Add(item.auditorEmail1.Trim());
                            // message.CC.Add("");
                            //  message.Bcc.Add("");
                            message.Subject = mailsendtonomineesubject;
                            message.Body = mailsendtonomineebody;
                            message.IsBodyHtml = true;
                            message.Attachments.Add(new Attachment(filepath));
                            message.Attachments.Add(new Attachment(filepathsecond));
                            var smtp = new SmtpClient();
                            //smtp.Credentials = new NetworkCredential("aac.do.not.reply@gmail.com", "uaaas@aac");
                            smtp.Credentials = new NetworkCredential("staffselection3@jntuh.ac.in", "jntu@123");
                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);    //this Committed for Testing
                               

                          //mailer.SendAttachmentToAllColleges(item.auditorEmail1, "", "",
                          //         "SCM PROCEEDINGS REQUEST", "Scm Requests", attachments).SendAsync();
                          status = UAAAS.Models.Utilities.SendSMS(item.auditorMobile1, SMSText);   //this Committed for Testing

                                foreach (var Ids in SCMId)
                                {
                                    UAAASSCM.Models.jntuh_auditor_assigned auditors = new UAAASSCM.Models.jntuh_auditor_assigned();
                                    auditors.AuditorId = item.id;
                                    auditors.ScmId = Ids;
                                    auditors.CollegeId = collegeId;
                                    auditors.IsActive = true;
                                    auditors.SCMListPath = PdfGenerationpath;
                                    auditors.CreatedBy = 1;
                                    auditors.CreatedOn = DateTime.Now;
                              db.jntuh_auditor_assigned.Add(auditors);
                                }
                            }
                        }
                          db.SaveChanges();   //this Committed for Testing

                        //Send Mail to College For SCM Request Body Text
                        string CollegeMailSubject = "JNTUH-DUAAC: Allotment of University Nominee for Faculty Selection:" + DepartmentText + ": Reg";
                        CollegeMailText = "<div><p>Dear Sir/Madam,</p></div>";
                        CollegeMailText += "<p><b>College Name: "+collegedata.CollegeCode+" - " + collegedata.CollegeName + "</b></p>";
                        CollegeMailText += "<p>As per your request, dated:<b style='color:red'>" + UAAAS.Models.Utilities.MMDDYY2DDMMYY(collegedata.ScmRequestDate.ToString()) + " (requested candidate's list is attached)</b> , ";
                        CollegeMailText += "the following University Nominees are allotted for Faculty Selection in <b style='color:red'>" + DepartmentText + "</b> Department  at your college.</p>";
                        CollegeMailText += "<p><b>You are required to complete the selection process and upload the SCM Minuties duly signed by the Selection Committee Members.</b></p>";
                        CollegeMailText += "<p><b>Nominee Details: </b></p><br/>";
                        CollegeMailText += "<table border='1'>";
                        CollegeMailText += "<tr>";
                        CollegeMailText += "<th>Name</th><th>Email</th><th>Mobile</th><th>Designation</th>";
                        CollegeMailText += "</tr>";
                        if (aditorslist.Count() !=0)
                        {
                            foreach (var auditor in aditorslist)
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
                        CollegeMailText += "<br/><p><b style='font-size:15px'>REGISTRAR,</b></p>";
                        CollegeMailText += "<p><b style='font-size:15px'>JNTUH</b></p>";

                        //Send Mail to College For SCM Request

                        foreach (var collegeEmailId in CollegeEmailIds)
                        {
                            var collegemessage = new MailMessage();
                            collegemessage.To.Add(collegeEmailId);
                            // message.CC.Add("");
                            //  message.Bcc.Add("");
                            collegemessage.Subject = CollegeMailSubject;
                            collegemessage.Body = CollegeMailText;
                            collegemessage.IsBodyHtml = true;
                            collegemessage.Attachments.Add(new Attachment(filepath));
                            // message.Attachments.Add(new Attachment(filepath));
                            var collegesmtp = new SmtpClient();
                            //collegesmtp.Credentials = new NetworkCredential("aac.do.not.reply@gmail.com", "uaaas@aac");
                            collegesmtp.Credentials = new NetworkCredential("staffselection3@jntuh.ac.in", "jntu@123");
                            collegesmtp.Host = "smtp.gmail.com";
                            collegesmtp.Port = 587;
                            collegesmtp.EnableSsl = true;
                            await collegesmtp.SendMailAsync(collegemessage);   //this Committed for Testing
                        }

                            TempData["Success"] = "Mail and SMS Sent Succesfully.....!";
                      return RedirectToAction("CollegeScmProceedingsRequestView", "CollegeSCMProceedingsRequest", new { id = collegeId });
                    }
                }
            }
            return RedirectToAction("SCMRequestsList", "CollegeSCMProceedingsRequest");
        }

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult AddAuditortoPrincipal(string scmcheckeddata)
        {
            var SCMDATA = scmcheckeddata.Split(',');
            List<int> SCMIds = new List<int>();
            foreach (var SCMstr in SCMDATA)
            {
                if (!string.IsNullOrEmpty(SCMstr))
                {
                    SCMIds.Add(Convert.ToInt16(SCMstr));
                }
            }
            
            string scmIds = "";
            foreach (var ids in SCMIds)
            {
                if (!string.IsNullOrEmpty(scmIds))
                {
                    scmIds += "," + ids.ToString();
                }
                else
                {
                    scmIds = ids.ToString();
                }
            }

            if (SCMIds.Count != 0)
            {
                var aditors = (from Adts in db.jntuh_ffc_auditor
                               join Desg in db.jntuh_designation on Adts.auditorDesignationID equals Desg.id
                               where Adts.isActive==true && Adts.auditorDepartmentID==60
                               select new AditorsDetails()
                               {
                                   SCMIds = scmIds,
                                  // SCMRequestId = SCMId,
                                   AditorId = Adts.id,
                                   AditorName = Adts.auditorName,
                                   Designation = Desg.designation,
                                   DesignationId = Desg.id,
                                   DepartmentId = Adts.auditorDepartmentID ?? 0,
                                   Checke = false
                               }).Distinct().ToList();


                List<ScmProceedingsRequest> SelectedPrincipalSCMRequestList= (from SCMReq in db.jntuh_scmproceedingsrequests
                                                join SCMADD in db.jntuh_scmproceedingrequest_addfaculty on SCMReq.ID equals SCMADD.ScmProceedingId
                                                join REG in db.jntuh_registered_faculty on SCMADD.RegistrationNumber.Trim() equals REG.RegistrationNumber.Trim()
                                                join CLG in db.jntuh_college on SCMReq.CollegeId equals CLG.id
                                                where SCMIds.Contains(SCMReq.ID)
                                              select new ScmProceedingsRequest
                                              {
                                                  CollegeCode = CLG.collegeCode,
                                                  CollegeName = CLG.collegeName,
                                                  CreatedDate = (DateTime)SCMReq.RequestSubmittedDate,
                                                  Remarks = SCMADD.RegistrationNumber
                                                //REG.FirstName+" "+REG.MiddleName+" "+REG.LastName+" ("+REG.RegistrationNumber+")"
                                              }).ToList();

                ViewBag.SelectedPrincipalSCMRequest = SelectedPrincipalSCMRequestList.ToList();

                return PartialView("_AddAuditortoPrincipal", aditors);
            }
            return RedirectToAction("SCMPrincipalview", "Admin");
        }

        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public async Task<ActionResult> AddAuditortoPrincipal(List<AditorsDetails> aditorsdata)
        {
            if (aditorsdata.Count() != 0)
            {
                PdfGenerationpath = "";
                bool status = false;
                var attachments = new List<Attachment>();
                IUserMailer mailer = new UserMailer();
                var details = aditorsdata.Where(e => e.Checke == true).Select(e => e).ToList();
                if (details.Count() != 0)
                {
                     var aditorsIds = details.Select(e => e.AditorId).ToArray();

                     var SCMIDS = new List<int>();
                     var SCMDATA = details[0].SCMIds.Split(',');
                     foreach (var strscmId in SCMDATA)
                     {
                         if (!string.IsNullOrEmpty(strscmId))
                         {
                             SCMIDS.Add(Convert.ToInt16(strscmId));
                         }
                     }

                     int[] SCMId = SCMIDS.Select(e => e).Distinct().ToArray();
                     List<jntuh_scmproceedingsrequests> scmdata = db.jntuh_scmproceedingsrequests.Where(e => SCMId.Contains(e.ID)).Select(e => e).ToList();
                    
                    if (scmdata.Count() != 0)
                    {

                        string CollegeMailText = string.Empty;
                        IEnumerable<string> CollegeEmailIds;
                        //SCM Report Generation Code
                        var filepath = SaveSCMReportPdfForPrincipal(0, scmdata);
                        filepath = filepath.Replace("/", "\\");
                        attachments.Add(new Attachment(filepath));


                        //second Attachment Get Path of the File
                        var collegeId = scmdata.GroupBy(e => e.CollegeId).Select(e => e.Key).FirstOrDefault();
                        var filepathsecond = SaveSCMReportSeconadAttachment(0, collegeId);
                        filepathsecond = filepathsecond.Replace("/", "\\");
                        attachments.Add(new Attachment(filepathsecond));


                        List<jntuh_ffc_auditor> aditorslist = db.jntuh_ffc_auditor.Where(e => e.isActive == true && aditorsIds.Contains(e.id)).Select(e => e).ToList();

                        var collegedata = (from SCMREQ in db.jntuh_scmproceedingsrequests
                                           join CLG in db.jntuh_college on SCMREQ.CollegeId equals CLG.id
                                           where SCMId.Contains(SCMREQ.ID)
                                           select new
                                           {
                                               CollegeCode = CLG.collegeCode,
                                               CollegeName = CLG.collegeName,
                                             //  DepartmentName = DEG.degree + "-" + SPEC.specializationName,
                                               ScmRequestDate = SCMREQ.RequestSubmittedDate
                                           }).FirstOrDefault();


                        CollegeEmailIds = db.jntuh_address.Where(e => e.collegeId == collegeId).Select(e => e.email).ToList();
                        var princiaplEmail = db.jntuh_college_principal_director.Where(e => e.collegeId == collegeId && e.type == "PRINCIPAL").Select(e => e.email).FirstOrDefault();
                        CollegeEmailIds = CollegeEmailIds.Concat(new string[] { princiaplEmail });



                        var nomineeRegistration = db.jntuh_registration.AsNoTracking().ToList();
                        foreach (var item in aditorslist)
                        {
                            if (item != null)
                            {

                               //await mailer.SendAttachmentToAllColleges(item.auditorEmail1, "", "",
                               //    "SCM PROCEEDINGS REQUEST", "Scm Requests", attachments).SendAsync();
                              //  status = UAAAS.Models.Utilities.SendSMS(item.auditorMobile1, "Your Nominee Details Send to Your mail");

                                string UserName = string.Empty;
                                string Password = string.Empty;
                                var nomineeusernameandpassword = nomineeRegistration.Where(e => e.Email == item.auditorEmail1).Select(e => e).FirstOrDefault();
                                if (nomineeusernameandpassword != null)
                                {
                                    UserName = nomineeusernameandpassword.Email;
                                    Password = nomineeusernameandpassword.Password == item.auditorMobile1.Substring(5, 5) ? nomineeusernameandpassword.Password : "***********"; ;
                                }


                                string mailsendtonomineesubject = "JNTUH-DUAAC: Allotment of University Nominee for Principal Selection:- Reg.";
                                string mailsendtonomineebody = "<div><p>Dear Sir/Madam,</p></div>";
                                mailsendtonomineebody += "<p>Vice-Chancellor is pleased to appoint you as University Nominee for principal selections of " + collegedata.CollegeName + " with College Code " + collegedata.CollegeCode + ". You are required to follow the procedure detailed below:</p>";//in ----Your Co-nominee is " + aditorslist.Where(e => e.id != item.id).Select(e => e.auditorName).FirstOrDefault() + ",
                                mailsendtonomineebody += "<p>1) The list of candidates to be interviewed along with their registrations Ids is herewith attached. You are required to conduct interviews only for these candidates in the list.</p>";
                                mailsendtonomineebody += "<p>2) Once the candidates are selected you are requested to fill the SCM Minutes in the enclosed format.</p>";
                                mailsendtonomineebody += "<p>3) Further, you are requested to login to the URL http://112.133.193.228:76/ with following login details.</p>";
                                mailsendtonomineebody += "<p style='text-align:left'> Login:" + UserName + "<br/>Password:" + Password + "</p>";
                                mailsendtonomineebody += "<p>4) After logging please upload the scanned copy of SCM minutes duly signed by all the members. Also please indicate the selected candidates with a tick mark in the list shown in the URL.</p>";
                               // mailsendtonomineebody += "<p><b><u>Note:</u></b> The above procedure can be performed in the college itself (where selections were done).</p>";// by one of the nominees in coordination with other nominee immediately after the completion of the selection process.
                                mailsendtonomineebody += "<br/><p><b style='font-size:15px'>REGISTRAR,JNTUH</b></p>";
                                //Send SNS to Nominee For SCM Request Message
                                string SMSText = "Dear Sir/Madam,\n Vice-Chancellor is pleased to appoint you as University nominee for principal selection  of " + collegedata.CollegeName + " with College Code " + collegedata.CollegeCode + ". Please go through your registered Mail Id for further details.\n REGISTRAR,JNTUH";//Your Co-nominee is " + aditorslist.Where(e => e.id != item.id).Select(e => e.auditorName).FirstOrDefault() + "


                                var message = new MailMessage();
                                message.To.Add(item.auditorEmail1);
                                // message.CC.Add("");
                                //  message.Bcc.Add("");
                                message.Subject = mailsendtonomineesubject;
                                message.Body = mailsendtonomineebody;
                                message.IsBodyHtml = true;
                                message.Attachments.Add(new Attachment(filepath));
                                message.Attachments.Add(new Attachment(filepathsecond));
                                var smtp = new SmtpClient();
                                //smtp.Credentials = new NetworkCredential("aac.do.not.reply@gmail.com", "uaaas@aac");
                                smtp.Credentials = new NetworkCredential("staffselection3@jntuh.ac.in", "jntu@123");
                                smtp.Host = "smtp.gmail.com";
                                smtp.Port = 587;
                                smtp.EnableSsl = true;
                                await smtp.SendMailAsync(message);
                                //Send SMS Code
                                status = UAAAS.Models.Utilities.SendSMS(item.auditorMobile1, SMSText);

                                foreach (var Ids in SCMId)
                                {
                                    UAAASSCM.Models.jntuh_auditor_assigned auditors = new UAAASSCM.Models.jntuh_auditor_assigned();
                                    auditors.AuditorId = item.id;
                                    auditors.ScmId = Ids;
                                    auditors.IsActive = true;
                                    auditors.SCMListPath = PdfGenerationpath;
                                    auditors.CreatedBy = 1;
                                    auditors.CreatedOn = DateTime.Now;
                                    db.jntuh_auditor_assigned.Add(auditors);
                                }
                              
                            }
                        }
                        db.SaveChanges();



                        //Send Mail to College For SCM Request Body Text
                        string CollegeMailSubject = "JNTUH-DUAAC: Allotment of University Nominee for Principal Selection:- Reg.";
                        CollegeMailText = "<div><p>Dear Sir/Madam,</p></div>";
                        CollegeMailText += "<p>College Name:" + collegedata.CollegeName + "</p>";
                        CollegeMailText += "<p>As per your requested dated: " + UAAAS.Models.Utilities.MMDDYY2DDMMYY(collegedata.ScmRequestDate.ToString()) + ", ";//
                        CollegeMailText += "the following University Nominee is allotted for Principal Selection at your college.</p>";//in --- Department" + collegedata.DepartmentName + "
                        CollegeMailText += "<p><b>You are required to complete the selection process and upload the SCM Minuties duly signed by the Selection Committee Members within 4 days from the date of receipt of this mail.</b></p>";
                        CollegeMailText += "<p><b>Nominee Details: </b></p><br/>";
                        CollegeMailText += "<table border='1'>";
                        CollegeMailText += "<tr>";
                        CollegeMailText += "<th>Name</th><th>Email</th><th>Mobile</th><th>Designation</th>";
                        CollegeMailText += "</tr>";
                        if (aditorslist.Count() != 0)
                        {
                            foreach (var auditor in aditorslist)
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
                        CollegeMailText += "<br/><p><b style='font-size:15px'>REGISTRAR,JNTUH</b></p>";

                        //Send Mail to College For SCM Request

                        foreach (var collegeEmailId in CollegeEmailIds)
                        {
                            var collegemessage = new MailMessage();
                            collegemessage.To.Add(collegeEmailId);
                            // message.CC.Add("");
                            //  message.Bcc.Add("");
                            collegemessage.Subject = CollegeMailSubject;
                            collegemessage.Body = CollegeMailText;
                            collegemessage.IsBodyHtml = true;
                            // message.Attachments.Add(new Attachment(filepath));
                            var collegesmtp = new SmtpClient();
                            //collegesmtp.Credentials = new NetworkCredential("aac.do.not.reply@gmail.com", "uaaas@aac");
                            collegesmtp.Credentials = new NetworkCredential("staffselection3@jntuh.ac.in", "jntu@123");
                            collegesmtp.Host = "smtp.gmail.com";
                            collegesmtp.Port = 587;
                            collegesmtp.EnableSsl = true;
                            await collegesmtp.SendMailAsync(collegemessage);
                        }

                        TempData["Success"] = "Mail and SMS Sent Succesfully.....!";
                        return RedirectToAction("SCMPrincipalview", "Admin");
                    }
                }
            }
            return RedirectToAction("SCMPrincipalview", "Admin");
        }

        #region SCM Download Only
        public string SaveSCMReportPdfDeptWise(int preview, List<jntuh_scmproceedingsrequests> scmdata)
        {
            string fullPath = string.Empty;
            var collegeId = scmdata.GroupBy(e => e.CollegeId).Select(e => e.Key).FirstOrDefault();
            var collegedata = db.jntuh_college.Where(e => e.isActive == true && e.id == collegeId).Select(e => e).FirstOrDefault();

            var specializationId= scmdata.GroupBy(e => e.SpecializationId).Select(e => e.Key).FirstOrDefault();
            var speci = db.jntuh_specialization.Where(e => e.id == specializationId).Select(e => e).FirstOrDefault();
            var Dept = "";
            if (speci != null)
            {
                Dept = speci.jntuh_department.jntuh_degree.degree + "-" + speci.specializationName;
            }
            //Set page size as A4
            Document pdfDoc = new Document(PageSize.A4, 60, 50, 60, 60);
            string path = Server.MapPath("~/Content/PDFReports/SCMReportDownload");
            if (!Directory.Exists(Server.MapPath("~/Content/PDFReports/SCMReportDownload")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Content/PDFReports/SCMReportDownload"));
            }

            if (preview == 0)
            {
              //  
               string  pdffilename = collegedata.collegeCode + "- SCM Report Download -" + Dept.Replace("/", "_") + DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf";
                PdfGenerationpath = pdffilename;
                fullPath = path + "/" + pdffilename;
                    //collegedata.collegeCode + "- SCM Report Download -" + Dept.Replace("/","_") + DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf";//
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(fullPath, FileMode.Create));
                ITextEvents iTextEvents = new ITextEvents();
                iTextEvents.CollegeCode = collegedata.collegeCode;
                iTextEvents.CollegeName = collegedata.collegeName;
                iTextEvents.formType = "Scm Report Download" + Dept.Replace("/", "_");
                pdfWriter.PageEvent = iTextEvents;
            }
            //Open PDF Document to write data
            pdfDoc.Open();

            //Assign Html content in a string to write in PDF
            string contents = string.Empty;

            StreamReader sr;

            //Read file from server path
            sr = System.IO.File.OpenText(Server.MapPath("~/Content/SCMReportDownloadDeptWise.html"));
            //store content in the variable
            contents = sr.ReadToEnd();
            sr.Close();

            //  contents = contents.Replace("##SERVERURL##", serverURL);



            int[] SCMIds = scmdata.GroupBy(e => e.ID).Select(e => e.Key).ToArray();

            contents = GetScmReportDataDeptWise(contents, collegeId, specializationId, SCMIds);

            //Read string contents using stream reader and convert html to parsed conent
            var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(contents), null);
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
        public string GetScmReportDataDeptWise(string contents, int collegeId, int specializationId, int[] SCMId)
        {
            string contentdata = string.Empty;

           
            var jntuh_college = db.jntuh_college.Where(e => e.isActive == true).Select(e => e).ToList();
            var collegedata = jntuh_college.Where(e => e.isActive == true && e.id == collegeId).Select(e => e).FirstOrDefault();

            //List<AddfacultyDetails> scmfacultydataList = (from SCM in db.jntuh_scmproceedingsrequests.AsNoTracking()
            //                                              join SCMADDFLY in db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking() on SCM.ID equals SCMADDFLY.ScmProceedingId
            //                                              where SCM.CollegeId == collegeId && SCM.SpecializationId == specializationId && SCM.ID == SCMId
            //                                              select new AddfacultyDetails
            //                                              {
            //                                                  RegistrationNumber = SCMADDFLY.RegistrationNumber,
            //                                                  SpecializationId = SCM.SpecializationId,
            //                                                  DegreeId = SCM.DegreeId,
            //                                                  ScmProceedingId = SCMADDFLY.ScmProceedingId,
            //                                                  FacultyAddId = SCMADDFLY.Id,
            //                                                  DepartmentId = SCM.DEpartmentId,
            //                                                  CollegeId = SCM.CollegeId

            //                                              }).ToList();



            List<ReportSCMRegData> scmregdata = (from SCM in db.jntuh_scmproceedingsrequests.AsNoTracking()
                              join SCMADDFLY in db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking() on SCM.ID equals SCMADDFLY.ScmProceedingId
                              join regfaculty in db.jntuh_registered_faculty on SCMADDFLY.RegistrationNumber.Trim() equals regfaculty.RegistrationNumber.Trim()
                              join speci in db.jntuh_specialization on SCM.SpecializationId equals speci.id
                              join deg in db.jntuh_degree on SCM.DegreeId equals deg.id
                              join dept in db.jntuh_department on SCM.DEpartmentId equals dept.id
                                                 where SCM.CollegeId == collegeId && SCMId.Contains(SCM.ID) && SCM.RequestSubmittedDate != null 
                              select new ReportSCMRegData
                              {
                                  FacultyName = regfaculty.FirstName + " " + regfaculty.LastName,
                                  RegNo = regfaculty.RegistrationNumber,
                                  PANNo = regfaculty.PANNumber,
                                  Experience = regfaculty.TotalExperience,
                                  facultyId = (int?)regfaculty.id,
                                  Branch = deg.degree + "-" + speci.specializationName,
                                //  PreviousCollege = SCMADDFLY.PreviousCollegeId,
                                  Remarks = SCMADDFLY.DeactiviationReason,
                                  FacultyTypeId=SCMADDFLY.FacultyType,
                                  AadhaarNo = regfaculty.AadhaarNumber,
                                  DeptId=dept.id,
                                  SpecId = speci.id,
                                  SCMRequestSubmittedDate = SCM.RequestSubmittedDate,
                                  SCMId = SCM.ID,
                                  Blacklist = regfaculty.Blacklistfaculy
                              }).ToList();


            scmregdata = scmregdata.Where(e => e.RegNo != null && e.facultyId != null &&e.FacultyTypeId!=1).OrderBy(e=>e.FacultyTypeId).Select(e => e).ToList();

            if (collegedata != null)
            {
                contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
                contentdata += "<tr><td style='text-align:left' width='90%'><b>College Name : </b>" + collegedata.collegeName + "</td>";
                contentdata += "<td style='text-align:left' width='20%'><b>Code : </b>" + collegedata.collegeCode + "</td></tr>";
                contentdata += "</table>";
            }
            //contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            //contentdata += "<tr><td style='text-align:left' width='70%'><b>Proceeding No : </b></td>";
            //contentdata += "<td style='text-align:left' width='30%'><b>Date : </b></td></tr>";
            //contentdata += "</table>";

            contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:center'><b>Affiliated to</b></td></tr>";
            contentdata += "<tr><td style='text-align:center'><b>(JAWAHARLAL NEHRU TECHNOLOGICAL UNIVERSITY HYDERABAD, KUKATPALLY, HYDERABAD)</b></td></tr>";
            contentdata += "</table>";

            contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left'>List of Candidates to be interviewed by the Selection Committee</td></tr></table>";

            //var DeptIds = scmregdata.GroupBy(e => e.DeptId).Select(e => e.Key).Distinct().ToArray();
            //foreach (var DeptId in DeptIds)
            //{
            //    contentdata += DeptWiseSCMData(scmregdata.Where(e => e.DeptId == DeptId).Select(e => e).ToList());
            //}

            var SpecIds = scmregdata.GroupBy(e => e.SpecId).Select(e => e.Key).Distinct().ToArray();
            foreach (var SpecId in SpecIds)
            {
                contentdata += DeptWiseSCMData(scmregdata.Where(e => e.SpecId == SpecId).Select(e => e).ToList());
            }


            #region
            //contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            //contentdata += "<tr><td style='text-align:left' width='10%'><b>Sno</b></td>";
            //contentdata += "<td style='text-align:left' width='30%' colspan='2'><b>Role</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b>Name</b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b>Signature</b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>1</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>Chairperson</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>2</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>Principal</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>3</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>University Nominee 1</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>4</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>University Nominee 2</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>5</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>Expert 1</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>6</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>Expert 2</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>7</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>SC/ST/OBC/Women/<br/>Differently Abled if any</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";
            //contentdata += "</table>";
            #endregion
            contents = contents.Replace("##SCMREPORT##", contentdata);
            return contents;
        }

        public string DeptWiseSCMData(List<ReportSCMRegData> scmregdata)
        {
            var jntuh_registered_faculty_education = db.jntuh_registered_faculty_education.AsNoTracking().ToList();
            scmregdata = scmregdata.OrderBy(e => e.FacultyTypeId).Select(e => e).ToList();

            string DateText = string.Empty;
            foreach (var date in scmregdata.GroupBy(e => e.SCMId).Select(e =>new{ SCMRequestdate=e.FirstOrDefault().SCMRequestSubmittedDate}).ToList())
            {
                DateText += UAAAS.Models.Utilities.MMDDYY2DDMMYY(date.SCMRequestdate.ToString()) + ",";
            }
            DateText = DateText.Substring(0, DateText.Length - 1);

            string Departmentnames = string.Empty;
            foreach (var date in scmregdata.GroupBy(e => e.SCMId).Select(e => new { DeptName = e.FirstOrDefault().Branch }).ToList())
            {
                Departmentnames += date.DeptName + ",";
            }
            Departmentnames = Departmentnames.Substring(0, Departmentnames.Length - 1);



            string contentdata = string.Empty;
            contentdata += "<br/><table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left' width='30%'><b>Post</b></td><td  width='5%'>:</td><td  style='text-align:left'></td></tr>";
            if (scmregdata.Count() != 0)
            {
                contentdata += "<tr><td style='text-align:left' width='30%'><b>Department</b></td><td width='5%'>:</td><td style='text-align:left'>" + Departmentnames + "</td></tr>";//  scmregdata[0].Branch
            }
            else
            {
                contentdata += "<tr><td style='text-align:left' width='30%'><b>Department</b></td><td width='5%'>:</td><td style='text-align:left'></td></tr>";
            }
            contentdata += "<tr><td style='text-align:left' width='30%'><b>Scale of Pay</b></td><td width='5%'>:</td><td style='text-align:left'></td></tr>";
            contentdata += "<tr><td style='text-align:left' width='30%'><b>SCM Request Submitted Date</b></td><td width='5%'>:</td><td style='text-align:left'>" + DateText + "</td></tr>";
            contentdata += "</table><br/>";

            contentdata += "<table border='1'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left' width='8%'><b>Sno</b></td>";
            contentdata += "<td style='text-align:left' width='22%'><b>Faculty Name</b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b>Reg. Number</b></td>";
            contentdata += "<td style='text-align:left' width='10%'><b>PAN No</b></td>";
            contentdata += "<td style='text-align:left' width='10%'><b>Aadhar No</b></td>";
            contentdata += "<td style='text-align:left' width='10%'><b>UG Branch</b></td>";
            contentdata += "<td style='text-align:left' width='11%'><b>PG Specialization</b></td>";
            contentdata += "<td style='text-align:left' width='9%'><b>Post</b></td>";

            contentdata += "</tr>";
            if (scmregdata.Count() != 0)
            {
                for (int i = 0; i < scmregdata.Count(); i++)
                {

                    string ugBranch = string.Empty;
                    string pgBranch = string.Empty;
                    string phdBranch = string.Empty;
                    string FacultyType = string.Empty;
                    string Blacklistfaculty = string.Empty;

                    if (scmregdata[i].facultyId != null)
                    {
                        var ugdata = jntuh_registered_faculty_education.Where(e => e.educationId == 3 && e.facultyId == scmregdata[i].facultyId).Select(e => e.specialization).FirstOrDefault();
                        if (ugdata != null)
                            ugBranch = ugdata;

                        var pgdata = jntuh_registered_faculty_education.Where(e => e.educationId == 4 && e.facultyId == scmregdata[i].facultyId).Select(e => e.specialization).FirstOrDefault();
                        if (pgdata != null)
                            pgBranch = pgdata;

                        var phddata = jntuh_registered_faculty_education.Where(e => e.educationId == 6 && e.facultyId == scmregdata[i].facultyId).Select(e => e.specialization).FirstOrDefault();
                        if (phddata != null)
                            phdBranch = phddata;
                    }

                    if (scmregdata[i].FacultyTypeId == 2)
                    {
                        FacultyType = "Assoc.Prof";
                    }
                    else if (scmregdata[i].FacultyTypeId == 3)
                    {
                        FacultyType = "Assit.Prof";
                    }

                    if (scmregdata[i].Blacklist == true)
                    {
                        Blacklistfaculty = "<b>(BLACK LIST)</b>";
                    }


                    contentdata += "<tr>";
                    contentdata += "<td style='text-align:left' width='8%'>" + (i + 1) + "</td>";
                    contentdata += "<td style='text-align:left' width='22%'>" + scmregdata[i].FacultyName + "</td>";
                    contentdata += "<td style='text-align:left' width='20%'>" + scmregdata[i].RegNo + "</br>" + Blacklistfaculty + "</td>";
                    contentdata += "<td style='text-align:left' width='10%'>" + scmregdata[i].PANNo + "</td>";
                    contentdata += "<td style='text-align:left' width='10%'>" + scmregdata[i].AadhaarNo + "</td>";
                    contentdata += "<td style='text-align:left' width='10%'>" + ugBranch + "</td>";
                    contentdata += "<td style='text-align:left' width='11%'>" + pgBranch + "</td>";
                    contentdata += "<td style='text-align:left' width='9%'>" + FacultyType + "</td>";

                    //contentdata += "<td style='text-align:left' width='10%'>" + scmregdata[i].Experience + "</td>";
                    //if (scmregdata[i].PreviousCollege != null)
                    //    contentdata += "<td style='text-align:left' width='10%'>" + jntuh_college.Where(e => e.id == scmregdata[i].PreviousCollege).Select(e => e.collegeName).FirstOrDefault() + "</td>";
                    //else
                    //    contentdata += "<td style='text-align:left' width='10%'></td>";
                    //contentdata += "<td style='text-align:left' width='10%'>" + scmregdata[i].Remarks + "</td>";
                    contentdata += "</tr>";
                }
            }
            contentdata += "</table><br/><br/><br/>";
            return contentdata;
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

            //var jntuh_registered_faculty_education = db.jntuh_registered_faculty_education.AsNoTracking().ToList();
            //var jntuh_college = db.jntuh_college.Where(e => e.isActive == true).Select(e => e).ToList();
            //var collegedata = jntuh_college.Where(e => e.isActive == true && e.id == collegeId).Select(e => e).FirstOrDefault();

            //var scmregdata = (from SCM in db.jntuh_scmproceedingsrequests.AsNoTracking()
            //                  join SCMADDFLY in db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking() on SCM.ID equals SCMADDFLY.ScmProceedingId
            //                  join regfaculty in db.jntuh_registered_faculty on SCMADDFLY.RegistrationNumber equals regfaculty.RegistrationNumber
            //                  join speci in db.jntuh_specialization on SCM.SpecializationId equals speci.id
            //                  join deg in db.jntuh_degree on SCM.DegreeId equals deg.id
            //                  join dept in db.jntuh_department on SCM.DEpartmentId equals dept.id
            //                  where SCM.CollegeId == collegeId && SCM.SpecializationId == specializationId && SCM.ID == SCMId
            //                  select new
            //                  {
            //                      FacultyName = regfaculty.FirstName + " " + regfaculty.LastName,
            //                      RegNo = regfaculty.RegistrationNumber,
            //                      PANNo = regfaculty.PANNumber,
            //                      Experience = regfaculty.TotalExperience,
            //                      facultyId = (int?)regfaculty.id,
            //                      Branch = deg.degree + "-" + speci.specializationName,
            //                      //  PreviousCollege = SCMADDFLY.PreviousCollegeId,
            //                      Remarks = SCMADDFLY.DeactiviationReason,
            //                      FacultyTypeId = SCMADDFLY.FacultyType,
            //                      AadhaarNo = regfaculty.AadhaarNumber
            //                  }).ToList();


            //scmregdata = scmregdata.Where(e => e.RegNo != null && e.facultyId != null && e.FacultyTypeId != 1).Select(e => e).ToList();



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
        #endregion

        #region SCM Report send For Principal
        public string SaveSCMReportPdfForPrincipal(int preview, List<jntuh_scmproceedingsrequests> scmdata)
        {
            string fullPath = string.Empty;
            var collegeId = scmdata.GroupBy(e => e.CollegeId).Select(e => e.Key).FirstOrDefault();
            var collegedata = db.jntuh_college.Where(e => e.isActive == true && e.id == collegeId).Select(e => e).FirstOrDefault();
          
            var Dept = "";
          
            //Set page size as A4
            Document pdfDoc = new Document(PageSize.A4, 60, 50, 60, 60);
            string path = Server.MapPath("~/Content/PDFReports/SCMReportDownloadForPrincipal");
            if (!Directory.Exists(Server.MapPath("~/Content/PDFReports/SCMReportDownloadForPrincipal")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Content/PDFReports/SCMReportDownloadForPrincipal"));
            }

            if (preview == 0)
            {
                string pdffilename = collegedata.collegeCode + "-Principal SCM Report Download -" + DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf";//
                PdfGenerationpath = pdffilename;
                fullPath = path + "/" + pdffilename;
                    //collegedata.collegeCode + "-Principal SCM Report Download -"+ DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf";//
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(fullPath, FileMode.Create));
                ITextEvents iTextEvents = new ITextEvents();
                iTextEvents.CollegeCode = collegedata.collegeCode;
                iTextEvents.CollegeName = collegedata.collegeName;
                iTextEvents.formType = "Scm Report Download For Principal";
                pdfWriter.PageEvent = iTextEvents;
            }
            //Open PDF Document to write data
            pdfDoc.Open();

            //Assign Html content in a string to write in PDF
            string contents = string.Empty;

            StreamReader sr;

            //Read file from server path
            sr = System.IO.File.OpenText(Server.MapPath("~/Content/SCMReportDownloadDeptWise.html"));
            //store content in the variable
            contents = sr.ReadToEnd();
            sr.Close();

            //  contents = contents.Replace("##SERVERURL##", serverURL);

            int[] SCMIds = scmdata.GroupBy(e => e.ID).Select(e => e.Key).ToArray();
            contents = GetScmReportDataForPrincipal(contents, collegeId, SCMIds);

            //Read string contents using stream reader and convert html to parsed conent
            var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(contents), null);
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

       // 
        public string GetScmReportDataForPrincipal(string contents, int collegeId,int[] SCMId)
        {
            string contentdata = string.Empty;

            var jntuh_registered_faculty_education = db.jntuh_registered_faculty_education.AsNoTracking().ToList();
            var jntuh_college = db.jntuh_college.Where(e => e.isActive == true).Select(e => e).ToList();
            var collegedata = jntuh_college.Where(e => e.isActive == true && e.id == collegeId).Select(e => e).FirstOrDefault();

            var scmregdata = (from SCM in db.jntuh_scmproceedingsrequests.AsNoTracking()
                              join SCMADDFLY in db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking() on SCM.ID equals SCMADDFLY.ScmProceedingId
                              join regfaculty in db.jntuh_registered_faculty on SCMADDFLY.RegistrationNumber equals regfaculty.RegistrationNumber
                              where SCM.DEpartmentId == 0 && SCM.SpecializationId == 0 && SCMId.Contains(SCM.ID) && SCM.DegreeId == 0 
                              select new
                              {
                                  FacultyName = regfaculty.FirstName + " " + regfaculty.LastName,
                                  RegNo = regfaculty.RegistrationNumber,
                                  PANNo = regfaculty.PANNumber,
                                  AadhaarNo = regfaculty.AadhaarNumber,
                                  Experience = regfaculty.TotalExperience,
                                  facultyId = (int?)regfaculty.id,
                                //  Branch = deg.degree + "-" + speci.specializationName,
                                  //  PreviousCollege = SCMADDFLY.PreviousCollegeId,
                                  Remarks = SCMADDFLY.DeactiviationReason,
                                  BlacklistFaculty = regfaculty.Blacklistfaculy
                              }).ToList();


            scmregdata = scmregdata.Where(e => e.RegNo != null && e.facultyId != null).Select(e => e).ToList();




            if (collegedata != null)
            {
                contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
                contentdata += "<tr><td style='text-align:left' width='90%'><b>College Name : </b>" + collegedata.collegeName + "</td>";
                contentdata += "<td style='text-align:left' width='20%'><b>Code : </b>" + collegedata.collegeCode + "</td></tr>";
                contentdata += "</table>";
            }
            //contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            //contentdata += "<tr><td style='text-align:left' width='70%'><b>Proceeding No : </b></td>";
            //contentdata += "<td style='text-align:left' width='30%'><b>Date : </b></td></tr>";
            //contentdata += "</table>";

            contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:center'><b>Affiliated to</b></td></tr>";
            contentdata += "<tr><td style='text-align:center'><b>(JAWAHARLAL NEHRU TECHNOLOGICAL UNIVERSITY HYDERABAD, KUKATPALLY, HYDERABAD)</b></td></tr>";
            contentdata += "</table>";

            contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left'>List of Candidates to be interviewed by the Selection Committee</td></tr></table>";


            contentdata += "<br/><table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left' width='30%'><b>Post</b></td><td  width='5%'>:</td><td  style='text-align:left'></td></tr>";
            //if (scmregdata.Count() != 0)
            //{
            //    contentdata += "<tr><td style='text-align:left' width='30%'><b>Department</b></td><td width='5%'>:</td><td style='text-align:left'>" + scmregdata[0].Branch + "</td></tr>";
            //}
            //else
            //{
            //    contentdata += "<tr><td style='text-align:left' width='30%'><b>Department</b></td><td width='5%'>:</td><td style='text-align:left'></td></tr>";
            //}
            contentdata += "<tr><td style='text-align:left' width='30%'><b>Department</b></td><td width='5%'>:</td><td style='text-align:left'></td></tr>";
            contentdata += "<tr><td style='text-align:left' width='30%'><b>Scale of Pay</b></td><td width='5%'>:</td><td style='text-align:left'></td></tr>";
            contentdata += "</table><br/>";

            contentdata += "<table border='1'cellspacing='0' cellpadding='4' width='100%'>";
            contentdata += "<tr><td style='text-align:left' width='8%'><b>Sno</b></td>";
            contentdata += "<td style='text-align:left' width='22%'><b>Faculty Name</b></td>";
            contentdata += "<td style='text-align:left' width='20%'><b>Reg. Number</b></td>";
            contentdata += "<td style='text-align:left' width='8%'><b>PAN No</b></td>";
            contentdata += "<td style='text-align:left' width='8%'><b>Aadhar No</b></td>";
            contentdata += "<td style='text-align:left' width='8%'><b>UG Branch</b></td>";
            contentdata += "<td style='text-align:left' width='8%'><b>PG Specialization</b></td>";
            contentdata += "<td style='text-align:left' width='8%'><b>Ph.D Area</b></td>";
            contentdata += "</tr>";
            if (scmregdata.Count() != 0)
            {
                for (int i = 0; i < scmregdata.Count(); i++)
                {

                    string ugBranch = string.Empty;
                    string pgBranch = string.Empty;
                    string phdBranch = string.Empty;
                    string blacklist = string.Empty;

                    if (scmregdata[i].facultyId != null)
                    {
                        var ugdata = jntuh_registered_faculty_education.Where(e => e.educationId == 3 && e.facultyId == scmregdata[i].facultyId).Select(e => e.specialization).FirstOrDefault();
                        if (ugdata != null)
                            ugBranch = ugdata;

                        var pgdata = jntuh_registered_faculty_education.Where(e => e.educationId == 4 && e.facultyId == scmregdata[i].facultyId).Select(e => e.specialization).FirstOrDefault();
                        if (pgdata != null)
                            pgBranch = pgdata;

                        var phddata = jntuh_registered_faculty_education.Where(e => e.educationId == 6 && e.facultyId == scmregdata[i].facultyId).Select(e => e.specialization).FirstOrDefault();
                        if (phddata != null)
                            phdBranch = phddata;
                    }

                    if (scmregdata[i].BlacklistFaculty == true)
                    {
                        blacklist = "<b>(BLACK LIST)</b>";
                    }




                    contentdata += "<tr>";
                    contentdata += "<td style='text-align:left' width='8%'>" + (i + 1) + "</td>";
                    contentdata += "<td style='text-align:left' width='22%'>" + scmregdata[i].FacultyName + "</td>";
                    contentdata += "<td style='text-align:left' width='20%'>" + scmregdata[i].RegNo + "<br/>" + blacklist + "</td>";
                    contentdata += "<td style='text-align:left' width='8%'>" + scmregdata[i].PANNo + "</td>";
                    contentdata += "<td style='text-align:left' width='8%'>" + scmregdata[i].AadhaarNo + "</td>";
                    contentdata += "<td style='text-align:left' width='8%'>" + ugBranch + "</td>";
                    contentdata += "<td style='text-align:left' width='8%'>" + pgBranch + "</td>";
                    contentdata += "<td style='text-align:left' width='8%'>" + phdBranch + "</td>";
                    //contentdata += "<td style='text-align:left' width='10%'>" + scmregdata[i].Experience + "</td>";
                    //if (scmregdata[i].PreviousCollege != null)
                    //    contentdata += "<td style='text-align:left' width='10%'>" + jntuh_college.Where(e => e.id == scmregdata[i].PreviousCollege).Select(e => e.collegeName).FirstOrDefault() + "</td>";
                    //else
                    //    contentdata += "<td style='text-align:left' width='10%'></td>";
                    //contentdata += "<td style='text-align:left' width='10%'>" + scmregdata[i].Remarks + "</td>";
                    contentdata += "</tr>";
                }
            }
            contentdata += "</table><br/>";


            //contentdata += "<table border='0'cellspacing='0' cellpadding='4' width='100%'>";
            //contentdata += "<tr><td style='text-align:left' width='10%'><b>Sno</b></td>";
            //contentdata += "<td style='text-align:left' width='30%' colspan='2'><b>Role</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b>Name</b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b>Signature</b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>1</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>Chairperson</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>2</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>Principal</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>3</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>University Nominee 1</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>4</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>University Nominee 2</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>5</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>Expert 1</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>6</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>Expert 2</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";

            //contentdata += "<tr><td style='text-align:left' width='10%'><b>7</b></td>";
            //contentdata += "<td style='text-align:left' width='25%'><b>SC/ST/OBC/Women/<br/>Differently Abled if any</b></td>";
            //contentdata += "<td style='text-align:left' width='5%'><b>:</b></td>";
            //contentdata += "<td style='text-align:left' width='40%'><b></b></td>";
            //contentdata += "<td style='text-align:left' width='20%'><b></b></td></tr>";
            //contentdata += "</table>";

            contents = contents.Replace("##SCMREPORT##", contentdata);
            return contents;
        }

        #endregion




        public async Task<ActionResult> SampleEmailDemo()
        {
             var message = new MailMessage();
                            message.To.Add("sureshpalsa5@gmail.com");
                            // message.CC.Add("");
                            //  message.Bcc.Add("");
                            message.Subject = "Demo Email";
                            message.Body = "Welcome to SCM Module";
                            message.IsBodyHtml = true;
                            //message.Attachments.Add(new Attachment(filepath));
                           // message.Attachments.Add(new Attachment(filepathsecond));
                            var smtp = new SmtpClient();
                            //smtp.Credentials = new NetworkCredential("aac.do.not.reply@gmail.com", "uaaas@aac");
                            smtp.Credentials = new NetworkCredential("staffselection3@jntuh.ac.in", "jntu@123");
                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            await smtp.SendMailAsync(message);
                            return RedirectToAction("UserCreationView","Admin");
        }

    }

    #region Model Class
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

        public int RequiredProfessors { get; set; }
        public int RequiredAssociateProfessors { get; set; }
        public int RequiredAssistantProfessors { get; set; }



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

        public DateTime CreatedDate { get; set; }

        public DateTime? RequestSubmittedDate { get; set; }


        public bool IsEdit { get; set; }
        [Required(ErrorMessage = "*")]
        public int? TotalFacultyRequired { get; set; }
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
        [Remote("CheckRegistrationNumber", "CollegeSCMProceedingsRequestForAdmin", HttpMethod = "POST", ErrorMessage = "Registration Number doesn't Exist")]
        public string RegistrationNo { get; set; }

        public string Regno { get; set; }
        public string RegName { get; set; }

        public int FacultyId { get; set; }
       
        public int PreviousCollegeId { get; set; }

        public string Designation { get; set; }

        public int DesignationId { get; set; }
        public bool? OtherFacultyMovingStatus { get; set; }
    }

    public class AditorsDetails
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
        public string SCMIds { get; set; }

    }

    public class ListSCMIds
    {
        public int SCMID { get; set; }
    }


    public class ReportSCMRegData
    {
        public string FacultyName { get; set; }
        public string RegNo { get; set; }
        public string PANNo { get; set; }
        public int? Experience { get; set; }
        public int? facultyId { get; set; }
        public string Branch { get; set; }
        public string Remarks { get; set; }
        public int? FacultyTypeId { get; set; }
        public string AadhaarNo { get; set; }
        public int? DeptId { get; set; }
        public DateTime? SCMRequestSubmittedDate { get; set; }
        public int SCMId { get; set; }
        public int? SpecId { get; set; }
        public bool? Blacklist { get; set; }
    }
#endregion

}