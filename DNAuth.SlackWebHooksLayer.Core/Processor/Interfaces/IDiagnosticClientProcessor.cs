using System;

namespace DNAuth.SlackWebHooksLayer.Core.Processor.Interfaces {

    public interface IDiagnosticClientProcessor {

        /// <summary>
        /// Envia o alerta para a diagnosticApi
        /// </summary>
        void Execute_DiagnosticApi();

        /// <summary>
        /// Envia o alerta para o selfTest
        /// </summary>
        void Execute_DiagnosticSelfTest();

        /// <summary>
        /// Envia erro para o slack
        /// </summary>
        /// <param name="ex"></param>
        void SendErroToSlack(Exception ex);
    }
}