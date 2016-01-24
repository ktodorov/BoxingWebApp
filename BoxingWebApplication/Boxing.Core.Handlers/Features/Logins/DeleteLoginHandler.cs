using Boxing.Contracts.Requests.Logins;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Logins
{
    public class DeleteLoginHandler : CommandHandler<DeleteLoginRequest>
    {
        private readonly BoxingContext _db;

        public DeleteLoginHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(DeleteLoginRequest command)
        {
            var boxer = await _db.Logins.FindAsync(command.Id).ConfigureAwait(false);

            if (boxer == null)
                throw new NotFoundException();

            _db.Logins.Remove(boxer);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
