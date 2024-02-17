using FluentValidation.Results;

namespace Minerva.Application.Common;
public record CommandResult(bool IsSuccess, string? Error, IReadOnlyCollection<ValidationFailure> Errors)
{
    public CommandResult() : this(true, null, [])
    {
    }

    public CommandResult(string error, IReadOnlyCollection<ValidationFailure> validationErrors) : this(false, error, validationErrors)
    {
    }

    public CommandResult(string error) : this(false, error, [])
    {
    }

    public static readonly CommandResult Success = new();
}

public record CommandResult<TResult> : CommandResult
{
    public TResult? Result { get; private init; }

    public CommandResult(TResult result) : base() => Result = result;

    public CommandResult(string error, IReadOnlyCollection<ValidationFailure> errors) : base(error, errors)
    {
    }

    public CommandResult(string error) : base(error, [])
    {
    }
}
