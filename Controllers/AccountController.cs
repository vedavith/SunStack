//using LoginRegistrationInMVCWithDatabase.Models;
using LoginRegistrationInMVCWithDatabase.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LoginRegistrationInMVCWithDatabase.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        //Return Register view
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveRegisterDetails(Register registerDetails)
        {
            if (ModelState.IsValid)
            {
                //create database context using Entity framework 
                using (var databaseContext = new LoginRegistrationInMVCEntities())
                {
                    RegisterUser reglog = new RegisterUser();
                    reglog.FirstName = registerDetails.FirstName;
                    reglog.LastName = registerDetails.LastName;
                    reglog.Email = registerDetails.Email;
                    reglog.Password = registerDetails.Password;

                 //Calling the SaveDetails method which saves the details.
                    databaseContext.RegisterUsers.Add(reglog);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";
                return View("Login");
            }
            else
            {
                return View("Register", registerDetails);
            }
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isValidUser = IsValidUser(model);

                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Failure", "Invalid Username or Password !");
                    return View();
                }
            }
            else
            {
                return View(model);
            }
        }

        //function to check if User is valid or not
        public RegisterUser IsValidUser(LoginViewModel model)
        {
            using (var dataContext = new LoginRegistrationInMVCEntities())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                RegisterUser user = dataContext.RegisterUsers.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                else
                    return user;
            }
        }
        // Used to log out 
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index");
        }

    }
}
