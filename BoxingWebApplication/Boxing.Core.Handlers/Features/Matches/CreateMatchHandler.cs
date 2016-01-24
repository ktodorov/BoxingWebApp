using AutoMapper;
using Boxing.Contracts.Requests.Matches;
using Boxing.Core.Sql;
using Boxing.Core.Sql.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Handlers.Features.Matches
{
    public class CreateMatchHandler : CommandHandler<CreateMatchRequest>
    {
        private readonly BoxingContext _db;

        public CreateMatchHandler(BoxingContext db)
        {
            _db = db;
        }

        protected override async Task HandleCore(CreateMatchRequest command)
        {
            var entity = Mapper.Map<MatchEntity>(command.Match);
            _db.Matches.Add(entity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
