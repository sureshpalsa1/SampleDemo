using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UAAASSCM.Models;

namespace UAAASSCM.Controllers
{
    public class UserController : Controller
    {
       
        SCMEntities db=new SCMEntities();

        [HttpGet]
        public ActionResult Index(int? collegeId, int? specializationId,int? designationId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var jntuh_scmproceedingsrequests =db.jntuh_scmproceedingsrequests.AsNoTracking().Where(e => e.ISActive == true).Select(e => e).ToList();
                var jntuh_auditor_assigned =db.jntuh_auditor_assigned.AsNoTracking().Where(e => e.IsActive == true).Select(e => e).ToList();

                var AuditorId = db.jntuh_ffc_auditor.Where(e => e.isActive == true && e.auditorEmail1 == User.Identity.Name).Select(e => e.id).Distinct().ToArray();
                var SCMIDs =jntuh_auditor_assigned.Where(e => e.IsActive == true && AuditorId.Contains(e.AuditorId) && e.RequestSubmittedDate==null).Select(e => e.ScmId).Distinct().ToArray();

                var collegeIds = jntuh_scmproceedingsrequests.Where(e => e.ISActive == true && SCMIDs.Contains(e.ID) && e.SpecializationId!=0).Select(e => e.CollegeId).Distinct().ToArray();
               
                ViewBag.Colleges =db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id)).Select(e => new {collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName}).OrderBy(e => e.collegeName).ToList();

               

