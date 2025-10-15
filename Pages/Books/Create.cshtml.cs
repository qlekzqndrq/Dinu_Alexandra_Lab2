using Dinu_Alexandra_Lab2.Data;
using Dinu_Alexandra_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dinu_Alexandra_Lab2.Pages.Books
{
    public class AssignedCategoryData
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }

    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Dinu_Alexandra_Lab2Context _context;

        public CreateModel(Dinu_Alexandra_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; }

        [BindProperty]
        public List<int> SelectedCategories { get; set; } = new List<int>();

        public List<AssignedCategoryData> AssignedCategoryDataList { get; set; }

        public IActionResult OnGet()
        {
            // LINQ pentru FullName
            var authors = _context.Author
                .Select(a => new
                {
                    a.ID,
                    FullName = a.FirstName + " " + a.LastName
                })
                .ToList();

            ViewData["AuthorID"] = new SelectList(authors, "ID", "FullName");

            var publishers = _context.Publisher
                .Select(p => new { p.ID, p.PublisherName })
                .ToList();

            ViewData["PublisherID"] = new SelectList(publishers, "ID", "PublisherName");

            var book = new Book();
            book.BookCategories = new List<BookCategory>();
            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            if (!ModelState.IsValid)
            {
                // re-populare ViewData dacă există eroare
                var authors = _context.Author
                    .Select(a => new { a.ID, FullName = a.LastName + " " + a.FirstName })
                    .ToList();
                ViewData["AuthorID"] = new SelectList(authors, "ID", "FullName");

                var publishers = _context.Publisher
                    .Select(p => new { p.ID, p.PublisherName })
                    .ToList();
                ViewData["PublisherID"] = new SelectList(publishers, "ID", "PublisherName");

                PopulateAssignedCategoryData(_context, Book, selectedCategories);

                return Page();
            }

            if (selectedCategories != null)
            {
                Book.BookCategories = new List<BookCategory>();
                foreach (var cat in selectedCategories)
                {
                    Book.BookCategories.Add(new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    });
                }
            }

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        // Funcție pentru popularea categorii checkbox
        private void PopulateAssignedCategoryData(Dinu_Alexandra_Lab2Context context, Book book, string[] selectedCategories = null)
        {
            var allCategories = context.Category.ToList();
            var bookCategories = selectedCategories != null ? selectedCategories.Select(int.Parse).ToList() : new List<int>();
            AssignedCategoryDataList = allCategories.Select(c => new AssignedCategoryData
            {
                CategoryID = c.ID,
                Name = c.CategoryName,
                Assigned = bookCategories.Contains(c.ID)
            }).ToList();
        }
    }
}
