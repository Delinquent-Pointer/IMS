using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace IMS.Pages
{
    public class SalesModel : PageModel
    {
        private readonly AppDbContext _context;

        public SalesModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<SalesTransaction> SalesTransactions { get; set; } = new List<SalesTransaction>();

        public async Task OnGetAsync()
        {
            SalesTransactions = await _context.SalesTransactions
                .Include(t => t.Items)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }
    }

}
