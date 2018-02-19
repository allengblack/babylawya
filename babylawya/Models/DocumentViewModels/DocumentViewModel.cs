using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace babylawya.Models.DocumentViewModels
{
    public class DocumentViewModel
    {
        [StringLength(50)]
        public string Title { get; set; }

        public IFormFile MyDocument { get; set; }

        public string Keywords { get; set; }
    }
}
