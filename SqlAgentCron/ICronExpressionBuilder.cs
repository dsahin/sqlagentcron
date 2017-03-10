namespace SqlAgentCron
{
    public interface ICronExpressionBuilder
    {
        string GetExpression(ISqlAgentJobSchedule sqlAgentJobSchedule);
    }
}
