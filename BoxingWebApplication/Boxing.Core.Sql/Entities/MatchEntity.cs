using System;

namespace Boxing.Core.Sql.Configurations
{
    public class MatchEntity
    {
        public int Id { get; set; }

        public int Boxer1Id { get; set; }

        public BoxerEntity Boxer1 { get; set; }

        public int Boxer2Id { get; set; }

        public BoxerEntity Boxer2 { get; set; }

        public string Address { get; set; }

        public DateTime Time { get; set; }

        public string Description { get; set; }

        public int? WinnerId { get; set; }

        public BoxerEntity Winner { get; set; }
    }
}
