using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Minerva.Application.Common;
internal class EntityTypeConfigurationBase<T> : IEntityTypeConfiguration<T> where T : Entity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        _ = builder.HasKey(entity => entity.Id);

        _ = builder.Property(entity => entity.CreatedAt)
            .IsRequired()
            .ValueGeneratedOnAdd();

        _ = builder.Property(entity => entity.TenantId)
            .IsRequired();

        _ = builder.HasIndex(entity => entity.TenantId);

        _ = builder.HasQueryFilter(entity => entity.TenantId == Guid.Empty);
    }
}
