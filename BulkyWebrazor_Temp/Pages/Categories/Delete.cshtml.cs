using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
	public class DeleteModel : PageModel
	{
 
			private readonly RazorAppDbContext _context;
			[BindProperty]
			public Category Category { get; set; }

			public DeleteModel(RazorAppDbContext context)
			{
				_context = context;
			}

			public void OnGet(int? id)
			{
				if (id != null || id != 0)
				{
				 Category = _context.Categories.Find(id);
					
				}

			}

			public IActionResult OnPost()
			{
				var categoryFromDb = _context.Categories.Find(Category.CategoryId);
				if (categoryFromDb == null)
				{
					return NotFound();
				}

				_context.Categories.Remove(categoryFromDb);
				_context.SaveChanges();
				TempData["success"] = "Category deleted successfully";
				return RedirectToPage("Index");
			}
		}
	}

