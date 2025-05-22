using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Optix.Data;
using Optix.Model;
using System.Formats.Asn1;
using System.Globalization;

namespace Optix.Service
{
    public class DataImportService
    {
        private readonly MovieContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DataImportService> _logger;

        public DataImportService(MovieContext context, IWebHostEnvironment env, ILogger<DataImportService> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        public async Task SeedDataAsync(string csvFilePath)
        {
            // Check if database already has data
            if (await _context.Movies.AnyAsync())
            {
                _logger.LogInformation("Database already has data.");
                return;
            }

            try
            {
                
                using var reader = new StreamReader(csvFilePath);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

                
                csv.Context.RegisterClassMap<CSVMapToMovie>();

                
                var records = csv.GetRecords<CSVToMovieRecord>().ToList();

                
                var movies = records.Select(r => new Movie
                {
                    Title = r.Title,
                    Overview = r.Overview,
                    Genre = r.Genre,
                    Year = r.Year > 0 ? r.Year : null,
                    Runtime = r.Runtime > 0 ? r.Runtime : null,
                    Rating = r.Rating > 0 ? r.Rating : null,
                    Revenue = r.Revenue > 0 ? r.Revenue : null,
                    Budget = r.Budget > 0 ? r.Budget : null,
                    Director = r.Director,
                    Actors = r.Actors,
                    Language = r.Language,
                    Country = r.Country
                }).ToList();

                // Add to database
                await _context.Movies.AddRangeAsync(movies);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Database seeded with {movies.Count} movies");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database from CSV file");
                throw;
            }
        }
    }
}
