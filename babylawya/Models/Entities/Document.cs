using System;
using System.Collections.Generic;

namespace babylawya.Models.Entities
{
    public class Document
    {
        public Guid Id { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public ICollection<Keyword> Keywords { get; set; }
    }
}
