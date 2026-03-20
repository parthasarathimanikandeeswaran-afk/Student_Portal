using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Student_Entry_Form.Models;
using Student_Entry_Form.Models.Repository;
using System;

namespace Student_Entry_Form.Controllers
{
    public class StudentController : Controller
    {
        private readonly DashboardRepository dashboardRepo;
        private readonly StudentRepository repo;




        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }


        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("UserName") != null;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Logout", "Account");
        }



        public StudentController(DashboardRepository dashboardRepo, StudentRepository repo)
        {
            this.dashboardRepo = dashboardRepo;
            this.repo = repo;
        }
        public IActionResult StudentEntry()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            if (!IsAdmin())
                return RedirectToAction("ResultForm");

            return View(new StudentEntryModel());
        }





        [HttpPost]
        public IActionResult StudentEntry(StudentEntryModel model)
        {
            if (!IsLoggedIn() || !IsAdmin())
                return RedirectToAction("Login", "Account");

    
            int lastSerial = repo.GetLastSerialNumber();
            int newSerial = lastSerial + 1;

        
            string namepart = model.Name.Length >= 3
                ? model.Name.Substring(0, 3).ToUpper()
                : model.Name.ToUpper();

            char thirdDigit = model.Mobile.Length > 2 ? model.Mobile[2] : '0';
            char seventhDigit = model.Mobile.Length > 6 ? model.Mobile[6] : '0';

            model.SerialNumber = newSerial;
            model.AcknowledgementNo = $"{namepart}{thirdDigit}{seventhDigit}{newSerial}";

            int studentId = repo.StudentEntryMethod(model);
            repo.StoreStudentMarks(studentId, model);
            repo.InsertStudentResult(studentId, model);

       
            return RedirectToAction("Success", new
            {
                ackNo = model.AcknowledgementNo,
                mobile = model.Mobile
            });
        }





  


        public IActionResult Success(string ackNo, string mobile)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            ViewBag.AckNo = ackNo;
            ViewBag.Mobile = mobile;
            return View();
        }


        [HttpGet]
        public IActionResult ResultForm()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            return View();
        }




        [HttpPost]
        public IActionResult ResultForm(ResultSearchViewModel model)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(model.AckNo) ||
                string.IsNullOrWhiteSpace(model.MobileNo))
            {
                ViewBag.Error = "Please enter both fields";
                return View(new ResultSearchViewModel());
            }

            var result = repo.GetStudentResult(model.AckNo, model.MobileNo);

            if (result == null)
            {
                ViewBag.Error = "Student not found!";
                return View(new ResultSearchViewModel());
            }

            return View("StudentResult", result);
        }

     

        public IActionResult MoveResultForm()
        {
            return RedirectToAction("ResultForm");
        }



        public IActionResult StudentsDetails(string? ackNo)
        {
            if (!IsLoggedIn() || !IsAdmin())
                return RedirectToAction("ResultForm");
            var students = repo.GetStudents(ackNo);
            return View(students);
        }




        public IActionResult Home()
        {
            if (!IsLoggedIn() || !IsAdmin())
                return RedirectToAction("Login", "Account");

            var stats = dashboardRepo.GetDashboardStats();


            if (stats == null)
                stats = new DashboardStatsViewModel();


            return View(stats);
        }







        [HttpGet]
        public IActionResult EditStudent(int id)
        {
            if (!IsLoggedIn() || !IsAdmin())
                return RedirectToAction("Login", "Account");

            var model = repo.GetStudentWithMarks(id);
            if (model == null)
                return NotFound();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStudent(StudentEditViewModel model)
        {

            if (ModelState.IsValid)
            {

                repo.UpdateStudentWithMarks(model);
                return RedirectToAction("StudentsDetails");
            }
            return View(model);
        }



    }
}
