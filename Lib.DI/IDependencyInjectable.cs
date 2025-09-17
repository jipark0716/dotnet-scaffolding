using JetBrains.Annotations;

namespace Lib.DI;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IDependencyInjectable;

public interface IScoped : IDependencyInjectable;

public interface ISingleton : IDependencyInjectable;

public interface IScoped<T>: IScoped;

public interface ISingleton<T>: ISingleton;