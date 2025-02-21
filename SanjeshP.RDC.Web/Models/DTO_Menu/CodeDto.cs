using SanjeshP.RDC.Entities.Menu;
using System.Collections.Generic;
using System;
using SanjeshP.RDC.WebFramework.Api;

namespace SanjeshP.RDC.Models.DTO_Menu
{
    public class CodeDto : BaseDto<CodeDto,Code,int>
    {
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public int? StandardCode { get; set; }
        public string AdditionalInformation { get; set; }
        public Guid? Creator { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string HostIp { get; set; }
        public virtual ICollection<Code> InverseParent { get; set; } = new List<Code>();
        public virtual Code Parent { get; set; }
    }

    public class CodeSelectDto : BaseDto<CodeSelectDto, Code, int>
    {
        public string Title { get; set; }
        public int? StandardCode { get; set; }
        public string AdditionalInformation { get; set; }
        public Guid? Creator { get; set; }
        public bool? IsActive { get; set; }
    }
}
