using System;

namespace Boxing.Contracts.Dto
{
    public class MatchDto
    {
        public int Id { get; set; }

        public int Boxer1Id { get; set; }

        public int Boxer2Id { get; set; }

        public string Address { get; set; }

        public DateTime Time { get; set; }

        public string Description { get; set; }

        public int? WinnerId { get; set; }

        public BoxerDto Boxer1 { get; set; }
        public BoxerDto Boxer2 { get; set; }

        public BoxerDto Winner { get; set; }
    }
}
