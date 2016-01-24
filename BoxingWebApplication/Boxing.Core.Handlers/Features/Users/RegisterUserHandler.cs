using AutoMapper;
using Boxing.Contracts.Requests.Users;
using Boxing.Core.Sql;
using Boxing.Core.Handlers.Interfaces;
using Boxing.Core.Sql.Configurations;
using System;
using System.Linq;
using System.Threading.Tasks;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Extensions;
using Boxing.Core.Handlers.Exceptions;

namespace Boxing.Core.Handlers.Features.Users
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, LoginDto>
    {
        private readonly BoxingContext _db;

        public RegisterUserHandler(BoxingContext db)
        {
            _db = db;
        }

        public async Task<LoginDto> HandleAsync(RegisterUserRequest request)
        {
            var matchingUsers = _db.Users.Any(u => u.Username == request.User.Username);
            if (matchingUsers)
            {
                throw new BadRequestException();
            }

            var userEntity = Mapper.Map<UserEntity>(request.User);

            var salt = AuthorizationExtensions.CreateSalt();
            var passwordHash = AuthorizationExtensions.CreatePasswordHash(request.User.Password, salt);
            userEntity.Password = passwordHash;
            userEntity.PasswordSalt = salt;

            _db.Users.Add(userEntity);

            var loginEntity = new LoginEntity();
            loginEntity.UserId = userEntity.Id;
            loginEntity.AuthToken = Guid.NewGuid().ToString();
            loginEntity.ExpirationDate = DateTimeExtensions.CreateExpirationDate();
            _db.Logins.Add(loginEntity);

            await _db.SaveChangesAsync().ConfigureAwait(false);

            var savedLogin = _db.Logins.Where(l => l.UserId == userEntity.Id).OrderByDescending(l => l.ExpirationDate).Select(u => new { u.Id, u.AuthToken }).FirstOrDefault();
            var loginDto = new LoginDto();
            loginDto.Id = savedLogin.Id;
            loginDto.AuthToken = savedLogin.AuthToken;

            return loginDto;
        }
    }
}
