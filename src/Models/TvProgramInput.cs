namespace awscsharp.Models
{
    public class Category
    {
        public int Id { get; set; }
    }

    public class TvProgramInput
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Description_long { get; set; }
        public string Ep_title { get; set; }
        public int Ep_nr { get; set; }
        public object Ep_all { get; set; }
        public int Season { get; set; }
        public string Start { get; set; }
        public int Start_unix { get; set; }
        public string Start_human_date { get; set; }
        public string Start_date_s { get; set; }
        public string Start_month_s { get; set; }
        public string Stop { get; set; }
        public int Stop_unix { get; set; }
        public string GenreString { get; set; }
        public string CategoryString { get; set; }
        public Category Category { get; set; }
        public int Movie_id { get; set; }
        public int Series_id { get; set; }
        public int Comments { get; set; }
        public int Favourites { get; set; }
        public int Clicks { get; set; }
        public bool Highlight { get; set; }
        public string Status { get; set; }
        public string Title_original { get; set; }
        public string Country { get; set; }
        public int Year { get; set; }
        public string Short_movie_info { get; set; }
        public string Cast { get; set; }
        public string Director { get; set; }
        public string Title_short { get; set; }
        public string Image { get; set; }
        public int Images_count { get; set; }
        public string Imdb { get; set; }
        public double? Imdb_rating { get; set; }
        public int? Imdb_votes { get; set; }
        public string Youtube { get; set; }
        public string Url { get; set; }
        public ChannelInput Channel { get; set; }
        public string Series_url { get; set; }
        public bool? Issport { get; set; }
    }    
}