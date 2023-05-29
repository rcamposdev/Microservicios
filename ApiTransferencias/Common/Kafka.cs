using Confluent.Kafka;

namespace ApiTransferencias.Common
{
    public class Kafka
    {

        // TODO : Pasar el topic y el server al config
        
        const string TOPIC = "transferencias-kafka";

        public static void RegistrarMensaje(string mensaje)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            producer.Produce(TOPIC, new Message<Null, string> { Value = mensaje });

            producer.Flush(TimeSpan.FromSeconds(10));
        }
        
    }
}