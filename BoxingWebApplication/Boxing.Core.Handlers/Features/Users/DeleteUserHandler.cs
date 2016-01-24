using Boxing.Contracts.Requests.Users;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Users
{
    public class DeleteUserHandler : CommandHandler<DeleteUserRequest>
    {
        private readonly BoxingContext _db;

        public DeleteUserHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(DeleteUserRequest command)
        {
            var boxer = await _db.Users.FindAsync(command.Id).ConfigureAwait(false);

            if (boxer == null)
                throw new NotFoundException();

            _db.Users.Remove(boxer);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
