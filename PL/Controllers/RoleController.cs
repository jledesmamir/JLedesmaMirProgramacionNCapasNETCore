using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ML;
using System.ComponentModel.DataAnnotations;
using System.Data;

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

            //Terminar Update

            //Hace Update
            //IdentityRole role= new IdentityRole();
            //role.Id = await roleManager.GetRoleIdAsync(rol);
            //role.Name = await roleManager.GetRoleNameAsync(rol);

            //await roleManager.UpdateAsync(role);
            //return View(rol);

            if (ModelState.IsValid)
            {
                IdentityRole role = await roleManager.FindByIdAsync(rol.Id.ToString());
                //Add o Insert
                if (role == null)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(rol.Name));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetAll");
                    }
                    else
                    {

                    }
                }
                else //Update
                {
                    role.Id = await roleManager.GetRoleIdAsync(rol);
                    role.Name = await roleManager.GetRoleNameAsync(rol);

                    IdentityResult result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetAll");
                    }
                }
            }
            return View(rol);
        }

        [HttpGet]
        public IActionResult Form(Guid IdRole, string Name)
        {
            IdentityRole role = new IdentityRole();
            if (Name != null)
            {
                role.Name = Name;
                role.Id = IdRole.ToString();
                return View(role);
            }
            else
            {
                return View(role);
            }
            
        }

        public async Task<IActionResult> Delete(Guid IdRole)
        {
            IdentityRole role = await roleManager.FindByIdAsync(IdRole.ToString());
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("GetAll");
                //else
                //    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("GetAll", roleManager.Roles);
        }
    }
}
