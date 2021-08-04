﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.FormModels
{
    public class JobAdvertModel
    {
        [Required]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en titel på stillingsopslaget")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en beskrivelse på stillingsopslaget")]
        public string Summary { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en gyldig registrerings dato")]
        public DateTime RegistrationDateTime { get; set; } = DateTime.UtcNow;

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Vælg en kategori fra listen")]
        [Range(1, int.MaxValue, ErrorMessage = "Der er ikke valgt nogen kategori.")]
        public int CategoryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Vælg et speciale fra listen")]
        [Range(0, int.MaxValue, ErrorMessage = "Vælg et speciale til den valgte kategori.")]
        public int SpecializationId { get; set; } = 0;
    }
}
