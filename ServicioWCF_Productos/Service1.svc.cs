using System.Collections.Generic;
using System.Linq;
using ServicioWCF_Productos.Data;
using ServicioWCF_Productos.Models;

namespace ServicioWCF_Productos
{
    /// <summary>Implementación del contrato IService1.</summary>
    public class Service1 : IService1
    {
        public List<Producto> ListarProductos()
        {
            return DataProducto.ObtenerProductos();
        }

        public Producto ObtenerProducto(int id)
        {
            return DataProducto.ObtenerProductos().FirstOrDefault(p => p.Id == id);
        }
    }
}