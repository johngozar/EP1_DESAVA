using System.Collections.Generic;
using System.ServiceModel;
using ServicioWCF_Productos.Models;

namespace ServicioWCF_Productos
{
    /// <summary>Contrato del servicio de productos.</summary>
    [ServiceContract]
    public interface IService1
    {
        /// <summary>Devuelve todos los productos.</summary>
        [OperationContract]
        List<Producto> ListarProductos();

        /// <summary>Devuelve el producto con el identificador indicado, o null si no existe.</summary>
        [OperationContract]
        Producto ObtenerProducto(int id);
    }
}