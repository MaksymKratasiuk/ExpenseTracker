using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using teamProject.Data;

namespace teamProject.Pages.Category
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public teamProject.Models.Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _context.Categories.FindAsync(id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Category == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(Category.CategoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
