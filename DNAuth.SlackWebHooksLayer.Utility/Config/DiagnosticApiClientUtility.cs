using DNAuth.SlackWebHooksLayer.Utility.Config.Interfaces;
using System.Configuration;

namespace DNAuth.SlackWebHooksLayer.Utility.Config {

    public class DiagnosticApiClientUtility : IDiagnosticApiClientUtility {

        /// <summary>
        /// Endereço do selfTest
        /// </summary>
        public string SelfTestResult => ConfigurationManager.AppSettings["SelfTestResult"];

        /// <summary>
        /// Endereço da api de diagnostico
        /// </summary>
        public string QueueInfo => ConfigurationManager.AppSettings["QueueInfo"];

        /// <summary>
        /// Endereço base
        /// </summary>
        public string BaseAddress => ConfigurationManager.AppSettings["BaseAddress"];

        /// <summary>
        /// Tempo em minutos para o serviço rodar
        /// </summary>
        public string TimeInMinutesToSendRequest_DiagnosticApi => ConfigurationManager.AppSettings["TimeInMinutesToSendRequest_DiagnosticApi"];

        /// <summary>
        /// Tempo em minutos para o serviço rodar
        /// </summary>
        public string TimeInMinutesToSendRequest_SelfTest => ConfigurationManager.AppSettings["TimeInMinutesToSendRequest_SelfTest"];

        /// <summary>
        /// Quem enviou a mensagem no slack
        /// </summary>
        public string SlackAuthor => ConfigurationManager.AppSettings["SlackAuthor"];

        /// <summary>
        /// Título da mensagem
        /// </summary>
        public string SlackPretext => ConfigurationManager.AppSettings["SlackPretext"];

        /// <summary>
        /// Canal dos mochileiros
        /// </summary>
        public string SlackChannel => ConfigurationManager.AppSettings["SlackChannel"];

        /// <summary>
        /// Nome do servidor que está enviando a notificação
        /// </summary>
        public string ServerName => ConfigurationManager.AppSettings["ServerName"];

        /// <summary>
        /// Icone para a mensagem
        /// </summary>
        public string IconUrl => ConfigurationManager.AppSettings["IconUrl"];

        /// <summary>
        /// Icone para autor da mensagem
        /// </summary>
        public string AuthorIconUrl => ConfigurationManager.AppSettings["AuthorIconUrl"];

        /// <summary>
        /// Titulo do alerta de queue no slack
        /// </summary>
        public string ProblemsQueue => ConfigurationManager.AppSettings["ProblemsQueue"];

        /// <summary>
        /// Titulo do alerta de selftest no slack
        /// </summary>
        public string ProblemsSelfTest => ConfigurationManager.AppSettings["ProblemsSelfTest"];

        /// <summary>
        /// Url do dashboard
        /// </summary>
        public string SelfTestUrlDashboard => ConfigurationManager.AppSettings["SelfTestUrlDashboard"];

        /// <summary>
        /// Url do dashboard
        /// </summary>
        public string QueueStatusUrlDashboard => ConfigurationManager.AppSettings["QueueStatusUrlDashboard"];
        
        /// <summary>
        /// Título da mensagem de erro
        /// </summary>
        public string ErroTitle => ConfigurationManager.AppSettings["ErroTitle"];

        /// <summary>
        /// Url do icone de erro
        /// </summary>
        public string ErrorIconUrl => ConfigurationManager.AppSettings["ErrorIconUrl"];
    }
}