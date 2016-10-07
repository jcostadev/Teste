using System;
using DNAuth.FrameworkUtility.Library;
using DNAuth.SlackWebHooksLayer.Core.Processor.Interfaces;
using DNAuth.SlackWebHooksLayer.Domain.Entities.Client;
using DNAuth.SlackWebHooksLayer.Domain.Entities.Slack;
using DNAuth.SlackWebHooksLayer.Utility.Config.Interfaces;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;

namespace DNAuth.SlackWebHooksLayer.Core.Processor {

    public class SlackClientProcessor : ISlackClientProcessor {

        #region [ Properties ]

        /// <summary>
        /// Classe de utilidade do projeto
        /// </summary>
        private readonly IDiagnosticApiClientUtility _diagnosticApiClientUtility;

        /// <summary>
        /// Canal para onde a mensagem será enviada
        /// </summary>
        private SlackClient _slackClient;

        #endregion [ Properties ]

        /// <summary>
        /// Construtor
        /// </summary>
        public SlackClientProcessor() {
            // Container de injeção de dependência
            var unityContainer = DependencyInjection.GetContainerInstance();

            // Resolve as dependências
            _diagnosticApiClientUtility = unityContainer.Resolve<IDiagnosticApiClientUtility>();

            // Preenche os clientes
            _slackClient = new SlackClient(_diagnosticApiClientUtility.SlackChannel);
        }

        /// <summary>
        /// Envia alerta de selfTest para o slack
        /// </summary>
        /// <param name="response"></param>
        public void SendSelfTestMessage(SelfTestResponse response) {
            // Pega lista de objetos do response
            var listResult = response.SelfTestResults;

            // Ordena pela data e pega a maior
            var selftest = listResult.OrderByDescending(o => o.CreateDate).FirstOrDefault();

            // Se não for zero, não envia alerta nenhum
            if (selftest.AffiliationReponseTimeMs != 0 &&
                selftest.AuthReponseTimeMs != 0 &&
                selftest.CancelReponseTimeMs != 0 &&
                selftest.ReversalReponseTimeMs != 0) {
                // Sai da função
                return;
            }

            // Monta o objeto
            var slackAttachment = BuildSelfTestMessage(selftest);

            // Cria um slackMessage para enviar
            var slackMessage = new SlackMessage {
                Attachments = new List<SlackAttachment> { slackAttachment }
            };

            // Enviando para o canal
            _slackClient.Post(slackMessage);
        }

        /// <summary>
        /// Envia alerta de queue para o slack
        /// </summary>
        /// <param name="response"></param>
        public void SendQueueMessage(Layer response) {
            // Percorre os itens do response
            foreach (var layer in response.QueueLayers) {
                // Varre as filas
                foreach (var queueu in layer.Queues) {
                    // Se não tiver consumidor na fila
                    if (queueu.Data.Consumers == 0) {
                        // Monta o objeto
                        var slackAttachment = BuildQueueMessage(queueu);

                        // Cria um slackMessage para enviar
                        var slackMessage = new SlackMessage {
                            Attachments = new List<SlackAttachment> { slackAttachment }
                        };

                        // Enviando para o canal
                        _slackClient.Post(slackMessage);
                    }
                }
            }
        }


        /// <summary>
        /// Envia alerta de erro para o slack
        /// </summary>
        /// <param name="ex"></param>
        public void SendErrorToSlack(Exception ex) {

            // Monta o slackField com a mensagem da fila
            // ReSharper disable once UseObjectOrCollectionInitializer
            var slackFieldQueueMessage = new SlackField();
            slackFieldQueueMessage.Short = true;
            slackFieldQueueMessage.Title = ">> Exception <<";
            slackFieldQueueMessage.Value = $"Message: {ex.Message}\n" +
                                           $"StackTrace: {ex.StackTrace}";

            // Monta o attachment
            var slackAttachment = new SlackAttachment();
            slackAttachment.Fields = new List<SlackField>
            {
                slackFieldQueueMessage,
            };

            // Cor do attachment
            slackAttachment.Color = "#FF0000";
            // Autor da mensagem
            slackAttachment.AuthorName = _diagnosticApiClientUtility.SlackAuthor;
            // Texto abaixo da data
            slackAttachment.Pretext = _diagnosticApiClientUtility.SlackPretext;
            // Título do attachment
            slackAttachment.Title = _diagnosticApiClientUtility.ErroTitle;
            // Ícone do lado do nome do autor
            slackAttachment.AuthorIcon = _diagnosticApiClientUtility.AuthorIconUrl;
            // Mensagem que aparece de alerta no celular ou nas notificações do slack
            slackAttachment.Fallback = _diagnosticApiClientUtility.ServerName;
            // Thumbnail de erro
            slackAttachment.ThumbUrl = _diagnosticApiClientUtility.ErrorIconUrl;

            // Cria um slackMessage para enviar
            var slackMessage = new SlackMessage {
                Attachments = new List<SlackAttachment> { slackAttachment }
            };

            // Enviando para o canal
            _slackClient.Post(slackMessage);
        }


