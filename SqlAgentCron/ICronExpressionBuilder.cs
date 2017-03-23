namespace SqlAgentCron
{
    public interface ICronExpressionBuilder
    {
        string GetExpression(SqlAgentJobSchedule sqlAgentJobSchedule);
    }
}
