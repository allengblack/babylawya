using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace babylawya.Models.Entities
{
    public class Document
    {
        public Document()
        {
            Keywords = new List<Keyword>();
        }

        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string Name { get; set; }

        [NotMapped]
        public IFormFile MyDocument { get; set; }

        public ICollection<Keyword> Keywords { get; set; }
    }
}
