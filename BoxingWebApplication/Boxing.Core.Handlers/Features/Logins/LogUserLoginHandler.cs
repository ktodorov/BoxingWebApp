using Boxing.Contracts.Extensions;
using AutoMapper;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Requests.Logins;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql;
using System.Threading.Tasks;
using Boxing.Core.Sql.Configurations;
using System;
using System.Linq;

namespace Boxing.Core.Handlers.Features.Logins
{
    public class LogUserLoginHandler : IRequestHandler<LogUserLoginRequest, LoginDto>
    {
        private readonly BoxingContext _db;

        public LogUserLoginHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<LoginDto> HandleAsync(LogUserLoginRequest command)
        {
            var salt = _db.Users.FirstOrDefault(u => u.Username == command.User.Username)?.PasswordSalt;

            if (salt == null)
            {
                throw new BadRequestException();
            }

            var passwordHash = AuthorizationExtensions.CreatePasswordHash(command.User.Password, salt);

            var registeredUser = _db.Users.Where(u => u.Username == command.User.Username && u.Password == passwordHash).FirstOrDefault();

            if (registeredUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            var currentLogin = _db.Logins.Where(l => l.UserId == registeredUser.Id).OrderByDescending(l => l.ExpirationDate).FirstOrDefault();

            if (currentLogin != null && currentLogin.ExpirationDate.IsValidExpirationDate())
            {
                currentLogin.ExpirationDate = DateTimeExtensions.CreateExpirationDate();
            }
            else
            {
                var entity = new LoginEntity();
                entity.UserId = registeredUser.Id;
                entity.AuthToken = Guid.NewGuid().ToString();
                entity.ExpirationDate = DateTimeExtensions.CreateExpirationDate();
                _db.Logins.Add(entity);
            }

            await _db.SaveChangesAsync().ConfigureAwait(false);
            var savedLogin = _db.Logins.Where(l => l.UserId == registeredUser.Id)
                                .OrderByDescending(l => l.ExpirationDate)
                                .Select(u => new { u.Id, u.AuthToken, u.UserId }).FirstOrDefault();

            var loginDto = new LoginDto();
            loginDto.Id = savedLogin.Id;
            loginDto.AuthToken = savedLogin.AuthToken;
            loginDto.UserId = savedLogin.UserId;

            return loginDto;
        }
    }
}
