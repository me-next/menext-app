using System;
namespace MeNext
{
    /// <summary>
    /// The result of attempting to leave an event
    /// </summary>
    public enum LeaveEventResult
    {
        /// <summary>
        /// Leaving the event succeeded
        /// </summary>
        SUCCESS,

        /// <summary>
        /// There was some unspecified error
        /// </summary>
        FAIL_GENERIC,

        /// <summary>
        /// There was trouble connecting to the server
        /// </summary>
        FAIL_NETWORK
    }
}
