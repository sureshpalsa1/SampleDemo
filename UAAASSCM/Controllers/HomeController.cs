using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UAAAS.Models;
using UAAASSCM.Models;
namespace UAAASSCM.Controllers
{
    public class HomeController : Controller
    {
        SCMEntities db=new SCMEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpGet]
        [AuthorizedUserAccess("Admin")]
        public ActionResult EmailChange()
        {
            if (User.Identity.IsAuthenticated)
            //if (User.Identity.Name == "admin")
            {
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
        }
        // Added by Naushad Khan
        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public ActionResult EmailChange(string OldEmail, string NewEmail)
        {
            if (User.Identity.IsAuthenticated)
           // if (User.Identity.Name == "admin")
            {
                if (OldEmail != null && OldEmail != "" && NewEmail != null && NewEmail != "")
                {

                    jntuh_registration registrationEmail =
                        db.jntuh_registration.Where(f => f.Email == OldEmail).FirstOrDefault();
                    jntuh_ffc_auditor auditoremail =
                        db.jntuh_ffc_auditor.Where(f => f.auditorEmail1 == OldEmail).FirstOrDefault();


                    if (registrationEmail != null && auditoremail != null)
                    {
                        registrationEmail.Email = NewEmail;
                        registrationEmail.UpdateOn = DateTime.Now;
                        registrationEmail.Updatedby = 1;
                        db.Entry(registrationEmail).State = EntityState.Modified;
                        db.SaveChanges();


                        auditoremail.auditorEmail1 = NewEmail;
                        auditoremail.updatedOn = DateTime.Now;
                        auditoremail.updatedBy = 1;
                        db.Entry(auditoremail).State = EntityState.Modified;
                        db.SaveChanges();


                        TempData["Success"] = "Email Address Updated Successfully.";

                        return RedirectToAction("EmailChange", "Home");
                        //return View();
                    }
                    else
                    {
                        TempData["Error"] = "Current Email Address Does not Exist";
                        return View();

                    }
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
        [AuthorizedUserAccess("Admin")]
        public ActionResult FindRegistrationNumber()
        {
            return View();
        }
        
        [HttpPost]
        [AuthorizedUserAccess("Admin")]
        public ActionResult FindRegistrationNumber(FacultyRegistration regno)
        {
            FacultyRegistration regFaculty = new FacultyRegistration();
            int fID = 0;

            if (regno != null)
            {
                regFaculty.GenderId = null;
                regFaculty.isFacultyRatifiedByJNTU = null;

                var faculty =
                    db.jntuh_registered_faculty.FirstOrDefault(
                        e => e.RegistrationNumber.Trim() == regno.RegistrationNumber.Trim());

                if (faculty != null)
                {
                    regFaculty.id = fID = faculty.id;
                    ViewBag.FacultyID = fID;
                    regFaculty.Type = faculty.type;
                    regFaculty.RegistrationNumber = faculty.RegistrationNumber;
                    regFaculty.UserName =
                        db.my_aspnet_users.Where(u => u.id == faculty.UserId).Select(u => u.name).FirstOrDefault();
                    regFaculty.Email = faculty.Email;
                    regFaculty.UniqueID = faculty.UniqueID;
                    regFaculty.FirstName = faculty.FirstName;
                    regFaculty.MiddleName = faculty.MiddleName;
                    regFaculty.LastName = faculty.LastName;
                    regFaculty.FatherOrhusbandName = faculty.FatherOrHusbandName;
                    regFaculty.MotherName = faculty.MotherName;
                    regFaculty.GenderId = faculty.GenderId;
                    regFaculty.facultyDateOfBirth = Utilities.MMDDYY2DDMMYY(faculty.DateOfBirth.ToString());
                    regFaculty.Mobile = faculty.Mobile;
                    regFaculty.facultyPhoto = faculty.Photo;
                    regFaculty.PANNumber = faculty.PANNumber;
                    regFaculty.facultyPANCardDocument = faculty.PANDocument;
                    regFaculty.AadhaarNumber = faculty.AadhaarNumber;
                    regFaculty.facultyAadhaarCardDocument = faculty.AadhaarDocument;
                    regFaculty.WorkingStatus = faculty.WorkingStatus;
                    regFaculty.TotalExperience = faculty.TotalExperience;
                    regFaculty.OrganizationName = faculty.OrganizationName;
                    if (faculty.collegeId != null)
                    {
                        regFaculty.CollegeName = db.jntuh_college.Find(faculty.collegeId).collegeName;
                    }
                    regFaculty.CollegeId = faculty.collegeId;
                    if (faculty.DepartmentId != null)
                    {
                        regFaculty.department = db.jntuh_department.Find(faculty.DepartmentId).departmentName;
                    }
                    regFaculty.DepartmentId = faculty.DepartmentId;
                    regFaculty.OtherDepartment = faculty.OtherDepartment;

                    if (faculty.DesignationId != null)
                    {
                        regFaculty.designation = db.jntuh_designation.Find(faculty.DesignationId).designation;
                    }
                    regFaculty.DesignationId = faculty.DesignationId;
                    regFaculty.OtherDesignation = faculty.OtherDesignation;

                    if (faculty.DateOfAppointment != null)
                    {
                        regFaculty.facultyDateOfAppointment =
                            UAAAS.Models.Utilities.MMDDYY2DDMMYY(faculty.DateOfAppointment.ToString());
                    }
                    regFaculty.TotalExperiencePresentCollege = faculty.TotalExperiencePresentCollege;
                    regFaculty.isFacultyRatifiedByJNTU = faculty.isFacultyRatifiedByJNTU;
                    if (faculty.DateOfRatification != null)
                    {
                        regFaculty.facultyDateOfRatification =
                            UAAAS.Models.Utilities.MMDDYY2DDMMYY(faculty.DateOfRatification.ToString());
                    }
                    regFaculty.ProceedingsNo = faculty.ProceedingsNumber;
                    regFaculty.SelectionCommitteeProcedings = faculty.ProceedingDocument;
                    regFaculty.AICTEFacultyId = faculty.AICTEFacultyId;
                    regFaculty.GrossSalary = faculty.grosssalary;
                    regFaculty.National = faculty.National;
                    regFaculty.InterNational = faculty.InterNational;
                    regFaculty.Citation = faculty.Citation;
                    regFaculty.Awards = faculty.Awards;
                    regFaculty.isActive = faculty.isActive;
                    regFaculty.isApproved = faculty.isApproved;
                    regFaculty.isView = true;
                    regFaculty.DeactivationReason = faculty.DeactivationReason;
                }

                else
                {
                    TempData["Error"] = "Registration Number Not Found...";
                    return RedirectToAction("FindRegistrationNumber");
                }



                string registrationNumber =
                    db.jntuh_registered_faculty.Where(of => of.id == fID)
                        .Select(of => of.RegistrationNumber)
                        .FirstOrDefault();
                int facultyId =
                    db.jntuh_college_faculty_registered.Where(of => of.RegistrationNumber == registrationNumber)
                        .Select(of => of.id)
                        .FirstOrDefault();
                int[] verificationOfficers =
                    db.jntuh_college_faculty_verified.Where(v => v.FacultyId == facultyId)
                        .Select(v => v.VerificationOfficer)
                        .Distinct()
                        .ToArray();
              //  int userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);

                ViewBag.FacultyDetails = regFaculty;
                TempData["FacultyDetails"] = regFaculty;
                ViewBag.HideVerifyLink = regFaculty.isApproved != null ? true : false;
                string RegistrationNo = regno.RegistrationNumber.ToString();
                //return RedirectToAction("FacultyView", regFaculty);
                return RedirectToAction("FacultyView", new { REGNO = RegistrationNo });
            }

            return View();
        }
        
         [AuthorizedUserAccess("Admin")]
        public ActionResult FacultyView(string REGNO)
        {

            FacultyRegistration regFaculty = new FacultyRegistration();

            if (REGNO != null)
            {
                regFaculty.GenderId = null;
                regFaculty.isFacultyRatifiedByJNTU = null;

                var faculty =db.jntuh_registered_faculty.FirstOrDefault(e => e.RegistrationNumber.Trim() == REGNO.Trim());

                if (faculty != null)
                {
                    regFaculty.id = faculty.id;
                    ViewBag.FacultyID = faculty.id;
                    regFaculty.Type = faculty.type;
                    regFaculty.RegistrationNumber = faculty.RegistrationNumber;
                    regFaculty.UserName =db.my_aspnet_users.Where(u => u.id == faculty.UserId).Select(u => u.name).FirstOrDefault();
                    regFaculty.Email = faculty.Email;
                    regFaculty.UniqueID = faculty.UniqueID;
                    regFaculty.FirstName = faculty.FirstName;
                    regFaculty.MiddleName = faculty.MiddleName;
                    regFaculty.LastName = faculty.LastName;
                    regFaculty.FatherOrhusbandName = faculty.FatherOrHusbandName;
                    regFaculty.MotherName = faculty.MotherName;
                    regFaculty.GenderId = faculty.GenderId;
                    regFaculty.facultyDateOfBirth = Utilities.MMDDYY2DDMMYY(faculty.DateOfBirth.ToString());
                    regFaculty.Mobile = faculty.Mobile;
                    regFaculty.facultyPhoto = faculty.Photo;
                    regFaculty.PANNumber = faculty.PANNumber;
                    regFaculty.facultyPANCardDocument = faculty.PANDocument;
                    regFaculty.AadhaarNumber = faculty.AadhaarNumber;
                    regFaculty.facultyAadhaarCardDocument = faculty.AadhaarDocument;
                    regFaculty.WorkingStatus = faculty.WorkingStatus;
                    regFaculty.TotalExperience = faculty.TotalExperience;
                    regFaculty.OrganizationName = faculty.OrganizationName;
                    if (faculty.collegeId != null)
                    {
                        regFaculty.CollegeName = db.jntuh_college.Find(faculty.collegeId).collegeName;
                    }
                    regFaculty.CollegeId = faculty.collegeId;
                    if (faculty.DepartmentId != null)
                    {
                        regFaculty.department = db.jntuh_department.Find(faculty.DepartmentId).departmentName;
                    }
                    regFaculty.DepartmentId = faculty.DepartmentId;
                    regFaculty.OtherDepartment = faculty.OtherDepartment;

                    if (faculty.DesignationId != null)
                    {
                        regFaculty.designation = db.jntuh_designation.Find(faculty.DesignationId).designation;
                    }
                    regFaculty.DesignationId = faculty.DesignationId;
                    regFaculty.OtherDesignation = faculty.OtherDesignation;

                    if (faculty.DateOfAppointment != null)
                    {
                        regFaculty.facultyDateOfAppointment =
                            UAAAS.Models.Utilities.MMDDYY2DDMMYY(faculty.DateOfAppointment.ToString());
                    }
                    regFaculty.TotalExperiencePresentCollege = faculty.TotalExperiencePresentCollege;
                    regFaculty.isFacultyRatifiedByJNTU = faculty.isFacultyRatifiedByJNTU;
                    if (faculty.DateOfRatification != null)
                    {
                        regFaculty.facultyDateOfRatification =
                            UAAAS.Models.Utilities.MMDDYY2DDMMYY(faculty.DateOfRatification.ToString());
                    }
                    regFaculty.ProceedingsNo = faculty.ProceedingsNumber;
                    regFaculty.SelectionCommitteeProcedings = faculty.ProceedingDocument;
                    regFaculty.AICTEFacultyId = faculty.AICTEFacultyId;
                    regFaculty.GrossSalary = faculty.grosssalary;
                    regFaculty.National = faculty.National;
                    regFaculty.InterNational = faculty.InterNational;
                    regFaculty.Citation = faculty.Citation;
                    regFaculty.Awards = faculty.Awards;
                    regFaculty.isActive = faculty.isActive;
                    regFaculty.isApproved = faculty.isApproved;
                    regFaculty.isView = true;
                    regFaculty.DeactivationReason = faculty.DeactivationReason;

                    regFaculty.PresentInstituteAssignedRole = faculty.PresentInstituteAssignedRole;
                    regFaculty.PresentInstituteAssignedResponsebility = faculty.PresentInstituteAssignedResponsebility;
                    regFaculty.Accomplish1 = faculty.Accomplish1;
                    regFaculty.Accomplish2 = faculty.Accomplish2;
                    regFaculty.Accomplish3 = faculty.Accomplish3;
                    regFaculty.Accomplish4 = faculty.Accomplish4;
                    regFaculty.Accomplish5 = faculty.Accomplish5;
                    regFaculty.Professional = faculty.Professional;
                    regFaculty.Professional2 = faculty.Professional2;
                    regFaculty.Professiona3 = faculty.Professiona3;
                    regFaculty.MembershipNo1 = faculty.MembershipNo1;
                    regFaculty.MembershipNo2 = faculty.MembershipNo2;
                    regFaculty.MembershipNo3 = faculty.MembershipNo3;
                    regFaculty.MembershipCertificate1 = faculty.MembershipCertificate1;
                    regFaculty.MembershipCertificate2 = faculty.MembershipCertificate2;
                    regFaculty.MembershipCertificate3 = faculty.MembershipCertificate3;
                    regFaculty.AdjunctDepartment = faculty.AdjunctDepartment;
                    regFaculty.AdjunctDesignation = faculty.AdjunctDesignation;
                    regFaculty.WorkingType = faculty.WorkingType;
                    regFaculty.NOCFile = faculty.NOCFile;
                    regFaculty.BlacklistFaculty = (bool)faculty.Blacklistfaculy;





                }

                regFaculty.FacultyEducation =db.jntuh_education_category.Where(e => e.isActive == true && (e.id == 1 || e.id == 3 || e.id == 4 || e.id == 5 || e.id == 6))
                        .Select(e => new RegisteredFacultyEducation
                        {
                            educationId = e.id,
                            educationName = e.educationCategoryName,
                            studiedEducation =db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == regFaculty.id).Select(fe => fe.courseStudied).FirstOrDefault(),
                            specialization =db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == regFaculty.id).Select(fe => fe.specialization).FirstOrDefault(),
                            passedYear =db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == regFaculty.id).Select(fe => fe.passedYear).FirstOrDefault(),
                            percentage =db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == regFaculty.id).Select(fe => fe.marksPercentage).FirstOrDefault(),
                            division =db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == regFaculty.id).Select(fe => fe.division).FirstOrDefault(),
                            university =
                                db.jntuh_registered_faculty_education.Where(
                                    fe => fe.educationId == e.id && fe.facultyId == regFaculty.id)
                                    .Select(fe => fe.boardOrUniversity)
                                    .FirstOrDefault(),
                            place =
                                db.jntuh_registered_faculty_education.Where(
                                    fe => fe.educationId == e.id && fe.facultyId == regFaculty.id)
                                    .Select(fe => fe.placeOfEducation)
                                    .FirstOrDefault(),
                            facultyCertificate =
                                db.jntuh_registered_faculty_education.Where(
                                    fe => fe.educationId == e.id && fe.facultyId == regFaculty.id)
                                    .Select(fe => fe.certificate)
                                    .FirstOrDefault(),
                        }).ToList();

                foreach (var item in regFaculty.FacultyEducation)
                {
                    if (item.division == null)
                        item.division = 0;
                }

            }
            return View(regFaculty);

        }


    }
}