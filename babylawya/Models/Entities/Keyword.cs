using System;

namespace babylawya.Models.Entities
{
    public class Keyword
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual Document Document { get; set; }
        public Guid DocumentId { get; set; }
    }
}
