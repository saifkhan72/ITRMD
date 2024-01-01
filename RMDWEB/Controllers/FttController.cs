using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RMDWEB.Models;
using System.Data;
using PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using RMDWEB.Interface;
using RMDWEB.Impl;
using RMDWEB.Services.Interface;
using RMDWEB.Services.Impl;
using NuGet.Protocol.Plugins;

namespace RMDWEB.Controllers
{


    [Authorize(Roles = "ftt-payment,ftt-compliance,ftt-senior-managment,ftt-dab-officer,ftt-dab-manager,ftt-dab-dg-dy")]
    public class FttController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<FttController> _logger;
        private readonly InterfaceSystemConfig _iConfig;
        private readonly InterfaceFtt _ftt;
        private readonly InterfaceActivityLog _activitylog;

        public FttController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment, ILogger<FttController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager=roleManager;
            _userManager=userManager;
            _logger=logger;
            _iConfig=new RepoSystemConfig();
            _ftt=new RepoFtt();
            _hostingEnvironment=environment;
            _httpContextAccessor=httpContextAccessor;
            _activitylog = new RepoActivityLog();
        }




        public async Task<IActionResult> Index()
        {
            var currentU = await _userManager.FindByNameAsync(User.Identity.Name);
            string IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
           
            if(currentU.BankId==null)
            {
                ViewBag.Active="ftt";
                ViewBag.MSG="no bank is configured for the user";
                ActivityLog l = new ActivityLog();
                l.Activity = "Ftt/Index";
                l.ActivityDate = DateTime.Now;
                l.UserName = User.Identity.Name;
                l.TableName = "null";
                l.Detail = "no bank was configured";
                _activitylog.Add(l);
                return View(null);
            }

            if(User.IsInRole("ftt-payment")||User.IsInRole("ftt-compliance")||User.IsInRole("ftt-senior-managment"))
            {

                var data = _ftt.AllFttTransactionByBank(currentU.BankId.Value);
                data=data.Where(a => a.StatusTbl.Name=="Created"||a.StatusTbl.Name=="Authorized").ToList();
                if (User.IsInRole("ftt-compliance"))
                {
                    data = data.Where(a => a.StatusTbl.Name == "Created").ToList();
                }

                if (User.IsInRole("ftt-senior-managment"))
                {
                    data = data.Where(a => a.StatusTbl.Name == "Authorized").ToList();
                }

                if (data.Count()==0)
                {
                    ViewBag.MSG=TempData["MSG"];
                }
                else
                {
                    ViewBag.MSG=TempData["MSG"];
                }
                ActivityLog l = new ActivityLog();
                l.Activity = "Ftt/Index";
                l.ActivityDate = DateTime.Now;
                l.UserName = User.Identity.Name;
                l.TableName = "Ftt";
                l.Detail = "list ftt " + data.Count();
                _activitylog.Add(l);
                ViewBag.Active = "ftt";
                return View(data);
            }
            else
            {

                ActivityLog l = new ActivityLog();
                if (IP!=currentU.IpAddress)
                {
                   
                    l.Activity = "Ftt/Index";
                    l.ActivityDate = DateTime.Now;
                    l.UserName = User.Identity.Name;
                    l.TableName = "Ftt";
                    l.Detail = "IP address not valid";
                    _activitylog.Add(l);
                    return Content("IP not valid!");

                    

                }
                else if(User.IsInRole("ftt-dab-officer") ||
                    User.IsInRole("ftt-dab-manager") ||
                    User.IsInRole("ftt-dab-dg-dy"))
                {
                    var data = _ftt.AllFttTransaction().ToList();

                    if (User.IsInRole("ftt-dab-officer"))
                    {
                        data = data.Where(a => a.StatusTbl.Name == "Approved").ToList();
                    }

                    if (User.IsInRole("ftt-dab-manager"))
                    {
                        data = data.Where(a => a.StatusTbl.Name == "Checked").ToList();
                    }

                    if (User.IsInRole("ftt-dab-dg-dy"))
                    {
                        data = data.Where(a => a.StatusTbl.Name == "Processed").ToList();
                    }

                    ViewBag.MSG = TempData["MSG"]; 
                    l.Activity = "Ftt/Index";
                    l.ActivityDate = DateTime.Now;
                    l.UserName = User.Identity.Name;
                    l.TableName = "Ftt";
                    l.Detail = "ftt list " + data.Count();
                    _activitylog.Add(l);
                    ViewBag.Active = "ftt";
                    return View(data);

                }
                else
                {
                    l.Activity = "Ftt/Index";
                    l.ActivityDate = DateTime.Now;
                    l.UserName = User.Identity.Name;
                    l.TableName = "Ftt";
                    l.Detail = "not role matched";
                    _activitylog.Add(l);
                    ViewBag.Active = "ftt";
                    return View(null);
                }
            }


        }

        public IActionResult IndexArchive()
        {

            string IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var currentU = _userManager.FindByNameAsync(User.Identity.Name).Result;

            ActivityLog l = new ActivityLog();
            List<FTTTransaction> data = new List<FTTTransaction>();
            if(currentU.BankId==null)
            {
                ViewBag.MSG="no bank is configured for the user";
                ViewBag.Active="ftta";
                return View("Index", data); ;
            }

            if(User.IsInRole("ftt-payment")||User.IsInRole("ftt-compliance")||User.IsInRole("ftt-senior-managment"))
            {


                var fetchedData = _ftt.AllFttTransactionByBank(currentU.BankId.Value).ToList();

                foreach(var d in fetchedData)
                {
                    FTTTransaction f = new FTTTransaction();
                    f=d;
                    f.StatusTbl=_iConfig.singelStatus(d.StatusId);
                    f.CurrencyTbl=_iConfig.singelCurrency(d.CurrencyId);
                    data.Add(f);

                }

                data=data.Where(a => a.StatusTbl.Name!="Created"&&a.StatusTbl.Name!="Authorized").ToList();


                if(data.Count()==0)
                {
                    ViewBag.MSG=TempData["MSG"];
                }
                else
                {
                    ViewBag.MSG=TempData["MSG"];
                }
            }
            else
            {
                if(IP!=currentU.IpAddress)
                {
                    return Content("ip not valid!");
                }
                data=_ftt.AllFttTransaction().ToList();
                ViewBag.MSG=TempData["MSG"];
            }

            ViewBag.Active="ftta";

            l.Activity = "Ftt/Archive";
            l.ActivityDate = DateTime.Now;
            l.UserName = User.Identity.Name;
            l.TableName = "Ftt";
            l.Detail = "List Archive data";
            _activitylog.Add(l);

            return View("Index", data);

        }


        [Authorize(Roles = "ftt-payment")]
        public IActionResult Change(int id)
        {
            FttTransactionView tbl = new FttTransactionView();
            var currentU = _userManager.FindByNameAsync(User.Identity.Name).Result;
            ViewBag.CurrencyId=new SelectList(_iConfig.AllCurrency(), "CurrencyId", "CurSign");
            if(id!=0)
            {
                var data = _ftt.AllFttTransactionByBank(currentU.BankId.Value).SingleOrDefault(a => a.TID==id);
                if(data.UserId!=currentU.Id)
                {
                    TempData["error"]="Bad request!";
                    return View(tbl);

                }
                tbl.TID=data.TID;
                tbl.Comment=data.Comment;
                tbl.BankLetterId=data.BankLetterId;
                tbl.TTDate=data.TTDate;
                tbl.TTNumber=data.TTNumber;
                tbl.TTAmount=data.TTAmount;
                tbl.SenderName=data.SenderName;
                tbl.BenBank=data.BenBank;
                tbl.PurposeTransaction=data.PurposeTransaction;
                tbl.BenCountry=data.BenCountry;
                tbl.BenCompany=data.BenCompany;
                tbl.CurrencyId=data.CurrencyId;
                tbl.InvoiceContractNo=data.InvoiceContractNo;
                ViewBag.CurrencyId=new SelectList(_iConfig.AllCurrency(), "CurrencyId", "CurSign", tbl.CurrencyId);
                if(tbl==null)
                {
                    return View(tbl);
                }
                else
                {
                    return View(tbl);
                }
            }
            ViewBag.MSG=TempData["MSG"];
            return View(tbl);
        }


        [HttpPost("Ftt/postComment")]
        public async Task<int> postComment([FromBody] postCommentView json)
        {

            //string fttcommentid, string Comment, string TID, string StatusId

            int TIDInt = json.TID; // int.Parse(json.TID);
            int StatusIdInt = json.StatusId;
            int fttcommentidInt = json.fttcommentid;
            string Comment = json.Comment;


            var currentU = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var sts = new List<FttStatusView>();
            sts.Add(new FttStatusView { Name="Info", StatusId=1 });
            sts.Add(new FttStatusView { Name="Return", StatusId=2 });
            sts.Add(new FttStatusView { Name="Reject", StatusId=3 });
            var userStatus = sts.SingleOrDefault(a => a.StatusId==StatusIdInt);
            if(userStatus.Name=="Return")
            {
                if(User.IsInRole("ftt-dab-manager")||User.IsInRole("ftt-dab-officer")||User.IsInRole("ftt-dab-dg-dy"))
                {
                    var ftttransaction = _ftt.singelFttTransaction(TIDInt);
                    if(ftttransaction!=null&&ftttransaction.StatusTbl.Name!="Rejected"&&ftttransaction.StatusTbl.Name!="Accepted")
                    {
                        ftttransaction.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Created").StatusId;
                        ftttransaction.UpdatedBy=currentU.UserId;
                        ftttransaction.Updated=DateTime.Now;
                        _ftt.Change(ftttransaction);
                    }

                }
                else
                {
                    var ftttransaction = _ftt.singelFttTransaction(TIDInt);
                    if(ftttransaction!=null&&ftttransaction.BankId==currentU.BankId.Value&&ftttransaction.StatusTbl.Name!="Rejected"&&ftttransaction.StatusTbl.Name!="Accepted")
                    {
                        ftttransaction.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Created").StatusId;
                        ftttransaction.UpdatedBy=currentU.UserId;
                        ftttransaction.Updated=DateTime.Now;
                        _ftt.Change(ftttransaction);
                    }
                }
            }
            else if(userStatus.Name=="Reject")
            {
                if(User.IsInRole("ftt-dab-manager")||User.IsInRole("ftt-dab-officer")||User.IsInRole("ftt-dab-dg-dy"))
                {
                    var ftttransaction = _ftt.singelFttTransaction(TIDInt);
                    if(ftttransaction!=null&&ftttransaction.StatusTbl.Name!="Rejected"&&ftttransaction.StatusTbl.Name!="Accepted")
                    {
                        ftttransaction.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Rejected").StatusId;
                        ftttransaction.UpdatedBy=currentU.UserId;
                        ftttransaction.Updated=DateTime.Now;
                        _ftt.Change(ftttransaction);
                    }
                }
                else
                {
                    var ftttransaction = _ftt.singelFttTransaction(TIDInt);
                    if(ftttransaction!=null&&ftttransaction.BankId==currentU.BankId.Value&&ftttransaction.StatusTbl.Name!="Rejected"&&ftttransaction.StatusTbl.Name!="Accepted")
                    {
                        ftttransaction.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Rejected").StatusId;
                        ftttransaction.UpdatedBy=currentU.UserId;
                        ftttransaction.Updated=DateTime.Now;
                        _ftt.Change(ftttransaction);
                    }
                }

            }
            FTTComment c = new FTTComment();
            c.UserId=currentU.Id;
            c.Created=DateTime.Now;
            c.comment=Comment;
            c.TID=TIDInt;
            c.StatusId=StatusIdInt;
            _ftt.changeComment(c);
            TempData["success"]="Comment added";

            ActivityLog l = new()
            {
                Activity = "Ftt/Archive",
                ActivityDate = DateTime.Now,
                UserName = User.Identity.Name,
                TableName = "Ftt",
                Detail = "Comment added"
            };
            _activitylog.Add(l);
            return 1;

        }


        [Authorize(Roles = "ftt-payment")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Change(FttTransactionView tbl)
        {
            try
            {

                ViewBag.CurrencyId=new SelectList(_iConfig.AllCurrency(), "CurrencyId", "CurSign", tbl.CurrencyId);
                if(!ModelState.IsValid)
                {/*
                string msg = "test---";
                foreach(var error in ModelState.Values)
                {
                    foreach(var err in error.Errors)
                    {
                        msg+=err.ErrorMessage;
                    }
                }*/
                    TempData["error"]="Bad request data!";
                    return View(tbl);
                }


                var currentU = await _userManager.FindByNameAsync(User.Identity.Name);

                if(currentU.BankId==null)
                {
                    TempData["error"]="User has no bank!";
                    return View(tbl);
                }


                //actual transaction
                FTTTransaction t = new FTTTransaction();
                if(tbl.TID!=0)
                {
                    t=_ftt.singelFttTransaction(tbl.TID);

                    if(t==null||t.BankId!=currentU.BankId)
                    {
                        TempData["error"]="Request is not authenticated!";
                        return View(tbl);
                    }



                }

                t.BankLetterId=tbl.BankLetterId;
                t.TTDate=tbl.TTDate;
                t.TTNumber=tbl.TTNumber;
                t.TTAmount=tbl.TTAmount;
                t.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Created").StatusId;
                t.UserId=currentU.Id;
                t.Created=DateTime.Now;
                t.Updated=DateTime.Now;
                t.CreatedBy=currentU.UserId.Value;
                t.UpdatedBy=currentU.UserId.Value;
                t.InvoiceContractNo=tbl.InvoiceContractNo;
                t.SenderName=tbl.SenderName;
                t.BenBank=tbl.BenBank;
                t.PurposeTransaction=tbl.PurposeTransaction;
                t.BenCountry=tbl.BenCountry;
                t.BenCompany=tbl.BenCompany;
                t.CurrencyId=tbl.CurrencyId;
                t.Comment=tbl.Comment;
                t.DepartmentId=currentU.DepartmentId;
                t.BankId=currentU.BankId;

                t=_ftt.Change(t);


                if(tbl.TID==0)
                {
                    TempData["success"]="New Transaction added!";
                    return RedirectToAction("Details", new { id = t.TID });
                }
                else
                {
                    TempData["success"]="Update was done!";
                    return RedirectToAction("Change", new { id = t.TID });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in MyClass");
                return RedirectToAction("Change", new { id = 0 });

            }



        }

        public async Task<IActionResult> Details(int id)
        {
            string IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();


            if(id==0) return NotFound();
            var currentU = await _userManager.FindByNameAsync(User.Identity.Name);

            if(currentU.BankId==null)
            {

                TempData["MSG"]="user is not registered with any bank";
                return RedirectToAction("Index");
            }

            FTTTransaction tbl = new FTTTransaction();

            if(User.IsInRole("ftt-dab-manager")||User.IsInRole("ftt-dab-dg-dy")||User.IsInRole("ftt-dab-officer"))
            {
                if(IP!=currentU.IpAddress) return NoContent();
                tbl=_ftt.singelFttTransaction(id);
            }
            else
            {
                tbl=_ftt.singelFttTransactionByBankId(id, currentU.BankId.Value);
                if(tbl==null)
                {
                    TempData["MSG"]="no record found";
                    return RedirectToAction("Index");
                }

                ActivityLog l = new ActivityLog();
                l.Activity = "Ftt/Archive";
                l.ActivityDate = DateTime.Now;
                l.UserName = User.Identity.Name;
                l.TableName = "Ftt";
                l.Detail = "Archive details viewd";
                _activitylog.Add(l);
            }

            if(tbl==null) return NoContent();
            var doc = _ftt.AllDocumentFileByFttId(tbl.TID).Where(a => a.StatusTbl.Name=="Active").ToList();
           // tbl.StatusTbl=_iConfig.singelStatus(tbl.StatusId);
           // tbl.CurrencyTbl=_iConfig.singelCurrency(tbl.CurrencyId);
            //tbl.DepartmentTbl=_iConfig.singleDepartment(tbl.DepartmentId);


            var sts = new List<FttStatusView>(){
                new FttStatusView { Name="Info", StatusId=1 },
                new FttStatusView { Name="Return", StatusId=2 },
                new FttStatusView { Name="Reject", StatusId=3 }
            };
            ViewBag.StatusId=new SelectList(sts, "StatusId", "Name");
            var mdl = new FttDetails() { FTTTransaction=tbl, FTTDocumentfile=doc };
            //ViewBag.MSG=TempData["MSG"]??"update transaction!";
            return View(mdl);
        }

        public async Task<FileResult> DownloadFttFile(string file, int id)
        {
            var currentU = await _userManager.FindByNameAsync(User.Identity.Name);
            string IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();


            var data = _ftt.singelFttDocument(id);
            if(data.DocImagePath!=file)
            {
                return null;
            }

            if(data!=null)
            {
                var FileVirtualPath = "~/Content/ftt-document-files-scaned/"+data.DocImagePath;
                return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
            }
            else
            {
                var FileVirtualPath = "";
                return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
            }

        }



        [Authorize]
        [HttpGet]
        public async Task<string> ChangeFttFile(int id, int Status)
        {



            var currentU = await _userManager.FindByNameAsync(User.Identity.Name);


            //string IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            try
            {
                var data = _ftt.singelFttDocument(id);

                if(data !=null)
                {

                    if(data.StatusTbl.Name!="New"||data.CreatedBy!=currentU.UserId.Value)
                    {
                        return "fail";
                    }

                    _ftt.DeleteFttDocument(data, Status);

                    return "success";
                }
                else
                {
                    return "no file";
                }


            }
            catch(Exception e)
            {
                return e.StackTrace + e.Message;
            }

        }



        public async Task<IActionResult> ChangeFttTransaction(int TID, int StatusId)
        {

            var currentU = await _userManager.FindByNameAsync(User.Identity.Name);
            var check = _ftt.singelFttTransaction(TID);


            string IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if(check.StatusTbl.Name=="Canceled"||check.StatusTbl.Name=="Rejected"||check.StatusTbl.Name=="Accepted")
            {
                return NotFound();
            }
            if(StatusId==0)
            {
                //check if the transaction is "New"
                if(check.StatusTbl.Name=="Created"&&check.CreatedBy==currentU.UserId)
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Canceled").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);
                }
            }

            if(StatusId==1)
            {
                //check if the transaction is "New" 
                if(User.IsInRole("ftt-dab-dg-dy")&&check.StatusTbl.Name=="Processed")
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Rejected").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);

                }

                if(User.IsInRole("ftt-dab-manager")&&check.StatusTbl.Name=="Checked")
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Rejected").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);

                }

                if(User.IsInRole("ftt-dab-officer")&&check.StatusTbl.Name=="Approved")
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Rejected").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);

                }

                if(check.BankId==currentU.BankId&&User.IsInRole("ftt-senior-managment")&&check.StatusTbl.Name=="Authorized")
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Rejected").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);
                }

                if(check.BankId==currentU.BankId&&User.IsInRole("ftt-compliance")&&check.StatusTbl.Name=="Created")
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Rejected").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);
                }

            }

            if(StatusId==2&&User.IsInRole("ftt-compliance"))
            {
                //check if the transaction is "New"
                if(check.StatusTbl.Name=="Created"&&check.BankId==currentU.BankId)
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Authorized").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);


                }
            }

            if(StatusId==3&&User.IsInRole("ftt-senior-managment"))
            {
                //check if the transaction is "New"
                if(check.StatusTbl.Name=="Authorized"&&check.BankId==currentU.BankId)
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Approved").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);
                }
            }

            if(StatusId==4&&User.IsInRole("ftt-dab-officer")&&IP==currentU.IpAddress)
            {
                //check if the transaction is "New"

                if(check.StatusTbl.Name=="Approved")
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Checked").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);
                }
            }

            if(StatusId==5&&User.IsInRole("ftt-dab-manager")&&IP==currentU.IpAddress)
            {
                //check if the transaction is "New"
                if(check.StatusTbl.Name=="Checked")
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Processed").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);
                }
            }

            if(StatusId==6&&User.IsInRole("ftt-dab-dg-dy")&&IP==currentU.IpAddress)
            {
                //check if the transaction is "New"
                if(check.StatusTbl.Name=="Processed")
                {
                    check.StatusId=_iConfig.AllStatus().SingleOrDefault(a => a.Name=="Accepted").StatusId;
                    check.UpdatedBy=currentU.UserId;
                    _ftt.changeFttStatus(check, currentU.Id);
                }
            }

            TempData["MSG"]="Change on FTT!";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<JsonResult> allComment(int? id)
        {
            if(id==null||id==0)
            {
                return null;
            }
            var data = _ftt.AllFttComment(id.Value);
            var data2 = new List<FttCommentView>();


            if(data.Count()!=0)
            {
                foreach(var d in data.OrderByDescending(a => a.fttcommentid))
                {
                    string StatusId = "";
                    var usr = _userManager.FindByIdAsync(d.UserId).Result;
                    if(d.StatusId==1)
                    {
                        StatusId="Info";
                    }
                    else if(d.StatusId==3)
                    {
                        StatusId="Return";
                    }
                    else if(d.StatusId==3)
                    {
                        StatusId="Reject";
                    }

                    FttCommentView f = new FttCommentView();
                    f.comment=d.comment;
                    f.UserId=usr.FirstName;
                    f.Created=d.Created.ToShortDateString();
                    f.StatusId=StatusId;
                    f.fttcommentid=d.fttcommentid;
                    f.TID=d.TID;
                    data2.Add(f);

                }

            }
            return this.Json(data2);
        }

        [HttpGet]
        public JsonResult allDocuments(int? id)
        {
            if(id==null||id==0)
            {
                return null;
            }
            var data = _ftt.AllFttDocument(id.Value);
            var data2 = new List<FttDocumentView>();
            if(data.Count()!=0)
            {
                foreach(var d in data.OrderByDescending(a => a.DocId))
                {
                    FttDocumentView f = new FttDocumentView();
                    f.id=d.DocId;
                    f.path=d.DocImagePath;
                    f.title=d.Title;
                    f.StatusId=d.StatusTbl.Name;
                    data2.Add(f);
                }
            }
            return this.Json(data2);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId=Activity.Current?.Id??HttpContext.TraceIdentifier });
        }
    }
}
