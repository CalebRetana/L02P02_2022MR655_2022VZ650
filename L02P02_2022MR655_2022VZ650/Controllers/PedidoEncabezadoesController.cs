using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using L02P02_2022MR655_2022VZ650.Models;

namespace L02P02_2022MR655_2022VZ650.Controllers
{
    public class PedidoEncabezadoesController : Controller
    {
        private readonly LibreriaContext _context;

        public PedidoEncabezadoesController(LibreriaContext context)
        {
            _context = context;
        }

        // GET: PedidoEncabezadoes
        public async Task<IActionResult> Index(int id)
        {
            // Obtener el pedido por ID
            var pedido = await _context.PedidoEncabezados
                .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.IdLibroNavigation)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound("Pedido no encontrado.");
            }

            // Cargar todos los libros disponibles en la librería
            var libros = await _context.Libros.ToListAsync();

            // Verificar si la lista de libros es nula
            if (libros == null || !libros.Any())
            {
                libros = new List<Libro>();
            }

            // Asignar los libros al ViewBag para que la vista pueda accederlos
            ViewBag.Libros = libros;
            ViewBag.PedidoId = id;

            return View(pedido);
        }




        // GET: PedidoEncabezadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoEncabezado = await _context.PedidoEncabezados
                .Include(p => p.IdClienteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoEncabezado == null)
            {
                return NotFound();
            }

            return View(pedidoEncabezado);
        }

        // GET: PedidoEncabezadoes/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Id");
            return View();
        }

        // POST: PedidoEncabezadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCliente,CantidadLibros,Total,Estado")] PedidoEncabezado pedidoEncabezado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedidoEncabezado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Id", pedidoEncabezado.IdCliente);
            return View(pedidoEncabezado);
        }

        // GET: PedidoEncabezadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoEncabezado = await _context.PedidoEncabezados.FindAsync(id);
            if (pedidoEncabezado == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Id", pedidoEncabezado.IdCliente);
            return View(pedidoEncabezado);
        }

        // POST: PedidoEncabezadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCliente,CantidadLibros,Total,Estado")] PedidoEncabezado pedidoEncabezado)
        {
            if (id != pedidoEncabezado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidoEncabezado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoEncabezadoExists(pedidoEncabezado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Id", pedidoEncabezado.IdCliente);
            return View(pedidoEncabezado);
        }

        // GET: PedidoEncabezadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoEncabezado = await _context.PedidoEncabezados
                .Include(p => p.IdClienteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoEncabezado == null)
            {
                return NotFound();
            }

            return View(pedidoEncabezado);
        }

        // POST: PedidoEncabezadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidoEncabezado = await _context.PedidoEncabezados.FindAsync(id);
            if (pedidoEncabezado != null)
            {
                _context.PedidoEncabezados.Remove(pedidoEncabezado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // Cerrar venta
        [HttpPost]
        public async Task<IActionResult> CerrarVenta([FromBody] PedidoEncabezadoViewModel pedidoData)
        {
            var pedido = await _context.PedidoEncabezados
                .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.IdLibroNavigation)
                .Include(p => p.IdClienteNavigation)
                .FirstOrDefaultAsync(p => p.Id == pedidoData.Id);

            if (pedido == null)
            {
                return Json(new { success = false, message = "Pedido no encontrado." });
            }

            // Actualizar el estado del pedido a "C" (CERRADA)
            pedido.Estado = "C";
            pedido.Total = pedidoData.Total;

            // Eliminar detalles previos para evitar duplicados
            _context.PedidoDetalles.RemoveRange(pedido.PedidoDetalles);

            // Guardar los detalles del pedido en la base de datos
            foreach (var item in pedidoData.Carrito)
            {
                var detalle = new PedidoDetalle
                {
                    IdPedido = pedido.Id,
                    IdLibro = item.libroId,
                    Cantidad = item.cantidad,  // Cantidad correctamente mapeada
                    CreatedAt = DateTime.Now
                };
                _context.PedidoDetalles.Add(detalle);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Venta cerrada exitosamente.", pedidoId = pedido.Id });
        }



        // Vista de cierre de venta
        public async Task<IActionResult> CierreVenta(int id)
        {
            var pedido = await _context.PedidoEncabezados
                .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.IdLibroNavigation) // Asegurarnos de cargar el libro
                .Include(p => p.IdClienteNavigation)    // Asegurarnos de cargar el cliente
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound("Pedido no encontrado.");
            }

            return View(pedido);
        }






        private bool PedidoEncabezadoExists(int id)
        {
            return _context.PedidoEncabezados.Any(e => e.Id == id);
        }
    }
}
