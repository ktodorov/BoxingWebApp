using AutoMapper;
using Boxing.Contracts.Requests.Boxers;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Boxers
{
    public class UpdateBoxerHandler : CommandHandler<UpdateBoxerRequest>
    {
        private readonly BoxingContext _db;

        public UpdateBoxerHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(UpdateBoxerRequest command)
        {
            var boxer = await _db.Boxers.FindAsync(command.Boxer.Id).ConfigureAwait(false);

            if (boxer == null)
                throw new NotFoundException();

            Mapper.Map(command.Boxer, boxer);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
