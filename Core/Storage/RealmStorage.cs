using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Realms;
using System.Linq;
using System.Linq.Expressions;
using Core.Utils;
using Core.Tools;

namespace Core.Storage
{
  public abstract class RealmStorage<R> : IStorage<R> where R : RealmObject
  {
    protected virtual Realm Realm
    {
      get
      {
        var name = typeof(R).Name;
        var config = new RealmConfiguration($"{name}.realm")
        {
          SchemaVersion = 1,
          ShouldDeleteIfMigrationNeeded = true,
          ObjectClasses = GetObjectClasses(),
          EncryptionKey = SecuredKeyProvider.Common.GetKey(name, 64)
        };
        return Realm.GetInstance(config);
      }
    }

    protected virtual Type[] GetObjectClasses()
    {
      return new Type[] { typeof(R) };
    }

    public async Task<R> Insert(R entity)
    {
      R result = default(R);

      await Realm.WriteAsync(realm =>
      {
        result = realm.Add(entity);
        result = result.Copy();
      });

      return result;
    }

    public async Task<R> InsertOrReplace(R entity)
    {
      R result = default(R);
      R copy = entity.Copy();

      await Realm.WriteAsync(realm =>
      {
        result = realm.Add(copy, true).Copy();
      });

      return result;
    }

    public async Task InsertOrReplace(IEnumerable<R> entities, string idProperty = "ID")
    {
      await Realm.WriteAsync(realm =>
      {
        if (entities == null || !entities.Any())
        {
          return;
        }

        var copy = entities.Copy();

        foreach (var entity in copy)
        {
          realm.Add(entity, true);
        }
      });
    }

    public async Task<R> FindByID(long id)
    {
      return await Task.Run(() => {

        R result = default(R);

        try
        {
          var realm = Realm;
          var obj = realm.Find(typeof(R).Name, id);

          if (obj is R robject)
          {
            result = robject.Copy();
          }
        }
        catch (Exception e)
        {
          System.Diagnostics.Debug.WriteLine($"Exception: {e.StackTrace}");
        }
        return result.Copy();
      });
    }

    public async Task<R> FindByID(string id)
    {
      return await Task.Run(() => {

        R result = default(R);

        try
        {
          var realm = Realm;
          var obj = realm.Find(typeof(R).Name, id);

          if (obj is R robject) 
          {
            result = robject.Copy();
          }
        }
        catch (Exception e)
        {
          System.Diagnostics.Debug.WriteLine($"Exception: {e.StackTrace}");
        }
        return result.Copy();
      });
    }

    public async Task DeleteByID(long id)
    {
      await Task.Run(async () =>
      {
        var realm = Realm;

        var item = realm.Find<R>(id);
        if (item != null)
        {
          await Realm.WriteAsync(r => r.Remove(item));
        }
      });
    }

    public async Task<IEnumerable<R>> All(Func<R, bool> condition = null)
    {
      IEnumerable<R> results;

      if (condition != null)
      {
        results = await Task.Run(() => Realm.All<R>()
                                 .Where(condition).AsEnumerable().Copy());
      }
      else
      {
        results = await Task.Run(() => Realm.All<R>().AsEnumerable().Copy());
      }
      return results;
    }

    public async Task DeleteAll()
    {
      await Realm.WriteAsync(realm =>
      {
        realm.RemoveAll();
      });
    }

    public async Task<int> DeleteAll(Expression<Func<R, bool>> predicate)
    {
      var cnt = 0;

      await Realm.WriteAsync(realm =>
      {
        var results = realm.All<R>().Where(predicate);
        cnt = results.Count();
        realm.RemoveRange(results);
      });
      return cnt;
    }

    public async Task<R> LastSortedBy<T>(Expression<Func<R, T>> predicate,
                                         Func<R, bool> condition = null)
    {
      return await Task.Run(() =>
      {
        if (condition != null)
        {
          return Realm.All<R>()
                    .OrderBy(predicate)
                    .LastOrDefault(condition)
                    .Copy();
        }

        return Realm.All<R>()
                    .OrderBy(predicate)
                    .LastOrDefault()
                    .Copy();
      });
    }

    public async Task<R> LastSortedByDescending<T>(Expression<Func<R, T>> predicate,
                                         Func<R, bool> condition = null)
    {
      return await Task.Run(() =>
      {
        if (condition != null)
        {
          return Realm.All<R>()
                      .OrderByDescending(predicate)
                      .LastOrDefault(condition)
                      .Copy();
        }

        return Realm.All<R>()
                    .OrderByDescending(predicate)
                    .LastOrDefault()
                    .Copy();
      });
    }
  }
}
