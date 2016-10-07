using System.Collections.Generic;

namespace DNAuth.SlackWebHooksLayer.Domain.Entities.Client {

    /// <summary>
    /// Classe do objeto principal
    /// </summary>
    public class Layer {

        /// <summary>
        /// data da Layer
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// Lista que contém todas as queues da Layer
        /// </summary>
        public List<QueueLayer> QueueLayers { get; set; }
    }
}