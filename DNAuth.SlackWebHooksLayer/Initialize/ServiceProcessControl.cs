using DNAuth.Domain.Enums.Generics;
using DNAuth.Domain.Enums.Logger;
using DNAuth.FrameworkDiagnostic.Manager;
using DNAuth.FrameworkDiagnostic.Manager.Interfaces;
using DNAuth.FrameworkDiagnostic.Utility;
using DNAuth.FrameworkDiagnostic.Utility.Interfaces;
using DNAuth.FrameworkRepository.Library.Utility;
using DNAuth.FrameworkRepository.Library.Utility.Interfaces;
using DNAuth.FrameworkRepository.TransactionalRepository.Layer.Repositories;
using DNAuth.FrameworkRepository.TransactionalRepository.Layer.Repositories.Interfaces;
using DNAuth.FrameworkService.Processs.Interface;
using DNAuth.FrameworkUtility.Cache;
using DNAuth.FrameworkUtility.Cache.Interfaces;
using DNAuth.FrameworkUtility.Configs;
using DNAuth.FrameworkUtility.Configs.Interfaces;
using DNAuth.FrameworkUtility.Library;
using DNAuth.FrameworkUtility.Logger;
using DNAuth.FrameworkUtility.Logger.Interfaces;
using DNAuth.SlackWebHooksLayer.Core.Manager;
using DNAuth.SlackWebHooksLayer.Core.Manager.Interface;
using DNAuth.SlackWebHooksLayer.Core.Processor;
using DNAuth.SlackWebHooksLayer.Core.Processor.Interfaces;
using DNAuth.SlackWebHooksLayer.Utility.Config;
using DNAuth.SlackWebHooksLayer.Utility.Config.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using InjectionConstructor = Microsoft.Practices.Unity.InjectionConstructor;

namespace DNAuth.SlackWebHooksLayer.Initialize {

    public class ServiceProcessControl : IServiceProcessControl {

        /// <summary>
        /// Gerenciador de Diagnostico
        /// </summary>
        private static IDiagnosticsManager _diagnosticsManager;

        private static IDiagnosticApiClientManager _diagnosticApiClientManager;

        /// <summary>
        /// Inicia o serviço
        /// </summary>
        /// <returns></returns>
        public bool Start() {
            //Define configurações
            Config();
            //Inicializa o serviço
            Init();

            return true;
        }

        /// <summary>
        /// Para o serviço
        /// </summary>
        /// <returns></returns>
        public bool Stop() {
            //para o serviço de diagnostico
            _diagnosticsManager.Stop();

            return true;
        }

        /// <summary>
        /// Configura as dependencias
        /// </summary>
        private static void Config() {
            //Utilitário de configuração
            var repositoryConfigurationUtility = new RepositoryConfigurationUtility();

            #region Registro de dependências

            //Container de injeção de dependência
            var unityContainer = DependencyInjection.GetContainerInstance();

            //Registra o tipo
            unityContainer.RegisterInstance(typeof(IRepositoryConfigurationUtility), repositoryConfigurationUtility);
            //Gerenciador de cache
            unityContainer.RegisterType<IMessageIdCacheManager, MessageIdCacheManager>();
            //Registra um tipo
            unityContainer.RegisterType<ITransactionRepository, TransactionRepository>(new InjectionConstructor());
            //Regista utilitário de configuração de cache
            unityContainer.RegisterType<IRedisConfigurationUtility, RedisConfigurationUtility>();
            //Gerenciador da API
            unityContainer.RegisterType<ILogManager, LogManager>(new InjectionConstructor());
            //Registra o objeto de configuração de log
            unityContainer.RegisterType<ILogFileProcessorConfiguration, LogFileProcessorConfiguration>();

            //Utilitário de configuração do sistema de diagnóstico
            var diagnosticsConfigurationUtility = new DiagnosticsConfigurationUtility();
            //Registra a instância
            unityContainer.RegisterInstance(typeof(IDiagnosticsConfigurationUtility), diagnosticsConfigurationUtility);
            //Registra o tipo
            unityContainer.RegisterType<IDiagnosticsManager, DiagnosticsManager>();
            //Registra o tipo
            unityContainer.RegisterType<IDiagnosticCacheManager, DiagnosticCacheManager>();

            #endregion Registro de dependências

            // Configuração de repositório genérico
            unityContainer.RegisterType<IDiagnosticsConfigurationUtility, DiagnosticsConfigurationUtility>();
            unityContainer.RegisterType<IDiagnosticApiClientManager, DiagnosticApiClientManager>();
            unityContainer.RegisterType<IDiagnosticClientProcessor, DiagnosticClientProcessor>();
            unityContainer.RegisterType<ISlackClientProcessor, SlackClientProcessor>();
            unityContainer.RegisterType<IDiagnosticApiClientUtility, DiagnosticApiClientUtility>();
        }

        private static void Init() {
            try {
                //Carrega a instância do container
                var unityContainer = DependencyInjection.GetContainerInstance();

                //Sistema de diagnóstico
                _diagnosticsManager = unityContainer.Resolve<IDiagnosticsManager>();

                #region Inicialização

                //Define o maximo de threads no pool
                ThreadUtility.SetUpMaxThreads();

                #endregion Inicialização

#warning Atualizar o scheduleManager para aceitar esse aqui também e não ficar separado

                //Tarefas autorizadores
                Task.Factory.StartNew(() => {
                    //Gerenciador da camada de transporte
                    _diagnosticApiClientManager = unityContainer.Resolve<IDiagnosticApiClientManager>();
                    //Inicia o gerenciador
                    _diagnosticApiClientManager.Start();
                });

                //Incializa a aplicação de diagnostico
                _diagnosticsManager.Start();
            } catch (Exception ex) {
#if DEBUG
                //Imprime no console
                ConsoleUtility.PrintMsg(ex.ToString());
#endif
                //Carrega a instância do container
                var unityContainer = DependencyInjection.GetContainerInstance();

                //Carrega o logManager
                var logManager = unityContainer.Resolve<ILogManager>();
                //Log de dados
                logManager.Log(ex, LogTypeEnum.Fail, NodeTypeEnum.SlackWebHooksLayer, null);
            }
        }
    }
}