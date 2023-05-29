using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTransferencias.Models
{
    public class TransferenciaResponse
    {
        public int id { get; set; }

        public string? resultado { get; set; }

        public decimal importe { get; set; }

        public string? cuentaOrigen { get; set; }

        public string? cuentaDestino { get; set; }


    }
}