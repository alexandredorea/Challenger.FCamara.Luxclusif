namespace Challenger.FCamara.Luxclusif.Application.Common.Results;

public class Result
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public object? Data { get; private set; }
    public List<ErrorDetail> Error { get; private set; }

    protected Result(bool success, string message, object? data, List<ErrorDetail>? errors)
    {
        Success = success;
        Message = message;
        Data = data;
        Error = errors ?? [];
    }

    public static Result Ok(string message = "Processamento realizado com sucesso")
        => new(true, message, null, null);

    public static Result Ok(object data, string message = "Processamento realizado com sucesso")
        => new(true, message, data, null);

    public static Result Fail(string message, string? code = null)
        => new(false, message, null,
        [
            new(code ?? "ERROR", message)
        ]);

    public static Result Fail(List<ErrorDetail> errors, string message = "Falha na operação")
        => new(false, message, null, errors);

    public static Result Fail(string code, string message, object? data = null)
        => new(false, message, data,
        [
            new ErrorDetail(code, message)
        ]);
}

public sealed class Result<T> : Result
{
    public new T? Data { get; private set; }

    private Result(bool success, string message, T? data, List<ErrorDetail>? errors)
        : base(success, message, data, errors)
    {
        Data = data;
    }

    public static Result<T> Ok(T data, string message = "Processamento realizado com sucesso")
        => new(true, message, data, null);

    public new static Result<T> Fail(string message, string? code = null)
        => new(false, message, default,
        [
            new(code ?? "ERROR", message)
        ]);

    public new static Result<T> Fail(List<ErrorDetail> errors, string message = "Falha na operação")
        => new(false, message, default, errors);
}

public record ErrorDetail(string Code, string Message);