namespace DNAuth.SlackWebHooksLayer.Domain.Entities.Client {

    /// <summary>
    ///  Informação da camada por código
    /// </summary>
    public class QueueInfo {

        /// <summary>
        /// Código da queue
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data da queue
        /// </summary>
        public QueueData Data { get; set; }
    }
}