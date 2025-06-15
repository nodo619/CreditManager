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
    public static Result<T> Failure(List<string> errors) => new(false, default, null, errors);
}

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public List<string>? Errors { get; }

    private Result(bool isSuccess, string? error = null, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        Errors = errors;
    }

    public static Result Success() => new(true);
    public static Result Failure(string error) => new(false, error);
    public static Result Failure(List<string> errors) => new(false, null, errors);
} 