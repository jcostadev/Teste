using System.Collections.Generic;

namespace DNAuth.SlackWebHooksLayer.Domain.Entities.Client {


    public class SelfTestResponse {

        /// <summary>
        /// Lista de seflfTestResults
        /// </summary>
        public IList<SelfTestResult> SelfTestResults { get; set; }
    }
}