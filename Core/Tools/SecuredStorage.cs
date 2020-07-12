using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Core.Tools
{
  public interface ISecuredStorage
  {
    /// <summary>
    /// Get the specified key.
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="key">Key.</param>
    string Get(string key);

    /// <summary>
    /// Set the specified key and value.
    /// </summary>
    /// <returns>The set.</returns>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    bool Set(string key, string value);

    /// <summary>
    /// Gets the async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="key">Key.</param>
    Task<string> GetAsync(string key);

    /// <summary>
    /// Sets the async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    Task<bool> SetAsync(string key, string value);

    /// <summary>
    /// Get the specified key.
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="key">Key.</param>
    IObservable<string> GetAsObservable(string key);

    /// <summary>
    /// Sets as observable.
    /// </summary>
    /// <returns>The as observable.</returns>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    IObservable<bool> SetAsObservable(string key, string value);
  }

  public class SecuredStorage : ISecuredStorage
  {
    static readonly object Lock = new object();

    readonly static IDictionary<string, string> _memCache = new Dictionary<string, string>();

    public async Task<string> GetAsync(string key)
    {
      if (_memCache.TryGetValue(key, out string value))
      {
        return _memCache[key];
      }

      string result = await Xamarin.Essentials.SecureStorage.GetAsync(key);
      _memCache[key] = result;
      return result;
    }

    public Task<bool> SetAsync(string key, string value)
    {
      return Task.Run(() =>
      {
        lock (Lock)
        {
          if (!string.IsNullOrEmpty(value))
          {
            _memCache[key] = value;
            var existing = Xamarin.Essentials.SecureStorage.GetAsync(key).Result;
            if (existing != value)
            {
              Xamarin.Essentials.SecureStorage.SetAsync(key, value).Wait();
            }
          }
          else
          {
            if (_memCache.ContainsKey(key))
            {
              _memCache.Remove(key);
            }
            Xamarin.Essentials.SecureStorage.Remove(key);
          }
        }
        return true;
      });
    }

    public string Get(string key)
    {
      return GetAsync(key).Result;
    }

    public bool Set(string key, string value)
    {
      return SetAsync(key, value).Result;
    }

    public IObservable<string> GetAsObservable(string key)
    {
      return GetAsync(key).ToObservable();
    }

    public IObservable<bool> SetAsObservable(string key, string value)
    {
      return SetAsync(key, value).ToObservable();
    }
  }
}
