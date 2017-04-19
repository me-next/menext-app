using System;
namespace MeNext
{
    /// <summary>
    /// Join event class.
    /// Used to organize CreateEventResult and JoinEventResult easily.
    /// </summary>
    public class JoinEventClass
    {
        public string JSON { get; set; }
        public string PID { get; set; }
        public CreateEventResult EventResult { get; set; }
        /// <summary>
        /// Initializes when creating a new event.
        /// </summary>
        /// <param name="ERes">Return value from Creating a new event.</param>
        public JoinEventClass(CreateEventResult ERes)
        {
            PID = ERes.ToString();
            EventResult = ERes;
            JSON = ERes.ToString();
        }
        /// <summary>
        /// Initializes when joining new event.
        /// </summary>
        /// <param name="ERes">Return value from Joining an event.</param>
        public JoinEventClass(JoinEventResult ERes)
        {
            PID = ERes.ToString();
            EventResult = (CreateEventResult) ERes;
            JSON = ERes.ToString();
        }
    }
}
