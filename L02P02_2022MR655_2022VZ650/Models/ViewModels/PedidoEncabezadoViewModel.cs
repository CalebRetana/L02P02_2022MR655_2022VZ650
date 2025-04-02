using System.Collections.Generic;

namespace L02P02_2022MR655_2022VZ650.Models
{
    public class PedidoEncabezadoViewModel
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public List<CarritoItem> Carrito { get; set; } = new List<CarritoItem>();
    }

    public class CarritoItem
    {
        public int libroId { get; set; }
        public string libroNombre { get; set; }
        public decimal precio { get; set; }
        public int cantidad { get; set; }
    }
}
