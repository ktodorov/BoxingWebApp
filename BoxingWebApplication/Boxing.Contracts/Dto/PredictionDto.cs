using System;

namespace Boxing.Contracts.Dto
{
    public class PredictionDto
    {
        public int Id { get; set; }

        public int MatchId { get; set; }

        public MatchDto Match { get; set; }

        public int PredictedBoxerId { get; set; }

        public BoxerDto PredictedBoxer { get; set; }

        public int UserId { get; set; }

        public UserDto User { get; set; }
    }
}
