using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Boxing.Core.Sql.Configurations
{
    class BoxerConfiguration : EntityTypeConfiguration<BoxerEntity>
    {
        public BoxerConfiguration()
        {
            ToTable("Boxer");
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.Name).HasMaxLength(2000).IsRequired();
        }
    }
}
