using CsvHelper.Configuration;

namespace Optix.Model
{
    public class CSVMapToMovie : ClassMap<CSVToMovieRecord>
    {
        public CSVMapToMovie() 
        {
            Map(m => m.Title).Name("Title");
            Map(m => m.Overview).Name("Overview");
            Map(m => m.Genre).Name("Genre");
            Map(m => m.Year).Name("Year");
            Map(m => m.Runtime).Name("Runtime");
            Map(m => m.Rating).Name("Rating");
            Map(m => m.Revenue).Name("Revenue");
            Map(m => m.Budget).Name("Budget");
            Map(m => m.Director).Name("Director");
            Map(m => m.Actors).Name("Actors");
            Map(m => m.Language).Name("Language");
            Map(m => m.Country).Name("Country");
        }
    }
}
