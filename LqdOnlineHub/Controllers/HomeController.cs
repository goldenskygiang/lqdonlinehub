using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LqdOnlineHub.Models;
using Microsoft.Identity.Web;
using LqdOnlineHub.Extensions;
using LqdOnlineHub.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace LqdOnlineHub.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string id = User.GetObjectId();
            var profile = _db.Members.Find(id);
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserProfile model)
        {
            int status = -1;

            if (ModelState.IsValid)
            {
                model.Id = User.GetObjectId();
                model.Email = User.Identity.Name;
                model.LastEdit = DateTimeOffset.UtcNow;
                string jobtitle = User.GetJobTitle();
                if (int.TryParse(jobtitle, out int year))
                {
                    model.StartYear = year + GeneralConstants.MIN_AGE;
                }

                var user = await _db.Members.AsNoTracking().FirstOrDefaultAsync(u => u.Id == model.Id);

                if (user == null)
                {
                    _db.Members.Add(model);
                    status = 0;
                }
                else
                {
                    status = 0;

                    if (model.Grade != user.Grade || model.ClassName != user.ClassName)
                    {
                        model.Grade = user.Grade;
                        model.ClassName = user.ClassName;
                        status = 1;
                    }

                    _db.Members.Update(model);
                }

                _db.SaveChanges();
            }

            if (status == 0)
            {
                ViewData["StsMessage"] = "Dữ liệu đã được cập nhật";
                ViewData["StsClass"] = "text-success";

                _logger.LogInformation($"User {model.FullName} - {model.Grade} {model.ClassName} ({model.Email}) updated information successfully.");
            }
            else if (status == -1)
            {
                ViewData["StsMessage"] = "Đã có lỗi xảy ra, vui lòng thử lại.";
                ViewData["StsClass"] = "text-danger";
            }
            else if (status == 1)
            {
                ViewData["StsMessage"] = "Bạn không được đổi lớp học. Vui lòng liên hệ với quản trị viên hệ thống để sửa lại";
                ViewData["StsClass"] = "text-danger";
            }

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
