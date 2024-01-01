using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMDWEB.Data;
using RMDWEB.Models;
using RMDWEB.Services.Impl;
using RMDWEB.Services.Interface;
using System.Data;

namespace RMDWEB.Controllers
{

    [Authorize(Roles = "sys-admin")]
    public class ActivityLogController : Controller
    {
        private readonly InterfaceActivityLog _activitylog;

        public ActivityLogController(ApplicationDbContext db)
        {
            _activitylog = new RepoActivityLog();

        }

        public IActionResult Index()
        {
            var activityLogs = _activitylog.AllLog();
            return View(activityLogs);
        }


        
    }
}
