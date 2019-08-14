namespace awscsharp.Models
{
    public class TvProgramInput
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryString { get; set; }
        public string Image { get; set; }
        public string Start_unix { get; set; }
        public string Stop_unix { get; set; }
        public string Ep_nr { get; set; }
        public string Country { get; set; }
        public int Year { get; set; }
        public ChannelInput Channel { get; set; }
    }
}