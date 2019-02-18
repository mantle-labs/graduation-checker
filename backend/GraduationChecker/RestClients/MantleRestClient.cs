using GraduationChecker.Config;
using Mantle.Core.Lib;
using System.Collections.Generic;

namespace GraduationChecker.RestClients
{
    public class MantleRestClient : RestClient
    {
        public MantleRestClient(AppSettings appSettings) : base(appSettings.MantleApiUrl, new Dictionary<string, string> { { "x-api-key", appSettings.ApiKey } })
        {
        }
    }
}
