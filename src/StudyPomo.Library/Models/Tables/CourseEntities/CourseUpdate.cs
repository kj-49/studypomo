using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Models.Tables.CourseEntities;

public class CourseUpdate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string HexColor { get; set; }
    public bool Archived { get; set; }
}
