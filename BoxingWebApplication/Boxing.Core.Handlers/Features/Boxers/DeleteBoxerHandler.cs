using Boxing.Contracts.Requests.Boxers;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Boxers
{
    public class DeleteBoxerHandler : CommandHandler<DeleteBoxerRequest>
    {
        private readonly BoxingContext _db;

        public DeleteBoxerHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(DeleteBoxerRequest command)
        {
            var boxer = await _db.Boxers.FindAsync(command.Id).ConfigureAwait(false);

            if (boxer == null)
                throw new NotFoundException();

            _db.Boxers.Remove(boxer);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
