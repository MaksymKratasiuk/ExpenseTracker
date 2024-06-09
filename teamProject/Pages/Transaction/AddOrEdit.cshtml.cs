using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using teamProject.Data;

namespace teamProject.Pages.Transaction
{
    public class AddOrEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddOrEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public teamProject.Models.Transaction Transaction { get; set; }
        public SelectList Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            PopulateCategories();
            if (id == null)
            {
                Transaction = new teamProject.Models.Transaction();
                return Page();
            }

            Transaction = await _context.Transactions.FindAsync(id);

            if (Transaction == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateCategories();
                return Page();
            }

            if (Transaction.TransactionId == 0)
            {
                _context.Transactions.Add(Transaction);
            }
            else
            {
                _context.Transactions.Update(Transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        private void PopulateCategories()
        {
            var categories = _context.Categories.ToList();
            Categories = new SelectList(categories, "CategoryId", "TitleWithIcon");
        }
    }
}
