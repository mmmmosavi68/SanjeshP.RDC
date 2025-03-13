using SanjeshP.RDC.Data.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SanjeshP.RDC.Services.TokenService
{
    public interface ITokenService
    {
        Task<bool> ValidateTokenAsync(Guid tokenId, CancellationToken cancellationToken);
        Task RemoveExpiredTokensAsync(CancellationToken cancellationToken);
    }
}
