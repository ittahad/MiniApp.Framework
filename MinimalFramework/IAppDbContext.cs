using System.Linq.Expressions;

namespace MiniApp.Core
{
    public interface IAppDbContext
    {
        public Task<bool> SaveItem<T>(T item, string tenant);

        public Task<bool> DeleteItem<T>(Expression<Func<T, bool>> expression, string tenant);

        public Task<T> GetItem<T>(Expression<Func<T, bool>> expression, string tenant);

        public Task<IEnumerable<T>> GetItems<T>(Expression<Func<T, bool>> expression, string tenant);

        public Task<bool> Update<T>(Expression<Func<T, bool>> expression, T updateObj, string tenant);
    }
}
