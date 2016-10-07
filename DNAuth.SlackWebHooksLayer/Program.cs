using DNAuth.Domain.Entities.Logger;
using DNAuth.Domain.Enums.Generics;
using DNAuth.FrameworkUtility.Library;
using DNAuth.FrameworkUtility.Logger.Processor;
using DNAuth.SlackWebHooksLayer.Initialize;
using System;

namespace DNAuth.SlackWebHooksLayer {

    internal class Program {

        /// <summary>
        /// Método principal
        /// </summary>
        private static void Main(string[] args) {
            try {
                //Controle do serviço
                var serviceProcessControl = new ServiceProcessControl();
#if DEBUG
                Console.Title = "{ SLACK WEB HOOKS }";

                //Inicia o serviço
                serviceProcessControl.Start();
                //Aguarda evento de parada
                Console.ReadKey();
#else
                //Inicializa o processo
                FrameworkService.Processs.ServiceProcessEvents
                    serviceProcess = new FrameworkService.Processs.ServiceProcessEvents(serviceProcessControl);

                //Inicia o serviço
                FrameworkService.Manager.ServiceManager.Run(serviceProcess);
#endif
            } catch (Exception ex) {
                //Imprime no console
                ConsoleUtility.PrintMsg(ex.ToString());

                //Procwwessador de log em arquivo
                var logFileProcessor = new LogFileProcessor();

                //Efetua o log
                logFileProcessor.Log(new LogMessage() {
                    //Dados do log
                    LogData = ex,
                    //Tipo
                    LogType = "Main",
                    //Camada
                    NodeType = NodeTypeEnum.SlackWebHooksLayer.ToString()
                });
            }
        }
    }
}