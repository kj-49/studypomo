using Dapper.Contrib.Extensions;
using PomodoroLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables;

[Table("Login")]
public class LoginModel
{

    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int AspNetUsersId { get; set; }

}
