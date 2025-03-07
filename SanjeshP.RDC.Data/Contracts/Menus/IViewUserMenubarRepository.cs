using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SanjeshP.RDC.Entities.Menu;

namespace SanjeshP.RDC.Data.Contracts.Menus
{
    public interface IViewUserMenubarRepository
    {
        public Task<List<View_UserMenubar>> GetUserMenusByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<View_UserMenubar>> GetUserSelectedAccessMenusByUserIdAsync(Guid userCurrentUserId, Guid userSelectedUserId, CancellationToken cancellationToken);
        Task<List<View_UserMenubar>> GetGroupSelectedAccessMenusByGroupIdAsync(Guid userCurrentUserId, Guid groupSelectedGroupId, CancellationToken cancellationToken);
        Task<bool> IsUserAuthorizedForMenuAsync(Guid userId, string controllerName, string actionName, CancellationToken cancellationToken);
        List<View_UserMenubar> GetUserAccessMenus(Guid userId, CancellationToken cancellationToken);
    }
}
