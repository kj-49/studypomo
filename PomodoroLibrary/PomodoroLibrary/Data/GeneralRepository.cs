using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Utility;

using System.Data;
using System.Reflection;


namespace PomodoroLibrary.Data;

public class GeneralRepository<T> : IGeneralRepository<T> where T : class
{
    private readonly IConfiguration _config;
    public GeneralRepository(IConfiguration config)
    {
        _config = config;
    }

    public async virtual Task<OperationResult<T>> InsertModelAndReturnAsync(string storedProcedure, T parameters)
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
        try
        {
            var result = await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            if (result != null)
            {
                return OperationResult<T>.Success(result);
            }
            else
            {
                return OperationResult<T>.Failure("No records created.");
            }
        }
        catch (Exception ex)
        {
            return OperationResult<T>.Failure($"An error has occured: {ex.Message}");
        }
    }

    public async virtual Task<OperationResult<T>> DeleteModelAndReturnAsync(string storedProcedure, int id)
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

        try
        {
            var result = await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, new { Id = id }, commandType: CommandType.StoredProcedure);
            if (result != null)
            {
                return OperationResult<T>.Success(result);
            }
            else
            {
                return OperationResult<T>.Failure("No records deleted.");
            }
        }
        catch (Exception ex)
        {
            return OperationResult<T>.Failure($"An error has occured: {ex.Message}");
        }
    }

    public async virtual Task<OperationResult<T>> UpdateModelAndReturnAsync(string storedProcedure, T parameters)
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

        try
        {
            var result = await connection.QueryFirstAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            return OperationResult<T>.Success(result);
        }
        catch (Exception ex)
        {
            return OperationResult<T>.Failure($"An error has occured: {ex.Message}");
        }
    }

    public async virtual Task<ICollection<T>> GetAllAsync()
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

        var result = await connection.GetAllAsync<T>();

        return result.ToList();
    }

    public async virtual Task<T?> GetByIdAsync(int id)
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

        var result = await connection.GetAsync<T>(id);

        return result;
    }
}
