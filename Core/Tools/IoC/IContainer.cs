using System;

namespace Core.Tools.IoC
{
  public interface IContainer
  {
    void RegisterAsSingleton<TInterface, TType>()
      where TInterface : class
      where TType : class, TInterface;

    void Register<TInterface, TType>()
      where TInterface : class
      where TType : class, TInterface;

    void RegisterAsSingleton<TInterface>(Func<TInterface> serviceConstructor) where TInterface : class;

    TInterface Resolve<TInterface>() where TInterface : class;

    TType Construct<TType>() where TType : class;
  }
}
