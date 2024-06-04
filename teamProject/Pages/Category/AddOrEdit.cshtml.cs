using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using teamProject.Data;

namespace teamProject.Pages.Category
{
    public class AddOrEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddOrEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public teamProject.Models.Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                Category = new teamProject.Models.Category();
            }
            else
            {
                Category = await _context.Categories.FindAsync(id);

                if (Category == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Category.CategoryId == 0)
            {
                _context.Categories.Add(Category);
            }
            else
            {
                _context.Categories.Update(Category);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
