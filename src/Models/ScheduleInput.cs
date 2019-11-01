using System.Collections.Generic;

namespace awscsharp.Models
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