using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Utility;

public class OperationResult
{
    public bool Successful { get; set; }
    public string? Message { get; set; }

    public OperationResult(bool successful, string? message)
    {
        Successful = successful;
        Message = message;
    }

    public static OperationResult Success(string? message = "Operation successful.") =>
        new OperationResult(true, message);

    public static OperationResult Failure(string? message) =>
        new OperationResult(false, message);

    public static OperationResult DeleteWithNoRowsEffected() =>
        new OperationResult(false, "Operation failed, no records deleted.");

    public static OperationResult UpdateWithNoRowsEffected() =>
        new OperationResult(true, "Operation successful, no changes necessary");

    public static OperationResult FailureWithExceptionMessage(Exception ex) =>
        new OperationResult(false, $"An error has occured: {ex.Message}.");

}

public class OperationResult<T>
{
    public bool Successful { get; set; }
    public string? Message { get; set; }

    public T? Data { get; }

    public OperationResult(bool successful, string? message, T? data)
    {
        Successful = successful;
        Message = message;
        Data = data;
    }

    public static OperationResult<T> Success(T data, string? message = "Operation successful.") =>
        new OperationResult<T>(true, message, data);


    public static OperationResult<T> Failure(string message) =>
        new OperationResult<T>(false, message, default(T));

    public static OperationResult<T> DeleteWithNoRowsEffected() =>
        new OperationResult<T>(false, "Operation failed, no records deleted.", default(T));

    public static OperationResult<T> UpdateWithNoRowsEffected() =>
        new OperationResult<T>(true, "Operation successful, no changes necessary", default(T));

    public static OperationResult<T> FailureWithExceptionMessage(Exception ex) =>
        new OperationResult<T>(false, $"An error has occured: {ex.Message}.", default(T));

    // Conversion method from OperationResult<T> to OperationResult
    public OperationResult ToNonGenericResult()
    {
        return new OperationResult(Successful, Message);
    }
}
