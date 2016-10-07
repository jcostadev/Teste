using DNAuth.FrameworkService;
using System.ComponentModel;

namespace DNAuth.SlackWebHooksLayer.Installers {

    /// <summary>
    /// Indica que está apto a executar instalação de serviço
    /// </summary>
    [RunInstaller(true)]
    public class ProgramInstaller : ProjectInstaller {

        /// <summary>
        /// Método construtor
        /// </summary>
        public ProgramInstaller() : base(
            serviceName: "DNAuth.SlackWebHooksLayer",
               description: "Serviço de alerta para Slack") {
        }
    }
}