using Dapper.Contrib.Extensions;
using PomodoroLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables;

[Table("StudyType")]
public class StudyTypeModel
{

    public int Id { get; set; }
    public string TypeName { get; set; }

}
