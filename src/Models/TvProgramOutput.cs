﻿namespace awscsharp.Models
{
  public class TvProgramOutput
  {
    public string Channel { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string ProgrammeImage { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public int Episode { get; set; }
    public string Country { get; set; }
    public int Year { get; set; }
  }
}