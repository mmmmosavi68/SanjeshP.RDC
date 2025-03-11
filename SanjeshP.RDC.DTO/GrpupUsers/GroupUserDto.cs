using System;

namespace SanjeshP.RDC.DTO.GrpupUsers
{
    public class GroupUserDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
