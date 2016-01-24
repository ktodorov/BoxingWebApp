using AutoMapper;
using Boxing.Contracts.Requests.Logins;
using Boxing.Core.Handlers.Exceptions;
using Boxing.Core.Sql;
using System.Linq;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Logins
{
    public class UpdateLoginHandler : CommandHandler<UpdateLoginRequest>
    {
        private readonly BoxingContext _db;

        public UpdateLoginHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(UpdateLoginRequest command)
        {
            var login = _db.Logins.Where(l => l.Id == command.Login.Id).FirstOrDefault();

            if (login == null)
                throw new NotFoundException();

            Mapper.Map(command.Login, login);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
