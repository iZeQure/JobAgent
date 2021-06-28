using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.FormModels
{
    public class ContractModel
    {
        private DateTime registrationDateTime = DateTime.UtcNow;
        private DateTime expiryDate;

        public int Id { get; set; }

        [Required(ErrorMessage = "Udfyld venligst et navn på kontraktens kontakt person.")]
        public string ContactPerson { get; set; } = string.Empty;

        [Required]
        public string ContractFileName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime RegistrationDate { get { return registrationDateTime; } set { registrationDateTime = value; } }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ExpiryDate { get { return registrationDateTime.AddYears(5); } set { expiryDate = value; } }

        [Required(ErrorMessage = "Vælg venligst en konsulent fra listen.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en konsulent fra listen.")]
        public int SignedByUser { get; set; }

        [Required(ErrorMessage = "Vælg venligst en virksomhed fra listen.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en virksomhed fra listen.")]
        public int SignedWithCompany { get; set; }

        //[Required(ErrorMessage = "Upload venlist en kontrakt.")]
        //[FileValidation(new[] { ".pdf" })]
        //public IBrowserFile Contract { get; set; }
    }

    //class FileValidationAttribute : ValidationAttribute
    //{
    //    public FileValidationAttribute(string[] allowedExtensions)
    //    {
    //        AllowedExtensions = allowedExtensions;
    //    }

    //    private string[] AllowedExtensions { get; }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var file = (IBrowserFile)value;

    //        var extension = System.IO.Path.GetExtension(file.Name);

    //        if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
    //        {
    //            return new ValidationResult($"Filen skal være af en af disse udvidelser: {string.Join(", ", AllowedExtensions)}.", new[] { validationContext.MemberName });
    //        }

    //        return ValidationResult.Success;
    //    }
    //}
}
