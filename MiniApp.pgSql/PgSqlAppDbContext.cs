using MiniApp.Core;
using System.Linq.Expressions;

namespace MiniApp.PgSQL
{
    public class PgSqlAppDbContext : IAppDbContext
    {
        public Task<bool> DeleteItem<T>(Expression<Func<T, bool>> expression, string tenant)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetItem<T>(Expression<Func<T, bool>> expression, string tenant)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetItems<T>(Expression<Func<T, bool>> expression, string tenant)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveItem<T>(T item, string tenant)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update<T>(Expression<Func<T, bool>> expression, T updateObj, string tenant)
        {
            throw new NotImplementedException();
        }
    }
}