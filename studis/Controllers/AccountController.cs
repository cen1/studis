using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using studis.Models;
using WebMatrix.WebData;

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
            System.Diagnostics.Debug.WriteLine("login post");
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("model ok");
                //poglej ce je IP zaklenjen
                string ip = Request.UserHostAddress;
                var ipl = IpLock.FindActiveByIp(ip);
                if (ipl == null)
                {
                    //ip zaklepa ni ali pa je potekel
                    var user = studis.Models.UserHelper.FindByName(model.UserName);
                    if (user != null)
                    {
                        System.Diagnostics.Debug.WriteLine("notnull " + user.my_aspnet_membership.FailedPasswordAttemptCount);
                        if (user.my_aspnet_membership.FailedPasswordAttemptCount >= 3)
                        {
                            System.Diagnostics.Debug.WriteLine("cntdecrease");
                            user.my_aspnet_membership.FailedPasswordAttemptCount = 0;
                        }

                    }
                    System.Diagnostics.Debug.WriteLine("pred membership");
                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        System.Diagnostics.Debug.WriteLine("po");
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

        public ActionResult PasswordRecovery() {
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

        public ActionResult PasswordRecoverySuccess()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            if (ModelState.IsValid)
            {
                var user = studis.Models.UserHelper.FindByEmail(model.Email);
                if (user == null )
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("PasswordRecoverySuccess");
                }

                string code = studis.Models.UserHelper.GeneratePasswordResetToken(user.userId);
                string baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
                string to = user.Email;
                string text = "Ponastavite geslo z obiskom <a href='" + baseUrl + "Account/ResetPassword/" + code + "'>tega naslova</a>";
                System.Diagnostics.Debug.WriteLine(text);
                
                studis.Models.UserHelper.SendEmail(text, to);

                password_recovery pr = new password_recovery();
                pr.token=code;
                pr.userId=user.userId;
                pr.valid_until=DateTime.Now.AddHours(1);

                db.password_recovery.Add(pr);
                db.SaveChanges();

                return RedirectToAction("PasswordRecoverySuccess", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ResetPassword(string id)
        {
            var L2EQuery = db.password_recovery.Where(t => t.token == id).Where(d => d.valid_until > DateTime.Now);
            var pr = L2EQuery.FirstOrDefault<password_recovery>();
            if (pr == null)
            {
                return RedirectToAction("PasswordRecoveryExpired", "Account");
            }
            else
            {
                ResetPasswordModel m = new ResetPasswordModel();
                m.token = pr.id;
                return View(m);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(model.token);
                var pr = db.password_recovery.Find(model.token);
                var user = pr.my_aspnet_users;

                System.Diagnostics.Debug.WriteLine(user.id);
                MembershipUser currentUser = Membership.GetUser(user.id);
                string newpass = currentUser.ResetPassword();

                db.password_recovery.Remove(pr);
                db.SaveChanges();

                TempData["pass"] = newpass;
                System.Diagnostics.Debug.WriteLine(newpass);
                return RedirectToAction("PasswordRecoveryComplete", "Account");
            }
            return View(model);
        }

        public ActionResult PasswordRecoveryExpired()
        {
            return View();
        }

        public ActionResult PasswordRecoveryComplete()
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