        #region [ Private methods ]

        /// <summary>
        /// Constrói a mensagem da fila
        /// </summary>
        /// <param name="queueu"></param>
        /// <returns></returns>
        private SlackAttachment BuildQueueMessage(QueueInfo queueu) {
            // Se vier este texto, coloca o texto Online
            var idle = queueu.Data.Idle_Since.Equals("0001-01-01T00:00:00Z") ? "Online" : queueu.Data.Idle_Since;

            // Monta o slackField com a mensagem da fila
            // ReSharper disable once UseObjectOrCollectionInitializer
            var slackFieldQueueMessage = new SlackField();
            slackFieldQueueMessage.Short = true;
            slackFieldQueueMessage.Title = "Queue Info";
            slackFieldQueueMessage.Value = $"name: {queueu.Data.Name}\n" +
                                           $"date: {queueu.Data.CreateDate}\n" +
                                           $"idle: {idle}\n" +
                                           $"messages: {queueu.Data.Messages}\n" +
                                           $"ready: {queueu.Data.Messages_Ready}\n" +
                                           $"unack: {queueu.Data.Messages_Unacknowledged}\n" +
                                           $"state: {queueu.Data.State}";

            // Monta o attachment
            var slackAttachment = new SlackAttachment();
            slackAttachment.Fields = new List<SlackField>
            {
                slackFieldQueueMessage,
            };

            // Cor do attachment
            slackAttachment.Color = "#F35A00";
            // Autor da mensagem
            slackAttachment.AuthorName = _diagnosticApiClientUtility.SlackAuthor;
            // Texto abaixo da data
            slackAttachment.Pretext = _diagnosticApiClientUtility.SlackPretext;
            // Título do attachment
            slackAttachment.Title = _diagnosticApiClientUtility.ProblemsQueue;
            // Link para o dashboard referenciado no título
            slackAttachment.TitleLink = _diagnosticApiClientUtility.QueueStatusUrlDashboard;
            // Ícone do lado do nome do autor
            slackAttachment.AuthorIcon = _diagnosticApiClientUtility.AuthorIconUrl;
            // Mensagem que aparece de alerta no celular ou nas notificações do slack
            slackAttachment.Fallback = _diagnosticApiClientUtility.ServerName;
            // Thumbnail dos mochileiros
            slackAttachment.ThumbUrl = _diagnosticApiClientUtility.IconUrl;

            return slackAttachment;
        }

        /// <summary>
        /// Constrói a mensagem do SelfTest
        /// </summary>
        /// <param name="selftest"></param>
        /// <returns></returns>
        private SlackAttachment BuildSelfTestMessage(SelfTestResult selftest) {
            // Monta o slackField com a mensagem da fila
            var slackFieldSelfTest = new SlackField();
            slackFieldSelfTest.Short = true;
            slackFieldSelfTest.Title = "SelfTest Response Time";

            // Monta a string a ser enviada para o slack
            slackFieldSelfTest.Value = $"Date: {selftest.CreateDate}\n" +
                                       $"Auth: {selftest.AuthReponseTimeMs}\n" +
                                       $"Cancel: {selftest.CancelReponseTimeMs}\n" +
                                       $"Reversal: {selftest.ReversalReponseTimeMs}\n" +
                                       $"Affiliation: {selftest.AffiliationReponseTimeMs}";

            // Monta o attachment
            var slackAttachment = new SlackAttachment();
            slackAttachment.Fields = new List<SlackField>
            {
                slackFieldSelfTest
            };

            // Cor do attachment
            slackAttachment.Color = "#F35A00";
            // Autor da mensagem
            slackAttachment.AuthorName = _diagnosticApiClientUtility.SlackAuthor;
            // Texto abaixo da data
            slackAttachment.Pretext = _diagnosticApiClientUtility.SlackPretext;
            // Título do attachment
            slackAttachment.Title = _diagnosticApiClientUtility.ProblemsSelfTest;
            // Link para o dashboard referenciado no título
            slackAttachment.TitleLink = _diagnosticApiClientUtility.SelfTestUrlDashboard;
            // Ícone do lado do nome do autor
            slackAttachment.AuthorIcon = _diagnosticApiClientUtility.AuthorIconUrl;
            // Mensagem que aparece de alerta no celular ou nas notificações do slack
            slackAttachment.Fallback = _diagnosticApiClientUtility.ServerName;
            // Thumbnail dos mochileiros
            slackAttachment.ThumbUrl = _diagnosticApiClientUtility.IconUrl;

            return slackAttachment;
        }

        #endregion [ Private methods ]
    }
}