using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Entities.Menu;

namespace SanjeshP.RDC.Data.Contracts
{
    public interface IView_UserMenubarRepository
    {
        public Task<List<View_UserMenubar>> GetUserMenuByUser_Id(Guid userid, CancellationToken cancellationToken);
        Task<List<View_UserMenubar>> GetUserSelectedAccessMenuByUserCurrentIdAndUSerSelectedID(Guid userCurrent_Userid, Guid UserSelected_UserId, CancellationToken cancellationToken);
        Task<List<View_UserMenubar>> GetGroupSelectedAccessMenuByUserCurrentIdAndGroupSelectedID(Guid userCurrent_Userid, Guid GroupSelected_GroupId, CancellationToken cancellationToken);
        Task<bool> GetUserMenuByControllerNameAndActionName(Guid userid, string controllerName, string actionName, CancellationToken cancellationToken);
        List<View_UserMenubar> GetUserAccessMenu(Guid userid, CancellationToken cancellationToken);
    }
}
