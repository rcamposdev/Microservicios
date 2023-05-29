using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTransferencias.Models
{
    public class TransferenciaRequest
    {
        public string? cuilOriginante { get; set; }

        public string? cuilDestinatario { get; set; }

        public string? cbuOrigen { get; set; }

        public string? cbuDestino { get; set; }

        public decimal importe { get; set; }

        public string? concepto { get; set; }

        public string? descripcion { get; set; }

    }
}