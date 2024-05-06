using PomodoroLibrary.Models.Utility;

namespace PomodoroLibrary.Data.Interfaces;

public interface IGeneralRepository<T> where T : class
{
    Task<OperationResult<T>> DeleteModelAndReturnAsync(string storedProcedure, int id);
    Task<ICollection<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<OperationResult<T>> InsertModelAndReturnAsync(string storedProcedure, T parameters);
    Task<OperationResult<T>> UpdateModelAndReturnAsync(string storedProcedure, T parameters);
}