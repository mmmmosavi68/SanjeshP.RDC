using SanjeshP.RDC.Common;
using SanjeshP.RDC.Data.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Services.TokenService
{
    public class TokenService : ITokenService, IScopedDependency
    {
        private readonly IUserTokenRepository _userTokenRepository;

        public TokenService(IUserTokenRepository userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
        }

        public async Task<bool> ValidateTokenAsync(Guid tokenId, CancellationToken cancellationToken)
        {
            var token = await _userTokenRepository.GetUserTokenByIdAsync(tokenId, cancellationToken);
            return token != null && token.ExpirationDate > DateTime.Now && !token.IsDeleted;
        }

        public async Task RemoveExpiredTokensAsync(CancellationToken cancellationToken)
        {
            var expiredTokens = await _userTokenRepository.GetExpiredTokensAsync(cancellationToken);
            foreach (var token in expiredTokens)
            {
                token.IsDeleted = true;
                await _userTokenRepository.UpdateAsync(token, cancellationToken);
            }
        }
    }
}
