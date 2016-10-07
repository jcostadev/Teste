namespace DNAuth.SlackWebHooksLayer.Utility.Config.Interfaces {

    public interface IDiagnosticApiClientUtility {

        /// <summary>
        /// Endereço do selfTest
        /// </summary>
        string SelfTestResult { get; }

        /// <summary>
        /// Endereço da api de diagnostico
        /// </summary>
        string QueueInfo { get; }

        /// <summary>
        /// Endereço base
        /// </summary>
        string BaseAddress { get; }

        /// <summary>
        /// Tempo em minutos para o serviço rodar
        /// </summary>
        string TimeInMinutesToSendRequest_DiagnosticApi { get; }

        /// <summary>
        /// Tempo em minutos para o serviço rodar
        /// </summary>
        string TimeInMinutesToSendRequest_SelfTest { get; }

        /// <summary>
        /// Quem enviou a mensagem no slack
        /// </summary>
        string SlackAuthor { get; }

        /// <summary>
        /// Título da mensagem
        /// </summary>
        string SlackPretext { get; }

        /// <summary>
        /// Canal dos mochileiros
        /// </summary>
        string SlackChannel { get; }

        /// <summary>
        /// Nome do servidor que está enviando a notificação
        /// </summary>
        string ServerName { get; }

        /// <summary>
        /// Icone para a mensagem
        /// </summary>
        string IconUrl { get; }

        /// <summary>
        /// Icone para autor da mensagem
        /// </summary>
        string AuthorIconUrl { get; }

        /// <summary>
        /// Titulo do alerta de queue no slack
        /// </summary>
        string ProblemsQueue { get; }

        /// <summary>
        /// Titulo do alerta de selftest no slack
        /// </summary>
        string ProblemsSelfTest { get; }

        /// <summary>
        /// Url do dashboard
        /// </summary>
        string SelfTestUrlDashboard { get; }


        /// <summary>
        /// Url do dashboard
        /// </summary>
        string QueueStatusUrlDashboard { get; }

        /// <summary>
        /// Título da mensagem de erro
        /// </summary>
        string ErroTitle { get; }

        /// <summary>
        /// Url do icone de erro
        /// </summary>
        string ErrorIconUrl { get; }
    }
}