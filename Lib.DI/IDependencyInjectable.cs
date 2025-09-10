namespace Lib.DI;

public interface IDependencyInjectable;
public interface IScoped : IDependencyInjectable;
public interface ISingleton : IDependencyInjectable;
public interface IScoped<T>: IScoped;
public interface ISingleton<T>: ISingleton;