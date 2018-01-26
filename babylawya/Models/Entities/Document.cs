using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

namespace babylawya.Models.Entities
{
    public class Document
    {
        public Guid Id { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        //[StringLength(50)]
        public string Title { get; set; }

        public string Path { get; set; }

        public ICollection<Keyword> Keywords { get; set; }
    }
}
