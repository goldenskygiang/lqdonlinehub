using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using LqdOnlineHub.Attributes;
using LqdOnlineHub.Data;
using LqdOnlineHub.Extensions;
using LqdOnlineHub.Models;
using LqdOnlineHub.ViewModels.Member;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace LqdOnlineHub.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MemberController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index([FromQuery] int? grade, int? className)
        {
            var user = _db.Members.Find(User.GetObjectId());

            if (user == null)
            {
                return Forbid();
            }

            Grade gr;
            Class cn;

            bool allowGet = false;

            if (!grade.HasValue || !className.HasValue)
            {
                gr = user.Grade;
                cn = user.ClassName;
                allowGet = true;
            }
            else
            {
                gr = (Grade)Enum.Parse(typeof(Grade), grade.Value.ToString());
                cn = (Class)Enum.Parse(typeof(Class), className.Value.ToString());

                if (User.IsInRole(ApplicationRoles.Administrator) ||
                    User.IsInRole(ApplicationRoles.YouthUnion) ||
                    (gr == user.Grade && cn == user.ClassName))
                {
                    allowGet = true;
                }
            }

            var students = allowGet ? _db.Members.AsNoTracking().Where(u => u.Grade == gr && u.ClassName == cn).ToList() :
                new List<UserProfile>();

            var vm = new MemberIndexViewModel()
            {
                SelectedGrade = gr,
                SelectedClass = cn,
                Downloadable = (students.Count > 0) && !User.IsInRole(ApplicationRoles.YouthUnion),
                Members = students,
                EmptyMessage = allowGet ? "Không có trong danh sách." : "Bạn không được coi thông tin này."
            };

            return View(vm);
        }

        public FileContentResult ExportList([FromQuery] int grade, int className)
        {
            Grade gr = (Grade)Enum.Parse(typeof(Grade), grade.ToString());
            Class cn = (Class)Enum.Parse(typeof(Class), className.ToString());

            if (!User.IsInRole(ApplicationRoles.Administrator) &&
                !User.IsInRole(ApplicationRoles.YouthUnion))
            {
                var user = _db.Members.Find(User.GetObjectId());
                if (user.ClassName != cn || user.Grade != gr) return null;
            }

            var students = _db.Members.AsNoTracking()
                .Where(u => u.Grade == gr && u.ClassName == cn)
                .ToList();

            var excel = _generateExcel(students);

            var file = new FileContentResult(excel.GetAsByteArray(), GeneralConstants.EXCEL_CONTENT_TYPE);
            file.FileDownloadName = $"{gr.ToDisplayName()} {cn.ToDisplayName()}.xlsx";

            return file;
        }

        private ExcelPackage _generateExcel(IEnumerable<UserProfile> profiles)
        {
            ExcelPackage pck = new ExcelPackage();

            //Create the worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet 1");

            // Sets header
            int col = 1;
            foreach (var prop in typeof(UserProfile).GetProperties())
            {
                ws.Cells[1, col].Value = prop.GetDisplayName();
                col++;
            }

            // Inserts Data
            const int SPACE = 2;
            for (int i = 0; i < profiles.Count(); i++)
            {
                col = 1;
                var prof = profiles.ElementAt(i);

                foreach (var prop in prof.GetType().GetProperties())
                {
                    var value = prop.GetValue(prof);
                    if (prop.PropertyType.IsEnum)
                    {
                        value = prop.EnumGetDisplayName(prof);
                    }
                    else if (prop.PropertyType == typeof(DateTimeOffset))
                    {
                        DateTimeOffset dto = (DateTimeOffset)value;
                        value = dto.ToString("o");
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        DateTime dt = (DateTime)value;
                        value = dt.ToString("dd/MM/yyyy");
                    }
                    else if (prop.Name == nameof(UserProfile.StartYear))
                    {
                        int yr = (int)value;
                        value = $"{yr} - {yr + 3}";
                    }

                    ws.Cells[i + SPACE, col].Value = value;
                    col++;
                }
            }

            ws.Cells.AutoFitColumns();

            // Format Header of Table
            using (ExcelRange rng = ws.Cells["A1:K1"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(Color.Navy); //Set color to Blue 
                rng.Style.Font.Color.SetColor(Color.White);
            }

            return pck;
        }
    }
}