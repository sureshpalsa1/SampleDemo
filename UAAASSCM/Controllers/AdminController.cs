using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.Ajax.Utilities;
using UAAAS.Controllers.College;
using UAAAS.Models;
using UAAASSCM.Models;


namespace UAAASSCM.Controllers
{
    public class AdminController : Controller
    {

        private SCMEntities db = new SCMEntities();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    FormsAuthentication.SignOut();
            //}
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserLogin logindata)
        {
            if (ModelState.IsValid)
            {
                bool isvaild1 = IsvalidorNot(logindata);
                if (isvaild1)
                {
                    string rolename = GetUserRole(logindata);
                    if (!string.IsNullOrEmpty(rolename))
                    {
                        if (rolename == "Admin")
                        {
                            FormsAuthentication.SetAuthCookie(logindata.UserName, false);
                            return RedirectToAction("UserCreationView");
                        }
                        else if (rolename == "Faculty")
                        {
                            FormsAuthentication.SetAuthCookie(logindata.UserName, false);
                            return RedirectToAction("showSCMmenuforUser", "User");
                        }
                        else if (rolename == "FacultyVerification")
                        {
                            FormsAuthentication.SetAuthCookie(logindata.UserName, false);
                            return RedirectToAction("UserCreationView");
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Please Enter Correct Details,Try Again.";
                    return RedirectToAction("Login");
                }


                //if (logindata.UserName == "admin" && logindata.Password == "admin@2017")
                //{
                //    FormsAuthentication.SetAuthCookie(logindata.UserName, false);
                //    return RedirectToAction("UserCreationView");
                //}
                //else if ((logindata.UserName == "v01" && logindata.Password == "jntu@v01123") || (logindata.UserName == "v02" && logindata.Password == "jntu@v02123") || (logindata.UserName == "v03" && logindata.Password == "jntu@v03123") || (logindata.UserName == "v04" && logindata.Password == "jntu@v04123") || (logindata.UserName == "v05" && logindata.Password == "jntu@v05123") || (logindata.UserName == "v06" && logindata.Password == "jntu@v06123") || (logindata.UserName == "v07" && logindata.Password == "jntu@v07123") || (logindata.UserName == "v08" && logindata.Password == "jntu@v08123") || (logindata.UserName == "v09" && logindata.Password == "jntu@v09123") || (logindata.UserName == "v10" && logindata.Password == "jntu@v10123"))
                //{
                //    FormsAuthentication.SetAuthCookie(logindata.UserName, false);
                //   // return RedirectToAction("CollegewiseDetails");
                //    return RedirectToAction("UserCreationView");
                //}
                //else
                //{
                //    bool isvaild = IsvalidorNot(logindata);
                //    if (isvaild == true)
                //    {

                //        FormsAuthentication.SetAuthCookie(logindata.UserName, false);
                //       // return RedirectToAction("Index", "User");
                //        return RedirectToAction("showSCMmenuforUser", "User");
                //    }
                //}
            }
            return View();
        }

        private bool IsvalidorNot(UserLogin logindata)
        {
            int userId =
                db.jntuh_registration.Where(
                    e =>
                        e.Email.Trim() == logindata.UserName.Trim() && e.Password.Trim() == logindata.Password.Trim() &&
                        e.IsActive == true)
                    .Select(e => e.Id)
                    .FirstOrDefault();
            if (userId != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetUserRole(UserLogin logindata)
        {
            string roleName = string.Empty;
            int userId =
                db.jntuh_registration.Where(
                    e => e.Email.Trim() == logindata.UserName.Trim() && e.Password.Trim() == logindata.Password.Trim())
                    .Select(e => e.Id)
                    .FirstOrDefault();
            if (userId != 0)
            {
                roleName =
                    db.my_aspnet_roles.Join(db.jntuh_registration, R => R.id, U => U.RoleType,
                        (R, U) => new {R = R, U = U})
                        .Where(e => e.U.Id == userId && (e.R.id == 1 || e.R.id == 7 || e.R.id == 8))
                        .Select(e => e.R.name)
                        .FirstOrDefault();
            }
            return roleName;

        }




        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult UserCreationView()
        {
            if (User.Identity.Name == "admin")
            {

                return View();
            }
            else if (User.Identity.IsAuthenticated)
                //if ((User.Identity.Name == "v01") || (User.Identity.Name == "v02") || (User.Identity.Name == "v03") || (User.Identity.Name == "v04") || (User.Identity.Name == "v05") || (User.Identity.Name == "v06") || (User.Identity.Name == "v07") || (User.Identity.Name == "v08") || (User.Identity.Name == "v09") || (User.Identity.Name == "v10"))
            {
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }

        }

        //[HttpGet]
        //public ActionResult UserRegistration()
        //{
        //    if (User.Identity.Name == "admin")
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Login");
        //    }

        //}

        //[HttpPost]
        //public ActionResult UserRegistration(UserCreation reg)
        //{
        //    if (ModelState.IsValid && User.Identity.Name == "admin")
        //    {
        //        jntuh_registration userreg = new jntuh_registration();
        //        userreg.UserName = reg.UserName;
        //        userreg.Password = reg.Password;
        //        userreg.IsActive = true;
        //        userreg.Email = reg.Email;
        //        userreg.Createdby = 1;
        //        userreg.CreatedOn = DateTime.Now;
        //        db.jntuh_registration.Add(userreg);
        //        db.SaveChanges();
        //        TempData["Success"] = "User created Successfully.....";
        //        return RedirectToAction("UserCreationView");
        //    }

        //    return View();
        //}

        [HttpGet]
        [AuthorizedUserAccess("Admin", "FacultyVerification")]
        public ActionResult CollegewiseDetails(int? collegeId)
        {
            if (User.Identity.IsAuthenticated)
            {

                var collegeIds =db.jntuh_auditors_dataentry.Where(e => e.IsActive == true && e.SpecializationId != 0 && e.DepartmentId != 0 && e.DegreeId != 0).Select(e => e.CollegeId).Distinct().ToArray();
                ViewBag.Colleges =db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id)).Select(e => new {collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName}).OrderBy(e => e.collegeName).ToList();

                if (collegeId != null)
                {
                    var jntuh_auditors_dataentry =db.jntuh_auditors_dataentry.Where(e => e.IsActive == true).Select(e => e).ToList();
                    var jntuh_scmproceedingrequest_addfaculty =db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking().ToList();


                    var Scmalldata = (from NomineeDTERY in db.jntuh_auditors_dataentry
                        join REG in db.jntuh_registered_faculty on NomineeDTERY.RegistrationNo equals REG.RegistrationNumber
                        join AUDI in db.jntuh_ffc_auditor on NomineeDTERY.AuditorId equals AUDI.id
                        join SPEC in db.jntuh_specialization on NomineeDTERY.SpecializationId equals SPEC.id
                        join DEPT in db.jntuh_department on NomineeDTERY.DepartmentId equals DEPT.id
                        join DESG in db.jntuh_degree on NomineeDTERY.DegreeId equals DESG.id
                        where NomineeDTERY.CollegeId == collegeId && NomineeDTERY.SpecializationId != 0 &&
                              NomineeDTERY.DepartmentId != 0 && NomineeDTERY.DegreeId != 0
                        select new ScmUploadedData()
                        {
                            RegistrationNumber = REG.RegistrationNumber,
                            SCMId = NomineeDTERY.ScmProceedingId,
                            Specialization = DESG.degree + "-" + SPEC.specializationName,
                            SpecializationId = NomineeDTERY.SpecializationId,
                            Department = DEPT.departmentName,
                            DepartmentId = NomineeDTERY.DepartmentId,
                            Degree = DESG.degree,
                            DegreeId = NomineeDTERY.DegreeId,
                            FirstName = REG.FirstName + " " + REG.LastName,
                            CollegeId = NomineeDTERY.CollegeId,
                            AuditorId = NomineeDTERY.AuditorId,
                            AuditorName = AUDI.auditorName,
                            Checked = NomineeDTERY.IsSelected != false ? true : false,
                            SCMhardcopyview = NomineeDTERY.SCMhardcopy,
                            SCMDate = NomineeDTERY.CreatedOn,
                            facultyId = REG.id
                        }).OrderBy(e => e.RegistrationNumber).ToList();





                    List<ScmUploadedData> collegewiseSCmdata = new List<ScmUploadedData>();
                    int ScmId = 0;
                    string Regno = string.Empty;
                    foreach (var item in Scmalldata)
                    {
                        var firstOrDefault = new jntuh_scmproceedingrequest_addfaculty();

                        if (ScmId == item.SCMId && Regno == item.RegistrationNumber)
                        {
                            firstOrDefault =
                                jntuh_scmproceedingrequest_addfaculty.Where(
                                    e =>
                                        e.ScmProceedingId == item.SCMId &&
                                        e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim() &&
                                        e.FacultyType != 1).Select(e => e).OrderByDescending(e => e.Id).FirstOrDefault();
                            ScmId = 0;
                            Regno = string.Empty;
                        }
                        else
                        {
                            firstOrDefault =
                                jntuh_scmproceedingrequest_addfaculty.Where(
                                    e =>
                                        e.ScmProceedingId == item.SCMId &&
                                        e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim() &&
                                        e.FacultyType != 1).Select(e => e).FirstOrDefault();
                            ScmId = item.SCMId;
                            Regno = item.RegistrationNumber.Trim();
                        }


                        if (firstOrDefault != null)
                        {
                            if (firstOrDefault.FacultyType != null)
                                item.DesignationId = Convert.ToInt16(firstOrDefault.FacultyType);
                            item.ScmfacultyaddedId = firstOrDefault.Id;
                            item.Approved = firstOrDefault.IsApproved;
                            item.Remarks = firstOrDefault.DeactiviationReason;
                        }

                        collegewiseSCmdata.Add(item);
                    }


                    return View(collegewiseSCmdata.Select(e => e).ToList());
                }
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }



        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult SCMPrincipalview()
        {
            if (User.Identity.IsAuthenticated)
                // if (User.Identity.Name == "admin")
            {
                var nomineeAssignedScmIds = (from SCM in db.jntuh_scmproceedingsrequests
                    join AUDA in db.jntuh_auditor_assigned on SCM.ID equals AUDA.ScmId
                    where SCM.SpecializationId == 0 && SCM.DEpartmentId == 0 && SCM.DegreeId == 0
                    select SCM.ID).Distinct().ToArray();

                List<ScmPrincipaldetails> scmprincipaldata = (from SCM in db.jntuh_scmproceedingsrequests
                    join SCMADD in db.jntuh_scmproceedingrequest_addfaculty on SCM.ID equals SCMADD.ScmProceedingId
                    join Reg in db.jntuh_registered_faculty on SCMADD.RegistrationNumber equals Reg.RegistrationNumber
                    join CLG in db.jntuh_college on SCM.CollegeId equals CLG.id
                    //  join PRECLG in db.jntuh_college on SCMADD.PreviousCollegeId equals PRECLG.id
                    where
                        SCM.SpecializationId == 0 && SCM.DEpartmentId == 0 && SCM.DegreeId == 0 &&
                        SCMADD.FacultyType == 0 && !nomineeAssignedScmIds.Contains(SCM.ID) &&
                        SCM.RequestSubmittedDate != null
                    select new ScmPrincipaldetails
                    {
                        ScmId = SCM.ID,
                        RegistrationNo = Reg.RegistrationNumber,
                        CollegeId = SCM.CollegeId,
                        ColleegCode = CLG.collegeCode,
                        CollegeName = CLG.collegeName,
                        FirstName = Reg.FirstName + " " + Reg.LastName,
                        Scmdocument = SCM.SCMNotification,
                        Scmdate = (DateTime) SCM.RequestSubmittedDate,
                        FacultyId = Reg.id,
                        PreviousWorkingCollegeName = SCMADD.PreviousCollegeId,
                        Checked = false
                    }).ToList();
                return View(scmprincipaldata);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }


        [HttpPost]
        public ActionResult SCMPrincipalview(List<ScmPrincipaldetails> princialscmdata)
        {
            if (princialscmdata.Count() != 0)
            {

                var checkedscmlistdata = princialscmdata.Where(e => e.Checked == true).Select(e => e).ToList();
                if (checkedscmlistdata.Count() != 0)
                {
                    ////////////////////Word Code
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition",
                        "attachment; filename=Principal SCM Request Data Report.doc");
                    Response.ContentType = "application/vnd.ms-word ";
                    Response.Charset = string.Empty;
                    StringBuilder str = new StringBuilder();
                    str.Append(GetPrincipalSCMRequestData(checkedscmlistdata));
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 60, 50, 60, 60);

                    pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    pdfDoc.SetMargins(60, 50, 60, 60);

                    string path = Server.MapPath("~/Content/PDFReports/SCMRequestToAll/Principal/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = path + "Principal SCM Requests Data" + "-" + DateTime.Now.ToString("yyyMMddHHmmss") + ".pdf";
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.Create));

                    pdfDoc.Open();

                    List<IElement> parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(str.ToString()), null);

                    foreach (var htmlElement in parsedHtmlElements)
                    {
                        pdfDoc.Add((IElement) htmlElement);
                    }

                    pdfDoc.Close();

                    Response.Output.Write(str.ToString());
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    TempData["Error"] = "Please Select Any One Checkbox For The Print";
                    return RedirectToAction("SCMPrincipalview");
                }
            }
            return RedirectToAction("SCMPrincipalview");
        }


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult SCMPrincipalviewPrint()
        {
            if (User.Identity.IsAuthenticated)
                //if (User.Identity.Name == "admin")
            {
                var nomineeAssignedScmIds = (from SCM in db.jntuh_scmproceedingsrequests
                    join AUDA in db.jntuh_auditor_assigned on SCM.ID equals AUDA.ScmId
                    where SCM.SpecializationId == 0 && SCM.DEpartmentId == 0 && SCM.DegreeId == 0
                    select SCM.ID).Distinct().ToArray();

                List<ScmPrincipaldetails> scmprincipaldata = (from SCM in db.jntuh_scmproceedingsrequests
                    join SCMADD in db.jntuh_scmproceedingrequest_addfaculty on SCM.ID equals SCMADD.ScmProceedingId
                    join Reg in db.jntuh_registered_faculty on SCMADD.RegistrationNumber equals Reg.RegistrationNumber
                    join CLG in db.jntuh_college on SCM.CollegeId equals CLG.id
                    //  join PRECLG in db.jntuh_college on SCMADD.PreviousCollegeId equals PRECLG.id
                    where
                        SCM.SpecializationId == 0 && SCM.DEpartmentId == 0 && SCM.DegreeId == 0 &&
                        SCMADD.FacultyType == 0 && !nomineeAssignedScmIds.Contains(SCM.ID) &&
                        SCM.RequestSubmittedDate != null
                    select new ScmPrincipaldetails
                    {
                        ScmId = SCM.ID,
                        RegistrationNo = Reg.RegistrationNumber,
                        CollegeId = SCM.CollegeId,
                        ColleegCode = CLG.collegeCode,
                        CollegeName = CLG.collegeName,
                        FirstName = Reg.FirstName + " " + Reg.LastName,
                        Scmdocument = SCM.SCMNotification,
                        Scmdate = (DateTime) SCM.RequestSubmittedDate,
                        FacultyId = Reg.id,
                        PreviousWorkingCollegeName = SCMADD.PreviousCollegeId,
                        Checked = false
                    }).ToList();
                return View(scmprincipaldata);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }


        public string GetPrincipalSCMRequestData(List<ScmPrincipaldetails> princialscmdata)
        {
            var collegeIds = princialscmdata.Where(e => e.Checked == true).Select(e => e.CollegeId).Distinct().ToArray();



            var scmaddfacultyData = db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking().ToList();

            var NomineeassignSCMIds = db.jntuh_auditor_assigned.Select(e => e.ScmId).Distinct().ToArray();



            var ScmAddFacultySCMIds = scmaddfacultyData.Select(e => e.ScmProceedingId).Distinct().ToArray();

            var SCMData = (from SCMREQ in db.jntuh_scmproceedingsrequests
                join CLG in db.jntuh_college on SCMREQ.CollegeId equals CLG.id
                join SCMREQADD in db.jntuh_scmproceedingrequest_addfaculty on SCMREQ.ID equals SCMREQADD.ScmProceedingId
                join RegF in db.jntuh_registered_faculty on SCMREQADD.RegistrationNumber.Trim() equals
                    RegF.RegistrationNumber.Trim()
                where
                    collegeIds.Contains(SCMREQ.CollegeId) && ScmAddFacultySCMIds.Contains(SCMREQ.ID) &&
                    !NomineeassignSCMIds.Contains(SCMREQ.ID) && SCMREQ.RequestSubmittedDate != null &&
                    SCMREQ.SpecializationId == 0
                select new
                {
                    CollegeId = SCMREQ.CollegeId,
                    CollegeCode = CLG.collegeCode,
                    CollegeName = CLG.collegeName,
                    // Department = DEG.degree + "-" + SPEC.specializationName,
                    SCMId = SCMREQ.ID,
                    //  DepartmentId = SCMREQ.DEpartmentId,
                    //  DepartmentName = DPET.departmentName,
                    //  DegreeName = DEG.degree,
                    Reqprof = SCMREQ.RequiredProfessor,
                    Reqassociprof = SCMREQ.RequiredAssociateProfessor,
                    ReqAssistprof = SCMREQ.RequiredAssistantProfessor,
                    Availprof = SCMREQ.ProfessorsCount,
                    Availassociprof = SCMREQ.AssociateProfessorsCount,
                    AvailAssistprof = SCMREQ.AssistantProfessorsCount,
                    RegNo = SCMREQADD.RegistrationNumber,
                    FirstName = RegF.FirstName,
                    LastName = RegF.LastName,
                    MiddleName = RegF.MiddleName
                }).ToList();


            string contextstring = string.Empty;
            contextstring += "<table border='1' style='width:100%'>";
            contextstring += "<tr>";
            contextstring += "<td width='5%'>S.No</td>";
            contextstring += "<td width='20%'>College Name</td>";
            contextstring += "<td width='25%'>Registration Numbers</td>";
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
            foreach (var item in SCMData.Select(e => e.CollegeId).Distinct().ToArray())
            {
                var CollegeAddress = jntuh_address.Where(e => e.CollegeId == item).Select(e => e).FirstOrDefault();
                var SCMDatabycollegeId =
                    SCMData.Where(e => e.CollegeId == item).Select(e => e).OrderBy(e => e.CollegeCode).ToList();

                var SCMDataByDepartmentId =
                    SCMData.Where(e => e.CollegeId == item)
                        .Select(e => e)
                        .OrderBy(e => e.CollegeCode)
                        .Distinct()
                        .ToList();

                //    var SCMDataByDepartmentId1 = SCMData.Where(e => e.CollegeId == item).Select(e => e).Select(e => e.DepartmentId).Distinct().ToArray();

                contextstring += "<tr>";
                contextstring += "<td width='5%' style='vertical-align:top'>" + count + "</td>";
                contextstring += "<td width='20%' style='vertical-align:top'>" + SCMDatabycollegeId[0].CollegeName +
                                 "<br/> " + CollegeAddress.Address + "<br/>" + CollegeAddress.Town + "(T), " +
                                 CollegeAddress.Mandal + "(M), <br/>" + CollegeAddress.District + "(D), PIN - " +
                                 CollegeAddress.PINcode + " <b>(" + SCMDatabycollegeId[0].CollegeCode + ")</b></td>";
                contextstring += "<td width='25%'  style='vertical-align:top'><table border='0' width='100%'>";

                //sample 
                int deeploop = 1;

                foreach (var CollegeId in SCMDataByDepartmentId)
                {
                    //  var totalcount =SCMDatabycollegeId.Where(e =>e.DepartmentName == DeptId.DepartmentName &&(e.DegreeName == "M.Tech" || e.DegreeName == "B.Tech")).Distinct().Count();
                    //var DeptName =jntuh_departments.Where(e => e.id == DeptId).Select(e => e.departmentName).FirstOrDefault();
                    //var DeptIds =jntuh_departments.Where(e => e.departmentName == DeptName).Select(e => e.id).Distinct().ToArray();
                    int indexof = SCMDataByDepartmentId.IndexOf(CollegeId);
                    if (indexof > 0 &&
                        SCMDataByDepartmentId[indexof].CollegeCode != SCMDataByDepartmentId[indexof - 1].CollegeCode)
                    {
                        deeploop = 1;
                    }
                    if (deeploop == 1)
                    {

                        contextstring += "<tr><td width='33%'>";
                        foreach (
                            var data in
                                SCMDatabycollegeId.Where(e => e.CollegeCode == CollegeId.CollegeCode)
                                    .Select(e => e)
                                    .ToList()) //e.DepartmentId==DeptId.DepartmentId
                        {

                            //var professor = scmaddfacultyData.Where(e => e.ScmProceedingId == data.SCMId && e.FacultyType == 1).Select(e => e.ScmProceedingId).Count();
                            //var Associateprofessor = scmaddfacultyData.Where(e => e.ScmProceedingId == data.SCMId && e.FacultyType == 2).Select(e => e.ScmProceedingId).Count();
                            //var Assistantprofessor = scmaddfacultyData.Where(e => e.ScmProceedingId == data.SCMId && e.FacultyType == 3).Select(e => e.ScmProceedingId).Count();

                            //var Reqprofessor = data.Reqprof;
                            //var ReqAssociateprofessor = data.Reqassociprof;
                            //var ReqAssistantprofessor = data.ReqAssistprof;

                            //var Availprofessor = data.Availprof;
                            //var AvailAssociateprofessor = data.Availassociprof;
                            //var AvailAssistantprofessor = data.AvailAssistprof;
                            contextstring += data.FirstName + " " + data.MiddleName + " " + data.LastName + "(" +
                                             data.RegNo + ")<br/><br/>";
                            //+ " (" + professor + "/" + Reqprofessor + "/" + Availprofessor + "+" + Associateprofessor + "/" + ReqAssociateprofessor + "/" + AvailAssociateprofessor + "+" + Assistantprofessor + "/" + ReqAssistantprofessor + "/" + AvailAssistantprofessor + ")<br/>";

                        }
                        contextstring += "</tr>";
                        //   contextstring += "</td><td width='67%'></td></tr>";
                        deeploop++;
                    }
                }
                contextstring += "</table></td>";
                contextstring += "<td width='50%'>" + " " + "</td>";
                contextstring += "</tr>";
                count++;
            }
            contextstring += "</table>";
            return contextstring;
        }

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult NomineeAssignedSCMPrincipalRequests()
        {
            if (User.Identity.IsAuthenticated)
                //if (User.Identity.Name == "admin")
            {
                var nomineeAssignedScmIds = (from SCM in db.jntuh_scmproceedingsrequests
                    join AUDA in db.jntuh_auditor_assigned on SCM.ID equals AUDA.ScmId
                    where SCM.SpecializationId == 0 && SCM.DEpartmentId == 0 && SCM.DegreeId == 0
                    select SCM.ID).Distinct().ToArray();
                //NomineeAssignedScmPrincipalRequest

                var scmprincipaldata = (from SCM in db.jntuh_scmproceedingsrequests
                    join SCMADD in db.jntuh_scmproceedingrequest_addfaculty on SCM.ID equals SCMADD.ScmProceedingId
                    join Reg in db.jntuh_registered_faculty on SCMADD.RegistrationNumber equals Reg.RegistrationNumber
                    join CLG in db.jntuh_college on SCM.CollegeId equals CLG.id
                    join AUDA in db.jntuh_auditor_assigned on SCM.ID equals AUDA.ScmId
                    join Nominee in db.jntuh_ffc_auditor on AUDA.AuditorId equals Nominee.id
                    //  join PRECLG in db.jntuh_college on SCMADD.PreviousCollegeId equals PRECLG.id
                    where
                        SCM.SpecializationId == 0 && SCM.DEpartmentId == 0 && SCM.DegreeId == 0 &&
                        SCMADD.FacultyType == 0 && nomineeAssignedScmIds.Contains(SCM.ID)
                    select new NomineeAssignedScmPrincipalRequest
                    {
                        ScmId = SCM.ID,
                        RegistrationNo = Reg.RegistrationNumber,
                        CollegeId = SCM.CollegeId,
                        ColleegCode = CLG.collegeCode,
                        CollegeName = CLG.collegeName,
                        FirstName = Reg.FirstName + " " + Reg.LastName,
                        Scmdocument = SCM.SCMNotification,
                        Scmdate = (DateTime) SCM.Notificationdate,
                        NomineeName = Nominee.auditorName,
                        NomineeAssignedDate = (DateTime) AUDA.CreatedOn
                        //  FacultyId = Reg.id,
                        // PreviousWorkingCollegeName = SCMADD.PreviousCollegeId
                    }).Distinct().OrderByDescending(e => e.NomineeAssignedDate).ToList();
                return View(scmprincipaldata);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult SCMPrincipalVerification(int? collegeId)
        {
            if (User.Identity.IsAuthenticated)
                //if (User.Identity.Name == "admin")
            {
                var jntuh_scmproceedingsrequests =
                    db.jntuh_scmproceedingsrequests.Where(
                        e => e.ISActive == true && e.SpecializationId == 0 && e.DEpartmentId == 0 && e.DegreeId == 0)
                        .Select(e => e)
                        .ToList();
                var collegeIds = jntuh_scmproceedingsrequests.Select(e => e.CollegeId).Distinct().ToArray();
                ViewBag.Colleges =
                    db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id))
                        .Select(e => new {collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName})
                        .OrderBy(e => e.collegeName)
                        .ToList();
                var jntuh_department = db.jntuh_department.ToList();
                var jntuh_specialization = db.jntuh_specialization.ToList();
                var jntuh_degree = db.jntuh_degree.ToList();
                List<FacultyRegistrationDetails> teachingFaculty = new List<FacultyRegistrationDetails>();
                if (collegeId != null)
                {
                    // List<jntuh_college_faculty_registered> jntuh_college_faculty_registered =db.jntuh_college_faculty_registered.Where(cf => cf.collegeId == collegeId).Select(cf => cf).ToList();
                    string[] strRegNoS = (from a in db.jntuh_scmproceedingsrequests
                        join b in db.jntuh_scmproceedingrequest_addfaculty on a.ID equals b.ScmProceedingId
                        where
                            a.CollegeId == collegeId && a.SpecializationId == 0 && a.DEpartmentId == 0 &&
                            a.DegreeId == 0
                        select b.RegistrationNumber).Distinct().ToArray();

                    int[] SCMProccedingIds =
                        jntuh_scmproceedingsrequests.Where(e => e.CollegeId == collegeId)
                            .Select(e => e.ID)
                            .Distinct()
                            .ToArray();

                    List<jntuh_registered_faculty> jntuh_registered_faculty = new List<jntuh_registered_faculty>();
                    jntuh_registered_faculty =
                        db.jntuh_registered_faculty.Where(
                            rf => strRegNoS.Contains(rf.RegistrationNumber) && rf.Notin116 != true)
                            .Select(rf => rf)
                            .ToList();

                    var jntuhScmproceedingrequestAddfaculty =
                        db.jntuh_scmproceedingrequest_addfaculty.Where(e => SCMProccedingIds.Contains(e.ScmProceedingId))
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
                        // faculty.Type = a.type;
                        //  faculty.CollegeId = collegeId;
                        faculty.RegistrationNumber = a.RegistrationNumber;
                        // faculty.UniqueID = a.UniqueID;
                        faculty.FirstName = a.FirstName;
                        faculty.MiddleName = a.MiddleName;
                        faculty.LastName = a.LastName;
                        // faculty.GenderId = a.GenderId;
                        // faculty.Email = a.Email;
                        //  faculty.facultyPhoto = a.Photo;
                        //  faculty.Mobile = a.Mobile;
                        // faculty.PANNumber = a.PANNumber;
                        //  faculty.AadhaarNumber = a.AadhaarNumber;
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
                        //   faculty.SamePANNumberCount = jntuh_registered_faculty.Where(f => f.PANNumber == a.PANNumber).ToList().Count;
                        // faculty.SameAadhaarNumberCount = jntuh_registered_faculty.Where(f => f.AadhaarNumber == a.AadhaarNumber).ToList().Count;
                        //  faculty.SpecializationIdentfiedFor = Specializationid > 0 ? Specializations.Where(S => S.id == Specializationid).Select(S => S.specializationName).FirstOrDefault() : "";
                        //  faculty.IdentfiedFor = jntuh_college_faculty_registered.Where(f => f.RegistrationNumber.Trim() == a.RegistrationNumber.Trim()).Select(f => f.IdentifiedFor).FirstOrDefault();
                        //  faculty.jntuh_registered_faculty_education = a.jntuh_registered_faculty_education;
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
        public ActionResult ApprovedFaculty(int facultyAddId, int collegeId)
        {
            //var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
            if (facultyAddId != 0 && collegeId != 0)
            {
                var data =
                    db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == facultyAddId)
                        .Select(e => e)
                        .FirstOrDefault();
                if (data != null)
                {
                    data.IsApproved = true;
                    data.Updatedby = 1;
                    data.UpdatedOn = DateTime.Now;
                    data.Isactive = true;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();

                    var registraredFacultydata =
                        db.jntuh_registered_faculty.Where(
                            e => e.RegistrationNumber.Trim() == data.RegistrationNumber.Trim())
                            .Select(e => e)
                            .FirstOrDefault();
                    if (registraredFacultydata != null)
                    {
                        registraredFacultydata.NoSCM = false;
                        registraredFacultydata.updatedBy = 1;
                        registraredFacultydata.updatedOn = DateTime.Now;
                        db.Entry(registraredFacultydata).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["Error"] = "Faculty Approved Failed";
                        // return RedirectToAction("ScmFacultyVerfication", "CollegeSCMProceedingsRequest",
                        return RedirectToAction("SCMPrincipalSelectView", "Admin",
                            new {collegeId = collegeId});
                    }



                }
                TempData["Success"] = "Faculty Approved Successfully";
                return RedirectToAction("SCMPrincipalSelectView", "Admin", new {collegeId = collegeId});
            }
            return RedirectToAction("SCMPrincipalSelectView", "Admin");
        }


        [HttpGet]
        public ActionResult NotApproveFaculty(int facultyAddId, int collegeId)
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


        [HttpPost]
        public ActionResult NotApproveFaculty(Facultynotapproved facultynotapproved)
        {
            TempData["Error"] = null;
            //  int userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
            if (ModelState.IsValid)
            {
                if (facultynotapproved != null)
                {
                    UAAASSCM.Models.jntuh_scmproceedingrequest_addfaculty addfaculty =
                        db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.Id == facultynotapproved.FacultyAddId)
                            .Select(e => e)
                            .FirstOrDefault();
                    if (addfaculty != null)
                    {
                        addfaculty.DeactiviationReason = facultynotapproved.DeactivationReason;
                        addfaculty.Updatedby = 1;
                        addfaculty.UpdatedOn = DateTime.Now;
                        addfaculty.Isactive = true;
                        addfaculty.IsApproved = false;
                        db.Entry(addfaculty).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["Success"] = "Faculty Not Approved Successfully";
                    }
                    return RedirectToAction("SCMPrincipalSelectView", "Admin",
                        new {collegeId = facultynotapproved.CollegeId});
                }
            }
            return RedirectToAction("SCMPrincipalSelectView", "Admin");
        }

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult SCMPrincipalSelectView(int? collegeId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var collegeIds = db.jntuh_auditors_dataentry.Where(e => e.IsActive == true && e.SpecializationId == 0 && e.DepartmentId == 0 && e.DegreeId == 0).Select(e => e.CollegeId).Distinct().ToArray();
                ViewBag.Colleges = db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id)).Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName }).OrderBy(e => e.collegeName).ToList();
                if (collegeId != null)
                {
                    var jntuh_scmproceedingrequest_addfaculty = db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking().ToList();
                    //  List<ScmUploadedData> collegewiseSCmdata = new List<ScmUploadedData>();
                    //var jntuh_auditors_dataentry = db.jntuh_auditors_dataentry.Where(e => e.IsActive == true).Select(e => e).ToList();

                    //var Scmalldata = (from SCM in db.jntuh_scmproceedingsrequests
                    //                  join SCMADDFLY in db.jntuh_scmproceedingrequest_addfaculty on SCM.ID equals SCMADDFLY.ScmProceedingId
                    //                  join REG in db.jntuh_registered_faculty on SCMADDFLY.RegistrationNumber equals REG.RegistrationNumber
                    //                  join SCMASSIN in db.jntuh_auditor_assigned on SCM.ID equals SCMASSIN.ScmId
                    //                  join AUDI in db.jntuh_ffc_auditor on SCMASSIN.AuditorId equals AUDI.id
                    //                  // join SPEC in db.jntuh_specialization on SCM.SpecializationId equals SPEC.id
                    //                  //  join DEPT in db.jntuh_department on SCM.DEpartmentId equals DEPT.id
                    //                  //  join DESG in db.jntuh_degree on SCM.DegreeId equals DESG.id
                    //                  //  join DESGN in db.jntuh_designation on SCMADDFLY.FacultyType equals DESGN.id
                    //                  where SCM.CollegeId == collegeId && SCM.SpecializationId == 0 && SCM.DEpartmentId == 0 && SCM.DegreeId == 0 && SCMADDFLY.IsApproved == true
                    //                  select new ScmUploadedData()
                    //                  {
                    //                      RegistrationNumber = REG.RegistrationNumber,
                    //                      SCMId = SCM.ID,
                    //                      // Specialization = DESG.degree + "-" + SPEC.specializationName,
                    //                      SpecializationId = SCM.SpecializationId,
                    //                      //  Department = DEPT.departmentName,
                    //                      DepartmentId = SCM.DEpartmentId,
                    //                      //   Degree = DESG.degree,
                    //                      DegreeId = SCM.DegreeId,
                    //                      FirstName = REG.FirstName + " " + REG.LastName,
                    //                      CollegeId = SCM.CollegeId,
                    //                      AuditorId = SCMASSIN.AuditorId,
                    //                      AuditorName = AUDI.auditorName,
                    //                      //  DesignationId = (int)SCMADDFLY.FacultyType,
                    //                      //   DesignationName = DESGN.designation

                    //                  }).Distinct().ToList();


                    //List<ScmUploadedData> collegewiseSCmdata = new List<ScmUploadedData>();
                    //foreach (var item in Scmalldata)
                    //{
                    //    item.Checked = jntuh_auditors_dataentry.Where(e => e.CollegeId == item.CollegeId && e.SpecializationId == 0 && e.RegistrationNo == item.RegistrationNumber && e.DepartmentId == 0 && e.DegreeId == 0).Select(e => e.IsSelected).FirstOrDefault() != null ? true : false;
                    //    item.SCMhardcopyview = jntuh_auditors_dataentry.Where(e => e.CollegeId == item.CollegeId && e.SpecializationId == 0 && e.RegistrationNo == item.RegistrationNumber && e.DepartmentId == 0 && e.DegreeId == 0).Select(e => e.SCMhardcopy).FirstOrDefault();
                    //    item.SCMDate = jntuh_auditors_dataentry.Where(e => e.CollegeId == item.CollegeId && e.SpecializationId == 0 && e.RegistrationNo == item.RegistrationNumber && e.DepartmentId == 0 && e.DegreeId == 0).Select(e => e.CreatedOn).FirstOrDefault();
                    //    //var firstOrDefault = jntuh_auditors_dataentry.Where(e =>e.CollegeId == item.CollegeId && e.SpecializationId == item.SpecializationId &&e.RegistrationNo == item.RegistrationNumber).Select(e => e.SCMhardcopy).FirstOrDefault();
                    //    //if (firstOrDefault != null)
                    //    //    item.SCMDate =UAAAS.Models.Utilities.MMDDYY2DDMMYY(firstOrDefault.ToString());
                    //    collegewiseSCmdata.Add(item);
                    //}

                    var Scmalldata = (from AUDDTRY in db.jntuh_auditors_dataentry
                                      //   SCM in db.jntuh_scmproceedingsrequests
                                      // join SCMADDFLY in db.jntuh_scmproceedingrequest_addfaculty on SCM.ID equals SCMADDFLY.ScmProceedingId
                                      join REG in db.jntuh_registered_faculty on AUDDTRY.RegistrationNo equals REG.RegistrationNumber
                                      //  join SCMASSIN in db.jntuh_auditor_assigned on AUDDTRY.AuditorId equals SCMASSIN.AuditorId
                                      join AUDI in db.jntuh_ffc_auditor on AUDDTRY.AuditorId equals AUDI.id
                                      where AUDDTRY.CollegeId == collegeId && AUDDTRY.SpecializationId == 0 && AUDDTRY.DepartmentId == 0 && AUDDTRY.DegreeId == 0
                                      select new ScmUploadedData()
                                      {
                                          RegistrationNumber = REG.RegistrationNumber,
                                          SCMId = AUDDTRY.ScmProceedingId,
                                          // Specialization = DESG.degree + "-" + SPEC.specializationName,
                                          SpecializationId = AUDDTRY.SpecializationId,
                                          //  Department = DEPT.departmentName,
                                          DepartmentId = AUDDTRY.DepartmentId,
                                          //   Degree = DESG.degree,
                                          DegreeId = AUDDTRY.DegreeId,
                                          FirstName = REG.FirstName + " " + REG.LastName,
                                          CollegeId = AUDDTRY.CollegeId,
                                          AuditorId = AUDDTRY.AuditorId,
                                          AuditorName = AUDI.auditorName,
                                          Checked = AUDDTRY.IsSelected != null ? true : false,
                                          SCMhardcopyview = AUDDTRY.SCMhardcopy,
                                          SCMDate = AUDDTRY.CreatedOn,
                                          facultyId = REG.id
                                          //  DesignationId = (int)SCMADDFLY.FacultyType,
                                          //   DesignationName = DESGN.designation
                                      }).Distinct().ToList();


                    List<ScmUploadedData> collegewiseSCmdata = new List<ScmUploadedData>();
                    foreach (var item in Scmalldata)
                    {

                        var firstOrDefault = jntuh_scmproceedingrequest_addfaculty.Where(e => e.ScmProceedingId == item.SCMId && e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim()).Select(e => e).FirstOrDefault();
                        if (firstOrDefault != null)
                        {
                            //if (firstOrDefault.FacultyType != null)
                            //    item.DesignationId = Convert.ToInt16(firstOrDefault.FacultyType);
                            item.ScmfacultyaddedId = firstOrDefault.Id;
                            item.Approved = firstOrDefault.IsApproved;
                            item.Remarks = firstOrDefault.DeactiviationReason;
                        }

                        collegewiseSCmdata.Add(item);
                    }






                    return View(collegewiseSCmdata.Where(e => e.Checked == true).Select(e => e).ToList());
                }
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }
   

    [HttpGet]
        [AuthorizedUserAccess("Admin", "FacultyVerification", "Faculty")]
        public ActionResult ChangePassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [AuthorizedUserAccess("Admin", "FacultyVerification", "Faculty")]
        public ActionResult ChangePassword(ChangepasswordModel changepassword)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var nomineedata =db.jntuh_registration.Where(e => e.Email == User.Identity.Name).Select(e => e).FirstOrDefault();
                    if (nomineedata != null)
                    {
                        if (nomineedata.Password == changepassword.OldPassword)
                        {
                            nomineedata.Password = changepassword.NewPassword;
                            db.Entry(nomineedata).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["Success"] = "Password Change Successfully.";
                        }
                        else
                        {
                            TempData["Error"] = "Current Password entered wrong.";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Details not Found.";
                    }
                    return RedirectToAction("ChangePassword");
                }
                else
                {
                    return RedirectToAction("ChangePassword");
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }


        [HttpGet]
        public ActionResult ApprovedFacultyBasedOnNominee(string SCMId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var UserEmailId = User.Identity.Name;
                var NomineeId =db.jntuh_ffc_auditor.Where(e => e.auditorEmail1 == UserEmailId).Select(e => e.id).Distinct().ToArray();
                List<int> ScmIds =new List<int>();
                if (NomineeId.Count() != 0)
                {
                   // int ScmIds;
                    if (!string.IsNullOrEmpty(SCMId))
                    {
                        List<int> SCMIds = new List<int>();
                        var SCMDATA = SCMId.Split(',');
                        foreach (var SCMstr in SCMDATA)
                        {
                            if (!string.IsNullOrEmpty(SCMstr))
                            {
                                SCMIds.Add(Convert.ToInt16(SCMstr));
                            }
                        }

                       // ScmIds = (int)SCMId;
                        ScmIds.AddRange(SCMIds);
                    }
                    else
                    {
                       //ScmIds.AddRange(db.jntuh_auditors_dataentry.Where(e => e.IsActive == true && NomineeId.Contains(e.AuditorId)).OrderByDescending(e => e.CreatedOn).Select(e => e.ScmProceedingId).Distinct().ToArray());
                      //  if (ScmIds.Count==0)
                       // {
                            ScmIds.AddRange(db.jntuh_auditor_assigned.Where(e=>e.IsActive==true && NomineeId.Contains(e.AuditorId)).Select(e=>e.ScmId).ToArray());
                       // }
                    }




                    if (ScmIds.Count() != 0)
                    {
                        var data1 = (from DataEntry in db.jntuh_auditors_dataentry
                                     join REg in db.jntuh_registered_faculty on DataEntry.RegistrationNo equals REg.RegistrationNumber
                                     join spec in db.jntuh_specialization on DataEntry.SpecializationId equals spec.id
                                     join Deg in db.jntuh_degree on DataEntry.DegreeId equals Deg.id
                                    join Dept in db.jntuh_department on DataEntry.DepartmentId equals Dept.id
                                     join AUDDATA in db.jntuh_ffc_auditor on DataEntry.AuditorId equals AUDDATA.id
                                     join CLG in db.jntuh_college on DataEntry.CollegeId equals CLG.id
                                     where ScmIds.Contains(DataEntry.ScmProceedingId)
                                     //DataEntry.ScmProceedingId == ScmIds
                                     select new ScmUploadedData()
                                     {
                                         SCMId = DataEntry.ScmProceedingId,
                                       // SpecializationId = DataEntry.SpecializationId,
                                        Specialization = Deg.degree + "-" + spec.specializationName,
                                      // DepartmentId = DataEntry.DepartmentId,
                                      //  DegreeId = DataEntry.DegreeId,
                                        Degree = Deg.degree,
                                        CollegeName = CLG.collegeName,
                                         FirstName = REg.FirstName + "-" + REg.LastName,
                                         RegistrationNumber = REg.RegistrationNumber,
                                         CollegeId = DataEntry.CollegeId,
                                         AuditorId = DataEntry.AuditorId,
                                         AuditorName = AUDDATA.auditorName,
                                         Checked = (bool) DataEntry.IsSelected,
                                         SCMhardcopyview = DataEntry.SCMhardcopy
                                     }).ToList();

                        return View(data1);
                    }
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
            return null;
        }

        public ActionResult Links()
        {
            return View();
        }

        //Admin SCM Screens


        //    [HttpGet]
        //    public ActionResult CollegeScmProceedingsRequest(int? CollegeId)
        //    {
        //        ViewBag.Colleges =
        //            db1.jntuh_college.Where(e => e.isActive == true)
        //                .Select(e => new {collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName})
        //                .OrderBy(e => e.collegeName)
        //                .ToList();
        //        ScmProceedingsRequest scmProceedings = new ScmProceedingsRequest();
        //        scmProceedings.IsEdit = false;
        //        string clgCode;
        //        if (CollegeId != null)
        //        {

        //            //  var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
        //            // var userCollegeId =db.jntuh_college_users.Where(u => u.userID == userId).Select(u => u.collegeID).FirstOrDefault();
        //            var firstOrDefault = db1.jntuh_college.FirstOrDefault(a => a.id == CollegeId);
        //            var specs = new List<DistinctSpecializations>();
        //            var depts = new List<DistinctDepartments>();
        //            var degrees = db1.jntuh_degree.AsNoTracking().ToList();
        //            var specializations = db1.jntuh_specialization.AsNoTracking().ToList();
        //            var departments = db1.jntuh_department.AsNoTracking().ToList();
        //            //int[] collegespecs = new int[];
        //            List<int> collegespecs = new List<int>();
        //            collegespecs.AddRange(
        //                db1.jntuh_college_intake_existing.Where(i => i.collegeId == CollegeId)
        //                    .Select(i => i.specializationId)
        //                    .Distinct()
        //                    .ToArray());

        //            //int[] degreeIds=(from a in db.jntuh_specialization join b in db.jntuh_department on a.departmentId equals b.id
        //            //               join c in db.jntuh_degree on b.degreeId equals c.id where collegespecs.Contains(a.id) select c.id).Distinct().ToArray();
        //            //if (degreeIds.Contains(4))
        //            //{
        //            //   var humanitesSpeci = new[] {37,48,42,31};
        //            //   collegespecs.AddRange(humanitesSpeci);
        //            //}



        //            foreach (var s in collegespecs)
        //            {
        //                var specid = specializations.FirstOrDefault(i => i.id == s);

        //                if (specid != null)
        //                {
        //                    var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
        //                    var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));
        //                    if (degree != null)
        //                        specs.Add(new DistinctSpecializations
        //                        {
        //                            SpecializationId = specid.id,
        //                            SpecializationName = degree.degree + " - " + specid.specializationName,
        //                            DepartmentId = specid.departmentId
        //                        });
        //                }
        //            }


        //            //  if(specs.Contains())

        //            ViewBag.departments = specs.OrderBy(i => i.DepartmentId);

        //            var collegescmrequestslist =
        //                db.jntuh_scmproceedingsrequests.AsNoTracking().Where(i => i.CollegeId == CollegeId).ToList();

        //            var proceedingsRequests = new List<ScmProceedingsRequest>();
        //            scmProceedings.ScmProceedingsRequestslist = new List<ScmProceedingsRequest>();
        //            foreach (var s in collegescmrequestslist)
        //            {

        //                //string cretedDate = string.Empty;
        //                //if (!string.IsNullOrEmpty(s.CreatedOn.ToString(CultureInfo.InvariantCulture)))
        //                //{
        //                //    cretedDate = UAAAS.Models.Utilities.MMDDYY2DDMMYY(cretedDate);
        //                //}
        //                var specid = specializations.FirstOrDefault(i => i.id == s.SpecializationId);

        //                if (specid != null && firstOrDefault != null)
        //                {
        //                    var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
        //                    var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));
        //                    if (degree != null)
        //                        // if (s.RequiredProfessor != null)
        //                        proceedingsRequests.Add(new ScmProceedingsRequest
        //                        {
        //                            CollegeName = firstOrDefault.collegeCode + " - " + firstOrDefault.collegeName,
        //                            CollegeCode = firstOrDefault.collegeCode,
        //                            ProfessorVacancies = s.ProfessorsCount.ToString(),
        //                            AssociateProfessorVacancies = s.AssociateProfessorsCount.ToString(),
        //                            AssistantProfessorVacancies = s.AssistantProfessorsCount.ToString(),
        //                            SpecializationName = degree.degree + " - " + specid.specializationName,
        //                            SpecializationId = specid.id,
        //                            CollegeId = firstOrDefault.id,
        //                            DepartmentId = specid.departmentId,
        //                            ScmNotificationpath = s.SCMNotification,
        //                            Id = s.ID,
        //                            CreatedDate = s.CreatedOn,
        //                            RequiredProfessorVacancies = s.RequiredProfessor.ToString(),
        //                            RequiredAssistantProfessorVacancies = s.RequiredAssistantProfessor.ToString(),
        //                            RequiredAssociateProfessorVacancies = s.RequiredAssociateProfessor.ToString(),
        //                            Checked = false
        //                        });
        //                }
        //            }
        //            // ViewBag.collegescmrequestslist = proceedingsRequests;
        //            scmProceedings.ScmProceedingsRequestslist.AddRange(
        //                proceedingsRequests.OrderByDescending(e => e.CreatedDate).Select(e => e).ToList());
        //            ViewBag.collegescmrequestslist = scmProceedings.ScmProceedingsRequestslist;
        //            // scmProceedings.ScmProceedingsRequestslist.AddRange(proceedingsRequests);
        //            scmProceedings.IsEdit = true;
        //        }
        //        return View(scmProceedings);

        //    }

        //    [HttpPost]
        //    public ActionResult CollegeScmProceedingsRequest(ScmProceedingsRequest scmrequest)
        //    {
        //        // var userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);
        //        //  var userCollegeId = db.jntuh_college_users.Where(u => u.userID == userId).Select(u => u.collegeID).FirstOrDefault();


        //        if (ModelState.IsValid)
        //        {
        //            var fileName = string.Empty;
        //            var filepath = string.Empty;
        //            var collegescmrequests = new UAAASSCM.Models.jntuh_scmproceedingsrequests();
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
        //                        db1.jntuh_college.Where(c => c.id == scmrequest.CollegeId)
        //                            .Select(c => c.id)
        //                            .FirstOrDefault() + "_" + "ScmNotofication" + "_" +
        //                        DateTime.Now.ToString("yyyMMddHHmmss");
        //                    scmrequest.ScmNotificationSupportDoc.SaveAs(string.Format("{0}/{1}{2}",
        //                        Server.MapPath(scmnotificationpath), scmfileName, ext));
        //                    collegescmrequests.SCMNotification = scmfileName + ext;
        //                }
        //                //   IUserMailer mailer = new UserMailer();
        //                collegescmrequests.CollegeId = scmrequest.CollegeId;
        //                collegescmrequests.SpecializationId = scmrequest.SpecializationId;
        //                var specialization =
        //                    db1.jntuh_specialization.AsNoTracking().FirstOrDefault(i => i.id == scmrequest.SpecializationId);
        //                var department =
        //                    db1.jntuh_department.AsNoTracking().FirstOrDefault(i => i.id == specialization.departmentId);
        //                collegescmrequests.DEpartmentId = specialization != null ? specialization.departmentId : 0;
        //                collegescmrequests.DegreeId = department != null ? department.degreeId : 0;
        //                collegescmrequests.ProfessorsCount = Convert.ToInt16(scmrequest.ProfessorVacancies);
        //                collegescmrequests.AssociateProfessorsCount = Convert.ToInt16(scmrequest.AssociateProfessorVacancies);
        //                collegescmrequests.AssistantProfessorsCount = Convert.ToInt16(scmrequest.AssistantProfessorVacancies);
        //                collegescmrequests.RequiredProfessor = Convert.ToInt16(scmrequest.RequiredProfessorVacancies);
        //                collegescmrequests.RequiredAssistantProfessor =
        //                    Convert.ToInt16(scmrequest.RequiredAssistantProfessorVacancies);
        //                collegescmrequests.RequiredAssociateProfessor =
        //                    Convert.ToInt16(scmrequest.RequiredAssociateProfessorVacancies);
        //                if (scmrequest.NotificationDate != null)
        //                    collegescmrequests.Notificationdate =
        //                        UAAAS.Models.Utilities.DDMMYY2MMDDYY(scmrequest.NotificationDate);
        //                collegescmrequests.Remarks = scmrequest.Remarks;
        //                collegescmrequests.SCMNotification = "Admin";
        //                collegescmrequests.CreatedBy = 1;
        //                collegescmrequests.CreatedOn = DateTime.Now;
        //                collegescmrequests.ISActive = true;
        //                db.jntuh_scmproceedingsrequests.Add(collegescmrequests);
        //                try
        //                {
        //                    db.SaveChanges();

        //                    //var attachments = new List<Attachment>();
        //                    //if (scmrequest.ScmNotificationSupportDoc != null)
        //                    //{

        //                    //    fileName = Path.GetFileName(scmrequest.ScmNotificationSupportDoc.FileName);
        //                    //    filepath = Path.Combine(Server.MapPath("~/Content/Attachments/SCMAttachments"), fileName);
        //                    //    scmrequest.ScmNotificationSupportDoc.SaveAs(filepath);
        //                    //    attachments.Add(new Attachment(filepath));
        //                    //    mailer.SendAttachmentToAllColleges("nageswararao.d623@gmail.com", "", "",
        //                    //        "SCM PROCEEDINGS REQUEST", "Scm Requests", attachments).SendAsync();
        //                    //    TempData["Success"] = "Your request has been proccessed successfully..";
        //                    //}
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
        //                //}
        //                //else
        //                //{
        //                //    TempData["Error"] = "Please Fill All Mandatory Fields..";
        //                //}
        //            }


        //            return RedirectToAction("CollegeScmProceedingsRequest", new {CollegeId = scmrequest.CollegeId});
        //        }

        //}


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult getAllAuditors()
        {
            if (User.Identity.IsAuthenticated)
            //if (User.Identity.Name == "admin")
            {
                // getting all the records from jntuh_ffc_auditor..
                List<UserCreation> user = (from auditor in db.jntuh_ffc_auditor
                                           join dept in db.jntuh_department on auditor.auditorDepartmentID equals dept.id
                                           join desig in db.jntuh_designation on auditor.auditorDesignationID equals desig.id
                                           select new UserCreation
                                           {

                                               Id = auditor.id,
                                               UserName = auditor.auditorName,
                                               Departmentid = (int)auditor.auditorDepartmentID,
                                               Designationid = (int)auditor.auditorDesignationID,
                                               Email = auditor.auditorEmail1,
                                               PhoneNumber = auditor.auditorMobile1,
                                               DepartmentName = dept.departmentName,
                                               DesignationName = desig.designation,
                                               University = auditor.auditorPlace





                                           }).ToList();

                return View(user);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");

            }
        }


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult EditUser(int Id)
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                jntuh_ffc_auditor auditor = db.jntuh_ffc_auditor.Where(f => f.id == Id).FirstOrDefault();
                UserCreation user = new UserCreation();
                user.UserName = auditor.auditorName;
                user.Email = auditor.auditorEmail1;
                user.PhoneNumber = auditor.auditorMobile1;
                user.Id = auditor.id;
                user.Departmentid = (int)auditor.auditorDepartmentID;
                user.Designationid = (int)auditor.auditorDesignationID;
                user.University = auditor.auditorPlace;
                
                // should delete this
                user.ConfirmPhoneNumber = auditor.auditorMobile1;

                ViewBag.departments = db.jntuh_department.Where(f => f.degreeId == 4 || f.id == 26 || f.id == 28).Select(f => new { id = f.id, department = f.departmentName }).ToList();


                ViewBag.designations = db.jntuh_designation
                    .Where(d => d.id == 1 | d.id == 2 | d.id == 3 | d.id == 4)
                    .Select(d => new
                    {
                        id = d.id,
                        designation = d.designation
                    })
                    .ToList();

              



                ViewBag.Update = true;
                return View("UserRegistration", user);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }


        }

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult UserRegistration(int? id)
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {

                ViewBag.departments = db.jntuh_department.Where(f => f.degreeId == 4 || f.id == 26 || f.id == 28).Select(f => new { id = f.id, department = f.departmentName }).ToList();


                ViewBag.designations = db.jntuh_designation.Where(d => d.id == 1 || d.id == 2 || d.id == 3 || d.id == 4).Select(d => new { id = d.id, designation = d.designation }).ToList();
             

                if (id != null)
                {
                    jntuh_ffc_auditor auditor = db.jntuh_ffc_auditor.Where(f => f.id == id).Select(e=>e).FirstOrDefault();
                    UserCreation user = new UserCreation();
                    user.UserName = auditor.auditorName;
                    user.Email = auditor.auditorEmail1;
                    user.PhoneNumber = auditor.auditorMobile1;
                    user.Id = auditor.id;
                    user.Designationid = (int)auditor.auditorDepartmentID;
                    user.Designationid = (int)auditor.auditorDesignationID;
                    user.University = auditor.auditorPlace;

                   // ViewBag.departments = db.jntuh_department.Where(f => f.degreeId == 4).Select(f => new{id = f.id,department = f.departmentName}).ToList();


                  //  ViewBag.designations = db.jntuh_designation.Where(d => d.id == 1 | d.id == 2 | d.id == 3 | d.id == 4).Select(d => new{id = d.id,designation = d.designation}).ToList();
                    return View("UserRegistration", user);
                }

                return View();

            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }

        }


        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public ActionResult UserRegistration(UserCreation reg)
        {
            //
           // if (reg.Id != 0 && reg.Id != null && User.Identity.Name == "admin")
            if (reg.Id != 0 && reg.Id != null && User.Identity.IsAuthenticated)
            {
              
                if (ModelState.IsValid)
                {
                    jntuh_ffc_auditor auditor = db.jntuh_ffc_auditor.Where(f => f.id == reg.Id).Select(e=>e).FirstOrDefault();

                    jntuh_registration register = db.jntuh_registration.Where(f => f.Email == auditor.auditorEmail1).Select(e=>e).FirstOrDefault();
                    auditor.auditorName = reg.UserName;
                    auditor.auditorDesignationID = reg.Designationid;
                    auditor.auditorEmail1 = reg.Email;
                    auditor.auditorDepartmentID = reg.Departmentid;
                    auditor.auditorMobile1 = reg.PhoneNumber;
                    auditor.auditorPlace = reg.University;
                    
                    auditor.updatedOn = DateTime.Now;
                    auditor.updatedBy = 1;
                    db.Entry(auditor).State = EntityState.Modified;
                    db.SaveChanges();

                    register.UserName = reg.UserName;
                    register.Email = reg.Email;
                    string passwordInput = reg.PhoneNumber;
                    string Password = passwordInput.Substring(5, 5);
                    register.Password = Password;
                    register.UpdateOn = DateTime.Now;
                    register.RoleType = 7;
                    register.Updatedby = 1;
                    db.Entry(register).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Success"] = "Auditor Updated Successfully.....";
                    return RedirectToAction("getAllAuditors");
                }
                else
                {
                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    TempData["Error"] = "Please select required fields..";
                }
            }
            // 



            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                if (reg.Id == null)
                {
                    if (reg.PhoneNumber == reg.ConfirmPhoneNumber)
                    {
                        jntuh_registration userreg = new jntuh_registration();
                        jntuh_ffc_auditor auditor = new jntuh_ffc_auditor();
                        var exists = db.jntuh_ffc_auditor.Where(f => f.auditorEmail1 == reg.Email && f.auditorMobile1 == reg.PhoneNumber).Select(f => f).FirstOrDefault();


                        if (exists == null)
                        {
                            string passwordInput = reg.PhoneNumber;
                            string Password = passwordInput.Substring(5, 5);

                            userreg.UserName = reg.UserName;
                            userreg.Email = reg.Email;
                            userreg.Password = Password;
                            userreg.IsActive = true;
                            userreg.RoleType = 7;
                            userreg.Createdby = 1;
                            userreg.CreatedOn = DateTime.Now;
                            db.jntuh_registration.Add(userreg);
                            db.SaveChanges();


                            auditor.auditorName = reg.UserName;
                            auditor.auditorDepartmentID = reg.Departmentid;
                            auditor.auditorDesignationID = reg.Designationid;
                            auditor.auditorEmail1 = reg.Email;
                            auditor.auditorMobile1 = reg.PhoneNumber;
                            auditor.auditorPlace = reg.University;
                            auditor.isActive = true;
                            auditor.createdBy = 1;
                            auditor.createdOn = DateTime.Now;
                            db.jntuh_ffc_auditor.Add(auditor);
                            db.SaveChanges();



                            TempData["Success"] = "Auditor Saved Successfully.....";
                            return RedirectToAction("getAllAuditors");
                        }

                        else
                        {

                            TempData["Error"] = "A user already exists with this Phone number and Email addrress.";

                            return RedirectToAction("getAllAuditors");
                        }


                    }
                    else
                    {


                        TempData["Error"] = "Phone Numbers do not match..";


                        return RedirectToAction("getAllAuditors");

                    }

                }
                else
                {
                    TempData["Error"] = "Please Provide all details..";
                }
            }
            else
            {
                TempData["Error"] = "Please Provide all details..";
             
            }
            return RedirectToAction("UserRegistration");
            }

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult getAllSCMdeatails()
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {

                List<AllScmRequests> allRequests = (from auditor in db.jntuh_ffc_auditor
                                                    join assinAuditor in db.jntuh_auditor_assigned on auditor.id equals assinAuditor.AuditorId
                                                    join scmreq in db.jntuh_scmproceedingsrequests on assinAuditor.ScmId equals scmreq.ID
                                                    join dept in db.jntuh_department on auditor.auditorDepartmentID equals dept.id
                                                    where scmreq.DEpartmentId != 0 && scmreq.SpecializationId != 0

                                                    select new AllScmRequests
                                                    {
                                                        id = assinAuditor.Id,
                                                        scmId = scmreq.ID,
                                                        auditorId = assinAuditor.AuditorId,
                                                        AuditorName = auditor.auditorName,
                                                        deptid = scmreq.DEpartmentId,
                                                        deptName = dept.departmentName,
                                                        createdOn = assinAuditor.CreatedOn,
                                                        collegeId = (int?)scmreq.CollegeId,
                                                        PrefereedLocation = auditor.auditorPreferredDesignation
                                                    }).ToList();

                foreach (var item in allRequests)
                {
                    item.CreatedOn = item.createdOn.ToString("dd/MM/yyyy");
                }

                var data1 = allRequests.Where(e => e.auditorId == 941).Select(e => e).ToList();
                List<AllScmRequests> allRequests1 = allRequests.GroupBy(e => new { e.auditorId, e.CreatedOn, e.collegeId }).Select(e => new AllScmRequests { auditorId = e.FirstOrDefault().auditorId, AuditorName = e.FirstOrDefault().AuditorName, deptName = e.FirstOrDefault().deptName, CreatedOn = e.FirstOrDefault().CreatedOn, scmId = e.FirstOrDefault().scmId,PrefereedLocation = e.FirstOrDefault().PrefereedLocation}).ToList();



                List<AllScmRequests> allRequests4 = new List<AllScmRequests>();
                foreach (var id in allRequests1.Select(e => e.auditorId).Distinct().ToArray())
                {
                    AllScmRequests data = new AllScmRequests();
                    data.AuditorName = allRequests1.Where(e => e.auditorId == id).Select(e => e.AuditorName).FirstOrDefault();
                    data.PrefereedLocation = allRequests1.Where(e => e.auditorId == id).Select(e => e.PrefereedLocation).FirstOrDefault();
                    data.deptName = allRequests1.Where(e => e.auditorId == id).Select(e => e.deptName).FirstOrDefault();
                    data.auditorId = id;
                    data.CountofSCM = allRequests1.Where(e => e.auditorId == id).Select(e => e.scmId).Count();
                    string strscmid = string.Empty;
                    var scmids = allRequests.Where(e => e.auditorId == id).Select(e => e.scmId).ToArray();
                    foreach (var item in scmids)
                    {

                        if (!string.IsNullOrEmpty(strscmid))
                        {
                            strscmid += item.ToString() + ",";
                        }
                        else
                        {
                            strscmid = item.ToString() + ",";
                        }
                    }
                    strscmid = strscmid.Substring(0, strscmid.Length - 1);
                    data.StrScmIds = strscmid;


                    allRequests4.Add(data);

                }



                return View(allRequests4);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");

            }
        }

        public ActionResult getSCMrecordsbyID(int auditorId)
        {

            if (auditorId != 0)
            {
                //string IDs,
                //var SCMIDS = new List<int>();
                //var SCMDATA = IDs.Split(',');
                //foreach (var strscmId in SCMDATA)
                //{
                //    if (!string.IsNullOrEmpty(strscmId))
                //    {
                //        SCMIDS.Add(Convert.ToInt16(strscmId));
                //    }
                //}



                List<AllScmRequests> Requests = (from scmreq in db.jntuh_scmproceedingsrequests
                    join col in db.jntuh_college on scmreq.CollegeId equals col.id
                    join dept in db.jntuh_department on scmreq.DEpartmentId equals dept.id
                    join spec in db.jntuh_specialization on scmreq.SpecializationId equals spec.id
                    join Assign in db.jntuh_auditor_assigned on scmreq.ID equals Assign.ScmId
                    where  Assign.AuditorId==auditorId
                    select new AllScmRequests
                    {
                        CollegeName = col.collegeName,
                        deptName = dept.departmentName,
                        SpecializationName = spec.specializationName,
                        createdOn = Assign.CreatedOn
                    }).ToList();

                // return PartialView("_getSCMrecordsbyID", Requests);
                return View("getSCMrecordsbyID", Requests);
            }
            else
            {
                return RedirectToAction("getAllSCMdeatails");
            }
        }

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult OthersDeptColleges(int? CollegeId)
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                var Colleges = db.jntuh_scmproceedingsrequests.Join(db.jntuh_college, SCM => SCM.CollegeId, CLG => CLG.id, (SCM, CLG) => new { SCM = SCM, CLG = CLG }).Where(e => e.SCM.RequestSubmittedDate != null &&(e.SCM.DEpartmentId == 65 || e.SCM.DEpartmentId == 66 || e.SCM.DEpartmentId == 67 || e.SCM.DEpartmentId == 68)).Select(e => new { CollegeId = e.CLG.id, CollegeName = e.CLG.collegeCode + " - " + e.CLG.collegeName }).ToList();
                ViewBag.CollegeData = Colleges.GroupBy(e => new { e.CollegeId, e.CollegeName }).Select(e => new { CollegeId = e.Key.CollegeId, CollegeName=e.Key.CollegeName}).OrderBy(e=>e.CollegeName).ToList();
                List<OthersDeptlist> CollegewiseOtherdepartmentData=new List<OthersDeptlist>();
                if (CollegeId != null)
                {
                     CollegewiseOtherdepartmentData =db.jntuh_scmproceedingsrequests.Join(db.jntuh_department, SCM => SCM.DEpartmentId,Dept => Dept.id, (SCM, Dept) => new {SCM = SCM, Dept = Dept}).Where(e => e.SCM.CollegeId == CollegeId && e.SCM.SpecializationId != 0 && e.SCM.RequestSubmittedDate != null && (e.SCM.DEpartmentId == 65 || e.SCM.DEpartmentId == 66 || e.SCM.DEpartmentId == 67 || e.SCM.DEpartmentId == 68))
                            .Select(e => new OthersDeptlist
                            {
                                ScmId = e.SCM.ID,
                               DepartmentName = e.Dept.departmentName,
                               DeptId = e.SCM.DEpartmentId,
                               RequestsubmittedDate = e.SCM.RequestSubmittedDate
                            })
                            .ToList();

                    IEnumerable<jntuh_scmproceedingsrequests> SCMRequestdata =db.jntuh_scmproceedingsrequests.Where(e => e.CollegeId == CollegeId && e.OldSCMId != null).Select(e => e).ToList();

                    foreach (var Othersdata in CollegewiseOtherdepartmentData)
                    {
                        if (Othersdata != null)
                        {
                            var Requestdivided = SCMRequestdata.Count(e => e.OldSCMId == Othersdata.ScmId);
                            Othersdata.IsRequestSplit = Requestdivided != 0?true:false;
                        }
                    }
                }

                





                return View(CollegewiseOtherdepartmentData);
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult ViewFaculty(int scmid)
        {
            if (User.Identity.IsAuthenticated)
            // if (User.Identity.Name == "admin")
            {
            if (scmid != 0)
            {
                List<ScmProceedingsRequestAddReg> addFacultyDetails = new List<ScmProceedingsRequestAddReg>();
                var scmaddfacultydata = (from SCM in db.jntuh_scmproceedingsrequests join SCMADDFLY in db.jntuh_scmproceedingrequest_addfaculty on SCM.ID equals SCMADDFLY.ScmProceedingId
                                         where SCM.ID == scmid
                                         select new
                                         {
                                             RegistrationNumber = SCMADDFLY.RegistrationNumber,
                                             SpecializationId = SCM.SpecializationId,
                                             DegreeId = SCM.DegreeId,
                                             ScmProceedingId = SCMADDFLY.ScmProceedingId,
                                             Id = SCMADDFLY.Id,
                                             FacultyTypeId = SCMADDFLY.FacultyType,
                                             OtherFacultyMovingStatus = SCMADDFLY.OtresFacultyMovingStatus
                                         }).ToList();


                var jntuh_designation = db.jntuh_designation.ToList();
                addFacultyDetails = (from a in scmaddfacultydata
                                     join c in db.jntuh_registered_faculty.AsNoTracking() on a.RegistrationNumber.Trim() equals c.RegistrationNumber.Trim()
                                     join d in db.jntuh_specialization on a.SpecializationId equals d.id
                                     join e in db.jntuh_degree on a.DegreeId equals e.id
                                     select new ScmProceedingsRequestAddReg
                                     {
                                         Id = a.Id,
                                         SpecializationId = a.SpecializationId,
                                         SpecializationName = e.degree + "-" + d.specializationName,
                                         Regno = c.RegistrationNumber,
                                         RegName = c.FirstName + " " + c.LastName,
                                         ScmId = a.ScmProceedingId,
                                         FacultyId = c.id,
                                         DesignationId = (int)a.FacultyTypeId,
                                        OtherFacultyMovingStatus=a.OtherFacultyMovingStatus,
                                         Designation = jntuh_designation.Where(x => x.id == a.FacultyTypeId).Select(x => x.designation).FirstOrDefault()
                                     }).ToList();

                return View(addFacultyDetails);
            }
            return RedirectToAction("OthersDeptColleges");
            }
             else
             {
                 FormsAuthentication.SignOut();
                 return RedirectToAction("Login");
             }
        }

        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult DivideRequestintoTwo(int scmid)
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                if (scmid != 0)
                {
                    var scmrequestData =
                        db.jntuh_scmproceedingsrequests.Where(e => e.ID == scmid).Select(e => e).FirstOrDefault();
                    if (scmrequestData != null)
                    {
                        List<int> DeptIds = new List<int>();
                        if (scmrequestData.DEpartmentId == 65)
                        {
                            DeptIds.Add(71);
                            DeptIds.Add(72);
                        }
                        else if ((scmrequestData.DEpartmentId == 66))
                        {
                            DeptIds.Add(73);
                            DeptIds.Add(74);
                        }
                        else if ((scmrequestData.DEpartmentId == 67))
                        {
                            DeptIds.Add(75);
                            DeptIds.Add(76);
                        }
                        else if ((scmrequestData.DEpartmentId == 68))
                        {
                            DeptIds.Add(77);
                            DeptIds.Add(78);
                        }

                        foreach (var deptId in DeptIds)
                        {
                            int SpecializationId = 0;
                            if (deptId == 71)
                            {
                                SpecializationId = 163;
                            }
                            else if (deptId == 72)
                            {
                                SpecializationId = 164;
                            }
                            else if (deptId == 73)
                            {
                                SpecializationId = 159;
                            }
                            else if (deptId == 74)
                            {
                                SpecializationId = 160;
                            }
                            else if (deptId == 75)
                            {
                                SpecializationId = 161;
                            }
                            else if (deptId == 76)
                            {
                                SpecializationId = 162;
                            }
                            else if (deptId == 77)
                            {
                                SpecializationId = 165;
                            }
                            else if (deptId == 78)
                            {
                                SpecializationId = 166;
                            }




                            jntuh_scmproceedingsrequests scmreqScmproceedingsrequests =
                                new jntuh_scmproceedingsrequests();
                            scmreqScmproceedingsrequests.OldSCMId = scmrequestData.ID;
                            scmreqScmproceedingsrequests.CollegeId = scmrequestData.CollegeId;
                            scmreqScmproceedingsrequests.TotalNoofFacultyRequired =
                                scmrequestData.TotalNoofFacultyRequired;
                            scmreqScmproceedingsrequests.SpecializationId = SpecializationId;
                            scmreqScmproceedingsrequests.DEpartmentId = deptId;
                            scmreqScmproceedingsrequests.DegreeId = scmrequestData.DegreeId;
                            scmreqScmproceedingsrequests.ProfessorsCount = scmrequestData.ProfessorsCount;
                            scmreqScmproceedingsrequests.AssistantProfessorsCount =
                                scmrequestData.AssistantProfessorsCount;
                            scmreqScmproceedingsrequests.AssociateProfessorsCount =
                                scmrequestData.AssociateProfessorsCount;
                            scmreqScmproceedingsrequests.Remarks = scmrequestData.Remarks;

                            scmreqScmproceedingsrequests.SCMNotification = scmrequestData.SCMNotification;
                            scmreqScmproceedingsrequests.CreatedOn = scmrequestData.CreatedOn;
                            scmreqScmproceedingsrequests.CreatedBy = scmrequestData.CreatedBy;
                            scmreqScmproceedingsrequests.ISActive = scmrequestData.ISActive;
                            scmreqScmproceedingsrequests.Notificationdate = scmrequestData.Notificationdate;

                            scmreqScmproceedingsrequests.RequiredProfessor = scmrequestData.RequiredProfessor;
                            ;
                            scmreqScmproceedingsrequests.RequiredAssistantProfessor =
                                scmrequestData.RequiredAssistantProfessor;
                            scmreqScmproceedingsrequests.RequiredAssociateProfessor =
                                scmrequestData.RequiredAssociateProfessor;
                            scmreqScmproceedingsrequests.SCMOrder = scmrequestData.SCMOrder;
                            scmreqScmproceedingsrequests.RequestSubmittedDate = scmrequestData.RequestSubmittedDate;
                            db.jntuh_scmproceedingsrequests.Add(scmreqScmproceedingsrequests);
                            db.SaveChanges();
                        }
                        TempData["Success"] = "SCM Request Divided Successfully.";
                        return RedirectToAction("OthersDeptColleges", new {collegeId = scmrequestData.CollegeId});
                    }
                    else
                    {
                        TempData["Error"] = "SCM Request Does not Exist.";
                        return RedirectToAction("OthersDeptColleges");
                    }
                }
                return RedirectToAction("OthersDeptColleges");
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }


        [HttpGet]
        public ActionResult FacultyMoving(int ScmId, string RegNo, int DesignationId, int AddFacultyId)
        {
            List<FacultyMovingModel> facultyMovingList=new List<FacultyMovingModel>();
            if (ScmId != 0 && !string.IsNullOrEmpty(RegNo) && DesignationId != 0 && AddFacultyId!=0)
            {
                var ScmRequestdata = (from SCM in db.jntuh_scmproceedingsrequests join DEPT in db.jntuh_department on SCM.DEpartmentId equals DEPT.id
                    where SCM.OldSCMId == ScmId
                    select new {NewSCMId = SCM.ID, id = DEPT.id, departmentName = DEPT.departmentName}).ToList();
                if (ScmRequestdata.Count() != 0)
                {
                    foreach (var departmentdata in ScmRequestdata)
                    {
                        FacultyMovingModel faculty=new FacultyMovingModel();
                        faculty.DesignationId = DesignationId;
                        faculty.RegistrationNumber = RegNo;
                        faculty.NewDeptId = departmentdata.id;
                        faculty.NewDeptName = departmentdata.departmentName;
                        faculty.SelectDeptId = false;
                        faculty.OldSCMId = ScmId;
                        faculty.AddFacultyId = AddFacultyId;
                        faculty.NewSCMId = departmentdata.NewSCMId;
                        facultyMovingList.Add(faculty);
                    }
                }

            }
            return PartialView("_FacultyMoving", facultyMovingList);
        }

        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public ActionResult FacultyMoving(List<FacultyMovingModel> model)
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                model = model.Where(e => e.SelectDeptId == true).Select(e => e).ToList();
                if (model.Count() != 0)
                {
                    int SCMId = model[0].OldSCMId;
                    if (model.Count() > 0 && model.Count() < 2)
                    {
                        int OtherFacultymovinfstatus = 0;
                        int AddFaculty = model[0].AddFacultyId;
                        var UpdatedAddFaculty =
                            db.jntuh_scmproceedingrequest_addfaculty.FirstOrDefault(e => e.Id == AddFaculty);
                        if (UpdatedAddFaculty != null)
                        {
                            UpdatedAddFaculty.OtresFacultyMovingStatus = true;
                            db.Entry(UpdatedAddFaculty).State = EntityState.Modified;
                            OtherFacultymovinfstatus = db.SaveChanges();
                        }

                        if (OtherFacultymovinfstatus == 1)
                        {
                            jntuh_scmproceedingrequest_addfaculty addfaculty =
                                new jntuh_scmproceedingrequest_addfaculty();
                            addfaculty.ScmProceedingId = model[0].NewSCMId;
                            addfaculty.RegistrationNumber = model[0].RegistrationNumber;
                            addfaculty.FacultyType = model[0].DesignationId;
                            addfaculty.Createdby = 1;
                            addfaculty.CreatedOn = DateTime.Now;
                            addfaculty.Isactive = true;
                            addfaculty.PreviousCollegeId = "0";
                            db.jntuh_scmproceedingrequest_addfaculty.Add(addfaculty);
                            db.SaveChanges();
                            TempData["Success"] = "Faculty Add Successfully.";
                            return RedirectToAction("ViewFaculty", "Admin", new {scmid = SCMId});
                        }
                        TempData["Error"] = "Faculty Add Failed";
                        return RedirectToAction("ViewFaculty", "Admin", new {scmid = SCMId});
                    }
                    else
                    {
                        TempData["Error"] = "Please Select Any One Department";
                        return RedirectToAction("ViewFaculty", "Admin", new {scmid = SCMId});
                    }
                }
                else
                {
                    TempData["Error"] = "Please Select At Least One Department";
                    return RedirectToAction("OthersDeptColleges");
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult DataEntryCreation()
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                var values = db.my_aspnet_roles.Where(e=>e.id==8).ToList()
               .Select(x => new SelectListItem
               {
                   Value = x.id.ToString(),
                   Text = x.name.ToString()
               });

                var menus = new SelectList(values, "Value", "Text");
                ViewData["Roles"] = menus;
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public ActionResult DataEntryCreation(DataEntryCreation model)
        {
            if (User.Identity.IsAuthenticated)
            //if (User.Identity.Name == "admin")
            {
                if (ModelState.IsValid)
                {
                    jntuh_registration reg=new jntuh_registration();
                    reg.UserName = model.UserName;
                    reg.Email = model.Email;
                    reg.Password = model.Password;
                    reg.RoleType = model.RoleType;
                    reg.Createdby = 1;
                    reg.CreatedOn = DateTime.Now;
                    reg.IsActive = true;
                    db.jntuh_registration.Add(reg);
                    db.SaveChanges();
                    TempData["Success"] = "DataEntry User Creation Successfully.";
                    return RedirectToAction("DataEntryCreation");

                }
                return RedirectToAction("DataEntryCreation");
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public JsonResult CheckEmail(string Email)
        {
            string CheckingEmail = db.jntuh_registration.Where(e => e.Email == Email.Trim()).Select(e => e.Email).FirstOrDefault();
            if (!string.IsNullOrEmpty(CheckingEmail))
            {
                if (CheckingEmail.Trim() == Email.Trim())
                    return Json(false);
                else
                    return Json(true);
            }
            else
                return Json(true);

        }


         [AuthorizedUserAccess("Admin")]
        public ActionResult CollegeWiseApprovedReport(int? collegeId,string type)
        {
            if (User.Identity.IsAuthenticated)
            {
                var SubmittedCollegeIds =db.jntuh_college_edit_status.AsNoTracking().Where(e => e.IsCollegeEditable == false).Select(e => e.collegeId).ToArray();
                var collegeIds = db.jntuh_auditors_dataentry.Where(e => e.IsActive == true && e.SpecializationId != 0 && e.DepartmentId != 0 && e.DegreeId != 0).Select(e => e.CollegeId).Distinct().ToArray();
                ViewBag.Colleges = db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id) && SubmittedCollegeIds.Contains(e.id)).Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName }).OrderBy(e => e.collegeName).ToList();

                if (collegeId != null)
                {
                    var jntuh_auditors_dataentry = db.jntuh_auditors_dataentry.Where(e => e.IsActive == true).Select(e => e).ToList();
                    var jntuh_scmproceedingrequest_addfaculty = db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking().ToList();


                    var Scmalldata = (from NomineeDTERY in db.jntuh_auditors_dataentry
                                      join REG in db.jntuh_registered_faculty on NomineeDTERY.RegistrationNo equals REG.RegistrationNumber
                                      join AUDI in db.jntuh_ffc_auditor on NomineeDTERY.AuditorId equals AUDI.id
                                      join SPEC in db.jntuh_specialization on NomineeDTERY.SpecializationId equals SPEC.id
                                      join DEPT in db.jntuh_department on NomineeDTERY.DepartmentId equals DEPT.id
                                      join DESG in db.jntuh_degree on NomineeDTERY.DegreeId equals DESG.id
                                      where NomineeDTERY.CollegeId == collegeId && NomineeDTERY.SpecializationId != 0 && NomineeDTERY.DepartmentId != 0 && NomineeDTERY.DegreeId != 0
                                      select new ScmUploadedData()
                                      {
                                          RegistrationNumber = REG.RegistrationNumber,
                                          SCMId = NomineeDTERY.ScmProceedingId,
                                          Specialization = DESG.degree + "-" + SPEC.specializationName,
                                          SpecializationId = NomineeDTERY.SpecializationId,
                                          Department = DEPT.departmentName,
                                          DepartmentId = NomineeDTERY.DepartmentId,
                                          Degree = DESG.degree,
                                          DegreeId = NomineeDTERY.DegreeId,
                                          FirstName = REG.FirstName + " " + REG.LastName,
                                          CollegeId = NomineeDTERY.CollegeId,
                                          AuditorId = NomineeDTERY.AuditorId,
                                          AuditorName = AUDI.auditorName,
                                          Checked = NomineeDTERY.IsSelected != false ? true : false,
                                          SCMhardcopyview = NomineeDTERY.SCMhardcopy,
                                          SCMDate = NomineeDTERY.CreatedOn,
                                          facultyId = REG.id
                                      }).OrderBy(e => e.RegistrationNumber).ToList();





                    List<ScmUploadedData> collegewiseSCmdata = new List<ScmUploadedData>();
                    int ScmId = 0;
                    string Regno = string.Empty;
                    int trackingFacultyId = 0;
                    List<jntuh_scmproceedingrequest_addfaculty> AddFacultyDetails =new List<jntuh_scmproceedingrequest_addfaculty>();
                    foreach (var item in Scmalldata.OrderBy(e=>e.SCMId).Select(e=>e).ToList())
                    {
                        if (item.SCMId == 380)
                        {

                        }

                        if (ScmId == 0 && string.IsNullOrEmpty(Regno) && Scmalldata.IndexOf(item)==0)
                        {
                             AddFacultyDetails = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.ScmProceedingId == item.SCMId && e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim() && e.FacultyType != 1).OrderBy(e => e.Id).Select(e => e).ToList();
                        }
                        if (ScmId != 0 && !string.IsNullOrEmpty(Regno) && ScmId != item.SCMId)
                        {
                            AddFacultyDetails = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.ScmProceedingId == item.SCMId && e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim() && e.FacultyType != 1).OrderBy(e => e.Id).Select(e => e).ToList();
                        }

                        if (ScmId == 0 && string.IsNullOrEmpty(Regno) && ScmId != item.SCMId)
                        {
                            AddFacultyDetails = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.ScmProceedingId == item.SCMId && e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim() && e.FacultyType != 1).OrderBy(e => e.Id).Select(e => e).ToList();
                        }
                        if (ScmId != 0 && Regno.Trim()!=item.RegistrationNumber.Trim() && ScmId == item.SCMId)
                        {
                            AddFacultyDetails = db.jntuh_scmproceedingrequest_addfaculty.Where(e => e.ScmProceedingId == item.SCMId && e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim() && e.FacultyType != 1).OrderBy(e => e.Id).Select(e => e).ToList();
                        }


                        //if (AddFacultyDetails.Count > 0)
                        //{
                        //    foreach (var adddata in AddFacultyDetails)
                        //    {
                        //        if(adddata.FacultyType!=null)
                        //            item.DesignationId = Convert.ToInt16(adddata.FacultyType);
                        //        item.ScmfacultyaddedId = adddata.Id;
                        //        item.Approved = adddata.IsApproved;
                        //        item.Remarks = adddata.DeactiviationReason;
                        //        collegewiseSCmdata.Add(item);
                        //    }
                        //}

                        //var firstOrDefault = new jntuh_scmproceedingrequest_addfaculty();

                        //if (ScmId == item.SCMId && Regno == item.RegistrationNumber)
                        //{
                        //    firstOrDefault = jntuh_scmproceedingrequest_addfaculty.Where(e => e.ScmProceedingId == item.SCMId && e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim() && e.FacultyType != 1).Select(e => e).OrderByDescending(e => e.Id).FirstOrDefault();
                        //    ScmId = 0;
                        //    Regno = string.Empty;
                        //}
                        //else
                        //{
                        //    firstOrDefault = jntuh_scmproceedingrequest_addfaculty.Where(e => e.ScmProceedingId == item.SCMId && e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim() && e.FacultyType != 1).Select(e => e).FirstOrDefault();
                        //    ScmId = item.SCMId;
                        //    Regno = item.RegistrationNumber.Trim();
                        //}


                        //if (firstOrDefault != null)
                        //{
                        //    if (firstOrDefault.FacultyType != null)
                        //        item.DesignationId = Convert.ToInt16(firstOrDefault.FacultyType);
                        //    item.ScmfacultyaddedId = firstOrDefault.Id;
                        //    item.Approved = firstOrDefault.IsApproved;
                        //    item.Remarks = firstOrDefault.DeactiviationReason;
                        //}

                        var firstOrDefault = new jntuh_scmproceedingrequest_addfaculty();

                        if (ScmId == item.SCMId && Regno == item.RegistrationNumber)
                        {
                            if (trackingFacultyId != 0)
                                firstOrDefault = AddFacultyDetails.Where(e => e.Id != trackingFacultyId).Select(e => e).OrderByDescending(e => e.Id).FirstOrDefault();
                            else
                                  firstOrDefault = AddFacultyDetails.Select(e => e).OrderByDescending(e => e.Id).FirstOrDefault();

                            ScmId = 0;
                            Regno = string.Empty;
                            trackingFacultyId = 0;
                        }
                        else
                        {
                            firstOrDefault = AddFacultyDetails.Select(e => e).OrderBy(e=>e.Id).FirstOrDefault();
                            ScmId = item.SCMId;
                            Regno = item.RegistrationNumber.Trim();
                        }


                        if (firstOrDefault != null)
                        {
                            if (firstOrDefault.FacultyType != null)
                                item.DesignationId = Convert.ToInt16(firstOrDefault.FacultyType);
                            item.ScmfacultyaddedId = firstOrDefault.Id;
                            item.Approved = firstOrDefault.IsApproved;
                            item.Remarks = firstOrDefault.DeactiviationReason;
                            trackingFacultyId = firstOrDefault.Id;
                        }
                        collegewiseSCmdata.Add(item);


                       
                    }


                    List<CollegeAssociatedFaculty> CollegeAssociatedFaculty = db.jntuh_college_faculty_registered.Where(e => e.collegeId == collegeId && e.DepartmentId != null && e.RegistrationNumber != null).Select(e => new CollegeAssociatedFaculty()
                    {
                        RegNo = e.RegistrationNumber, 
                        CollegeId = e.collegeId, 
                        DeptId = (int)e.DepartmentId
                    }).ToList();


                    var CollegeData =collegewiseSCmdata.Join(CollegeAssociatedFaculty, Clg => Clg.RegistrationNumber, D => D.RegNo,(Clg, D) => new {Clg = Clg, D = D})
                            .Where(e => e.Clg.RegistrationNumber == e.D.RegNo && e.Clg.DepartmentId == e.D.DeptId).Select(e => e.Clg).ToList();
                    //new ScmUploadedData()
                    //{

                    //    RegistrationNumber = e.Clg.RegistrationNumber,
                    //    SCMId = e.Clg.SCMId,
                    //    Specialization = e.Clg.Specialization,
                    //    SpecializationId = e.Clg.SpecializationId,
                    //    Department = e.Clg.Department,
                    //    DepartmentId = e.Clg.DepartmentId,
                    //    Degree = e.Clg.Degree,
                    //    DegreeId = e.Clg.DegreeId,
                    //    FirstName = e.Clg.LastName,
                    //    CollegeId = e.Clg.CollegeId,
                    //    AuditorId = e.Clg.AuditorId,
                    //    AuditorName = e.Clg.AuditorName,
                    //    Checked = e.Clg.Checked,
                    //    SCMhardcopyview = e.Clg.SCMhardcopyview,
                    //    SCMDate = e.Clg.SCMDate,
                    //    facultyId = e.Clg.facultyId
                    //}).ToList();
                    if (type == "Excel")
                    {
                        Response.ClearContent();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment; filename=" + collegeId + " SCM Verification.xls");
                        Response.ContentType = "application/vnd.ms-excel";
                        return PartialView("~/Views/Admin/_CollegeWiseApprovedReport.cshtml", CollegeData);
                    }

                    return View(CollegeData.Select(e => e).ToList());
                }
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
        }



         [HttpGet]
         [AuthorizedUserAccess("Admin")]
         public ActionResult CollegewiseDetailsReset(int? collegeId)
         {
             if (User.Identity.IsAuthenticated)
             {

                 var collegeIds = db.jntuh_auditors_dataentry.Where(e => e.IsActive == true && e.SpecializationId != 0 && e.DepartmentId != 0 && e.DegreeId != 0).Select(e => e.CollegeId).Distinct().ToArray();
                 ViewBag.Colleges = db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id)).Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName }).OrderBy(e => e.collegeName).ToList();

                 if (collegeId != null)
                 {
                     var jntuh_auditors_dataentry = db.jntuh_auditors_dataentry.Where(e => e.IsActive == true).Select(e => e).ToList();
                     var jntuh_scmproceedingrequest_addfaculty = db.jntuh_scmproceedingrequest_addfaculty.AsNoTracking().ToList();


                     var Scmalldata = (from NomineeDTERY in db.jntuh_auditors_dataentry
                                       join REG in db.jntuh_registered_faculty on NomineeDTERY.RegistrationNo equals REG.RegistrationNumber
                                       join AUDI in db.jntuh_ffc_auditor on NomineeDTERY.AuditorId equals AUDI.id
                                       join SPEC in db.jntuh_specialization on NomineeDTERY.SpecializationId equals SPEC.id
                                       join DEPT in db.jntuh_department on NomineeDTERY.DepartmentId equals DEPT.id
                                       join DESG in db.jntuh_degree on NomineeDTERY.DegreeId equals DESG.id
                                       where NomineeDTERY.CollegeId == collegeId && NomineeDTERY.SpecializationId != 0 &&
                                             NomineeDTERY.DepartmentId != 0 && NomineeDTERY.DegreeId != 0
                                       select new ScmUploadedData()
                                       {
                                           RegistrationNumber = REG.RegistrationNumber,
                                           SCMId = NomineeDTERY.ScmProceedingId,
                                           Specialization = DESG.degree + "-" + SPEC.specializationName,
                                           SpecializationId = NomineeDTERY.SpecializationId,
                                           Department = DEPT.departmentName,
                                           DepartmentId = NomineeDTERY.DepartmentId,
                                           Degree = DESG.degree,
                                           DegreeId = NomineeDTERY.DegreeId,
                                           FirstName = REG.FirstName + " " + REG.LastName,
                                           CollegeId = NomineeDTERY.CollegeId,
                                           AuditorId = NomineeDTERY.AuditorId,
                                           AuditorName = AUDI.auditorName,
                                           Checked = NomineeDTERY.IsSelected != false ? true : false,
                                           SCMhardcopyview = NomineeDTERY.SCMhardcopy,
                                           SCMDate = NomineeDTERY.CreatedOn,
                                           facultyId = REG.id
                                       }).OrderBy(e => e.RegistrationNumber).ToList();





                     List<ScmUploadedData> collegewiseSCmdata = new List<ScmUploadedData>();
                     int ScmId = 0;
                     string Regno = string.Empty;
                     foreach (var item in Scmalldata)
                     {
                         var firstOrDefault = new jntuh_scmproceedingrequest_addfaculty();

                         if (ScmId == item.SCMId && Regno == item.RegistrationNumber)
                         {
                             firstOrDefault =
                                 jntuh_scmproceedingrequest_addfaculty.Where(
                                     e =>
                                         e.ScmProceedingId == item.SCMId &&
                                         e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim() &&
                                         e.FacultyType != 1).Select(e => e).OrderByDescending(e => e.Id).FirstOrDefault();
                             ScmId = 0;
                             Regno = string.Empty;
                         }
                         else
                         {
                             firstOrDefault =
                                 jntuh_scmproceedingrequest_addfaculty.Where(
                                     e =>
                                         e.ScmProceedingId == item.SCMId &&
                                         e.RegistrationNumber.Trim() == item.RegistrationNumber.Trim() &&
                                         e.FacultyType != 1).Select(e => e).FirstOrDefault();
                             ScmId = item.SCMId;
                             Regno = item.RegistrationNumber.Trim();
                         }


                         if (firstOrDefault != null)
                         {
                             if (firstOrDefault.FacultyType != null)
                                 item.DesignationId = Convert.ToInt16(firstOrDefault.FacultyType);
                             item.ScmfacultyaddedId = firstOrDefault.Id;
                             item.Approved = firstOrDefault.IsApproved;
                             item.Remarks = firstOrDefault.DeactiviationReason;
                         }

                         collegewiseSCmdata.Add(item);
                     }


                     return View(collegewiseSCmdata.Select(e => e).ToList());
                 }
                 return View();
             }
             else
             {
                 FormsAuthentication.SignOut();
                 return RedirectToAction("Login");
             }
         }


        public ActionResult SampleData()
        {
            return View();
        }

    }


    #region Model Class

    public class UserCreation
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Enter User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Enter Password")]
        //public string Password { get; set; }

        [Required(ErrorMessage = "Enter Phone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Your Number must have 10 digits.")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.PhoneNumber)]
        //  [System.Web.Mvc.Compare("PhoneNumber", ErrorMessage = "PhoneNumber & ConfirmPhoneNumber  do not match.")]
        [Required(ErrorMessage = "Enter Confirm Phone Number")]
        public string ConfirmPhoneNumber { get; set; }

        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }

        [Required(ErrorMessage = "Select Department")]
        public int Departmentid { get; set; }

        //  public string Department { get; set; }
        [Required(ErrorMessage = "Select Designation")]
        public int Designationid { get; set; }

       


        [Required(ErrorMessage = "Enter University")]
        public string University { get; set; }

        //[DataType(DataType.Password)]
        //[System.Web.Mvc.Compare("Password", ErrorMessage = "Password & Confirmation Password do not match.")]
        //public string ConfirmPassword { get; set; }

    }


    //public class UserCreation
    //{
    //    public int Id { get; set; }

    //    [Required(ErrorMessage = "Enter User Name")]
    //    public string UserName { get; set; }

    //    [Required(ErrorMessage = "Enter Email")]
    //    public string Email { get; set; }

    //    [Required(ErrorMessage = "Enter Password")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [System.Web.Mvc.Compare("Password", ErrorMessage = "Password & Confirmation Password do not match.")]
    //    public string ConfirmPassword { get; set; }

    //}

    public class UserLogin
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }


    public class ScmPrincipaldetails
    {
        public int ScmId { get; set; }
        public string ColleegCode { get; set; }
        public string CollegeName { get; set; }
        public int CollegeId { get; set; }
        public string RegistrationNo { get; set; }
        public string FirstName { get; set; }
        public string strScmdate { get; set; }
        public DateTime? Scmdate { get; set; }
        public string Scmdocument { get; set; }
        public int PreviousWorkingCollegeId { get; set; }
        public string PreviousWorkingCollegeName { get; set; }
        public int ScmaddFaculty { get; set; }
        public int FacultyId { get; set; }
        public bool Checked { get; set; }
    }

    public class ChangepasswordModel
    {
        [Display(Name = "Current Password")]
        [Required(ErrorMessage = "Enter Current Password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Required(ErrorMessage = "Enter New Password")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Must be 6-15 characters long.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password does not match.")]
        public string ConfirmPassword { get; set; }
    }


    public class NomineeAssignedScmPrincipalRequest
    {
        public int ScmId { get; set; }
        public string ColleegCode { get; set; }
        public string CollegeName { get; set; }
        public int CollegeId { get; set; }
        public string RegistrationNo { get; set; }
        public string FirstName { get; set; }
        public string strScmdate { get; set; }
        public DateTime? Scmdate { get; set; }
        public string Scmdocument { get; set; }
        public string NomineeName { get; set; }
        public DateTime NomineeAssignedDate { get; set; }
    }



    public class AllScmRequests
    {
        public int id { get; set; }
        public int auditorId { get; set; }
        public int scmId { get; set; }
        public string AuditorName { get; set; }
        public int auditorassingedid { get; set; }
        public int deptid { get; set; }
        public string deptName { get; set; }
        public int Specid { get; set; }
        public string SpecializationName { get; set; }
        public DateTime createdOn { get; set; }
        public string CreatedOn { get; set; }
        public int CountofSCM { get; set; }
        public string CollegeName { get; set; }
        public int? collegeId { get; set; }
        public string StrScmIds { get; set; }
        public string PrefereedLocation  { get; set; }

    }


    public class OthersDeptlist
    {
        public string DepartmentName { get; set; }
        public int DeptId { get; set; }
        public int ScmId { get; set; }
        public DateTime? RequestsubmittedDate { get; set; }
        public bool IsRequestSplit { get; set; }

    }

    public class FacultyMovingModel
    {
        public int NewSCMId { get; set; }
        public int OldSCMId { get; set; }
        public int OldDeptId { get; set; }
        public int NewDeptId { get; set; }
        public string OldDeptName { get; set; }
        public string NewDeptName { get; set; }
        public string RegistrationNumber { get; set; }
        public int DesignationId { get; set; }
        public bool SelectDeptId { get; set; }
        public int AddFacultyId { get; set; }
    }

    public class DataEntryCreation
    {
        [Required(ErrorMessage = "Please Enter Name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Must be 3-100 characters long.")]
        [RegularExpression(@"[a-zA-Z0-9_]{3,100}", ErrorMessage = "Valid username required")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Please Enter Email")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Must be 6-100 characters long.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Valid email address required.")]
        [Remote("CheckEmail", "Admin", HttpMethod = "POST", ErrorMessage = "Email Already exists.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please Enter Password")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Must be 6-15 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"[a-zA-Z0-9.!#$&@]{5,25}", ErrorMessage = "Valid password required. Allowed characters: a-z A-Z 0-9 . ! # $ & @")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Please Select Role")]
        public int RoleType { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Password & ConfirmPassword  do not match.")]
        public string ConfirmPassword { get; set; }

    }

    public class CollegeAssociatedFaculty
    {
        public string RegNo { get; set; }
        public int CollegeId { get; set; }
        public int DeptId { get; set; }
    }

    #endregion
}


