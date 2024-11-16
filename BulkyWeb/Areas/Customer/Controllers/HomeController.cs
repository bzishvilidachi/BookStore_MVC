using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;
		

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			

			IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
			return View(productList);
		}

		public IActionResult Details(int id)
		{
			ShoppingCart cart = new()
			{ 
				Product = _unitOfWork.Product.Get(u=>u.Id==id, includeProperties: "Category,ProductImages"),
				Count=1,
				ProductId=id

			};
		   
			return View(cart);
		}

		public IActionResult ActionCategory(int id)
		{
			IEnumerable<Product> productList = _unitOfWork.Product.GetAll(u => u.CategoryId == id, includeProperties: "Category,ProductImages");
			return View(productList);
		}

		public IActionResult HistoryCategory(int id)
		{
			IEnumerable<Product> productList = _unitOfWork.Product.GetAll(u => u.CategoryId == id, includeProperties: "Category,ProductImages");
			return View(productList);
		}

		public IActionResult SciFiCategory(int id)
		{
			IEnumerable<Product> productList = _unitOfWork.Product.GetAll(u => u.CategoryId == id, includeProperties: "Category,ProductImages");
			return View(productList);
		}

		public IActionResult ViewAll(int id)
		{
			var categoryFromDb = _unitOfWork.Product.GetAll(u => u.CategoryId == id, includeProperties: "Category,ProductImages");
			return View(categoryFromDb);
		}


		public IActionResult NewlyAdded()
		{
			var productsFromDb = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages").OrderByDescending(p => p.AddDate).Take(15).ToList(); 
			return View(productsFromDb);
		}

		[HttpPost]
		[Authorize]
		public IActionResult Details(ShoppingCart shoppingCart)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			shoppingCart.ApplicationUserId = userId; 

			ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

			if(cartFromDb != null)
			{
				
				cartFromDb.Count += shoppingCart.Count;
				_unitOfWork.ShoppingCart.Update(cartFromDb);
				_unitOfWork.Save();
				
				
			}
			else
			{

				shoppingCart.Id = 0;
				_unitOfWork.ShoppingCart.Add(shoppingCart);
				_unitOfWork.Save();

				
				HttpContext.Session.SetInt32(SD.SessionCart,
					_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());

			}
			TempData["success"] = "Cart updated successfully";

			
			return RedirectToAction("Index");
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		

		public IActionResult AddToCart(int productId, int count)
		{
			if (!User.Identity.IsAuthenticated)
			{
				// Step 2: Redirect to the login page if not authenticated
				return RedirectToPage("/Account/Login", new
				{
					area = "Identity",
					ReturnUrl = Url.Action("AddToCart", "YourControllerName", new { productId, count })
				});
			}

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			// Create a new ShoppingCart object
			ShoppingCart shoppingCart = new ShoppingCart
			{
				ApplicationUserId = userId,
				ProductId = productId,
				Count = count // Use the value from the form submission
			};

			// Check if the item already exists in the cart
			ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == productId);

			if (cartFromDb != null)
			{
				// Update existing item count
				cartFromDb.Count += shoppingCart.Count;
				_unitOfWork.ShoppingCart.Update(cartFromDb);
				_unitOfWork.Save();
			}
			else
			{
				// Add new item to cart
				_unitOfWork.ShoppingCart.Add(shoppingCart);
				_unitOfWork.Save();

				// Update session cart count
				HttpContext.Session.SetInt32(SD.SessionCart,
					_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
			}

			TempData["success"] = "Cart updated successfully";
			return RedirectToAction("Index");
		}

	}
}
