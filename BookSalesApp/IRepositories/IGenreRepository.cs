using BookSalesApp.DTOs;
using BookSalesApp.Models;

namespace BookSalesApp.IRepositories
{
    public interface IGenreRepository
    {
        public Task<CustomReturnDTO> AddGenre(AddGenreDTO addGenreDTO);
        public Task<CustomReturnDTO> UpdateGenre(AddGenreDTO addGenreDTO);
        public Task<CustomReturnDTO> DeleteGenre(int id);
        public IQueryable<Genre> AllGenres();
        public Task<AddGenreDTO> GetGenre(int id);
        public IQueryable<PopularGenresDTO> PopularGenres();
    }
}
