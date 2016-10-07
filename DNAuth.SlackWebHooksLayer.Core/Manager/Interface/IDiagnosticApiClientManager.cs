namespace DNAuth.SlackWebHooksLayer.Core.Manager.Interface {

    public interface IDiagnosticApiClientManager {

        /// <summary>
        /// Inicia o gerenciador
        /// </summary>
        void Start();

        /// <summary>
        /// Para o gerenciador
        /// </summary>
        void Stop();

        /// <summary>
        /// Remover objeto da memória
        /// </summary>
        void Dispose();
    }
}