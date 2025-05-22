using AutoMapper;
using Optix.Data.DTO;
using Optix.Model;

namespace Optix.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieDTO>();
            CreateMap<CreateMovieDTO, Movie>();
            CreateMap<UpdateMovieDTO, Movie>();
        }
    }
}
