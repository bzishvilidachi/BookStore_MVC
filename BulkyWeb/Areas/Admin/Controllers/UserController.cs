using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers
{
	 [Area("Admin")]
	 [Authorize(Roles = SD.Role_Admin)]
	public class UserController : Controller
	{
		
	
			
			private readonly UserManager<IdentityUser> _userManager;
			private readonly RoleManager<IdentityRole> _roleManager;
			private IUnitOfWork _unitOfWork;
			public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
				_roleManager = roleManager;
				_userManager=userManager;
			}
			public IActionResult Index()
			{
				return View();
			}


		public IActionResult RoleManagement(string id)
		{

			UserVM userVM = new UserVM()
			{
				ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == id, includeProperties:"Company"),
				RoleList = _roleManager.Roles.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Name
				}),

				CompanyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem
				{
					Text= i.Name,
					Value = i.Id.ToString()
				})
			};

			userVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == id))
				.GetAwaiter().GetResult().FirstOrDefault(); 
			return View(userVM);
		}
		[HttpPost]

		public IActionResult UpdateRole(UserVM userVM)
		{
			string oldRole = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == userVM.ApplicationUser.Id))
				.GetAwaiter().GetResult().FirstOrDefault();
			ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userVM.ApplicationUser.Id);
	
			if (!(userVM.ApplicationUser.Role == oldRole && userVM.ApplicationUser.CompanyId == applicationUser.CompanyId))
			{
				
				if (userVM.ApplicationUser.Role == SD.Role_Company)
				{
					applicationUser.CompanyId = userVM.ApplicationUser.CompanyId;
				}
				if (oldRole == SD.Role_Company && userVM.ApplicationUser.Role != SD.Role_Company)
				{
					applicationUser.CompanyId = null;
				}
				applicationUser.Name = userVM.ApplicationUser.Name;

				_unitOfWork.ApplicationUser.Update(applicationUser);
				_unitOfWork.Save();
				_userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
				_userManager.AddToRoleAsync(applicationUser, userVM.ApplicationUser.Role).GetAwaiter().GetResult();

				
			}
			
			return RedirectToAction("Index");
		}

		#region API CALLS
		[HttpGet]
			public IActionResult GetAll()
			{
				List<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll(includeProperties:"Company").ToList();
				

				foreach(var user in objUserList)
				{
					user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
					if (user.Company == null)
					{
						user.Company = new() { Name = "" };
					}
				}
				return Json(new { data = objUserList });
			}

		 

			[HttpPost]
		public IActionResult LockUnlock([FromBody] string id)
			{
				var objFromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
			if(objFromDb == null)
			{
				return Json(new {success = false, message = "Error while Locking/Unlocking"});
			}
			
			if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now) 
			{ 
				//user is currently locked
				objFromDb.LockoutEnd = DateTime.Now;
			}else
			{
				objFromDb.LockoutEnd =DateTime.Now.AddMinutes(30);
			}
			_unitOfWork.ApplicationUser.Update(objFromDb);
			_unitOfWork.Save();
				return Json(new { success = true, message = "Operation Successful" });
			}
		#endregion
	}
}


