using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RMDWEB.Models;
using RMDWEB.Services.Impl;
using RMDWEB.Services.Interface;
using System.Data;

namespace RMDWEB.Controllers
{
    [Authorize(Roles = "sys-admin")]
    public class DepartmentController : Controller
    {
        private readonly InterfaceDepartment _idepartment;
        private readonly InterfaceActivityLog _activitylog;
        private readonly InterfaceBank _ibank;
        private readonly InterfaceStatus _istatus ;
        public DepartmentController()
        {
            _idepartment = new RepoDepartment();
            _activitylog = new RepoActivityLog();
            _ibank = new RepoBank();
            _istatus = new RepoStatus(); ;
        }
        public IActionResult Index()
        {
            var data = _idepartment.AllDepartments();
            ViewBag.MSG = TempData["MSG"];
            ViewBag.Active = "Departments";

            //add log

            ActivityLog l = new()
            {
                Activity = "UserRole/Department",
                ActivityDate = DateTime.Now,
                UserName = User.Identity.Name,
                TableName = "Department",
                Detail = "Department Viewed" + " " + data.Count,
            };
            _activitylog.Add(l);


            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DepartmentTbl data)
        {
            if (!ModelState.IsValid)
            {
                _idepartment.changeDepartment(data);
                TempData["success"] = "Department added successfully!";
                // Add log for editing Banks
                ActivityLog l = new ActivityLog()
                {
                    Activity = "UserRole/Edit Department",
                    ActivityDate = DateTime.Now,
                    UserName = User.Identity.Name,
                    TableName = "Department",
                    Detail = "successfull Department Edit",
                };
                _activitylog.Add(l);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.MSG = "department transaction was not effected!";
                // Add log for editing Banks
                ActivityLog l = new ActivityLog()
                {
                    Activity = "UserRole/Edit Department",
                    ActivityDate = DateTime.Now,
                    UserName = User.Identity.Name,
                    TableName = "Department",
                    Detail = "successfull Department Edit",
                };
                _activitylog.Add(l);

            }
            ViewBag.Active = "Departments";
            ViewBag.BankId = new SelectList(_ibank.AllBanks(), "BankId", "Name", data.BankId);
            ViewBag.StatusId = new SelectList(_istatus.AllStatus(), "StatusId", "Name", data.StatusId);

            return View(data);
        }
        public IActionResult Edit(int? id)
        {
            DepartmentTbl d = new DepartmentTbl();
            if (id != null && id != 0)
            {
                d = _idepartment.AllDepartments().SingleOrDefault(a => a.DepartmentId == id.Value);
                ViewBag.StatusId = new SelectList(_istatus.AllStatus(), "StatusId", "Name", d.StatusId);
                ViewBag.BankId = new SelectList(_ibank.AllBanks(), "BankId", "Name", d.BankId);

                // Add log for viewing Department edit
                ActivityLog l = new ActivityLog()
                {
                    Activity = "UserRole/Edit Department",
                    ActivityDate = DateTime.Now,
                    UserName = User.Identity.Name,
                    TableName = "Bank",
                    Detail = "View Department Edit",
                };
                _activitylog.Add(l);
            }
            else
            {
                ViewBag.StatusId = new SelectList(_istatus.AllStatus(), "StatusId", "Name");
                ViewBag.BankId = new SelectList(_ibank.AllBanks(), "BankId", "Name");
            }
            ViewBag.MSG = TempData["MSG"];
            ViewBag.Active = "Departments";
            return View(d);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetDepartment()
        {
            var department = _idepartment.AllDepartments();
            return Json(new {data = department});   
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Record not found!" });
            }

            var department = _idepartment.singleDepartment(id.Value);

            if (department != null)
            {
                bool result = _idepartment.Delete(department);

                if (result)
                {
                    return Json(new { success = true, message = "Record deleted" });
                }
                else
                {
                    return Json(new { success = false, message = "Error occurred while deleting" });
                }
            }

            return Json(new { success = false, message = "Record not found!" });
        }

        #endregion
    }
}
