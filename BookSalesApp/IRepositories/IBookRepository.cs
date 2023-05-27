using BookSalesApp.DTOs;

namespace BookSalesApp.IRepositories
{
    public interface IBookRepository
    {
        public AddBookDTO AllGenres();
        public Task<CustomReturnDTO> AddBook(AddBookDTO addBookDTO);
        public Task<CustomReturnDTO> UpdateBook(AddBookDTO addBookDTO);
        public Task<AddBookDTO> GetBook(int id);
        public Task<CustomReturnDTO> DeleteBook(int id);
        public IQueryable<AdminAllBooksDTO> AllBooks();
        public Task<IEnumerable<IndexResponseDTO>> AllBooks(int? id);
        public Task<IndexResponseDTO> GetBook(int id, int? userid);
        public IQueryable<AdminAllBooksDTO> YourBooks(int userid);
        public Task<string> Check(int id, int userid);
        public Task<IEnumerable<IndexResponseDTO>> SearchBooks(int? id, string name);
    }
}