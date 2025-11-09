using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dinu_Alexandra_Lab2.Data;
using Dinu_Alexandra_Lab2.Models;

namespace Dinu_Alexandra_Lab2.Pages.Members
{
    public class DetailsModel : PageModel
    {
        private readonly Dinu_Alexandra_Lab2.Data.Dinu_Alexandra_Lab2Context _context;

        public DetailsModel(Dinu_Alexandra_Lab2.Data.Dinu_Alexandra_Lab2Context context)
        {
            _context = context;
        }

        public Member Member { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }
            else
            {
                Member = member;
            }
            return Page();
        }
    }
}
