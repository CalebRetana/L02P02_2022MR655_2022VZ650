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
    public class PedidoDetallesController : Controller
    {
        private readonly LibreriaContext _context;

        public PedidoDetallesController(LibreriaContext context)
        {
            _context = context;
        }

        // GET: PedidoDetalles
        public async Task<IActionResult> Index()
        {
            var libreriaContext = _context.PedidoDetalles
                .Include(p => p.IdLibroNavigation)
                .Include(p => p.IdPedidoNavigation);
            return View(await libreriaContext.ToListAsync());
        }

        // POST: PedidoDetalles/Create desde la vista de libros
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoDetalle pedidoDetalle)
        {
            if (ModelState.IsValid)
            {
                var libro = await _context.Libros.FindAsync(pedidoDetalle.IdLibro);
                if (libro == null)
                {
                    return Json(new { success = false, message = "Libro no encontrado." });
                }
                pedidoDetalle.CreatedAt = DateTime.Now;
                _context.PedidoDetalles.Add(pedidoDetalle);
                await _context.SaveChangesAsync();
                return Json(new { success = true, precio = libro.Precio });
            }
            return Json(new { success = false, message = "Error al agregar el libro." });
        }

        // GET: PedidoDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoDetalle = await _context.PedidoDetalles
                .Include(p => p.IdLibroNavigation)
                .Include(p => p.IdPedidoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }

            return View(pedidoDetalle);
        }

        // GET: PedidoDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoDetalle = await _context.PedidoDetalles
                .Include(p => p.IdLibroNavigation)
                .Include(p => p.IdPedidoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }

            return View(pedidoDetalle);
        }

        // POST: PedidoDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidoDetalle = await _context.PedidoDetalles.FindAsync(id);
            if (pedidoDetalle != null)
            {
                _context.PedidoDetalles.Remove(pedidoDetalle);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoDetalleExists(int id)
        {
            return _context.PedidoDetalles.Any(e => e.Id == id);
        }
    }
}