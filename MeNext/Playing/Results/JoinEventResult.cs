using System;
namespace MeNext
{
    /// <summary>
    /// The result of the request to join an event
    /// </summary>
    public enum JoinEventResult
    {
        /// <summary>
        /// The join request succeeded
        /// </summary>
        SUCCESS,

        /// <summary>
        /// There was some unspecified error
        /// </summary>
        FAIL_GENERIC,

        /// <summary>
        /// The event was full
        /// </summary>
        FAIL_EVENT_FULL,

        /// <summary>
        /// No event with the given slug exists
        /// </summary>
        FAIL_EVENT_NONEXISTENT,

        /// <summary>
        /// A user with the requested username is already in the event
        /// </summary>
        FAIL_USERNAME_EXISTS,

        /// <summary>
        /// The user was denied entry into the event
        /// </summary>
        FAIL_BLOCKED,

        /// <summary>
        /// There was trouble connecting to the server
        /// </summary>
        FAIL_NETWORK
    }
}
