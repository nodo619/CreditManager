using System.Collections.Generic;

namespace CreditManager.Application.Common.Models;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public List<string>? Errors { get; }

    private Result(bool isSuccess, T? value, string? error = null, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        Errors = errors;
    }

    public static Result<T> Success(T value) => new(true, value);
    public static Result<T> Failure(string error) => new(false, default, error);
}