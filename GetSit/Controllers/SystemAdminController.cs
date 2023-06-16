using GetSit.Data.ViewModels;
using GetSit.Data.enums;
using GetSit.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using GetSit.Data;
using Microsoft.Win32;
using GetSit.Data.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using GetSit.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Routing;
using GetSit.Data.Services;
namespace GetSit.Controllers
{
    public class SystemAdminController
    {
        private readonly AppDBcontext _context;
        private readonly IUserManager _userManager;
        public SystemAdminController(AppDBcontext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Tuple<int,int>> JoinRequest = new List<Tuple<int,int>>();
            foreach (var space in _context.Space)
            {
                if (space.IsApproved==false)
                {
                    int ProviderId = _context.SpaceEmployee.Where(i => i.SpaceId == space.Id).FirstOrDefault().Id;
                    JoinRequest.Add(Tuple.Create(ProviderId , space.Id)); 
                }
            }
            List<Tuple<int, int>> HallRequest = new List<Tuple<int, int>>();
            
            return View();
        }
    }
}
