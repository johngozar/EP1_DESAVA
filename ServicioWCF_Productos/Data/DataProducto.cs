using System.Collections.Generic;
using ServicioWCF_Productos.Models;

namespace ServicioWCF_Productos.Data
{
    /// <summary>Origen de datos en memoria (sin base de datos).</summary>
    public static class DataProducto
    {
        /// <summary>Devuelve la lista fija de productos del ejemplo.</summary>
        public static List<Producto> ObtenerProductos()
        {
            return new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Laptop Lenovo",      Precio = 2500m },
                new Producto { Id = 2, Nombre = "Mouse Logitech",     Precio = 80m },
                new Producto { Id = 3, Nombre = "Teclado Redragon",   Precio = 150m },
                new Producto { Id = 4, Nombre = "Monitor Samsung",    Precio = 900m },
                new Producto { Id = 5, Nombre = "Auriculares HyperX", Precio = 320m }
            };
        }
    }
}