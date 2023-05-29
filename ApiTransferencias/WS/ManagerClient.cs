using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTransferencias.Models;
using Newtonsoft.Json;


namespace ApiTransferencias.WS
{
    public class ManagerClient
    {

        public static Cliente? ConsultaPorCuil(string? cuil)
        {
            var url = $"http://localhost:5005/{cuil}"; // TODO : Mover al appsettings

            HttpClient client = new HttpClient();

            var httpResponse = client.GetAsync(url).GetAwaiter();

            if (!httpResponse.GetResult().IsSuccessStatusCode) return null;
    
            var content = httpResponse.GetResult().Content.ReadAsStringAsync().GetAwaiter();

            var response = JsonConvert.DeserializeObject<Cliente>(content.GetResult());

            return response;
        }
        
    }
}