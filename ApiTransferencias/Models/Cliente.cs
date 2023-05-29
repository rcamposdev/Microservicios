namespace ApiTransferencias.Models
{
    public class Cliente
    {

        public int id { get; set; }

        public string? nombre { get; set; }

        public string? apellido { get; set; }

        public string? cuil { get; set; }

        public long nroDocumento { get; set; }

        public bool esEmpleadoBna { get; set; }

        public string? paisOrigen { get; set; }


    }
}