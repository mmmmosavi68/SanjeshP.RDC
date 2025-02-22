using SanjeshP.RDC.Entities.Common;
using System;
using System.Collections.Generic;

namespace SanjeshP.RDC.Entities.Menu
{
    public class Code : BaseEntity
    {
        public Code()
        {
            IsActive = true;
            IsDelete = false;
            CreateDate = DateTime.Now;
        }
        //public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public int? StandardCode { get; set; }
        public string AdditionalInformation { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Creator { get; set; }
        public string HostIp { get; set; }
        public virtual ICollection<Code> InverseParent { get; set; } = new List<Code>();
        public virtual Code Parent { get; set; }
    }
}
