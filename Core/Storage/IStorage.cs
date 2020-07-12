using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Storage
{
  public interface IStorage<R>
  {
    Task<R> Insert(R entity);

    Task<R> InsertOrReplace(R entity);

    Task InsertOrReplace(IEnumerable<R> entities, string idProperty = "ID");

    Task<R> FindByID(long id);

    Task DeleteByID(long id);

    Task<IEnumerable<R>> All(Func<R, bool> condition = null);

    Task<int> DeleteAll(Expression<Func<R, bool>> predicate);

    Task<R> LastSortedBy<T>(Expression<Func<R, T>> predicate,
                            Func<R, bool> condition = null);
  }
}
