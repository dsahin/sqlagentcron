namespace SqlAgentCron
{
    public interface ICronSettings
    {
        bool IsLastSupported { get; set; }
        bool IsHashSupported { get; set; }
    }
}