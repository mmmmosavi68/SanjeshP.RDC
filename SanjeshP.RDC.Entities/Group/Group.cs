using SanjeshP.RDC.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SanjeshP.RDC.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanjeshP.RDC.Entities.Group
{
    public class Group : BaseEntity
    {
        [Required(ErrorMessage = "عنوان گروه الزامی است")]
        [MaxLength(100, ErrorMessage = "عنوان گروه نباید بیش از 100 کاراکتر باشد")]
        [Display(Name = "عنوان گروه")]
        public string GroupName { get; set; }

        [Display(Name = "کاربر")]
        public virtual User.User Creator { get; set; }

        public virtual ICollection<GroupUsers> GroupUsers { get; set; } = new List<GroupUsers>();

    }
}
