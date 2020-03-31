using System;
using System.Net.Http;

namespace Metricas.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 1;

            HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:8080") };

            try
            {
                while (true)
                {
                    if (count != 1)
                    {
                        var r = client.GetAsync("/api/pedido").Result;
                    }
                    else
                    {
                        var r = client.GetAsync("/api/pedido/error").Result;
                    }

                    if (count == 4)
                        count = 1;
                    else
                        count++;
                }
            }
            catch (Exception e)
            { }
        }
    }
}
