using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Boxing.Core.Sql.Configurations
{
    class UserConfiguration : EntityTypeConfiguration<UserEntity>
    {
        public UserConfiguration()
        {
            ToTable("User");
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.Username).HasMaxLength(100).IsRequired();
            Property(e => e.Password).IsRequired();
            Property(e => e.PasswordSalt).IsRequired();
            Property(e => e.FullName).HasMaxLength(250);
        }
    }
}