                if (collegeId != null && AuditorId.Count()!=0)
                {

                    var NomineeDataEntrySCmIds1 = db.jntuh_auditors_dataentry.Where(e => e.IsActive == true && e.CollegeId == collegeId).Select(e => e.ScmProceedingId).Distinct().ToArray();


                    var AudiotAssignedSCMIds1 = db.jntuh_auditor_assigned.Where(e => e.CollegeId == collegeId && AuditorId.Contains(e.AuditorId) && e.RequestSubmittedDate == null).Select(e => e.ScmId).Distinct().ToArray();



                    #region Department Code

                    var DepartmentsData = (from SCMReq in db.jntuh_scmproceedingsrequests
                                           join ADDfaculty in db.jntuh_scmproceedingrequest_addfaculty on SCMReq.ID equals ADDfaculty.ScmProceedingId
                                           join AUDASSIGN in db.jntuh_auditor_assigned on SCMReq.ID equals AUDASSIGN.ScmId
                                           join spec in db.jntuh_specialization on SCMReq.SpecializationId equals spec.id
                                           join Deg in db.jntuh_degree on SCMReq.DegreeId equals Deg.id
                                           join Dept in db.jntuh_department on SCMReq.DEpartmentId equals Dept.id
                                           join Desig in db.jntuh_designation on ADDfaculty.FacultyType equals Desig.id
                                           where AudiotAssignedSCMIds1.Contains(SCMReq.ID) && !NomineeDataEntrySCmIds1.Contains(SCMReq.ID) && ADDfaculty.FacultyType != 1 && AuditorId.Contains(AUDASSIGN.AuditorId) 
                                           select new
                                           {
                                               SpecializationId = spec.id,
                                               SpecializationName = Deg.degree + " - " + spec.specializationName,
                                               DepartmentId = Dept.id,
                                               DegreeId = Deg.id
                                           }).Distinct().ToList();


                    ViewBag.departments = DepartmentsData.OrderBy(i => i.DepartmentId).ToList();
                   
                    

                    #endregion

                    if (specializationId != null)
                    {
                        var NomineeDataEntrySCmIds = db.jntuh_auditors_dataentry.AsNoTracking().Where(e => e.IsActive == true && e.CollegeId == collegeId && e.SpecializationId == specializationId).Select(e => e.ScmProceedingId).Distinct().ToArray();
                        var AudiotAssignedSCMIds = db.jntuh_auditor_assigned.AsNoTracking().Where(e => e.CollegeId == collegeId && AuditorId.Contains(e.AuditorId) && e.RequestSubmittedDate == null).Select(e => e.ScmId).Distinct().ToArray();

                        var data = (from SCMReq in db.jntuh_scmproceedingsrequests
                                    join ADDfaculty in db.jntuh_scmproceedingrequest_addfaculty on SCMReq.ID equals ADDfaculty.ScmProceedingId
                                    join Reg in db.jntuh_registered_faculty on ADDfaculty.RegistrationNumber.Trim() equals Reg.RegistrationNumber.Trim()
                                    join AUDASSIGN in db.jntuh_auditor_assigned on SCMReq.ID equals AUDASSIGN.ScmId
                                    join spec in db.jntuh_specialization on SCMReq.SpecializationId equals spec.id
                                    join Deg in db.jntuh_degree on SCMReq.DegreeId equals Deg.id
                                    join Dept in db.jntuh_department on SCMReq.DEpartmentId equals Dept.id
                                    join Desig in db.jntuh_designation on ADDfaculty.FacultyType equals Desig.id
                                    where AudiotAssignedSCMIds.Contains(SCMReq.ID) && !NomineeDataEntrySCmIds.Contains(SCMReq.ID) && ADDfaculty.FacultyType != 1 && AuditorId.Contains(AUDASSIGN.AuditorId) && Reg.isActive == true && SCMReq.CollegeId == collegeId && SCMReq.SpecializationId == specializationId
                                    select new ScmUploadedData()
                                    {
                                        SCMId = SCMReq.ID,
                                        SpecializationId = SCMReq.SpecializationId,
                                        Specialization = spec.specializationName,
                                        DepartmentId = SCMReq.DEpartmentId,
                                        DesignationId = Desig.id,
                                        DesignationName = Desig.designation,
                                        Department = Dept.departmentName,
                                        DegreeId = SCMReq.DegreeId,
                                        Degree = Deg.degree,
                                        FirstName = Reg.FirstName + "-" + Reg.LastName,
                                        RegistrationNumber = Reg.RegistrationNumber,
                                        Checked = false,
                                        CollegeId = SCMReq.CollegeId,
                                        AuditorId = AUDASSIGN.AuditorId,
                                        AId = AUDASSIGN.Id,
                                        Blacklist = Reg.Blacklistfaculy
                                    }).OrderBy(e => e.Department).ThenBy(e => e.DegreeId).ThenBy(e => e.SpecializationId).ThenBy(e => e.DesignationId).AsNoTracking().ToList();
                        ViewBag.SCMData = data.Count();
                        return View(data);
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    #region old Code
                    //if (collegeId != null && specializationId != null)
                //{
                //    int scmOrderId =jntuh_scmproceedingsrequests.Where(e =>e.ISActive == true && e.CollegeId == collegeId && e.SpecializationId == specializationId).Max(e => e.SCMOrder);

                // var NomineeDataEntrySCmIds =db.jntuh_auditors_dataentry.Where(e =>e.IsActive == true && e.CollegeId == collegeId && e.SpecializationId == specializationId)
                //            .Select(e => e.ScmProceedingId).Distinct().ToArray();



                //    var data = (from SCMReq in db.jntuh_scmproceedingsrequests
                //        join ADDfaculty in db.jntuh_scmproceedingrequest_addfaculty on SCMReq.ID equals ADDfaculty.ScmProceedingId
                //        join Reg in db.jntuh_registered_faculty on ADDfaculty.RegistrationNumber equals Reg.RegistrationNumber
                //      //  join spec in db.jntuh_specialization on SCMReq.SpecializationId equals spec.id
                //     //   join Deg in db.jntuh_degree on SCMReq.DegreeId equals Deg.id
                //      //  join Dept in db.jntuh_department on SCMReq.DEpartmentId equals Dept.id
                //                where SCMReq.CollegeId == collegeId && SCMReq.SpecializationId == specializationId && SCMReq.SCMOrder == scmOrderId && !NomineeDataEntrySCmIds.Contains(SCMReq.ID) && ADDfaculty.FacultyType!=1
                //                //ADDfaculty.IsApproved == true &&
                //        select new ScmUploadedData()
                //        {
                //            SCMId = SCMReq.ID,
                //            SpecializationId = SCMReq.SpecializationId,
                //          //  Specialization = Deg.degree + "-" + spec.specializationName,
                //            DepartmentId = SCMReq.DEpartmentId,
                //            DegreeId = SCMReq.DegreeId,
                //          //  Degree = Deg.degree,
                //            FirstName = Reg.FirstName + "-" + Reg.LastName,
                //            RegistrationNumber = Reg.RegistrationNumber,
                //            Checked = false,
                //            CollegeId = SCMReq.CollegeId
                //          // AuditorId=FFC.AuditorId
                //        }).ToList();
                //    ViewBag.SCMData = data.Count();
                //     return View(data);
                    //}
                    #endregion
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login","Admin");
            }
            }

