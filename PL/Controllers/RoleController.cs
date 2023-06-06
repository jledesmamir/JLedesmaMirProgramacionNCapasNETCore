using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PL.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        public RoleController(RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Role rol = new ML.Role();
            rol.Roles = new List<object>();

            var Roles = roleManager.Roles.ToList();
            return View(Roles);
        }


        [HttpPost]
        public async Task<IActionResult> Form([Required] Microsoft.AspNetCore.Identity.IdentityRole rol)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(rol.Name));
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAll");
                }
                else
                {

                }
                //  Errors(result);
            }
            return View(rol);
        }


        public IActionResult Form(Guid? Id)
        {
            IdentityRole role = new IdentityRole();

            return View(role);
        }
    }
    //public ViewResult Index() => View(roleManager.Roles);

    //private void Errors(IdentityResult result)
    //{
    //    foreach (IdentityError error in result.Errors)
    //        ModelState.AddModelError("", error.Description);
    //}
}
