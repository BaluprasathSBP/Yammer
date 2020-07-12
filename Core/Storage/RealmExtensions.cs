using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Realms;

namespace Core.Utils
{
  public static class RealmExtensions
  {
    /// <summary>
    /// Copies from realm object. Returns new instance of T
    /// </summary>
    /// <returns>The from realm object.</returns>
    /// <param name="realmObject">Realm object.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T CopyFromRealmObject<T>(this RealmObject realmObject)
    {
      var standAlone = Activator.CreateInstance(typeof(T));

      foreach (var property in standAlone.GetType().GetRuntimeProperties())
      {
        var realmProperty = realmObject.GetType().GetRuntimeProperty(property.Name);

        if (!realmProperty.CanRead || (realmProperty.GetIndexParameters().Length > 0))
        {
          continue;
        }

        object val;

        //DateTimeOffset should be DateTime
        if (realmProperty.PropertyType == typeof(DateTimeOffset))
        {
          val = ((DateTimeOffset)realmProperty.GetValue(realmObject, null)).DateTime;
        }
        else
        {
          val = realmProperty.GetValue(realmObject, null);
        }
        property.SetValue(standAlone, val, null);
      }

      return (T)standAlone;
    }

    /// <summary>
    /// Copies all from realm objects.
    /// </summary>
    /// <returns>The all from realm objects.</returns>
    /// <param name="realmObjects">Realm objects.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static IEnumerable<T> CopyAllFromRealmObjects<T>(this IEnumerable<RealmObject> realmObjects)
    {
      var newStandAlones = new List<T>();

      foreach (var realmObject in realmObjects)
      {
        newStandAlones.Add(realmObject.CopyFromRealmObject<T>());
      }

      return newStandAlones;
    }

    /// <summary>
    /// Copies to realm object. Returns new instance of U
    /// </summary>
    /// <returns>The to realm object.</returns>
    /// <param name="standAlone">Stand alone.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    /// <typeparam name="U">The 2nd type parameter.</typeparam>
    public static U CopyToRealmObject<U>(this object standAlone, object realmObject = null)
      where U : RealmObject
    {
      if (realmObject == null)
      {
        realmObject = Activator.CreateInstance(typeof(U));
      }

      foreach (var property in standAlone.GetType().GetRuntimeProperties())
      {
        if (!property.CanRead || (property.GetIndexParameters().Length > 0))
        {
          continue;
        }

        var realmProperty = realmObject.GetType().GetRuntimeProperty(property.Name);

        if (!realmProperty.CanWrite || !realmProperty.SetMethod.IsPublic)
        {
          continue;
        }

        object val;

        //DateTimeO should be DateTimeOffset 
        if (property.PropertyType == typeof(DateTime))
        {
          val = new DateTimeOffset((DateTime)property.GetValue(standAlone, null));
        }
        else
        {
          val = property.GetValue(standAlone, null);
        }
        realmProperty.SetValue(realmObject, val, null);
      }

      return (U)realmObject;
    }

    /// <summary>
    /// Copies all to realm objects.
    /// </summary>
    /// <returns>The all to realm objects.</returns>
    /// <param name="standAlones">Stand alones.</param>
    /// <typeparam name="U">The 1st type parameter.</typeparam>
    public static IEnumerable<U> CopyAllToRealmObjects<U>(this IEnumerable<object> standAlones)
      where U : RealmObject
    {
      var newRealmObjects = new List<U>();

      foreach (var standAlone in standAlones)
      {
        newRealmObjects.Add(standAlone.CopyToRealmObject<U>());
      }

      return newRealmObjects;
    }


    /// <summary>
    /// Copies the realm properties.
    /// </summary>
    /// <returns>The realm.</returns>
    /// <param name="first">First.</param>
    /// <param name="second">Second.</param>
    /// <typeparam name="U">The 1st type parameter.</typeparam>
    public static U Copy<U>(this U first, U second = null) where U : RealmObject
    {
      if (first == null)
      {
        return default(U);
      }

      if (second == null)
      {
        second = (U)Activator.CreateInstance(typeof(U));
      }

      foreach (var property in typeof(U).GetRuntimeProperties())
      {
        var isList = property.PropertyType.IsConstructedGenericType
          && property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>);

        if (!property.CanRead
            || (property.GetIndexParameters().Length > 0)
            || (!property.CanWrite && !isList)
           )
        {
          continue;
        }

        object val = property.GetValue(first, null);

        if(property.PropertyType.IsSubclassOf(typeof(RealmObject)))
        {
          try
          {
            var copyMethod = typeof(RealmExtensions).GetRuntimeMethods()
                                                    .FirstOrDefault(m => m.Name == "Copy");
            copyMethod = copyMethod.MakeGenericMethod(property.PropertyType);

            var copy = copyMethod.Invoke(val, new object[] { val, null });

            property.SetValue(second, copy, null);
          }
          catch(Exception e)
          {
            Console.WriteLine($"{e.StackTrace}");
          }
        }
        else if (isList)
        {
          var baseType = property.PropertyType.GenericTypeArguments[0];

          object secondVal = property.GetValue(second, null);

          var addMethod = secondVal.GetType().GetRuntimeMethod("Add", new Type[] { baseType });
          MethodInfo copyMethod = null;

          foreach (var item in (IEnumerable)val)
          {
            if (copyMethod == null)
            {
              copyMethod = typeof(RealmExtensions).GetRuntimeMethods()
                                                  .FirstOrDefault(m => m.Name == "Copy");
              copyMethod = copyMethod.MakeGenericMethod(new Type[] { baseType });
            }

            if (copyMethod != null)
            {
              var copy = copyMethod.Invoke(null, new object[] { item, null });
              addMethod.Invoke(secondVal, new object[] { copy });
            }
          }
        }
        else
        {
          property.SetValue(second, val, null);
        }
      }
      return second;
    }

    public static IEnumerable<U> Copy<U>(this IEnumerable<U> realmObjects)
      where U : RealmObject
    {
      var newRealmObjects = new List<U>();

      foreach (var realmObject in realmObjects)
      {
        newRealmObjects.Add(realmObject.Copy());
      }

      return newRealmObjects;
    }
  }
}
