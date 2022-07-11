using BookStore.Models.DataModels;

namespace BookStore.Repository
{
    public class BaseRepository
    {
        protected readonly BookStoreDBContext _context = new();
    }
}
