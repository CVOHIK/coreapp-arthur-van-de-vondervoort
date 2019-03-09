using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly StoreContext _context;

        public StoreController(StoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListGenres()
        {
            var genres = _context.Genres
                .OrderBy(g => g.Name);

            return View(await genres.ToListAsync());
        }

        public async Task<IActionResult> ListAlbums(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = _context.Genres
                .Include(g => g.Albums)
                .Where(g => g.GenreID == id)
                .OrderBy(g => g.Albums.Select(a => a.Title));

            return View(await genre.SingleOrDefaultAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .Where(a => a.AlbumID == id);

            return View(await album.SingleOrDefaultAsync());
        }

    }
}