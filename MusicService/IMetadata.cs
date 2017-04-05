using System;
namespace MeNext.MusicService
{
    public interface IMetadata
    {
        /// <summary>
        /// Gets a unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        string UniqueId
        {
            get;
        }
    }
}
