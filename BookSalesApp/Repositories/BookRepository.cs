using BookSalesApp.DTOs;
using BookSalesApp.IRepositories;
using BookSalesApp.IServices;
using BookSalesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSalesApp.Repositories
{
    public class BookRepository:IBookRepository
    {
        private readonly BookSalesDbContext _db;
        private readonly IFileService _fileService;
        public BookRepository(BookSalesDbContext bookSalesDbContext, IFileService fileService)
        {
            _db = bookSalesDbContext;
            _fileService = fileService;
        }
        public AddBookDTO AllGenres()
        {
            var addBookDTO=new AddBookDTO();
            addBookDTO.Genres= from g in _db.Genres where g.Status == true select g.Name;
            return addBookDTO;
        }
        public async Task<CustomReturnDTO> AddBook(AddBookDTO addBookDTO)
        {
            var r = await _db.Genres.Where(g => g.Name == addBookDTO.Genre && g.Status == true).FirstOrDefaultAsync();
            if (r == null)
                return new CustomReturnDTO { Type = false };
            var book=new Book();
            book.Name=addBookDTO.Name;
            book.Author=addBookDTO.Author;
            book.Description=addBookDTO.Description;
            book.Price = (double)addBookDTO.Price;
            book.Genreid=r.Id;
            if (addBookDTO.Image != null)
            {
                book.Imageurl = await _fileService.AddImage(addBookDTO.Image);
            }
            book.Fileurl = await _fileService.AddFile(addBookDTO.File, book.Name);
            await _db.Books.AddAsync(book);
            await _db.SaveChangesAsync();
            return new CustomReturnDTO { Type = true, Message = "New book added successfully!" };
        }
        public async Task<CustomReturnDTO> UpdateBook(AddBookDTO addBookDTO)
        {
            var result = await _db.Books.Where(b => b.Id == addBookDTO.Id && b.Status == true).FirstOrDefaultAsync();
            if (result == null)
            {
                return new CustomReturnDTO { Type = false };
            }
            var r = await _db.Genres.Where(g => g.Name == addBookDTO.Genre && g.Status == true).FirstOrDefaultAsync();
            if (r == null)
            {
                return new CustomReturnDTO { Type = false };
            }
            if (addBookDTO.Image != null)
            {
                if (result.Imageurl != null)
                {
                    _fileService.DeleteImage(result.Imageurl);
                }
                result.Imageurl=await _fileService.AddImage(addBookDTO.Image);
            }
            _fileService.DeleteFile(result.Fileurl);
            result.Fileurl=await _fileService.AddFile(addBookDTO.File, addBookDTO.Name);
            result.Name = addBookDTO.Name;
            result.Author = addBookDTO.Author;
            result.Description = addBookDTO.Description;
            result.Price = (double)addBookDTO.Price;
            result.Genreid = r.Id;
            await _db.SaveChangesAsync();
            return new CustomReturnDTO { Type = true, Message = "The book updated successfully!" };
        }
        public async Task<AddBookDTO> GetBook(int id)
        {
            var result = await _db.Books.Where(b => b.Id == id && b.Status == true).FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }
            var addBookDTO = new AddBookDTO();
            addBookDTO.Id = id;
            addBookDTO.Author = result.Author;
            addBookDTO.Name = result.Name;
            addBookDTO.Description = result.Description;
            addBookDTO.Price = (double)result.Price;
            addBookDTO.Genre = (await _db.Genres.Where(g => g.Id == result.Genreid && g.Status == true).FirstOrDefaultAsync()).Name;
            addBookDTO.Genres = AllGenres().Genres;
            return addBookDTO;
        }
        public async Task<CustomReturnDTO>DeleteBook(int id)
        {
            var result = await _db.Books.Where(b => b.Id == id && b.Status == true).FirstOrDefaultAsync();
            if (result == null)
            {
                return new CustomReturnDTO { Type = false };
            }
            if (result.Imageurl != null)
            _fileService.DeleteImage(result.Imageurl);
            _fileService.DeleteFile(result.Fileurl);
            result.Status = false;
            await _db.SaveChangesAsync();
            return new CustomReturnDTO { Type = true, Message= "The book deleted successfully!" };
        }
        public IQueryable<AdminAllBooksDTO> AllBooks()
        {
            return from b in _db.Books
                   where b.Status == true
                   select new AdminAllBooksDTO
                   {
                       Id= b.Id,
                       Author=b.Author,
                       Name=b.Name
                   };
        }
        public async Task<IEnumerable<IndexResponseDTO>> AllBooks(int? id)
        {
            var books = await _db.Books.Where(b => b.Status == true).ToListAsync();
            var carts = _db.Carts;
            if (id > 0)
            {
                var filteredBooks = new List<Book>();
                foreach (var book in books)
                {
                    bool shouldRemove = false;
                    foreach (var cart in carts)
                    {
                        if (book.Id == cart.Bookid && cart.Userid == id && (cart.Status == true || cart.Issold == true))
                        {
                            shouldRemove = true;
                            break;
                        }
                    }
                    if (!shouldRemove)
                    {
                        filteredBooks.Add(book);
                    }
                }
                books = filteredBooks;
            }
            return from b in books
                   join g in _db.Genres on b.Genreid equals g.Id
                   select new IndexResponseDTO
                   {
                       Author = b.Author,
                       Description = b.Description,
                       Genre = g.Name,
                       Imageurl = b.Imageurl,
                       Name = b.Name,
                       Price = b.Price,
                       Id = b.Id
                   };
        }
        public async Task<IndexResponseDTO> GetBook(int id, int? userid)
        {
            if (userid > 0)
            {
                var check = await _db.Carts.Where(c => c.Userid == userid && c.Bookid == id && (c.Status == true || c.Issold == true)).FirstOrDefaultAsync();
                if (check != null)
                {
                    return null;
                }
            }
            return await (from b in _db.Books
                          join g in _db.Genres on b.Genreid equals g.Id
                          where b.Id == id && b.Status == true
                          select
                   new IndexResponseDTO { Author = b.Author, Description = b.Description, Genre = g.Name, Id = b.Id, Imageurl = b.Imageurl, Name = b.Name, Price = b.Price }).FirstOrDefaultAsync();
        }
        public IQueryable<AdminAllBooksDTO> YourBooks(int userid)
        {
            return from c in _db.Carts
                   join b in _db.Books
                   on c.Bookid equals b.Id
                   where c.Userid == userid && c.Status == false && c.Issold == true
                   select new AdminAllBooksDTO
                   {
                       Author = b.Author,
                       Name = b.Name,
                       Id = b.Id,
                   };
        }
        public async Task<string> Check(int id, int userid)
        {
            var r = await _db.Books.FindAsync(id);
            if (r == null)
                return null;
            var result = await _db.Carts.Where(c => c.Bookid == id && c.Userid == userid && c.Issold == true && c.Status == false).FirstOrDefaultAsync();
            if (result == null)
                return null;
            return r.Fileurl;
        }
        public async Task<IEnumerable<IndexResponseDTO>> SearchBooks(int? id, string name)
        {
            var books = await _db.Books.Where(b => b.Status == true&&(b.Author.ToLower()+"-"+b.Name.ToLower()).Contains(name.ToLower())).ToListAsync();
            var carts = _db.Carts;
            if (id > 0)
            {
                var filteredBooks = new List<Book>();
                foreach (var book in books)
                {
                    bool shouldRemove = false;
                    foreach (var cart in carts)
                    {
                        if (book.Id == cart.Bookid && cart.Userid == id && (cart.Status == true || cart.Issold == true))
                        {
                            shouldRemove = true;
                            break;
                        }
                    }
                    if (!shouldRemove)
                    {
                        filteredBooks.Add(book);
                    }
                }
                books = filteredBooks;
            }
            return from b in books
                   join g in _db.Genres on b.Genreid equals g.Id
                   select new IndexResponseDTO
                   {
                       Author = b.Author,
                       Description = b.Description,
                       Genre = g.Name,
                       Imageurl = b.Imageurl,
                       Name = b.Name,
                       Price = b.Price,
                       Id = b.Id
                   };
        }
    }
}
