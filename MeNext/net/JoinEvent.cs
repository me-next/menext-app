using System;
namespace MeNext
{
    public class JoinEventClass
    {
        //Add other Classes we want.
        private string JSON { get; set; }
        public string PID { get; set; }
        public CreateEventResult EventResult { get; set; }
        public JoinEventClass(CreateEventResult ERes)
        {
            this.PID = ERes.ToString();
            this.JSON = ERes.ToString();
            this.EventResult = ERes;
        }
    }
}
