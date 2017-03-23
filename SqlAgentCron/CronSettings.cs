namespace SqlAgentCron
{
    public class CronSettings : ICronSettings
    {
        public bool IsLastSupported { get; set; }
        public bool IsHashSupported { get; set; }
    }
}