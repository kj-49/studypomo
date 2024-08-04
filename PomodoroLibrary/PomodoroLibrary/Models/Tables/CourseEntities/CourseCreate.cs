using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables.CourseEntities;

public class CourseCreate
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string HexColor { get; set; }
}
