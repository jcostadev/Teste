using System;
using DNAuth.SlackWebHooksLayer.Domain.Entities.Client;

namespace DNAuth.SlackWebHooksLayer.Core.Processor.Interfaces {

    public interface ISlackClientProcessor {

        /// <summary>
        /// Envia alerta de selfTest para o slack
        /// </summary>
        void SendSelfTestMessage(SelfTestResponse response);

        /// <summary>
        /// Envia alerta de queue para o slack
        /// </summary>
        void SendQueueMessage(Layer response);

        /// <summary>
        /// Envia alerta de erro para o slack
        /// </summary>
        /// <param name="ex"></param>
        void SendErrorToSlack(Exception ex);
    }
}