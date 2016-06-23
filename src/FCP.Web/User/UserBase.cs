using System;
using System.ComponentModel.DataAnnotations;

namespace FCP.Web
{
    [Serializable]
    public class UserBase
    {
        public string ID { get; set; }

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [StringLength(20, ErrorMessage = "用户名不能超过{1}字符")]
        public string userName { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(32, ErrorMessage = "密码长度必须是6到32个字符", MinimumLength = 6)]
        public string passWord { get; set; }
    }
}
