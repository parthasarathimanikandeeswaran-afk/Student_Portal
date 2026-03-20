using Microsoft.AspNetCore.Mvc;
using Student_Entry_Form.Models;
using Student_Entry_Form.Models.Repository;

namespace Student_Entry_Form.Controllers
{
    public class AccountController : Controller
    {

        private readonly LoginRepository loginRepo;

        public AccountController(LoginRepository loginRepo)
        {
            this.loginRepo= loginRepo;
        }
        public IActionResult Login()
        {
            var model = new LoginPageViewModel
            {
                Student = new StudentLoginViewModel(),
                Admin = new AdminLoginViewModel()
            };
          return View(model);
        }

       


        [HttpPost]
        public IActionResult AdminLogin(LoginPageViewModel model)
        {
            var adminModel = model.Admin;

            if (adminModel == null ||
                string.IsNullOrEmpty(adminModel.Username) ||
                string.IsNullOrEmpty(adminModel.Password))
            {
                ViewBag.Error = "Invalid login data";
                return View("Login");
            }

            bool success = loginRepo.ValidAdmin(adminModel);

            if (success)
            {
                HttpContext.Session.SetString("UserName", adminModel.Username);
                HttpContext.Session.SetString("Role", "Admin");
                return RedirectToAction("Home", "Student");
            }
            else
            {
                ViewBag.Error = "Invalid Username or Password";
                return View("Login");
            }
        }

        [HttpPost]
        public IActionResult StudentLogin(LoginPageViewModel model)
        {
            var studentModel = model.Student;

            if (studentModel == null ||
                string.IsNullOrWhiteSpace(studentModel.AckNo) ||
                string.IsNullOrWhiteSpace(studentModel.DateOfBirth?.ToString()) ||
                studentModel.DateOfBirth == DateTime.MinValue)
            {
                ViewBag.Error = "Invalid login details";
                return View("Login", model);
            }

            bool success = loginRepo.ValidStudent(studentModel);

            if (success)
            {
                HttpContext.Session.SetString("UserName", studentModel.AckNo);
                HttpContext.Session.SetString("Role", "Student");
                return RedirectToAction("ResultForm", "Student");
            }
            else
            {
                ViewBag.Error = "Invalid Ack No or Date of Birth";
                return View("Login", model);
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


    }
}
