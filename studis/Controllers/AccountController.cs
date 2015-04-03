using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using studis.Models;

namespace studis.Controllers
{
    public class AccountController : Controller
    {

        public studisEntities db = new studisEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //Membership.CreateUser("kf4147", "testtest", "kf4147@student.uni-lj.si");
                //Membership.CreateUser("profesor1", "testtest", "nekimail@neki.com");
                //Membership.CreateUser("referent", "testtest", "nekimail2@neki.com");
                //poglej ce je IP zaklenjen
                string ip = Request.UserHostAddress;
                var ipl = IpLock.FindActiveByIp(ip);
                if (ipl == null)
                {
                    //ip zaklepa ni ali pa je potekel
                    var user = studis.Models.User.FindByName(db, model.UserName);
                    if (user != null)
                    {
                        System.Diagnostics.Debug.WriteLine("notnull " + user.my_aspnet_membership.FailedPasswordAttemptCount);
                        if (user.my_aspnet_membership.FailedPasswordAttemptCount >= 3)
                        {
                            System.Diagnostics.Debug.WriteLine("cntdecrease");
                            user.my_aspnet_membership.FailedPasswordAttemptCount = 0;
                        }

                    }

                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        //resetiraj failed ob uspesnem loginu
                        user.my_aspnet_membership.FailedPasswordAttemptCount = 0;
                        db.SaveChanges();

                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        if (user != null)
                        {
                            user.my_aspnet_membership.FailedPasswordAttemptCount++;
                            if (user.my_aspnet_membership.FailedPasswordAttemptCount >= 3)
                            {
                                ip_lock ipln = new ip_lock();
                                ipln.ip = Request.UserHostAddress;
                                ipln.locked_at = DateTime.Now;
                                ipln.locked_until = DateTime.Now.AddMinutes(3);
                                ipln.userId = user.id;
                                IpLock.Add(ipln);

                                System.Diagnostics.Debug.WriteLine("iplock");
                            }
                            ModelState.AddModelError("", "Geslo ni pravilno. Poskus (" + user.my_aspnet_membership.FailedPasswordAttemptCount + "/3)");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Uporabniško ime ni pravilno.");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Vaš IP je zaklenjen. Poskusite po " + ipl.locked_until.ToString());
                }
            }
            db.SaveChanges();

            return View(model);
        }

        public ActionResult SingOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                bool succeeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true);
                    succeeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    succeeded = false;
                }

                if (succeeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Trenutno ali novo geslo je nepravilno.");
                }
            }

            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {

            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Email is invalid. Please enter a different value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please enter a different value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify and try again.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been cancelled. Please verify and try again.";

                default:
                    return "An unknown error occurred.";
            }
        }
        #endregion
    }
}
