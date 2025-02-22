using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.WebFramework.Api;
using System.Collections.Generic;


namespace SanjeshP.Entities.User
{
    public class RoleDto : BaseDto<RoleDto, Role, int>
    {
        public string RoleTitleEn { get; set; }
        public string NormalizedRoleTitleEn { get; set; }
        public string RoleTitleFa { get; set; }
        public virtual ICollection<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
        public string Name { get; set; }
    }
    public class RoleSelectDto : BaseDto<RoleSelectDto, Role, int>
    {
        public string RoleTitleEn { get; set; }
        public string NormalizedRoleTitleEn { get; set; }
        public string RoleTitleFa { get; set; }
        public virtual ICollection<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
        public string Name { get; set; }
    }
}
