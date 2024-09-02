namespace Fun3;

public interface IProviderContainer
{
    static abstract void ConfigureProvider(string[] lines);
    static abstract IServiceProvider Provider { get; }
}
