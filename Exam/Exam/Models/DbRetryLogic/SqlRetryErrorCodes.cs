using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam.Models.DbRetryLogic
{
    public static class SqlRetryErrorCodes
    {
        public const int TimeoutExpired = -2;
        public const int Deadlock = 1205;
        public const int CouldNotOpenConnection = 53;
        public const int TransportFail = 121;
    }
}