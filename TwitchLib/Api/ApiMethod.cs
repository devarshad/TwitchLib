namespace TwitchLib.Api
{
    public class ApiMethod : IFluentInterface
    {
        protected Settings Settings;
        protected Requests Requests;

        public ApiMethod(Settings settings, Requests requests)
        {
            Settings = settings;
            Requests = requests;
        }
    }
}
