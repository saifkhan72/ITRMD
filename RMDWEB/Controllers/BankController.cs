using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RMDWEB.Impl;
using RMDWEB.Interface;
using RMDWEB.Models;
using RMDWEB.Services.Impl;
using RMDWEB.Services.Interface;
using System.Data;

namespace RMDWEB.Controllers
{
    [Authorize(Roles = "sys-admin")]
    public class BankController : Controller
    {
        private readonly InterfaceActivityLog _activitylog;
        private readonly InterfaceBank _ibank;
        private readonly InterfaceStatus _istatus;
        public BankController()
        {
            _activitylog = new RepoActivityLog();
            _ibank = new RepoBank();
            _istatus = new RepoStatus();
        }
        public IActionResult Index()
        {
            //var bank = _ibank.AllBanks();

            ////add log
            //ActivityLog l = new()
            //{
            //    Activity = "UserRole/Banks",
            //    ActivityDate = DateTime.Now,
            //    UserName = User.Identity.Name,
            //    TableName = "Department",
            //    Detail = "Banks Viewed" + " " + bank.Count,
            //};
            //_activitylog.Add(l);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BankTbl data)
        {
            if (!ModelState.IsValid)
            {
                _ibank.changeBank(data);
                TempData["success"] = "Bank Created successfully!";

                // Add log for editing Banks
                ActivityLog l = new ActivityLog()
                {
                    Activity = "UserRole/Edit Banks",
                    ActivityDate = DateTime.Now,
                    UserName = User.Identity.Name,
                    TableName = "Bank",
                    Detail = "successfull Edit",
                };
                _activitylog.Add(l);

                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.MSG = "Bank transaction was not effected!";
                // Add log for editing Banks
                ActivityLog l = new ActivityLog()
                {
                    Activity = "UserRole/Edit Banks",
                    ActivityDate = DateTime.Now,
                    UserName = User.Identity.Name,
                    TableName = "Bank",
                    Detail = "unsuccessfull Edit",
                };
                _activitylog.Add(l);
            }
            ViewBag.Active = "Banks";

            ViewBag.StatusId = new SelectList(_ibank.AllStatus(), "StatusId", "Name", data.StatusId);
            return View(data);
        }
        public IActionResult Edit(int? id)
        {
            BankTbl b = new BankTbl();
            if (id != null && id != 0)
            {
                b = _ibank.AllBanks().SingleOrDefault(a => a.BankId == id.Value);
                ViewBag.StatusId = new SelectList(_ibank.AllStatus(), "StatusId", "Name", b.StatusId);

                // Add log for viewing banks edit
                ActivityLog l = new ActivityLog()
                {
                    Activity = "Bank/Add Banks",
                    ActivityDate = DateTime.Now,
                    UserName = User.Identity.Name,
                    TableName = "Bank",
                    Detail = "View Banks Insert",
                };
                _activitylog.Add(l);

            }
            else
            {
                ViewBag.StatusId = new SelectList(_ibank.AllStatus(), "StatusId", "Name");

            }
            ViewBag.MSG = TempData["MSG"];
            ViewBag.Active = "Banks";
            return View(b);
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetBanks()
        {
            var banks = _ibank.AllBanks();
            return Json(new { data = banks });
        }

        [HttpDelete]
        public IActionResult Delete(int? id) {

            if(id == null)
            {
                return Json(new { success = false, message = "Record not found!" });
            }
            var bank = _ibank.single(id.Value);
            if(bank != null)
            {
                bool result = _ibank.delete(bank);
                return Json(new { success = true, message = "Record deleted!" });
            }
            return Json(new { success = false, message = "Error while deleting" });
            
        }
        #endregion
    }
}
