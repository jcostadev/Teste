using DNAuth.Domain.Enums.Generics;
using DNAuth.Domain.Enums.Logger;
using DNAuth.FrameworkUtility.Library;
using DNAuth.FrameworkUtility.Logger.Interfaces;
using DNAuth.SlackWebHooksLayer.Core.Manager.Interface;
using DNAuth.SlackWebHooksLayer.Core.Processor.Interfaces;
using DNAuth.SlackWebHooksLayer.Utility.Config.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Threading;
using System.Timers;

namespace DNAuth.SlackWebHooksLayer.Core.Manager {

    public class DiagnosticApiClientManager : IDisposable, IDiagnosticApiClientManager {

        #region Properties

        /// <summary>
        /// Gerenciador de log
        /// </summary>
        private readonly ILogManager _logManager;

        /// <summary>
        /// Processadores de tarefas
        /// </summary>
        private readonly IDiagnosticClientProcessor _diagnosticClientProcessor;

        /// <summary>
        /// Temporizador DiagnosticApi
        /// </summary>
        private readonly System.Timers.Timer _clockCacheDiagnosticApi;

        /// <summary>
        /// Temporizador SelfTest
        /// </summary>
        private readonly System.Timers.Timer _clockCacheSelfTest;

        /// <summary>
        /// Controlador de lock de thread
        /// </summary>
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

        #endregion Properties

        public DiagnosticApiClientManager() {
            //Container de injeção de dependência
            var unityContainer = DependencyInjection.GetContainerInstance();

            //Genreciador da log
            _logManager = unityContainer.Resolve<ILogManager>();

            //Processadores de tarefas
            _diagnosticClientProcessor = unityContainer.Resolve<IDiagnosticClientProcessor>();

            // Configurações
            var diagnosticApiClientUtility = unityContainer.Resolve<IDiagnosticApiClientUtility>();

            // Tempo para o temporizador DiagnosticApi
            var timeInMinutesToSendRequestDiagnosticApi =
                TimeUtility.ConvertMinutesToMilliseconds(
                    Convert.ToDouble(diagnosticApiClientUtility.TimeInMinutesToSendRequest_DiagnosticApi));

            // Tempo para o temporizador SelfTest
            var timeInMinutesToSendRequestSelfTest =
                TimeUtility.ConvertMinutesToMilliseconds(
                    Convert.ToDouble(diagnosticApiClientUtility.TimeInMinutesToSendRequest_SelfTest));

            //Inicia o temporizador DiagnosticApi
            _clockCacheDiagnosticApi = new System.Timers.Timer(timeInMinutesToSendRequestDiagnosticApi);

            //Inicia o temporizador SelfTest
            _clockCacheSelfTest = new System.Timers.Timer(timeInMinutesToSendRequestSelfTest);

            //Atribui método ao evento do temporizador da DiagnosticApi
            _clockCacheDiagnosticApi.Elapsed += ClockCacheOnElapsedDiagnosticApi;

            //Atribui método ao evento do temporizador do SelfTest
            _clockCacheSelfTest.Elapsed += ClockCacheOnElapsedSelfTest;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void ClockCacheOnElapsedDiagnosticApi(object sender, ElapsedEventArgs elapsedEventArgs) {
            try {
                //Ativa relogio
                _clockCacheDiagnosticApi.Enabled = false;

                //Executa o processador
                _diagnosticClientProcessor.Execute_DiagnosticApi();
            } catch (Exception ex) {
                // Envia erro para o slack
                _diagnosticClientProcessor.SendErroToSlack(ex);
                //Imprime mensagem
                ConsoleUtility.PrintMsg(ex.ToString());
                //Gerenciador de log
                _logManager.Log(ex, LogTypeEnum.Fail, NodeTypeEnum.SlackWebHooksLayer, null);
            } finally {
                //Desativa o relogio
                _clockCacheDiagnosticApi.Enabled = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void ClockCacheOnElapsedSelfTest(object sender, ElapsedEventArgs elapsedEventArgs) {
            try {
                //Ativa relogio
                _clockCacheSelfTest.Enabled = false;

                //Executa o processador
                _diagnosticClientProcessor.Execute_DiagnosticSelfTest();
            } catch (Exception ex) {
                // Envia erro para o slack
                _diagnosticClientProcessor.SendErroToSlack(ex);
                //Imprime mensagem
                ConsoleUtility.PrintMsg(ex.ToString());
                //Gerenciador de log
                _logManager.Log(ex, LogTypeEnum.Fail, NodeTypeEnum.SlackWebHooksLayer, null);
            } finally {
                //Desativa o relogio
                _clockCacheSelfTest.Enabled = true;
            }
        }

        /// <summary>
        /// Inicia o gerenciador
        /// </summary>
        public void Start() {
            try {
                //Reinicia o controlador de lock da thread
                _manualResetEvent.Reset();

                //habilita o temporizador
                _clockCacheDiagnosticApi.Enabled = true;
                _clockCacheSelfTest.Enabled = true;

                //Aguarda evento manual para continuar
                _manualResetEvent.WaitOne();
            } catch (Exception ex) {
                //Imprime mensagem
                ConsoleUtility.PrintMsg(ex.ToString());
                //Gerenciador de log
                _logManager.Log(ex, LogTypeEnum.Fail, NodeTypeEnum.SlackWebHooksLayer, null);
            } finally {
                //Para o gerenciador
                Stop();
            }
        }

        /// <summary>
        /// Para o gerenciador
        /// </summary>
        public void Stop() {
            //Retira o bloqueio da thread
            _manualResetEvent.Set();

            //Desativa o temporizador
            _clockCacheDiagnosticApi.Enabled = false;
            _clockCacheSelfTest.Enabled = false;
        }

        #region IDisposable Support

        //Descarte se já foi chamado
        private bool _disposedValue = false;

        /// <summary>
        /// Protegida de padrão Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                // Liberando recursos gerenciados
                if (disposing) {
                    //Libera o objeto
                    _manualResetEvent.Dispose();
                }
                // Seta a variável booleana para true,
                // indicando que os recursos já foram liberados
                _disposedValue = true;
            }
        }

        /// <summary>
        /// Remover objeto da memória
        /// </summary>
        public void Dispose() {
            Dispose(true);
            //Chama o coletor
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}