using System;
namespace MeNext
{
    /// <summary>
    /// The result of a request to end an event
    /// </summary>
    public enum EndEventResult
    {
        /// <summary>
        /// Ending the event succeeded
        /// </summary>
        SUCCESS,

        /// <summary>
        /// There was some unspecified error
        /// </summary>
        FAIL_GENERIC,

        /// <summary>
        /// This user does not have permission to end this event
        /// </summary>
        FAIL_PERMISSION,

        /// <summary>
        /// There was trouble connecting to the server
        /// </summary>
        FAIL_NETWORK
    }
}
