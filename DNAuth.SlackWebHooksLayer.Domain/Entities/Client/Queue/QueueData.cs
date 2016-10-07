using System;

namespace DNAuth.SlackWebHooksLayer.Domain.Entities.Client {

    /// <summary>
    /// Dados da Queue
    /// </summary>
    public class QueueData {

        /// <summary>
        /// Data da queue
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Nome da queue
        /// </summary>
        public string Name { get; set; }

        public bool Auto_Delete { get; set; }

        public string Idle_Since { get; set; }

        public int Messages { get; set; }

        public int Messages_Ready { get; set; }

        public int Messages_Unacknowledged { get; set; }

        public int Consumers { get; set; }

        public string State { set; get; }
    }
}