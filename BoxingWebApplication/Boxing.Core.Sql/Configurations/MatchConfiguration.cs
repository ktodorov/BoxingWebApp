using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Boxing.Core.Sql.Configurations
{
    class MatchConfiguration : EntityTypeConfiguration<MatchEntity>
    {
        public MatchConfiguration()
        {
            ToTable("Match");
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(e => e.Boxer1).WithMany().HasForeignKey(e => e.Boxer1Id).WillCascadeOnDelete(false);
            HasRequired(e => e.Boxer2).WithMany().HasForeignKey(e => e.Boxer2Id).WillCascadeOnDelete(false);
            Property(e => e.Address).HasMaxLength(2000).IsRequired();
            Property(e => e.Time).IsRequired();
            Property(e => e.Description).HasMaxLength(2000).IsRequired();
            HasOptional(e => e.Winner).WithMany().HasForeignKey(e => e.WinnerId).WillCascadeOnDelete(false);
        }
    }
}
