using System;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace SqlAgentCron.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(".");
            Job job = server.JobServer.GetJobByID(Guid.Parse("37CC970D-2F27-417B-A165-8E23D42A6A17"));
        }
    }
}
