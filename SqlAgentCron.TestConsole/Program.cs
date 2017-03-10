using System;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace SqlAgentCron.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("DESKTOP-2MK15TL");
            Job job = server.JobServer.GetJobByID(Guid.Parse("079A5512-8482-43B3-8F6E-2C8C79C990EA"));
        }
    }
}
