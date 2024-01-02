using FluentValidation.Results;

namespace Minerva.Application.Common;
public record CommandResult(bool IsSuccess, IReadOnlyCollection<ValidationFailure> Errors)
{
    public CommandResult() : this(true, [])
    {
    }

    public CommandResult(IReadOnlyCollection<ValidationFailure> errors) : this(false, errors)
    {
    }
}

public record CommandResult<TResult> : CommandResult
{
    public TResult? Result { get; private init; }

    public CommandResult(TResult result) : base() => Result = result;

    public CommandResult(IReadOnlyCollection<ValidationFailure> errors) : base(errors)
    {
    }
}
