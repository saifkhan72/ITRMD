using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RMDWEB.Models;
using Microsoft.AspNetCore.Cors;
using System.Security.Cryptography;
using System.Diagnostics;
using RMDWEB.Interface;
using RMDWEB.Impl;

namespace RMDWEB.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SysConfigurationController : Controller
    {

        private IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserRoleController> _logger;
        private readonly InterfaceSystemConfig _iConfig;
        private static InterfaceFtt _ftt;

        public SysConfigurationController(IWebHostEnvironment environment,ILogger<UserRoleController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager=roleManager;
            _userManager=userManager;
            _logger=logger;
            _iConfig=new RepoSystemConfig();
            _env=environment;
            _ftt=new RepoFtt();


        }



        [HttpPost("uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file,[FromForm] string title, [FromForm] int tid)
        {
            if(file.Length>0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var rondom = Guid.NewGuid()+fileName;
                string ext = System.IO.Path.GetExtension(file.FileName);

                var currentuser = _userManager.FindByNameAsync(User.Identity.Name).Result;

                if(ext==".pdf"||ext==".jpg")
                {

                    string path = Path.Combine(_env.WebRootPath, "Content/ftt-document-files-scaned", rondom);

                    if(!Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    }

                    using(var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    FTTDocumentfile d = new FTTDocumentfile();
                    d.Title=title;
                    d.Created=DateTime.Now;
                    d.Updated=DateTime.Now;
                    d.CreatedBy=currentuser.UserId.Value;
                    d.UpdatedBy=currentuser.UserId.Value;
                    d.StatusId=_iConfig.AllStatus().SingleOrDefault(x => x.Name=="New").StatusId;
                    d.TID=tid;
                    d.DocImage=null;
                    d.DocImagePath=rondom;
                    _ftt.changeFttDocumentFile(d);
                    return Ok();
                }
            }
            return BadRequest();
        }



        [HttpPost("connection")]
        public async Task<string> connectionAsync(int? id)
        {
            return "success!"+id.Value;
        }


        [HttpGet("currencyList")]
        public JsonResult currencyList(int? id)
        {
            var data = _iConfig.AllCurrency();
            if(id!=null&&id!=0)
            {
                data=data.Where(a => a.CurrencyId==id).ToList();
            }
            return Json(data);

        }


        [HttpGet("getBanks")]
        public JsonResult ListOfBanks(int? id)
        {

            var data = _iConfig.AllBanks();
            List<getSelectItemsView> list = new List<getSelectItemsView>();
            if(data.Count()!=0)
            {
                foreach(var d in data)
                {
                    getSelectItemsView v = new getSelectItemsView();
                    v.id=d.BankId;
                    v.name=d.Name;
                    list.Add(v);
                }
            }
            return Json(list);
        }


        [HttpGet("getDepartments")]
        public JsonResult ListDepartments(int? id, int? pid)
        {
            var data = _iConfig.AllDepartments();

            if(pid!=null&&pid!=0)
            {
                data=data.Where(a => a.BankId==pid).ToList();
            }

            if(id!=0)
            {
                var current = _iConfig.AllDepartments().Single(a => a.DepartmentId==id);
                data=data.Where(a => a.BankId==current.BankId).ToList();
            }

            List<getSelectItemsView> list = new List<getSelectItemsView>();

            if(data.Count()!=0)
            {
                foreach(var d in data)
                {
                    getSelectItemsView v = new getSelectItemsView();
                    v.id=d.DepartmentId;
                    v.name=d.Name;
                    list.Add(v);
                }
            }
            return Json(list);

        }

        [HttpGet("getStatuss")]
        public JsonResult ListStatus(int? StatusId)
        {
            var data = _iConfig.AllStatus();
            if(StatusId!=null)
            {
                data=data.Where(a => a.StatusId==StatusId).ToList();
            }

            List<getSelectItemsView> list = new List<getSelectItemsView>();
            if(data.Count()!=0)
            {
                foreach(var d in data)
                {
                    getSelectItemsView v = new getSelectItemsView();
                    v.id=d.StatusId;
                    v.name=d.Name;
                    list.Add(v);
                }
            }
            return Json(list);
        }


        [HttpPost("changePasswordOfUser")]
        public async Task<string> changePasswordOfUserAsync(String UserId, string Password)
        {
            if(UserId==null||Password==null||Password.Length<8||Password.Length>15)
            {
                return "fail";
            }
            try
            {
                if(UserId==null||Password==null)
                {
                    return "null";
                }
                //Get User By Id
                int uId = int.Parse(UserId);
                var user = _userManager.Users.SingleOrDefault(a => a.UserId.Value==uId);

                if(user==null)
                {
                    return null;
                }

                //Generate Token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                //Set new Password
                var result = await _userManager.ResetPasswordAsync(user, token, Password);
                return result.ToString();
            }
            catch(Exception e)
            {
                return "fail"+e.Message;
            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId=Activity.Current?.Id??HttpContext.TraceIdentifier });
        }
    }
}
