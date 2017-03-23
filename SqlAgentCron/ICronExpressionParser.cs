namespace SqlAgentCron
{
    public interface ICronExpressionParser
    {
        SqlAgentJobSchedule GetSchedule(string cronExpression);
    }
}