namespace NRails.Database
{
    public interface IKeyedElementInfo<TKey>
    {
        TKey Key { get; }
    }
}