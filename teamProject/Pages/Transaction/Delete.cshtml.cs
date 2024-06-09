using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using teamProject.Data;

namespace teamProject.Pages.Transaction
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public teamProject.Models.Transaction Transaction { get; set; }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Transaction = await _context.Transactions.FindAsync(id);

            if (Transaction != null)
            {
                _context.Transactions.Remove(Transaction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
