using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class JobVacancyModel
    {
        [Required]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en titel på stillingsopslaget")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en email adresse")]
        [MaxLength(128)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast et telefon nummer")]
        [MaxLength(64)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en beskrivelse på stillingsopslaget")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en lokation for stillingsopslaget")]
        [MaxLength(255)]
        public string Location { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en gyldig registrerings dato")]
        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en gyldig dato for ansøgningsfrist")]
        public DateTime DeadlineDate { get; set; } = DateTime.UtcNow.AddDays(1);

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast et gyldigt kilde link til stillingsopslaget")]
        [MaxLength(1000)]
        public string SourceURL { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Vælg en virksomhed fra listen")]
        [Range(1, int.MaxValue, ErrorMessage = "Der er ikke valgt nogen virksomhed.")]
        public int? CompanyId { get; set; } = null;

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Vælg en kategori fra listen")]
        [Range(1, int.MaxValue, ErrorMessage = "Der er ikke valgt nogen kategori.")]
        public int CategoryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Vælg et speciale fra listen")]
        public int? SpecializationId { get; set; } = 0;
    }
}
