using System;
using System.ServiceModel;
using ClienteConsola_Productos.ServicioProductoReferencia;

namespace ClienteConsola_Productos
{
    /// <summary>Cliente de consola que consume el servicio WCF de productos.</summary>
    internal static class Program
    {
        private static int Main()
        {
            var cliente = new Service1Client();
            try
            {
                Console.WriteLine("=== ListarProductos() ===");
                foreach (var p in cliente.ListarProductos())
                {
                    Console.WriteLine("{0} | {1} | {2}", p.Id, p.Nombre, p.Precio);
                }

                Console.WriteLine();
                Console.WriteLine("=== ObtenerProducto(3) ===");
                var producto = cliente.ObtenerProducto(3);
                Console.WriteLine(producto == null
                    ? "No existe un producto con Id 3."
                    : string.Format("{0} | {1} | {2}", producto.Id, producto.Nombre, producto.Precio));

                cliente.Close();
                return 0;
            }
            catch (Exception ex) when (ex is CommunicationException || ex is TimeoutException)
            {
                cliente.Abort();
                Console.Error.WriteLine("No se pudo consultar el servicio: " + ex.Message);
                return 1;
            }
        }
    }
}