namespace Optix.Data.DTO
{
    public class MovieSearchResultDTO
    {
        public IEnumerable<MovieDTO> Movies { get; set; } = new List<MovieDTO>();
        public PaginationDTO Pagination { get; set; } = new PaginationDTO();
    }
}
