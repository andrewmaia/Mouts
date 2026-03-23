namespace Mouts.Application.Common;

public abstract class ResultResponse
{
    private readonly List<string> _errors = new();

    public IReadOnlyCollection<string> Errors => _errors;
    public bool IsSuccess => !_errors.Any();
    public BusinessError BusinessError { get; set; } = BusinessError.None;
    public void AddError(string error)
        => _errors.Add(error);

}

public enum BusinessError
{
    None,
    NotFound,
    ValidationFailed,
    Conflict
}
