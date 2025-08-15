namespace TestTask.Core.Services.Interface;

public interface INormalizerService<T>
{
    T Normalize(T source);
}
