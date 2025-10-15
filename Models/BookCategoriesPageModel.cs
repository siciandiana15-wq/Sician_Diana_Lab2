using Microsoft.AspNetCore.Mvc.RazorPages;
using Sician_Diana_Lab2.Data;

namespace Sician_Diana_Lab2.Models
{
    public class BookCategoriesPageModel : PageModel
    {
        public List<AssignedCategoryData> AssignedCategoryDataList { get; set; } = new();

        public void PopulateAssignedCategoryData(Sician_Diana_Lab2Context context, Book book)
        {
            var allCategories = context.Category.OrderBy(c => c.CategoryName).ToList();

            // ✅ null-safe: dacă BookCategories e null, folosim colecție goală
            var bookCatIds = new HashSet<int>(
                (book.BookCategories ?? Enumerable.Empty<BookCategory>())
                .Select(c => c.CategoryID)
            );

            AssignedCategoryDataList = allCategories.Select(cat => new AssignedCategoryData
            {
                CategoryID = cat.ID,
                CategoryName = cat.CategoryName,
                Assigned = bookCatIds.Contains(cat.ID)
            }).ToList();
        }

        public void UpdateBookCategories(Sician_Diana_Lab2Context context,
            string[]? selectedCategories, Book bookToUpdate)
        {
            // ✅ asigurăm colecția (dacă e null, o inițializăm)
            bookToUpdate.BookCategories ??= new List<BookCategory>();

            if (selectedCategories == null)
            {
                // ✅ mai curat: curățăm colecția existentă
                bookToUpdate.BookCategories.Clear();
                return;
            }

            var selectedIds = selectedCategories.Select(int.Parse).ToHashSet();
            var currentIds = bookToUpdate.BookCategories.Select(c => c.CategoryID).ToHashSet();

            // adăugări
            foreach (var id in selectedIds.Except(currentIds))
            {
                bookToUpdate.BookCategories.Add(new BookCategory
                {
                    BookID = bookToUpdate.ID,
                    CategoryID = id
                });
            }

            // ștergeri
            foreach (var id in currentIds.Except(selectedIds).ToList())
            {
                var link = bookToUpdate.BookCategories.First(bc => bc.CategoryID == id);
                bookToUpdate.BookCategories.Remove(link);
            }
        }
    }
}


