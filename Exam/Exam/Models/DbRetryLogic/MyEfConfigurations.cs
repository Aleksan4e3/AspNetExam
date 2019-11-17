using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Exam.Models.DbRetryLogic
{
    public class MyEfConfigurations : DbConfiguration
    {
        public MyEfConfigurations()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new ExponentialExecutionStrategy(5, TimeSpan.FromSeconds(10)));
        }
    }
}