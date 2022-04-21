using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CynthiasWebApp.Support
{
    public class ReqParms
    {
        public const string Action = "[action]";
        public const string ActionEmail = "[action]/{email}";
        public const string ActionEmailCost = "[action]/{email}/{cost}";
        public const string ActionRequest = "[action]/{request}";
    }
}
