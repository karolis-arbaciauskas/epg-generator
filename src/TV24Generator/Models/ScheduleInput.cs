using System.Collections.Generic;

namespace EpgGenerator.Models
{
    public class RootObject
    {
        public ScheduleInput Schedule { get; set; }
    }

    public class ScheduleInput
    {
        public List<TvProgramInput> Programme { get; set; }
    }
}