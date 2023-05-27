using BookSalesApp.DTOs;
using BookSalesApp.IRepositories;
using BookSalesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSalesApp.Repositories
{
    public class GenreRepository:IGenreRepository
    {
        private readonly BookSalesDbContext _db;
        public GenreRepository(BookSalesDbContext bookSalesDbContext)
        {
            _db = bookSalesDbContext;
        }
        public async Task<CustomReturnDTO> AddGenre(AddGenreDTO addGenreDTO)
        {
            var result = await _db.Genres.Where(g => g.Name == addGenreDTO.Name && g.Status==true).FirstOrDefaultAsync();
            if (result == null)
            {
                var genre = new Genre { Name = addGenreDTO.Name };
                await _db.Genres.AddAsync(genre);
                await _db.SaveChangesAsync();
                return new CustomReturnDTO { Type = true, Message = "New genre added successfully!" };
            }
            return new CustomReturnDTO { Type = false, Message = "This genre already exists!" };
        }
        public async Task<CustomReturnDTO> UpdateGenre(AddGenreDTO addGenreDTO)
        {
            var result = await _db.Genres.Where(g=>g.Id==addGenreDTO.Id && g.Status==true).FirstOrDefaultAsync();
            if (result == null)
            {
                return new CustomReturnDTO { Type = false };
            }
            var check = await _db.Genres.Where(g => g.Id != addGenreDTO.Id && g.Name == addGenreDTO.Name && g.Status==true).FirstOrDefaultAsync();
            if (check == null)
            {
                result.Name = addGenreDTO.Name;
                await _db.SaveChangesAsync();
                return new CustomReturnDTO { Type = true, Message = "The genre updated successfully!" };
            }
            return new CustomReturnDTO { Type = false, Message = "This genre already exists!" };
        }
        public async Task<CustomReturnDTO> DeleteGenre(int id)
        {
            var result = await _db.Genres.Where(g=>g.Id==id && g.Status==true).FirstOrDefaultAsync();
            if (result == null)
            {
                return new CustomReturnDTO { Type = false };
            }
            result.Status = false;
            await _db.SaveChangesAsync();
            return new CustomReturnDTO { Type = true, Message = "The genre deleted successfully!" };
        }
        public IQueryable<Genre> AllGenres()
        {
            return _db.Genres.Where(g => g.Status == true);
        }
        public async Task<AddGenreDTO> GetGenre(int id)
        {
            var result = await _db.Genres.Where(g=>g.Id==id && g.Status==true).FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }
            var addGenreDTO = new AddGenreDTO { Id=result.Id, Name=result.Name };
            return addGenreDTO;
        } 
        public IQueryable<PopularGenresDTO>PopularGenres()
        {
            var result=from c in _db.Carts join b in _db.Books on c.Bookid equals b.Id join g in _db.Genres on b.Genreid equals g.Id
                   where c.Issold==true&&c.Status==false group g by g.Name into genre 
                   select new PopularGenresDTO
                   {
                       Genre =genre.Key,
                       Count = genre.Count()
                   };
            return result.OrderByDescending(b => b.Count).Take(5);
        }
    }
}