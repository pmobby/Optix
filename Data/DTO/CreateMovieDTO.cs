using System.ComponentModel.DataAnnotations;

namespace Optix.Data.DTO
{
    public class CreateMovieDTO
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        public string? Overview { get; set; }

        [StringLength(255)]
        public string? Genre { get; set; }

        [Range(1900, 2100)]
        public int? Year { get; set; }

        [Range(1, 1000)]
        public int? Runtime { get; set; }

        [Range(0, 10)]
        public decimal? Rating { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Revenue { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Budget { get; set; }

        [StringLength(255)]
        public string? Director { get; set; }

        public string? Actors { get; set; }

        [StringLength(100)]
        public string? Language { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }
    }
}
