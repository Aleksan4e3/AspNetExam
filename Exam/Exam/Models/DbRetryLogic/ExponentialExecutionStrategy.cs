using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Exam.Models.DbRetryLogic
{
    public class ExponentialExecutionStrategy : DbExecutionStrategy
    {
        public ExponentialExecutionStrategy(int maxRetryCount, TimeSpan maxDelay) : base(maxRetryCount, maxDelay) { }

        private readonly List<int> _errorCodesToRetry = new List<int>
        {
            SqlRetryErrorCodes.Deadlock,
            SqlRetryErrorCodes.TimeoutExpired,
            SqlRetryErrorCodes.CouldNotOpenConnection,
            SqlRetryErrorCodes.TransportFail
        };
        protected override bool ShouldRetryOn(Exception exception)
        {
            var sqlException = exception as SqlException;
            if (sqlException != null)
            {
                foreach (SqlError err in sqlException.Errors)
                {
                    // Enumerate through all errors found in the exception.
                    if (_errorCodesToRetry.Contains(err.Number))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}