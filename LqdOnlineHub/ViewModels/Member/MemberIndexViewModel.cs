using LqdOnlineHub.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LqdOnlineHub.ViewModels.Member
{
    public class MemberIndexViewModel
    {
        public string Title { get; set; } = "Danh sách đoàn viên";
        public Grade SelectedGrade { get; set; }
        public Class SelectedClass { get; set; }
        public bool Downloadable { get; set; }
        public string EmptyMessage { get; set; }

        public IEnumerable<UserProfile> Members { get; set; }
    }
}
