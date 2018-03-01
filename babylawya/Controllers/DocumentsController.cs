using babylawya.Data;
using babylawya.Models.DocumentViewModels;
using babylawya.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static babylawya.Constants;

namespace babylawya.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            return View(await _context.Documents.ToListAsync());
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upload(DocumentViewModel document)
        {
            if (ModelState.IsValid)
            {
                if (document.MyDocument == null || document.MyDocument.Length == 0)
                    return Content("file not selected");

                var path = Path.Combine(PATH, document.MyDocument.FileName);

                var keywords = document.Keywords.Split(" ").ToList();

                var doc = new Document()
                {
                    Id = Guid.NewGuid(),
                    Name = document.MyDocument.FileName
                };

                keywords.ForEach(x => {
                    var key = new Keyword { Name = x, Id = Guid.NewGuid() };
                    doc.Keywords.Add(key);
                });

                doc.Keywords = doc.Keywords.Distinct().ToList();

                _context.Documents.Add(doc);
                _context.SaveChanges();

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await document.MyDocument.CopyToAsync(stream);
                }

                return RedirectToAction("Index");
            }
                return Content("Error uploading document");
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Download(string filename)
        {
            //var filename = document.MyDocument.FileName;

            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(PATH, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .SingleOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        //GET: Documents/Upload
        public IActionResult Upload()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .SingleOrDefaultAsync(m => m.Id == id);

            if (document != null)
            {
                return View(document);
            }
            return NotFound();
        }

        // GET: Documents/Delete/5
        [HttpGet]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var document = await _context.Documents.SingleOrDefaultAsync(m => m.Id == id);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        public IActionResult Search(string searchString)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(searchString))
                {
                    return Content("You have not entered a search string");
                }

                var searchArray = searchString.Split(" ");
                List<Guid> results = new List<Guid>();
                foreach (var searchItem in searchArray)
                {
                    var keywords = _context.Keywords.Where(x => x.Name == searchItem).Select(x => x.DocumentId).Distinct().ToList();

                    results.AddRange(keywords);
                }
                
                return RedirectToAction("SearchResult", new {documentIds = results});
            }
            return Content("Unable to find document matching keywords.");
        }

        public IActionResult SearchResult(List<Guid> documentIds)
        {
            var docs = new List<Document>();

            foreach (var docId in documentIds)
            {
                var query = (from d in _context.Documents
                            where d.Id == docId
                            select d).ToList().FirstOrDefault();

                docs.Add(query);
            }
            return View(docs);
        }

        #region PrivateMethods

        private bool DocumentExists(Guid id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        #endregion
    }
}
