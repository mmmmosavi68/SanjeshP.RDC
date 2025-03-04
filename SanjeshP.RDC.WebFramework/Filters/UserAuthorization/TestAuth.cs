//// مدل توکن
//using SanjeshP.RDC.Data.Repositories;
//using System.Threading.Tasks;
//using System;

//public class UserToken
//{
//    public Guid Id { get; set; }
//    public Guid UserId { get; set; }
//    public string Device { get; set; } // نوع دستگاه: موبایل، لپ‌تاپ، ...
//    public string Token { get; set; }
//    public DateTime CreatedAt { get; set; }
//    public DateTime? RevokedAt { get; set; }
//}

//// مدل لاگ فعالیت
//public class UserActivityLog
//{
//    public Guid Id { get; set; }
//    public Guid UserId { get; set; }
//    public string Device { get; set; }
//    public DateTime ActionTime { get; set; }
//    public string ActionType { get; set; } // نوع فعالیت: ورود، خروج
//}

//// در سرویس احراز هویت
//public class AuthService
//{
//    private readonly ITokenRepository _tokenRepository;
//    private readonly IUserActivityLogRepository _logRepository;

//    public AuthService(ITokenRepository tokenRepository, IUserActivityLogRepository logRepository)
//    {
//        _tokenRepository = tokenRepository;
//        _logRepository = logRepository;
//    }

//    public async Task<string> LoginAsync(Guid userId, string device)
//    {
//        var token = GenerateToken(); // تولید توکن جدید
//        var userToken = new UserToken
//        {
//            Id = Guid.NewGuid(),
//            UserId = userId,
//            Device = device,
//            Token = token,
//            CreatedAt = DateTime.UtcNow
//        };

//        await _tokenRepository.AddAsync(userToken);
//        await _logRepository.AddAsync(new UserActivityLog
//        {
//            Id = Guid.NewGuid(),
//            UserId = userId,
//            Device = device,
//            ActionTime = DateTime.UtcNow,
//            ActionType = "Login"
//        });

//        return token;
//    }

//    public async Task LogoutAsync(string token)
//    {
//        var userToken = await _tokenRepository.GetByTokenAsync(token);
//        if (userToken != null)
//        {
//            userToken.RevokedAt = DateTime.UtcNow;
//            await _tokenRepository.UpdateAsync(userToken);
//            await _logRepository.AddAsync(new UserActivityLog
//            {
//                Id = Guid.NewGuid(),
//                UserId = userToken.UserId,
//                Device = userToken.Device,
//                ActionTime = DateTime.UtcNow,
//                ActionType = "Logout"
//            });
//        }
//    }
//}
