using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Boxing.Core.Sql.Configurations
{
    class LoginConfiguration : EntityTypeConfiguration<LoginEntity>
    {
        public LoginConfiguration()
        {
            ToTable("Login");
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(e => e.User).WithMany().HasForeignKey(e => e.UserId).WillCascadeOnDelete(false);
            Property(e => e.ExpirationDate).IsRequired();
            Property(e => e.AuthToken).IsRequired();
        }
    }
}
