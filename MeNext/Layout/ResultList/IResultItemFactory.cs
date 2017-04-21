using MeNext.MusicService;

namespace MeNext
{
    /// <summary>
    /// Represents a factory which can produce a ResultItemData from some type of metadata.
    /// </summary>
    public interface IResultItemFactory<T> where T : IMetadata
    {
        /// <summary>
        /// Generates a ResultItemData from a chunk of metadata.
        /// </summary>
        /// <returns>The data wrapper with salient fields filled.</returns>
        /// <param name="from">The metatdata we are wrapping.</param>
        ResultItemData GetResultItem(T from);
    }
}