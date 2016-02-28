using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IT4GOV.IdentityServer.UI.SignUp
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Το όνομα χρήστη είναι απαραίτητο")]
        [Display(Name = "Όνομα χρήστη", Prompt = "Όνομα χρήστη")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Ο κωδικός είναι απαραίτητος")]
        [Display(Name = "Κωδικός")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Η επαλήθευση κωδικού είναι απαραίτητη")]
        [DataType(DataType.Password)]
        [Display(Name = "Επαλήθευση κωδικού")]
        [Compare("Password", ErrorMessage = "Ο κωδικός σας δεν ταιριάζει")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Το επώνυμο είναι απαραίτητο")]
        [Display(Name = "Επώνυμο")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Το όνομα είναι απαραίτητο")]
        [Display(Name = "Όνομα")]
        public string GivenName { get; set; }

        [Required(ErrorMessage = "Το πατρώνυμο είναι απαραίτητο")]
        [Display(Name = "Πατρώνυμο")]
        public string FamilyName { get; set; }


        [Required(ErrorMessage = "Η ημερομηνία γέννησης είναι απαραίτητη")]
        [Display(Name = "Ημερομηνία γέννησης")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:dd-MM-yyyy}",ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Το email είναι απαραίτητο")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EmailVerified { get; set; }

        [Required(ErrorMessage = "Η διεύθυνση είναι απαραίτητη")]
        [Display(Name = "Διεύθυνση")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Το τηλέφωνο είναι απαραίτητο")]
        [Display(Name = "Κινητό")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberVerified { get; set; }

        [Required(ErrorMessage = "Το ΑΜΚΑ είναι απαραίτητο")]
        [Display(Name = "ΑΜΚΑ")]
        [StringLength(11, ErrorMessage = "Ο ΑΜΚΑ πρέπει να έχει 11 χαρακτήρες.")]
        public string AMKA { get; set; }

        [Required(ErrorMessage = "Το ΑΦΜ είναι απαραίτητο")]
        [Display(Name = "ΑΦΜ")]
        [StringLength(9, ErrorMessage = "Ο ΑΦΜ πρέπει να έχει 9 χαρακτήρες.")]
        public string AFM { get; set; }

        [Required(ErrorMessage = "Ο αριθμός ταυτότητας είναι απαραίτητος")]
        [Display(Name = "Αριθμός Ταυτότητας")]
        [StringLength(9, ErrorMessage = "Ο αριθμός ταυτότητας δεν είναι σωστός.", MinimumLength =7)]
        public string AT { get; set; }

        [Display(Name = "Φωτογραφία")]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage ="Η εικόνα πρέπει να είναι jpeg ή png")]
        public IFormFile ImageUpload { get; set; }


       
    }
}