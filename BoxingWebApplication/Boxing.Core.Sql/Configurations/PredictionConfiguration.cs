using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Boxing.Core.Sql.Configurations
{
    class PredictionConfiguration : EntityTypeConfiguration<PredictionEntity>
    {
        public PredictionConfiguration()
        {
            ToTable("Prediction");
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(e => e.PredictedBoxer).WithMany().HasForeignKey(e => e.PredictedBoxerId).WillCascadeOnDelete(false);
            HasRequired(e => e.Match).WithMany().HasForeignKey(e => e.MatchId).WillCascadeOnDelete(false);
            HasRequired(e => e.User).WithMany().HasForeignKey(e => e.UserId).WillCascadeOnDelete(false);
        }
    }
}
