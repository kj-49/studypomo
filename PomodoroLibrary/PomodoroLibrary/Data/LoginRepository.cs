
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Tables;
using PomodoroLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data;

public class LoginRepository : GeneralRepository<LoginModel>, ILoginRepository
{
    private readonly IConfiguration _config;
    public LoginRepository(IConfiguration config) : base(config)
    {
        _config = config;
    }

    public async Task<ICollection<LoginModel>> GetModelsAsync(int aspNetUsersId)
    {
        try {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

            string query = $"""
            SELECT * FROM Login
            WHERE AspNetUsersId = @AspNetUsersId
            """;

            var parameters = new { AspNetUsersId = aspNetUsersId };

            var result = await connection.QueryAsync<LoginModel>(query, parameters);

            return result.ToList();

        } catch (Exception ex)
        {
            throw;
        }
    }

}
