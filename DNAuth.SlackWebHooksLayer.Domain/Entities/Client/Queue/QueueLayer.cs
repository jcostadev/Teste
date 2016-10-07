using System.Collections.Generic;

namespace DNAuth.SlackWebHooksLayer.Domain.Entities.Client {

   /// <summary>
   /// Informação da camada por nome
   /// </summary>
    public class QueueLayer {

        /// <summary>
        /// Nome da queue 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id da Queue
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Lista de QueueInfos da Queue
        /// </summary>
        public List<QueueInfo> Queues { get; set; }
    }
}