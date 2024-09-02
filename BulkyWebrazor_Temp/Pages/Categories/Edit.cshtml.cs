using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyWebRazor_Temp.Pages.Categories
{
	public class EditModel : PageModel
	{
		private readonly RazorAppDbContext _context;
		[BindProperty]
		public Category Category { get; set; }

		public EditModel(RazorAppDbContext context)
		{
			_context=context;
		}

		  public IActionResult OnGet(int? id)
	{
		if (id == null || id == 0)
		{
			return NotFound();
		}

		Category = _context.Categories.Find(id);

		if (Category == null)
		{
			return NotFound();
		}

		return Page();
	}

	public IActionResult OnPost()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		var categoryFromDb = _context.Categories.Find(Category.CategoryId);
		if (categoryFromDb == null)
		{
			return NotFound();
		}

		categoryFromDb.Name = Category.Name;
		categoryFromDb.DisplayOrder = Category.DisplayOrder;
		TempData["success"] = "Category updated successfully";
		_context.SaveChanges();
		return RedirectToPage("Index");
	}
	}
}

