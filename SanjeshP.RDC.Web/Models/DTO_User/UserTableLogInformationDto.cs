using SanjeshP.RDC.Entities.User;
using SanjeshP.RDC.WebFramework.Api;

namespace SanjeshP.Entities.User
{
    public class UserTableLogInformationDto : BaseDto<UserTableLogInformationDto,UserTableLogInformation,int>
    {
        public long? UserLgoTableId { get; set; }
        public string DataValue { get; set; }
        public int? UserLgoTableId1 { get; set; }
        public int? UserLgoTableId1NavigationId { get; set; }
        public virtual UserTablesLogDto UserLgoTableId1Navigation { get; set; }
    }
    public class UserTableLogInformationSelectDto : BaseDto<UserTableLogInformationSelectDto, UserTableLogInformation, int>
    {
        public long? UserLgoTableId { get; set; }
        public string DataValue { get; set; }
        public int? UserLgoTableId1 { get; set; }
        public int? UserLgoTableId1NavigationId { get; set; }
        public virtual UserTablesLogDto UserLgoTableId1Navigation { get; set; }
    }
}
