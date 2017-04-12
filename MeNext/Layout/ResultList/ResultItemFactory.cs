using MeNext.MusicService;

namespace MeNext
{
    public interface ResultItemFactory<T> where T : IMetadata
    {
        ResultItemData GetResultItem(T from);
    }
}