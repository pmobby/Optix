using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Optix.Data;
using Optix.Data.DTO;
using Optix.Model;

namespace Optix.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<MovieSearchResultDTO>> GetMovies([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var totalCount = await _context.Movies.CountAsync();

            var movies = await _context.Movies
                .OrderBy(m => m.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var movieDtos = _mapper.Map<IEnumerable<MovieDTO>>(movies);

            var result = new MovieSearchResultDTO
            {
                Movies = movieDtos,
                Pagination = new PaginationDTO
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = page
                }
            };

            return result;
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDTO>(movie);
            return movieDto;
        }

        
        [HttpGet("search")]
        public async Task<ActionResult<MovieSearchResultDTO>> SearchMovies([FromQuery] string? title,
            [FromQuery] string? genre,
            [FromQuery] int? year,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(m => m.Title != null && m.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(m => m.Genre != null && m.Genre.Contains(genre));
            }

            if (year.HasValue)
            {
                query = query.Where(m => m.Year == year.Value);
            }

            var totalCount = await query.CountAsync();

            var movies = await query
                .OrderBy(m => m.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var movieDtos = _mapper.Map<IEnumerable<MovieDTO>>(movies);

            var result = new MovieSearchResultDTO
            {
                Movies = movieDtos,
                Pagination = new PaginationDTO
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = page
                }
            };

            return result;
        }

        
        [HttpPost]
        public async Task<ActionResult<MovieDTO>> PostMovie(CreateMovieDTO createMovieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = _mapper.Map<Movie>(createMovieDto);

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var movieDto = _mapper.Map<MovieDTO>(movie);

            return CreatedAtAction(
                nameof(GetMovie),
                new { id = movie.Id },
                movieDto);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, UpdateMovieDTO updateMovieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _mapper.Map(updateMovieDto, movie);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error occurred when updating movie {MovieId}", id);
                return StatusCode(500, "An error occurred while updating the movie.");
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
