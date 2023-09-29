using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab.Models;
using X.PagedList;

namespace Lab.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class AccountsController : Controller
    {
        private readonly BookStoreContext _context;

        public AccountsController(BookStoreContext context)
        {
            _context = context;
        }

        // GET: Admins/Accounts
        public async Task<IActionResult> Index(string? name, int page=1)
        {

          //  var a = await _context.Accounts.ToListAsync();
            // nếu có tham số name trên url
          //  if (!String.IsNullOrEmpty(name))
          //  {
          //      a = await _context.Accounts.Where(c => c.FullName.Contains(name)).ToListAsync();
          //  }

            int limit = 2;


            var a = await _context.Accounts.OrderBy(c => c.AccountId).ToPagedListAsync(page, limit);

            if (!String.IsNullOrEmpty(name))
            {
                a = await _context.Accounts.Where(c => c.FullName.Contains(name)).OrderBy(c => c.AccountId).ToPagedListAsync(page, limit);
            }


            ViewBag.keyword = name;
            return View(a);
        }

        // GET: Admins/Accounts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admins/Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Username,Password,FullName,Picture,Email,Address,Phone,IsAdmin,Active")] Account account)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    // upload ảnh vào thư mục wwwroot\\images\\Category var path = Path.Combine(Directory.GetCurrentDirectory(),
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\account", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        account.Picture = "/images/account/" + FileName; // gán tên
                    }

                }


               // account.CreatedDate = DateTime.Now;
                _context.Add(account);
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index));

            }
            return View(account);
        }
        

        // GET: Admins/Accounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Admins/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AccountId,Username,Password,FullName,Picture,Email,Address,Phone,IsAdmin,Active")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Admins/Accounts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admins/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'BookStoreContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(string id)
        {
          return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
