namespace Astron.Files
{
    public interface IValidation<in T>
    {
        bool IsValid(T value);
    }
}
