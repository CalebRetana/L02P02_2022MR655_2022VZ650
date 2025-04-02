using System;
using System.Collections.Generic;

namespace L02P02_2022MR655_2022VZ650.Models;

public partial class PedidoDetalle
{
    public int Id { get; set; }

    public int? IdPedido { get; set; }

    public int? IdLibro { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Libro? IdLibroNavigation { get; set; }

    public virtual PedidoEncabezado? IdPedidoNavigation { get; set; }
}
