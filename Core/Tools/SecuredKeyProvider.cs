using System;
using System.Collections.Generic;

namespace Core.Tools
{
  public interface ISecuredKeyProvider
  {
    byte[] GetKey(string key, int length);
  }

  public class SecuredKeyProvider : ISecuredKeyProvider
  {
    static SecuredKeyProvider _common;
    public static SecuredKeyProvider Common => _common ?? (_common = new SecuredKeyProvider());

    static readonly object Lock = new object();

    readonly ISecuredStorage _securedStorage;

    readonly static Lazy<IDictionary<string, byte[]>> Cache =
      new Lazy<IDictionary<string, byte[]>>(() =>
    {
      return new Dictionary<string, byte[]>();
    });

    public SecuredKeyProvider()
    {
      _securedStorage = new SecuredStorage();
    }

    public SecuredKeyProvider(ISecuredStorage securedStorage)
    {
      _securedStorage = securedStorage;
    }

    public byte[] GetKey(string key, int length)
    {
      lock(Lock)
      {
        if (Cache.Value.TryGetValue(key, out byte[] secureKey))
        {
          return secureKey;
        }

        var storedKey = _securedStorage.Get(key);
        if (string.IsNullOrEmpty(storedKey))
        {
          Random random = new Random();

          byte[] buffer = new byte[length];
          random.NextBytes(buffer);

          storedKey = Convert.ToBase64String(buffer);

          _securedStorage.Set(key, storedKey);
        }

        var result = Convert.FromBase64String(storedKey);
        Cache.Value[key] = result;

        return result;
      }
    }
  }
}
