using babylawya.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace babylawya.Models.DocumentViewModels
{
    public class DocumentViewModel
    {
        public Guid Id { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; } = DateTime.Now;

        [StringLength(50)]
        public string Title { get; set; }

        //public string Path { get; set; }

        public ICollection<Keyword> Keywords { get; set; }
    }
}
