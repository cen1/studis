﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace studis.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Trenutno geslo")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora biti vsaj {2} znakov.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        [Display(Name = "Novo geslo")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potrdi novo geslo")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("NewPassword", ErrorMessage = "Gesli se ne ujemata.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Uporabniško ime")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Geslo")]
        public string Password { get; set; }

        [Display(Name = "Zapomni si me")]
        public bool RememberMe { get; set; }
    }

    public class CreateUserModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Password Question")]
        public string PasswordQuestion { get; set; }

        [Required]
        [Display(Name = "Password Answer")]
        public string PasswordAnswer { get; set; }

    }

    public class PasswordRecoveryModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email naslov")]
        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {

        [Required]
        public int token { get; set; }

    }
}
