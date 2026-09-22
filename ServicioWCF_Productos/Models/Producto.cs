using System.Runtime.Serialization;

namespace ServicioWCF_Productos.Models
{
    /// <summary>Representa un producto del catálogo.</summary>
    [DataContract]
    public class Producto
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Nombre { get; set; }
        [DataMember] public decimal Precio { get; set; }
    }
}