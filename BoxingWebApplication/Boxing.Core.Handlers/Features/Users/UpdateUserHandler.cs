using AutoMapper;
using Boxing.Contracts.Requests.Users;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Users
{
    public class UpdateUserHandler : CommandHandler<UpdateUserRequest>
    {
        private readonly BoxingContext _db;

        public UpdateUserHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(UpdateUserRequest command)
        {
            var boxer = await _db.Users.FindAsync(command.User.Id).ConfigureAwait(false);

            if (boxer == null)
                throw new NotFoundException();

            Mapper.Map(command.User, boxer);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