        [HttpPost]
        public ActionResult Index(List<ScmUploadedData> scmdata, HttpPostedFileBase SCMHardcopy, int? DesignationId, string Remarks)
        {
            List<ScmUploadedData> scmcheckeddata = scmdata.ToList();
            int collegeId = 0;
            int specializationId = 0;
            int SCMId = 0;
            if (scmcheckeddata.Count() != 0 && User.Identity.Name != null && SCMHardcopy!=null)
            {
                List<int> SCMIds = new List<int>();
                collegeId = scmcheckeddata[0].CollegeId;
                specializationId = scmcheckeddata[0].SpecializationId;

               // var emailId =db.jntuh_registration.Where(e => e.Email == User.Identity.Name).Select(e => e.Email).FirstOrDefault();
                int userId = scmcheckeddata.GroupBy(e => e.AuditorId).Select(e => e.Key).FirstOrDefault();

                var AuditorAssignSCMIds = scmcheckeddata.GroupBy(e => e.SCMId).Select(e => e.Key).ToArray();
                SCMIds.AddRange(scmcheckeddata.GroupBy(e => e.SCMId).Select(e => e.Key).ToArray());

                    //db.jntuh_ffc_auditor.Where(e => e.isActive == true && e.auditorEmail1 == emailId).Select(e => e.id).FirstOrDefault();
              //  var AuditorsIds =db.jntuh_auditor_assigned.Where(e => e.ScmId == scmdata[0].SCMId).Select(e => e.AuditorId).Distinct().ToArray();
                if (userId != 0)
                {
                   

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






                    
                var fileName = string.Empty;
                var filepath = string.Empty;
                var collegescmrequests = new jntuh_scmproceedingsrequests();
                const string scmnotificationpath = "~/Content/Upload/SCMPROCEEDINGSREQUEST/UploadSCMHardCopy";
                if (SCMHardcopy!=null)
                 {
                        if (!Directory.Exists(Server.MapPath(scmnotificationpath)))
                        {
                            Directory.CreateDirectory(Server.MapPath(scmnotificationpath));
                        }

                        var ext = Path.GetExtension(SCMHardcopy.FileName);
                        if (ext != null)
                        {
                            var scmfileName = scmcheckeddata[0].CollegeId + "_" + "UploadSCMHardCopy" + "_" + DateTime.Now.ToString("yyyMMddHHmmss");
                            SCMHardcopy.SaveAs(string.Format("{0}/{1}{2}", Server.MapPath(scmnotificationpath), scmfileName, ext));
                            fileName = scmfileName + ext;
                        }
                    }

                    foreach (var item in scmcheckeddata)
                    {
                        if (item != null)
                        {
                            jntuh_auditors_dataentry dataentry = new jntuh_auditors_dataentry();
                            dataentry.CollegeId = item.CollegeId;
                            dataentry.SpecializationId = item.SpecializationId;
                            dataentry.DepartmentId = item.DepartmentId;
                            dataentry.DegreeId = item.DegreeId;
                            dataentry.DesignationId = (int) (DesignationId ?? 0);
                            dataentry.RegistrationNo = item.RegistrationNumber;
                            dataentry.AuditorId = userId;
                            dataentry.IsSelected = item.Checked;
                            dataentry.IsActive = true;
                            dataentry.CreatedBy = userId;
                            dataentry.NomineeRemarks = Remarks;
                            dataentry.CreatedOn = DateTime.Now;
                            dataentry.SCMhardcopy = fileName;
                            dataentry.ScmProceedingId = item.SCMId;
                            dataentry.DesignationId = item.DesignationId;
                            SCMId = item.SCMId;
                           
                            db.jntuh_auditors_dataentry.Add(dataentry);
                        }
                    }
                    db.SaveChanges();
                    var jntuh_auditor_assign = db.jntuh_auditor_assigned.AsNoTracking().ToList();
                    if (AuditorAssignSCMIds.Count() != 0)
                    {
                        foreach (var adata in AuditorAssignSCMIds)
                        {

                            var AuditoeAssigneddata = db.jntuh_auditor_assigned.Where(e => e.ScmId == adata && e.CollegeId == collegeId).ToList();
                           if (AuditoeAssigneddata.Count() != 0)
                            {
                               AuditoeAssigneddata.ForEach(e =>
                               {
                                   e.RequestSubmittedDate = DateTime.Now;
                                   e.UpdatedBy = userId;
                                   e.UpdatedOn = DateTime.Now;
                               });

                               // var AuditorAssigndata = db.jntuh_auditor_assigned.Find(Aid);
                               // AuditorAssigndata.RequestSubmittedDate = DateTime.Now;
                               // AuditorAssigndata.UpdatedBy = userId;
                               // AuditorAssigndata.UpdatedOn = DateTime.Now;
                               //db.Entry(AuditorAssigndata).State=EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }







                    TempData["Success"] = "Information Saved Successfully";
                   
                  //  return RedirectToAction("Index", "User",new {collegeId = collegeId, specializationId = specializationId});
                    return RedirectToAction("ApprovedFacultyBasedOnNominee", "Admin", new { SCMId = scmIds });
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult DeptWiseApprovedFaculty(int? collegeId)
        {
            if (User.Identity.IsAuthenticated)
            {

                var collegeIds = db.jntuh_auditors_dataentry.Where(e => e.IsActive == true && e.SpecializationId!=0 && e.DepartmentId!=0 && e.DegreeId!=0).Select(e => e.CollegeId).Distinct().ToArray();
                ViewBag.Colleges =db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id)).Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName }).OrderBy(e => e.collegeName).ToList();

                if (collegeId != null)
                {

                    #region
                    //    var specs = new List<DistinctSpecializations>();
                //    var degrees = db.jntuh_degree.AsNoTracking().ToList();
                //    var specializations = db.jntuh_specialization.AsNoTracking().ToList();
                //    var departments = db.jntuh_department.AsNoTracking().ToList();

                //    List<int> collegespecs = new List<int>();
                //    collegespecs.AddRange(
                //        db.jntuh_college_intake_existing.Where(i => i.collegeId == collegeId)
                //            .Select(i => i.specializationId)
                //            .Distinct()
                //            .ToArray());

                //    foreach (var s in collegespecs)
                //    {
                //        var specid = specializations.FirstOrDefault(i => i.id == s);

                //        if (specid != null)
                //        {
                //            var deptment = departments.FirstOrDefault(i => i.id == specid.departmentId);
                //            var degree = degrees.FirstOrDefault(i => i.id == (deptment != null ? deptment.degreeId : 0));
                //            if (degree != null)
                //                specs.Add(new DistinctSpecializations
                //                {
                //                    SpecializationId = specid.id,
                //                    SpecializationName = degree.degree + " - " + specid.specializationName,
                //                    DepartmentId = specid.departmentId,
                //                    DepartmentName = specid.jntuh_department.departmentName
                //                });
                //        }
                //    }

                //    ViewBag.departments = specs.OrderBy(i => i.DepartmentName);
                //}
                //if (collegeId != null && specializationId != null)
                    //{
                    #endregion

                    List<ScmUploadedData> scmUploaded=new List<ScmUploadedData>();
                    int AId = 0;
                    var jntuh_ffc_auditor = db.jntuh_ffc_auditor.Where(e => e.isActive == true).Select(e => e).ToList();
                    var jntuh_auditors_dataentry =db.jntuh_auditors_dataentry.Where(e => e.IsActive == true).Select(e => e).ToList();
                

                   // var data_entry =jntuh_auditors_dataentry.Where(e =>e.IsActive == true && e.CollegeId == collegeId && e.SpecializationId == specializationId).Select(e => e.IsSelected).FirstOrDefault();
                    var data = (from SCMReq in db.jntuh_scmproceedingsrequests
                                join ADDfaculty in db.jntuh_scmproceedingrequest_addfaculty on SCMReq.ID equals ADDfaculty.ScmProceedingId
                                join Reg in db.jntuh_registered_faculty on ADDfaculty.RegistrationNumber equals Reg.RegistrationNumber
                                join spec in db.jntuh_specialization on SCMReq.SpecializationId equals spec.id
                                join Deg in db.jntuh_degree on SCMReq.DegreeId equals Deg.id
                                join Dept in db.jntuh_department on SCMReq.DEpartmentId equals Dept.id
                                join AUD in db.jntuh_auditor_assigned on SCMReq.ID equals AUD.ScmId
                                join DESG in db.jntuh_designation on ADDfaculty.FacultyType equals DESG.id
                                where SCMReq.CollegeId == collegeId 
                                //&& SCMReq.SpecializationId == specializationId
                                select new ScmUploadedData()
                                {
                                    SCMId = SCMReq.ID,
                                    SpecializationId = SCMReq.SpecializationId,
                                    Specialization = Deg.degree + "-" + spec.specializationName,
                                    DepartmentId = SCMReq.DEpartmentId,
                                    DegreeId = SCMReq.DegreeId,
                                    Degree = Deg.degree,
                                    FirstName = Reg.FirstName + "-" + Reg.LastName,
                                    RegistrationNumber = Reg.RegistrationNumber,
                                    CollegeId = SCMReq.CollegeId,
                                    AuditorId = AUD.AuditorId,
                                    DesignationId = (int)ADDfaculty.FacultyType,
                                    DesignationName = DESG.designation
                                    //jntuh_auditor_assigned.Where(e => e.ScmId == SCMReq.ID).Select(e => e.AuditorId).FirstOrDefault(),
                                    //AId = jntuh_auditor_assigned.Where(e => e.ScmId == SCMReq.ID).Select(e => e.AuditorId).FirstOrDefault(),
                                 //  Checked = jntuh_auditors_dataentry.Where(e => e.IsActive == true && e.CollegeId == collegeId && e.SpecializationId == specializationId && e.RegistrationNo == ADDfaculty.RegistrationNumber).Select(e => e.IsSelected).FirstOrDefault() != null ? true : false,//&& e.AuditorId == AId
                                   // AuditorName = jntuh_ffc_auditor.Where(e => e.id == AId).Select(e => e.auditorName).FirstOrDefault() != null ? jntuh_ffc_auditor.Where(e => e.id == AId).Select(e => e.auditorName).FirstOrDefault() : string.Empty
                                }).Distinct().ToList();
                    foreach (var item in data)
                    {
                        item.AuditorName =jntuh_ffc_auditor.Where(e => e.id == item.AuditorId).Select(e => e.auditorName).FirstOrDefault();
                        item.Checked =jntuh_auditors_dataentry.Where(e =>e.CollegeId == item.CollegeId && e.SpecializationId == item.SpecializationId &&e.RegistrationNo == item.RegistrationNumber).Select(e => e.IsSelected).FirstOrDefault() != null? true: false;
                        item.SCMhardcopyview =jntuh_auditors_dataentry.Where(e =>e.CollegeId == item.CollegeId && e.SpecializationId == item.SpecializationId &&e.RegistrationNo == item.RegistrationNumber).Select(e => e.SCMhardcopy).FirstOrDefault();
                        scmUploaded.Add(item);
                    }


                    var data1 = (from DataEntry in db.jntuh_auditors_dataentry join REg in db.jntuh_registered_faculty on DataEntry.RegistrationNo equals REg.RegistrationNumber
                        join spec in db.jntuh_specialization on DataEntry.SpecializationId equals spec.id
                        join Deg in db.jntuh_degree on DataEntry.DegreeId equals Deg.id
                        join Dept in db.jntuh_department on DataEntry.DegreeId equals Dept.id
                      //  join AUD in db.jntuh_auditor_assigned on DataEntry.ScmProceedingId equals AUD.ScmId
                        join AUDDATA in db.jntuh_ffc_auditor on DataEntry.AuditorId equals AUDDATA.id
                        // join DESG in db.jntuh_designation on DataEntry.DesignationId equals DESG.id
                        where DataEntry.CollegeId == collegeId
                        select new ScmUploadedData()
                        {
                            SCMId = DataEntry.ScmProceedingId,
                            SpecializationId = DataEntry.SpecializationId,
                            Specialization = Deg.degree + "-" + spec.specializationName,
                            DepartmentId = DataEntry.DepartmentId,
                            DegreeId = DataEntry.DegreeId,
                            Degree = Deg.degree,
                            FirstName = REg.FirstName + "-" + REg.LastName,
                            RegistrationNumber = REg.RegistrationNumber,
                            CollegeId = DataEntry.CollegeId,
                            AuditorId = DataEntry.AuditorId,
                            AuditorName = AUDDATA.auditorName,
                            Checked = DataEntry.IsSelected!=null?true:false,
                            SCMhardcopyview = DataEntry.SCMhardcopy
                           // DesignationId = (int)ADDfaculty.FacultyType,
                            //DesignationName = DESG.designation
                        }).Distinct().ToList();







                    ViewBag.SCMData = data1;
                    return View(data1.Where(e => e.Checked == true).Select(e => e).ToList());
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }


        [HttpGet]
        public ActionResult PrincipalNomineeDataEntry(int? collegeId)
        {
            if (User.Identity.IsAuthenticated)
            {

                var AuditorId = db.jntuh_ffc_auditor.Where(e => e.isActive == true && e.auditorEmail1 == User.Identity.Name && e.auditorDepartmentID==60).Select(e => e.id).Distinct().ToArray();
                var SCMIDs = db.jntuh_auditor_assigned.Where(e => e.IsActive == true && AuditorId.Contains(e.AuditorId) && e.RequestSubmittedDate == null).Select(e => e.ScmId).Distinct().ToArray();

                var collegeIds = db.jntuh_scmproceedingsrequests.Where(e => e.ISActive == true && e.SpecializationId == 0 && e.DEpartmentId == 0 && e.DegreeId == 0 && SCMIDs.Contains(e.ID)).Select(e => e.CollegeId).Distinct().ToArray();
                ViewBag.Colleges =db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id)).Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName }).OrderBy(e => e.collegeName).ToList();
                if (collegeId != null)
                {
                    var jntuh_auditors_dataentry = db.jntuh_auditors_dataentry.Where(e => e.IsActive == true).Select(e => e).ToList();
                    var PrincialSCMIDs =jntuh_auditors_dataentry.Where(e =>e.CollegeId == collegeId && e.SpecializationId == 0 && e.DepartmentId == 0 &&
                                e.DegreeId == 0).Select(e => e.ScmProceedingId).Distinct().ToArray();
                    var data1 = (from SCMReq in db.jntuh_scmproceedingsrequests
                                 join ADDfaculty in db.jntuh_scmproceedingrequest_addfaculty on SCMReq.ID equals ADDfaculty.ScmProceedingId
                                 join Reg in db.jntuh_registered_faculty on ADDfaculty.RegistrationNumber equals Reg.RegistrationNumber
                                 where SCMReq.CollegeId == collegeId && SCMReq.SpecializationId == 0 && ADDfaculty.FacultyType == 0 && !PrincialSCMIDs.Contains(SCMReq.ID) && Reg.Blacklistfaculy!=true
                                 select new ScmUploadedData()
                                 {
                                     SCMId = SCMReq.ID,
                                     SpecializationId = SCMReq.SpecializationId,
                                     FirstName = Reg.FirstName + "-" + Reg.LastName,
                                     RegistrationNumber = Reg.RegistrationNumber,
                                     Checked = false,
                                     CollegeId = SCMReq.CollegeId
                                 }).ToList();

                    ViewBag.SCMData = data1.Count();

                    List<ScmUploadedData> scmUploaded = new List<ScmUploadedData>();
                    var jntuh_ffc_auditor = db.jntuh_ffc_auditor.Where(e => e.isActive == true).Select(e => e).ToList();
                  


                    //Nominee Approved Faculty List
                    var nomineeApprovedFacultyData = (from SCMReq in db.jntuh_scmproceedingsrequests
                                join ADDfaculty in db.jntuh_scmproceedingrequest_addfaculty on SCMReq.ID equals ADDfaculty.ScmProceedingId
                                join Reg in db.jntuh_registered_faculty on ADDfaculty.RegistrationNumber equals Reg.RegistrationNumber
                                join AUD in db.jntuh_auditor_assigned on SCMReq.ID equals AUD.ScmId
                                where SCMReq.CollegeId == collegeId && SCMReq.SpecializationId == 0 && SCMReq.DEpartmentId == 0 && SCMReq.DegreeId == 0
                                select new ScmUploadedData()
                                {
                                    SCMId = SCMReq.ID,
                                    SpecializationId = SCMReq.SpecializationId,
                                    DepartmentId = SCMReq.DEpartmentId,
                                    DegreeId = SCMReq.DegreeId,
                                    FirstName = Reg.FirstName + "-" + Reg.LastName,
                                    RegistrationNumber = Reg.RegistrationNumber,
                                    CollegeId = SCMReq.CollegeId,
                                    AuditorId = AUD.AuditorId
                                }).ToList();


                    foreach (var item in nomineeApprovedFacultyData)
                    {
                        item.AuditorName = jntuh_ffc_auditor.Where(e => e.id == item.AuditorId).Select(e => e.auditorName).FirstOrDefault();
                        item.Checked = jntuh_auditors_dataentry.Where(e => e.CollegeId == item.CollegeId && e.SpecializationId == 0 && e.DepartmentId == 0 && e.DegreeId == 0 &&e.RegistrationNo == item.RegistrationNumber && e.ScmProceedingId==item.SCMId).Select(e => e.IsSelected)
                                .FirstOrDefault() != false? true: false;
                        item.SCMhardcopyview = jntuh_auditors_dataentry.Where(e => e.CollegeId == item.CollegeId && e.SpecializationId == 0 && e.DepartmentId == 0 && e.DegreeId == 0 && e.RegistrationNo == item.RegistrationNumber && e.ScmProceedingId == item.SCMId).Select(e => e.SCMhardcopy).FirstOrDefault();
                        scmUploaded.Add(item);
                    }

                  //  ViewBag.SCMData = scmUploaded;
                    ViewBag.NomineeApprovedFaculty = scmUploaded.ToList();


                    return View(data1);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login","Admin");
            }
        }

        [HttpPost]
        public ActionResult PrincipalNomineeDataEntry(List<ScmUploadedData> scmdata, HttpPostedFileBase SCMHardcopy, string Remarks)
        {
            List<ScmUploadedData> scmcheckeddata = scmdata.ToList();
            int collegeId = 0;
            int SCMId = 0;
            if (scmcheckeddata.Count() != 0 && User.Identity.Name != null && SCMHardcopy != null)
            {
                collegeId = scmcheckeddata[0].CollegeId;
                var emailId = db.jntuh_registration.Where(e => e.Email == User.Identity.Name).Select(e => e.Email).FirstOrDefault();
                int userId = db.jntuh_ffc_auditor.Where(e => e.isActive == true && e.auditorEmail1 == emailId && e.auditorDepartmentID==60).Select(e => e.id).FirstOrDefault();
                if (userId != 0)
                {

                    var fileName = string.Empty;
                    var filepath = string.Empty;
                    var collegescmrequests = new jntuh_scmproceedingsrequests();
                    const string scmnotificationpath = "~/Content/Upload/SCMPROCEEDINGSREQUEST/UploadSCMHardCopy";
                    if (SCMHardcopy != null)
                    {
                        if (!Directory.Exists(Server.MapPath(scmnotificationpath)))
                        {
                            Directory.CreateDirectory(Server.MapPath(scmnotificationpath));
                        }

                        var ext = Path.GetExtension(SCMHardcopy.FileName);
                        if (ext != null)
                        {
                            var scmfileName = scmcheckeddata[0].CollegeId + "_" + "UploadSCMHardCopyForPrincipal" + "_" + DateTime.Now.ToString("yyyMMddHHmmss");
                            SCMHardcopy.SaveAs(string.Format("{0}/{1}{2}", Server.MapPath(scmnotificationpath), scmfileName, ext));
                            fileName = scmfileName + ext;
                        }
                    }

                    foreach (var item in scmcheckeddata)
                    {
                        if (item != null)
                        {
                            jntuh_auditors_dataentry dataentry = new jntuh_auditors_dataentry();
                            dataentry.CollegeId = item.CollegeId;
                            dataentry.SpecializationId = 0;
                            dataentry.DepartmentId = 0;
                            dataentry.DegreeId = 0;
                            dataentry.DesignationId = 0;
                            dataentry.RegistrationNo = item.RegistrationNumber;
                            dataentry.AuditorId = userId;
                            dataentry.IsSelected = item.Checked;
                            dataentry.IsActive = true;
                            dataentry.NomineeRemarks = Remarks;
                            dataentry.CreatedBy = userId;
                            dataentry.CreatedOn = DateTime.Now;
                            dataentry.SCMhardcopy = fileName;
                            dataentry.ScmProceedingId = item.SCMId;
                            SCMId = item.SCMId;
                            db.jntuh_auditors_dataentry.Add(dataentry);
                        }
                    }
                    TempData["Success"] = "Information Saved Successfully";
                    db.SaveChanges();

                    var AuditorAssignedData =db.jntuh_auditor_assigned.Where(e => e.AuditorId == userId && e.ScmId == SCMId).Select(e => e).FirstOrDefault();
                    if (AuditorAssignedData != null)
                    {
                        AuditorAssignedData.RequestSubmittedDate = DateTime.Now;
                        AuditorAssignedData.UpdatedBy = userId;
                        AuditorAssignedData.UpdatedOn = DateTime.Now;
                        db.Entry(AuditorAssignedData).State=EntityState.Modified;
                        db.SaveChanges();
                    }



                    // return RedirectToAction("PrincipalNomineeDataEntry", "User", new { collegeId = collegeId });
                    return RedirectToAction("ApprovedFacultyBasedOnNominee", "Admin", new { SCMId = SCMId });
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult PrincipalSelectedView(int? collegeId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var collegeIds =db.jntuh_auditors_dataentry.Where(e => e.IsActive == true && e.SpecializationId == 0 && e.DepartmentId == 0 && e.DegreeId == 0).Select(e => e.CollegeId).Distinct().ToArray();
                ViewBag.Colleges =db.jntuh_college.Where(e => e.isActive == true && collegeIds.Contains(e.id)).Select(e => new { collegeId = e.id, collegeName = e.collegeCode + "-" + e.collegeName }).OrderBy(e => e.collegeName).ToList();

                if (collegeId != null)
                {
                    List<ScmUploadedData> scmUploaded = new List<ScmUploadedData>();
                    int AId = 0;
                    var jntuh_ffc_auditor = db.jntuh_ffc_auditor.Where(e => e.isActive == true).Select(e => e).ToList();
                    var jntuh_auditors_dataentry = db.jntuh_auditors_dataentry.Where(e => e.IsActive == true).Select(e => e).ToList();
                   

                    var data = (from SCMReq in db.jntuh_scmproceedingsrequests
                                join ADDfaculty in db.jntuh_scmproceedingrequest_addfaculty on SCMReq.ID equals ADDfaculty.ScmProceedingId
                                join Reg in db.jntuh_registered_faculty on ADDfaculty.RegistrationNumber equals Reg.RegistrationNumber
                                join AUD in db.jntuh_auditor_assigned on SCMReq.ID equals AUD.ScmId
                                where SCMReq.CollegeId == collegeId && SCMReq.SpecializationId == 0 && SCMReq.DEpartmentId == 0 && SCMReq.DegreeId == 0 
                                select new ScmUploadedData()
                                {
                                    SCMId = SCMReq.ID,
                                    SpecializationId = SCMReq.SpecializationId,
                                    DepartmentId = SCMReq.DEpartmentId,
                                    DegreeId = SCMReq.DegreeId,
                                    FirstName = Reg.FirstName + "-" + Reg.LastName,
                                    RegistrationNumber = Reg.RegistrationNumber,
                                    CollegeId = SCMReq.CollegeId,
                                    AuditorId = AUD.AuditorId
                                }).Distinct().ToList();


                    foreach (var item in data)
                    {
                        item.AuditorName =jntuh_ffc_auditor.Where(e => e.id == item.AuditorId).Select(e => e.auditorName).FirstOrDefault();
                        item.Checked =jntuh_auditors_dataentry.Where(e =>e.CollegeId == item.CollegeId && e.SpecializationId == 0 && e.DepartmentId==0 && e.DegreeId==0 &&
                                    e.RegistrationNo == item.RegistrationNumber)
                                .Select(e => e.IsSelected)
                                .FirstOrDefault() != null
                                ? true
                                : false;
                        item.SCMhardcopyview = jntuh_auditors_dataentry.Where(
                            e =>
                                e.CollegeId == item.CollegeId && e.SpecializationId == 0 && e.DepartmentId == 0 && e.DegreeId == 0 &&
                                e.RegistrationNo == item.RegistrationNumber)
                            .Select(e => e.SCMhardcopy)
                            .FirstOrDefault();
                        scmUploaded.Add(item);
                    }



                    ViewBag.SCMData = scmUploaded;
                    return View(scmUploaded.Where(e => e.Checked == true).Select(e => e).ToList());
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }


        [HttpGet]
        public ActionResult showSCMmenuforUser()
        {
             if (User.Identity.IsAuthenticated)
            {
            return View();
            }
             else
             {
                 return RedirectToAction("Login", "Admin");
             }
        }

    }

    


    public class ScmUploadedData
    {
        public int SCMId { get; set; }
        public int SpecializationId { get; set; }
        public string Specialization { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int DegreeId { get; set; }
        public string Degree { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegistrationNumber { get; set; }
        public bool Checked { get; set; }
        public int AuditorId { get; set; }
        public string AuditorName { get; set; }
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
        public HttpPostedFileBase ScmHardCopyBase { get; set; }
        public int AId { get; set; }

        public int DesignationId { get; set; }
        public string DesignationName { get; set; }

        public string SCMhardcopyview { get; set; }
        public string strSCMDate { get; set; }
        public DateTime? SCMDate { get; set; }
        public int facultyId { get; set; }
        public int ScmfacultyaddedId { get; set; }
        public bool? Approved { get; set; }
        public string Remarks { get; set; }
        public bool? Blacklist { get; set; }
        public bool? IsApproved { get; set; }
    }
}