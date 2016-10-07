using System;

namespace DNAuth.SlackWebHooksLayer.Domain.Entities.Client {

    public class SelfTestResult {

        /// <summary>
        /// Data do selfTestResult
        /// </summary>
        public DateTime CreateDate { get; set; }

        public int AuthReponseTimeMs { get; set; }

        public int CancelReponseTimeMs { get; set; }

        public int ReversalReponseTimeMs { get; set; }

        public int AffiliationReponseTimeMs { get; set; }
    }
}