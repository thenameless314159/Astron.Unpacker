namespace Astron.Files
{
    public interface IFileValueUnpacker<out TValue>
    {
        TValue Value { get; }
        void Unpack(IFileAccessor fileAccessor);
    }
}
