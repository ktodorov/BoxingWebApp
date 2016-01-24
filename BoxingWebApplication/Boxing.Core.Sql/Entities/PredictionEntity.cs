using System;

namespace Boxing.Core.Sql.Configurations
{
    public class PredictionEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public int MatchId { get; set; }

        public MatchEntity Match { get; set; }

        public int PredictedBoxerId { get; set; }

        public BoxerEntity PredictedBoxer { get; set; }
    }
}
