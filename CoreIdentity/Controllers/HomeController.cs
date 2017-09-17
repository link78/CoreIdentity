using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CoreIdentity.Controllers
{
 
    public class HomeController : Controller
    {
       [Authorize]
        public IActionResult Index()
        {
            return View(GetData(nameof(Index)));
        }

        [Authorize(Roles ="Users")]
        public IActionResult OtherAction() => View("Index", GetData(nameof(OtherAction)));

        private Dictionary<string, object> GetData(string actionName) =>

            new Dictionary<string, object>
            {
                ["Action"] = actionName,
                ["User"] = HttpContext.User.Identity.Name,
                ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
                ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
                ["In User Role"] = HttpContext.User.IsInRole("Users")
            };
        
    }
}