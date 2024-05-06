
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables;

[Table("Friend")]
public class FriendModel
{

    public int Id { get; set; }
    public int AspNetUsersId1 { get; set; }
    public int AspNetUsersId2 { get; set; }
    public DateTime DateCreated { get; set; }

}
