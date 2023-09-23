using MasterDetailsUsingCoreJQAjax.Models;
using MasterDetailsUsingCoreJQAjax.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MasterDetailsUsingCoreJQAjax.Controllers
{
    public class ResumeController:Controller
    {
        private readonly AppDBContext _db;
        private readonly IWebHostEnvironment _webHost;
        public ResumeController(AppDBContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            var applicants = _db.Applicants
          .Include(i => i.Experiences)
          .Include(i => i.Designation)
          .ToList();
            return View(applicants);
        }       
        [HttpGet]
        public IActionResult Create()
        {
            ApplicantViewModel applicant=new ApplicantViewModel();
            applicant.Designations=_db.Designations.ToList();
            applicant.Experiences.Add(new Experience() { ExperienceId = 1 });
            return View(applicant);
        }
        [HttpPost]
        public IActionResult Create(ApplicantViewModel applicant)
        {
            string uniqueFileName = GetUploadedFileName(applicant);
            applicant.ImageUrl = uniqueFileName;
            Applicant obj = new Applicant();
            obj.ApplicantName = applicant.ApplicantName;
            obj.DesignationId = applicant.DesignationId;
            obj.MobileNo = applicant.MobileNo;
            obj.IsActive = applicant.IsActive;
            obj.DOB = applicant.DOB;
            obj.ImageUrl = applicant.ImageUrl;
            _db.Add(obj);
            _db.SaveChanges();
            var user = _db.Applicants.SingleOrDefault(x => x.ApplicantName == applicant.ApplicantName);
            if (user != null)
            {
                if (applicant.Experiences.Count > 0)
                {
                    foreach (var item in applicant.Experiences)
                    {
                        Experience objE = new Experience();
                        objE.ApplicantId = user.ApplicantId;
                        objE.YearsWorked = item.YearsWorked;
                        objE.CompanyName = item.CompanyName;
                        _db.Add(objE);
                    }
                }
            }
            _db.SaveChanges();
            return RedirectToAction("index");
        }
        private string GetUploadedFileName(ApplicantViewModel applicant)
        {
            string uniqueFileName = null;

            if (applicant.ProfilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + applicant.ProfilePhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    applicant.ProfilePhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Applicant app = _db.Applicants.First(x => x.ApplicantId == id);
            var exps = _db.Experiences.Where(x => x.ApplicantId == id).ToList();

            if (app != null)
            {
                ApplicantViewModel appVm = new ApplicantViewModel()
                {
                    ApplicantId = app.ApplicantId,
                    ApplicantName = app.ApplicantName,
                    MobileNo = app.MobileNo,
                    DOB = app.DOB.Date,
                    ImageUrl = app.ImageUrl,
                    DesignationId = app.DesignationId,
                    Designations = _db.Designations.ToList()
                };
                if (exps.Count() > 0)
                {
                    foreach (var item in exps)
                    {
                        Experience obj = new Experience();
                        obj.ApplicantId = app.ApplicantId;
                        obj.YearsWorked = item.YearsWorked;
                        obj.CompanyName = item.CompanyName;
                        appVm.Experiences.Add(obj);
                    }
                }
                return View("Edit", appVm);
            }
            return View();
        }
        
        [HttpPost]
        public ActionResult Edit(ApplicantViewModel applicant)
        {
            string uniqueFileName = GetUploadedFileName(applicant);
            applicant.ImageUrl = uniqueFileName;
            Applicant obj = new Applicant();
            obj.ApplicantName = applicant.ApplicantName;
            obj.DesignationId = applicant.DesignationId;
            obj.MobileNo = applicant.MobileNo;
            obj.IsActive = applicant.IsActive;
            obj.DOB = applicant.DOB;
            obj.ImageUrl = applicant.ImageUrl;
            obj.ApplicantId = applicant.ApplicantId;
            if (applicant.Experiences.Count > 0)
            {
                foreach (var item in applicant.Experiences)
                {
                    Experience objE = new Experience();
                    objE.ApplicantId = obj.ApplicantId;
                    objE.YearsWorked = item.YearsWorked;
                    objE.CompanyName = item.CompanyName;
                }
            }
            _db.Entry(obj).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var applicant = _db.Applicants.Find(id);
            var exps = _db.Experiences.Where(e=>e.ApplicantId==id);
            foreach(var exp in exps)
            {
                _db.Experiences.Remove(exp);
            }
            _db.Entry(applicant).State= EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index");           
        }
    }
}
