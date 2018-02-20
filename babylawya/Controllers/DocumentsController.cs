using babylawya.Data;
using babylawya.Models.DocumentViewModels;
using babylawya.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
                    Path = document.MyDocument.FileName
                };

                keywords.ForEach(x => {
                    var key = new Keyword { Name = x, Id = Guid.NewGuid() };
                    doc.Keywords.Add(key);
                });

                _context.Documents.Add(doc);
                _context.SaveChanges();

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await document.MyDocument.CopyToAsync(stream);
                }

                return RedirectToAction("Upload");
            }
            else
            {
                return Content("Error uploading document");
            }
        }

        [Authorize]
        public async Task<IActionResult> Download(string filename)
        {
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

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            return View(await _context.Documents.ToListAsync());
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

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var document = await _context.Documents.SingleOrDefaultAsync(m => m.Id == id);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search(string searchString)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    return Content("Invalide search array");
                }

                var searchArray = searchString.Split(" ");
                List<Guid> results = new List<Guid>();
                foreach (var searchItem in searchArray)
                {
                    var keywords = _context.Keywords.Where(x => x.Name == searchItem).Select(x => x.DocumentId).ToList();
                    results.AddRange(keywords);
                }

                return RedirectToAction(nameof(DocumentsController.SearchResult), results);
            }
            else
            {
                return Content("Unable to find document matching keywords.");
            }
        }

        public async Task<IActionResult> SearchResult(IList<Guid> documentIds)
        {
            //throw new NotImplementedException(); 
            var docs = new List<Document>();
            foreach (var docId in documentIds)
            {
                var doc = await _context.Documents.FirstAsync(x => x.Id == docId);
                docs.Add(doc);
            }
            return RedirectToAction(nameof(DocumentsController.ListSearchResults), docs);
        }

        public IActionResult ListSearchResults(List<Document> documents)
        {
            //throw new NotImplementedException();
            if (ModelState.IsValid)
            {
                return View(documents);
            }
            else
            {
                return RedirectToAction(nameof(DocumentsController.Search));
            }
        }

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
                //{".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                //{".xls", "application/vnd.ms-excel"},
                //{".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                //{".png", "image/png"},
                //{".jpg", "image/jpeg"},
                //{".jpeg", "image/jpeg"},
                //{".gif", "image/gif"},
                //{".csv", "text/csv"}
            };
        }
    }
}
