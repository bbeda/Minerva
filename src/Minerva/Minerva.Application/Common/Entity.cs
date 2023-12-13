using System.Diagnostics.CodeAnalysis;

namespace Minerva.Application.Common;
public abstract class Entity
{
    public required Guid Id { get; init; }

    [SetsRequiredMembers]
    public Entity() : this(Guid.NewGuid())
    {
    }

    [SetsRequiredMembers]
    public Entity(Guid id) => Id = id;
}
