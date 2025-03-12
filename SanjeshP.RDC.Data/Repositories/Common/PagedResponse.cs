using SanjeshP.RDC.Data.Contracts.Common;

namespace SanjeshP.RDC.Data.Repositories.Common
{
    public class PagedResponse : IPagedResponse
    {
        private readonly ApplicationDbContext _context;

        public PagedResponse(ApplicationDbContext context) : base()
        {
            _context = context;
        }

        //public async Task<PagedResponse<UserListDto>> GetUsersAsync(UserSearchDto searchDto)
        //{
        //    var query = _context.Users.AsQueryable();

        //    // اعمال فیلتر جستجو
        //    if (!string.IsNullOrEmpty(searchDto.SearchTerm))
        //    {
        //        query = query.Where(u => u.UserName.Contains(searchDto.SearchTerm) || u.EmailAddress.Contains(searchDto.SearchTerm));
        //    }

        //    var totalCount = await query.CountAsync();
        //    var data = await query
        //        .Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
        //        .Take(searchDto.PageSize)
        //        .Select(u => new UserListDto
        //        {
        //            UserId = u.Id,
        //            UserName = u.UserName,
        //            EmailAddress = u.EmailAddress,
        //            IsActive = u.IsActive
        //        })
        //        .ToListAsync();

        //    return new PagedResponse<UserListDto>(data, totalCount, searchDto.PageNumber, searchDto.PageSize);
        //}

    }
}
