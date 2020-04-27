using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LqdOnlineHub.Models
{
    public class UserProfile
    {
        public string Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DisplayName("Họ và tên")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Ngày sinh")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DisplayName("Giới tính")]
        public Gender Gender { get; set; }

        [Required]
        [Phone]
        [DisplayName("Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Địa chỉ nhà")]
        public string Address { get; set; }

        [Required]
        [DisplayName("Khối")]
        public Grade Grade { get; set; }

        [Required]
        [DisplayName("Lớp chuyên")]
        public Class ClassName { get; set; }

        [DisplayName("Niên khóa")]
        public int StartYear { get; set; }

        [DisplayName("Sửa đổi lần cuối")]
        public DateTimeOffset LastEdit { get; set; }
    }

    public enum Grade
    {
        [Display(Name = "10")]
        _10,
        [Display(Name = "11")]
        _11,
        [Display(Name = "12")]
        _12
    }

    public enum Class
    {
        [Display(Name = "Toán 1")]
        Toan1,
        [Display(Name = "Toán 2")]
        Toan2,
        [Display(Name = "Toán 3")]
        Toan3,
        [Display(Name = "Tin")]
        Tin,
        [Display(Name = "Lý")]
        Ly,
        [Display(Name = "Hóa 1")]
        Hoa1,
        [Display(Name = "Hóa 2")]
        Hoa2,
        [Display(Name = "Sinh")]
        Sinh,
        [Display(Name = "Văn")]
        Van,
        [Display(Name = "AV1")]
        AV1,
        [Display(Name = "AV2")]
        AV2,
        [Display(Name = "AV3")]
        AV3
    }

    public enum Gender
    {
        [Display(Name = "Nam")]
        Male,
        [Display(Name = "Nữ")]
        Female
    }
}
