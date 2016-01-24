using AutoMapper;
using Boxing.Contracts.Requests.Boxers;
using Boxing.Core.Sql;
using Boxing.Core.Sql.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Boxers
{
    public class CreateBoxerHandler : CommandHandler<CreateBoxerRequest>
    {
        private readonly BoxingContext _db;

        public CreateBoxerHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(CreateBoxerRequest command)
        {
            var entity = Mapper.Map<BoxerEntity>(command.Boxer);
            _db.Boxers.Add(entity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
