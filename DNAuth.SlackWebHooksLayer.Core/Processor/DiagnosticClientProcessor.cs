using DNAuth.FrameworkUtility.Library;
using DNAuth.SlackWebHooksLayer.Core.Processor.Interfaces;
using DNAuth.SlackWebHooksLayer.Domain.Entities.Client;
using DNAuth.SlackWebHooksLayer.Utility.Config.Interfaces;
using DNAuth.SlackWebHooksLayer.Utility.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Net.Http;

namespace DNAuth.SlackWebHooksLayer.Core.Processor {

    public class DiagnosticClientProcessor : IDiagnosticClientProcessor {

        #region [ Properties ]

        /// <summary>
        /// Classe de utilidade do projeto
        /// </summary>
        private readonly IDiagnosticApiClientUtility _diagnosticApiClientUtility;

        /// <summary>
        /// Processador do Slack
        /// </summary>
        private readonly ISlackClientProcessor _slackClientProcessor;

        /// <summary>
        /// Endereço base para envio do alerta
        /// </summary>
        private string _baseAddress = string.Empty;

        #endregion [ Properties ]

        /// <summary>
        /// Construtor
        /// </summary>
        public DiagnosticClientProcessor() {
            // Container de injeção de dependência
            var unityContainer = DependencyInjection.GetContainerInstance();

            // Resolve as dependências
            _diagnosticApiClientUtility = unityContainer.Resolve<IDiagnosticApiClientUtility>();
            _slackClientProcessor = unityContainer.Resolve<ISlackClientProcessor>();

            // Pega o endereço base para enviar o alerta
            _baseAddress = _diagnosticApiClientUtility.BaseAddress;
        }

        /// <summary>
        /// Envia o alerta para a diagnosticApi
        /// </summary>
        public void Execute_DiagnosticApi() {
            // Pega configuração do app.config
            var diagnosticApi = _diagnosticApiClientUtility.QueueInfo;
            // Lê o response
            var response = ReadResponse<Layer>(_baseAddress, diagnosticApi);
            SendAlert(response);
        }

        /// <summary>
        /// Envia o alerta para o selfTest
        /// </summary>
        public void Execute_DiagnosticSelfTest() {
            // Pega configuração do app.config
            var selfTest = _diagnosticApiClientUtility.SelfTestResult;
            // Lê o response
            var response = ReadResponse<SelfTestResponse>(_baseAddress, selfTest);
            SendAlert(response);
        }

        /// <summary>
        /// Envia erro para o slack
        /// </summary>
        /// <param name="ex"></param>
        public void SendErroToSlack(Exception ex) {
            // Envia o alerta
            _slackClientProcessor.SendErrorToSlack(ex);
        }

        #region [ Private Methods ]

        /// <summary>
        /// Retorna o objeto de response
        /// </summary>
        /// <param name="baseAddress">Endereço base</param>
        /// <param name="uri">Complemento do endereço</param>
        private TResponse ReadResponse<TResponse>(string baseAddress, string uri) where TResponse : class {
            // Constrói a menssagem http
            var responseMessage = BuildMessage.BuildHttpMessage(baseAddress, uri);

            // Response para enviar para o Slack
            var response = default(TResponse);

            // Se conseguiu construir a mensagem
            if (responseMessage.IsSuccessStatusCode == true) {
                // Pega o conteúdo
                response = responseMessage.Content.ReadAsAsync<TResponse>().Result;
            }

            // Retorno
            return response;
        }

        private void SendAlert<TResponse>(TResponse response) {
            // Se não for nulo
            if (response != null) {
                if (response is Layer) {
                    var layerResponse = (Layer)Convert.ChangeType(response, typeof(Layer));
                    // Envia o alerta
                    _slackClientProcessor.SendQueueMessage(layerResponse);
                }
                if (response is SelfTestResponse) {
                    var selfTestResponse = (SelfTestResponse)Convert.ChangeType(response, typeof(SelfTestResponse));
                    // Envia o alerta
                    _slackClientProcessor.SendSelfTestMessage(selfTestResponse);
                }

#if DEBUG
                // Se estiver em debug, imprime a mensagem
                Console.WriteLine("Alerta enviado!");
#endif
            }
        }
        #endregion
    }
}