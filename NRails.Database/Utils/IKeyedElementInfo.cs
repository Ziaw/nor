namespace Rsdn.Janus
{
    public interface IKeyedElementInfo<TKey>
    {
        TKey Key { get; }
    }
}