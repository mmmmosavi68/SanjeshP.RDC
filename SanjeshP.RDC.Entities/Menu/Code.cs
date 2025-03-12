using SanjeshP.RDC.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanjeshP.RDC.Entities.Menu
{
    public class Code : BaseEntity<int>
    {
        public Code()
        {
            IsActive = true;
            IsDeleted = false;
            CreatedDate = DateTime.Now;
        }

        [Required(ErrorMessage = "عنوان الزامی است")]
        [MaxLength(100, ErrorMessage = "عنوان نباید بیش از 100 کاراکتر باشد")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "شناسه والد")]
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        [Display(Name = "کد استاندارد")]
        public int? StandardCode { get; set; }

        [MaxLength(200, ErrorMessage = "اطلاعات اضافی نباید بیش از 200 کاراکتر باشد")]
        [Display(Name = "اطلاعات اضافی")]
        public string AdditionalInformation { get; set; }

        [Display(Name = "والد")]
        public virtual Code Parent { get; set; }

        [Display(Name = "فرزندان")]
        public virtual ICollection<Code> InverseParent { get; set; } = new List<Code>();

    }
}
