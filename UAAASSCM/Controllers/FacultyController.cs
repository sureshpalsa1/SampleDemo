using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using UAAASSCM.Models;

namespace UAAASSCM.Controllers
{
    public class FacultyController : Controller
    {
      
        private SCMEntities db = new SCMEntities();
        public ActionResult ViewFacultyDetails(string fid)
        {
            FacultyRegistration regFaculty = new FacultyRegistration();
            int fID = 0;

            if (fid != null)
            {
                regFaculty.GenderId = null;
                regFaculty.isFacultyRatifiedByJNTU = null;
                fID = Convert.ToInt32(UAAAS.Models.Utilities.DecryptString(fid, WebConfigurationManager.AppSettings["CryptoKey"]));


                var jntuh_registered_faculty_log = db.jntuh_registered_faculty_log.AsNoTracking().ToList();
                var RegfacultyId = jntuh_registered_faculty_log.Where(e => e.RegFacultyId == fID).Select(e => e.RegFacultyId).FirstOrDefault();

                //Log Data
                if (RegfacultyId != 0)
                {
                    var faculty =jntuh_registered_faculty_log.Where(e => e.RegFacultyId == fID).Select(e => e).FirstOrDefault();
                    regFaculty.id = fID;
                    regFaculty.Type = faculty.type;
                    regFaculty.RegistrationNumber = faculty.RegistrationNumber;
                    regFaculty.UserName = db.my_aspnet_users.Where(u => u.id == faculty.UserId).Select(u => u.name).FirstOrDefault();
                    regFaculty.Email = faculty.Email;
                    regFaculty.UniqueID = faculty.UniqueID;
                    regFaculty.FirstName = faculty.FirstName;
                    regFaculty.MiddleName = faculty.MiddleName;
                    regFaculty.LastName = faculty.LastName;
                    regFaculty.FatherOrhusbandName = faculty.FatherOrHusbandName;
                    regFaculty.MotherName = faculty.MotherName;
                    regFaculty.GenderId = faculty.GenderId;
                    if (faculty.DateOfBirth != null)
                    {
                        regFaculty.facultyDateOfBirth = UAAAS.Models.Utilities.MMDDYY2DDMMYY(faculty.DateOfBirth.ToString());
                    }
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
                        regFaculty.facultyDateOfAppointment = UAAAS.Models.Utilities.MMDDYY2DDMMYY(faculty.DateOfAppointment.ToString());
                    }
                    regFaculty.TotalExperiencePresentCollege = faculty.TotalExperiencePresentCollege;
                    regFaculty.isFacultyRatifiedByJNTU = faculty.isFacultyRatifiedByJNTU;
                    if (faculty.DateOfRatification != null)
                    {
                        regFaculty.facultyDateOfRatification = UAAAS.Models.Utilities.MMDDYY2DDMMYY(faculty.DateOfRatification.ToString());
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


                    regFaculty.FacultyEducation = db.jntuh_education_category.Where(e => e.isActive == true && (e.id == 1 || e.id == 3 || e.id == 4 || e.id == 5 || e.id == 6))
                                                                .Select(e => new RegisteredFacultyEducation
                                                                {
                                                                    educationId = e.id,
                                                                    educationName = e.educationCategoryName,
                                                                    studiedEducation = db.jntuh_registered_faculty_education_log.Where(fe => fe.educationId == e.id && fe.RegFacultyId == fID).Select(fe => fe.courseStudied).FirstOrDefault(),
                                                                    specialization = db.jntuh_registered_faculty_education_log.Where(fe => fe.educationId == e.id && fe.RegFacultyId == fID).Select(fe => fe.specialization).FirstOrDefault(),
                                                                    passedYear = db.jntuh_registered_faculty_education_log.Where(fe => fe.educationId == e.id && fe.RegFacultyId == fID).Select(fe => fe.passedYear).FirstOrDefault(),
                                                                    percentage = db.jntuh_registered_faculty_education_log.Where(fe => fe.educationId == e.id && fe.RegFacultyId == fID).Select(fe => fe.marksPercentage).FirstOrDefault(),
                                                                    division = db.jntuh_registered_faculty_education_log.Where(fe => fe.educationId == e.id && fe.RegFacultyId == fID).Select(fe => fe.division).FirstOrDefault(),
                                                                    university = db.jntuh_registered_faculty_education_log.Where(fe => fe.educationId == e.id && fe.RegFacultyId == fID).Select(fe => fe.boardOrUniversity).FirstOrDefault(),
                                                                    place = db.jntuh_registered_faculty_education_log.Where(fe => fe.educationId == e.id && fe.RegFacultyId == fID).Select(fe => fe.placeOfEducation).FirstOrDefault(),
                                                                    facultyCertificate = db.jntuh_registered_faculty_education_log.Where(fe => fe.educationId == e.id && fe.RegFacultyId == fID).Select(fe => fe.certificate).FirstOrDefault(),
                                                                }).ToList();

                    foreach (var item in regFaculty.FacultyEducation)
                    {
                        if (item.division == null)
                            item.division = 0;
                    }
                }
                else
                {
                    jntuh_registered_faculty faculty = db.jntuh_registered_faculty.Find(fID);
                    regFaculty.id = fID;
                    regFaculty.Type = faculty.type;
                    regFaculty.RegistrationNumber = faculty.RegistrationNumber;
                    regFaculty.UserName = db.my_aspnet_users.Where(u => u.id == faculty.UserId).Select(u => u.name).FirstOrDefault();
                    regFaculty.Email = faculty.Email;
                    regFaculty.UniqueID = faculty.UniqueID;
                    regFaculty.FirstName = faculty.FirstName;
                    regFaculty.MiddleName = faculty.MiddleName;
                    regFaculty.LastName = faculty.LastName;
                    regFaculty.FatherOrhusbandName = faculty.FatherOrHusbandName;
                    regFaculty.MotherName = faculty.MotherName;
                    regFaculty.GenderId = faculty.GenderId;
                    if (faculty.DateOfBirth != null)
                    {
                        regFaculty.facultyDateOfBirth = UAAAS.Models.Utilities.MMDDYY2DDMMYY(faculty.DateOfBirth.ToString());
                    }
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
                        regFaculty.facultyDateOfAppointment = UAAAS.Models.Utilities.MMDDYY2DDMMYY(faculty.DateOfAppointment.ToString());
                    }
                    regFaculty.TotalExperiencePresentCollege = faculty.TotalExperiencePresentCollege;
                    regFaculty.isFacultyRatifiedByJNTU = faculty.isFacultyRatifiedByJNTU;
                    if (faculty.DateOfRatification != null)
                    {
                        regFaculty.facultyDateOfRatification = UAAAS.Models.Utilities.MMDDYY2DDMMYY(faculty.DateOfRatification.ToString());
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


                    regFaculty.FacultyEducation = db.jntuh_education_category.Where(e => e.isActive == true && (e.id == 1 || e.id == 3 || e.id == 4 || e.id == 5 || e.id == 6))
                                                                .Select(e => new RegisteredFacultyEducation
                                                                {
                                                                    educationId = e.id,
                                                                    educationName = e.educationCategoryName,
                                                                    studiedEducation = db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == fID).Select(fe => fe.courseStudied).FirstOrDefault(),
                                                                    specialization = db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == fID).Select(fe => fe.specialization).FirstOrDefault(),
                                                                    passedYear = db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == fID).Select(fe => fe.passedYear).FirstOrDefault(),
                                                                    percentage = db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == fID).Select(fe => fe.marksPercentage).FirstOrDefault(),
                                                                    division = db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == fID).Select(fe => fe.division).FirstOrDefault(),
                                                                    university = db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == fID).Select(fe => fe.boardOrUniversity).FirstOrDefault(),
                                                                    place = db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == fID).Select(fe => fe.placeOfEducation).FirstOrDefault(),
                                                                    facultyCertificate = db.jntuh_registered_faculty_education.Where(fe => fe.educationId == e.id && fe.facultyId == fID).Select(fe => fe.certificate).FirstOrDefault(),
                                                                }).ToList();

                    foreach (var item in regFaculty.FacultyEducation)
                    {
                        if (item.division == null)
                            item.division = 0;
                    }
                }


                

                string registrationNumber = db.jntuh_registered_faculty.Where(of => of.id == fID).Select(of => of.RegistrationNumber).FirstOrDefault();
                int facultyId = db.jntuh_college_faculty_registered.Where(of => of.RegistrationNumber == registrationNumber).Select(of => of.id).FirstOrDefault();
                int[] verificationOfficers = db.jntuh_college_faculty_verified.Where(v => v.FacultyId == facultyId).Select(v => v.VerificationOfficer).Distinct().ToArray();
              //  int userId = Convert.ToInt32(Membership.GetUser(User.Identity.Name).ProviderUserKey);

                //bool isValid = ShowHideLink(fID);

                //ViewBag.HideVerifyLink = isValid;

                //if (verificationOfficers.Contains(userId))
                //{
                //    if (isValid)
                //    {
                //        ViewBag.HideVerifyLink = true;
                //    }
                //    else
                //    {
                //        ViewBag.HideVerifyLink = false;
                //    }
                //}

                //if (verificationOfficers.Count() == 3)
                //{
                //    ViewBag.HideVerifyLink = true;
                //}

                ViewBag.HideVerifyLink = regFaculty.isApproved != null ? true : false;
            }

            return View(regFaculty);
        }
	}

    public class FacultyRegistration
    {
        public FacultyRegistration()
        {
            this.jntuh_registered_faculty_education = new HashSet<jntuh_registered_faculty_education>();
            //this.jntuh_registered_faculty_education_log=new HashSet<jntuh_registered_faculty_education_log>();
        }


        public int id { get; set; }
        public int DegreeId { get; set; }
        public int Eid { get; set; }
        public string Type { get; set; }
        public string PanVerificationStatus { get; set; }
        public string PanDeactivationReasion { get; set; }
        public string PanStatusAfterDE { get; set; }
        public string IdentfiedFor { get; set; }
        public string SpecializationIdentfiedFor { get; set; }
        public string Principal { get; set; }
        public int? FIsApproved { get; set; }
        //[Required(ErrorMessage = "*")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Must be 5-15 characters long.")]
        [Display(Name = "Username")]
        [RegularExpression(@"[a-zA-Z0-9_]{1,15}", ErrorMessage = "Allowed characters : 'alphabets', 'numbers', '_'")]
        public string UserName { get; set; }

        [Display(Name = "Faculty Registration ID")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "Faculty Unique ID")]
        public string UniqueID { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Must be 3-50 characters long.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //LastName-MiddleName
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        //Surname-LastName
        [Required(ErrorMessage = "*")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Must be 3-50 characters long.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Must be 3-100 characters long.")]
        [Display(Name = "Father's Name")]
        public string FatherOrhusbandName { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Must be 3-50 characters long.")]
        [Display(Name = "Mother's Name")]
        public string MotherName { get; set; }

        [Display(Name = "Designation in the Parent Organization")]
        public string AdjunctDesignation { get; set; }

        [Display(Name = "Parent Organization Department")]
        public string AdjunctDepartment { get; set; }


        [Display(Name = "Date of Birth")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Date of Birth")]
        public string facultyDateOfBirth { get; set; }

        //[Required(ErrorMessage = "*")]
        [StringLength(500, ErrorMessage = "Maximum 500 characters")]
        public string OrganizationName { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        public string department { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Designation")]
        public int? DesignationId { get; set; }

        public string designation { get; set; }

        [Display(Name = "Date of Appointment")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfAppointment { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Date of Appointment")]
        public string facultyDateOfAppointment { get; set; }

        [StringLength(25, ErrorMessage = "Must be 25 characters")]
        [Display(Name = "Proceedings No")]
        public string ProceedingsNo { get; set; }

        [Display(Name = "Selection Committee Proceedings Document")]
        public HttpPostedFileBase SelectionCommitteeProceedingsDocument { get; set; }

        [Display(Name = "Selection Committee Proceedings")]
        public string SelectionCommitteeProcedings { get; set; }

        [StringLength(100, ErrorMessage = "Maximum 100 characters")]
        [Display(Name = "AICTE Faculty Id")]
        public string AICTEFacultyId { get; set; }

        [StringLength(100, ErrorMessage = "Maximum 100 characters")]
        [Display(Name = "Honorarium")]
        public string GrossSalary { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Total Experience")]
        public int? TotalExperience { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Experience Present Institution")]
        public int? TotalExperiencePresentCollege { get; set; }

        //[Required(ErrorMessage = "*")]
        [StringLength(10, ErrorMessage = "Must be 10 characters")]

        //[RegularExpression(@"\w{10}", ErrorMessage = "Invalid PAN number")]
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"[A-Z]{3}[P][A-Z]{1}\d{4}[A-Z]{1}", ErrorMessage = "Invalid PAN number")]
        [Display(Name = "PAN Number")]
       
        public string PANNumber { get; set; }

        //[RegularExpression(@"\w{10}", ErrorMessage = "Invalid PAN number")]
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"[A-Z]{3}[P][A-Z]{1}\d{4}[A-Z]{1}", ErrorMessage = "Invalid PAN number")]
        
        [Display(Name = "PAN Number")]
        public string EditPANNumber { get; set; }
        //[Required(ErrorMessage = "*")]
        [Display(Name = "PAN Card Document")]
        public HttpPostedFileBase PANCardDocument { get; set; }

        public string facultyPANCardDocument { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(10, ErrorMessage = "Maximum 10 characters")]
        [RegularExpression(@"\d{10}", ErrorMessage = "Invalid mobile")]
        [Display(Name = "Mobile No")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Must be 6-100 characters long.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Invalid email")]
        [Display(Name = "Email Address")]
       
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Must be 6-100 characters long.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Invalid email")]
        [Display(Name = "Email Address")]
        public string EditEmail { get; set; }

        [StringLength(500, ErrorMessage = "Maximum 500 characters")]
        [Display(Name = "National")]
        public string National { get; set; }

        [StringLength(500, ErrorMessage = "Maximum 500 characters")]
        [Display(Name = "International")]
        public string InterNational { get; set; }

        [StringLength(500, ErrorMessage = "Maximum 500 characters")]
        [Display(Name = "Citation")]
        public string Citation { get; set; }

        [StringLength(500, ErrorMessage = "Maximum 500 characters")]
        [Display(Name = "Awards")]
        public string Awards { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Photo")]
        //[ValidateFacultyImage(ErrorMessage = "Please select a JPEG image smaller than 50KB")]
        public HttpPostedFileBase Photo { get; set; }

        public string facultyPhoto { get; set; }

        [StringLength(16, ErrorMessage = "Must be 12 characters")]
        [RegularExpression(@"\d{12}", ErrorMessage = "Invalid AADHAAR number")]
        [Display(Name = "Aadhaar Number")]
        public string AadhaarNumber { get; set; }

        [Display(Name = "Aadhaar Card Document")]
        public HttpPostedFileBase AadhaarCardDocument { get; set; }

        public string facultyAadhaarCardDocument { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Status")]
        public bool isActive { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Status")]
        public bool? isApproved { get; set; }

        [Display(Name = "ViewType")]
        public bool? isView { get; set; }

        public Nullable<System.DateTime> createdOn { get; set; }
        public Nullable<int> createdBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public Nullable<int> updatedBy { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Must be 6-15 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Ratified by JNTUH")]
        public bool? isFacultyRatifiedByJNTU { get; set; }

        [Display(Name = "Date of Ratification")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfRatification { get; set; }

        [Display(Name = "Date of Ratification")]
        public string facultyDateOfRatification { get; set; }

        //[Required(ErrorMessage = "*")]
        [Display(Name = "Presently Working(If 'YES' Upload NOC from parent organization)")]
        public bool? WorkingStatus { get; set; }

        [Display(Name = "Other Department")]
        public string OtherDepartment { get; set; }

        [Display(Name = "Other Designation")]
        public string OtherDesignation { get; set; }

        public int? CollegeId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeCode { get; set; }

        public int? SamePANNumberCount { get; set; }
        public int? SameAadhaarNumberCount { get; set; }

        public bool isVerified { get; set; }
        public bool isValid { get; set; }



        [Required(ErrorMessage = "*")]
        public string WorkingType { get; set; }
        public string NOCFile { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Roles")]
        public string PresentInstituteAssignedRole { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Responsibilities")]
        public string PresentInstituteAssignedResponsebility { get; set; }
        [Required(ErrorMessage = "*")]
        public string Accomplish1 { get; set; }
        [Required(ErrorMessage = "*")]
        public string Accomplish2 { get; set; }
        [Required(ErrorMessage = "*")]
        public string Accomplish3 { get; set; }
        public string Accomplish4 { get; set; }
        public string Accomplish5 { get; set; }
        public string Professional { get; set; }
        public string Professional2 { get; set; }
        public string Professiona3 { get; set; }
        public string MembershipNo1 { get; set; }
        public string MembershipNo2 { get; set; }
        public string MembershipNo3 { get; set; }
        public string MembershipCertificate1 { get; set; }
        public string MembershipCertificate2 { get; set; }
        public string MembershipCertificate3 { get; set; }
        public string DeactivationReason { get; set; }
        public string DeactivationNew { get; set; }


        public HttpPostedFileBase NOCUploadFile { get; set; }
        public HttpPostedFileBase MembershipFile1 { get; set; }
        public HttpPostedFileBase MembershipFile2 { get; set; }
        public HttpPostedFileBase MembershipFile3 { get; set; }

        public virtual my_aspnet_users my_aspnet_users { get; set; }
        public virtual my_aspnet_users my_aspnet_users1 { get; set; }
        public virtual jntuh_designation jntuh_designation { get; set; }
        public virtual jntuh_department jntuh_department { get; set; }
        public virtual ICollection<jntuh_registered_faculty_education> jntuh_registered_faculty_education { get; set; }
        public virtual ICollection<jntuh_reinspection_registered_faculty_education> jntuh_reinspection_registered_faculty_education { get; set; }
        // public virtual ICollection<jntuh_registered_faculty_education_log> jntuh_registered_faculty_education_log { get; set; }

        public List<RegisteredFacultyEducation> FacultyEducation { get; set; }
        public List<RegisteredfacultyExperience> RFExperience { get; set; }


        public bool Absent { get; set; }
        [RegularExpression(@"[A-Z]{3}[P][A-Z]{1}\d{4}[A-Z]{1}", ErrorMessage = "Invalid PAN number")]
        [Display(Name = "ModifiedPANNo")]
        public string ModifiedPANNo { get; set; }
        public bool InvalidPANNo { get; set; }
        public bool BlacklistFaculty { get; set; }
        public string NORelevantUG { get; set; }
        public string NORelevantPG { get; set; }
        public string NORelevantPHD { get; set; }
        public bool NoSCM { get; set; }
        public bool NOForm16 { get; set; }
        public DateTime MOdifiedDateofAppointment { get; set; }
        public string MOdifiedDateofAppointment1 { get; set; }
        public bool NOTQualifiedAsPerAICTE { get; set; }
        public bool PHDundertakingnotsubmitted { get; set; }
        public bool MultipleReginSamecoll { get; set; }
        public bool MultipleReginDiffcoll { get; set; }
        public bool SamePANUsedByMultipleFaculty { get; set; }
        public bool PhotocopyofPAN { get; set; }
        public bool AppliedPAN { get; set; }
        public bool LostPAN { get; set; }
        public bool OriginalsVerifiedUG { get; set; }
        public bool OriginalsVerifiedPG { get; set; }
        public bool OriginalsVerifiedPHD { get; set; }
        public bool? FacultyVerificationStatus { get; set; }
        public bool InCompleteCeritificates { get; set; }
        public bool NONoc { get; set; }
        public bool NOProfessionalBodiesMembership { get; set; }
        public string SSC { get; set; }
        public string UG { get; set; }
        public string PG { get; set; }
        public string MTECH { get; set; }
        public string PHD { get; set; }

        public bool FalsePAN { get; set; }

        public int? SpecializationId { get; set; }
        public string SpecializationName { get; set; }

    }

    public class RegisteredFacultyEducation
    {
        public int educationId { get; set; }
        public string educationName { get; set; }
        public string studiedEducation { get; set; }
        public Nullable<int> passedYear { get; set; }
        public Nullable<decimal> percentage { get; set; }
        public Nullable<int> division { get; set; }
        public string university { get; set; }
        public string place { get; set; }
        public HttpPostedFileBase certificate { get; set; }
        public string facultyCertificate { get; set; }
        public string specialization { get; set; }
    }
    public class RegisteredfacultyExperience
    {
        public int? CollegeId { get; set; }
        public int? DesignationId { get; set; }
        public string CollegeName { get; set; }
        public string facultyDesignation { get; set; }

        public DateTime? facultyDateOfAppointment { get; set; }
        public DateTime? facultyDateOfResignation { get; set; }

        public string facultyDateOfAppointment1 { get; set; }
        public string facultyDateOfResignation1 { get; set; }
        public string RelievingLetter { get; set; }
        public string JoiningOrder { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public int? IsApproved { get; set; }

    }
}