using System;
namespace MeNext
{
    public class JoinEventClass
    {
        //Add other Classes we want.
        public string JSON { get; set; }
        public string PID { get; set; }
        public CreateEventResult EventResult { get; set; }
        public JoinEventClass(CreateEventResult ERes)
        {
            PID = ERes.ToString();
            EventResult = ERes;
            JSON = ERes.ToString();
        }
        public JoinEventClass(JoinEventResult ERes)
        {
            PID = ERes.ToString();
            EventResult = (CreateEventResult) ERes;
            JSON = ERes.ToString();
        }
    }
    public class commandClass
    {
        public string name { get; set; }
        public MainController mc {get; set;}
        public commandClass(MainController mc, Xamarin.Forms.Entry entry)
        {
            this.mc = mc;
            this.name = entry.Text;
        }
    }
}
