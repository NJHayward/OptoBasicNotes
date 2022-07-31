using Microsoft.EntityFrameworkCore;
using OptoBasicNotesApi.Application.Interfaces;
using OptoBasicNotesApi.Core;
using OptoBasicNotesApi.Core.Models;

namespace OptoBasicNotesApi.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly AppDbContext _context;

        public NoteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Note>> GetAllAsync()
        {
            return await _context.Notes.Include(x => x.NoteCategories)
                                       .ThenInclude(x => x.Category)
                                       .ToListAsync();
        }

        public async Task<Note> FindByIdAsync(int id)
        {
            return await _context.Notes.Include(x => x.NoteCategories)
                                       .ThenInclude(x => x.Category)
                                       .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Note> CreateNote(Note note)
        {
            _context.Add(note);
            await _context.SaveChangesAsync();

            return note;
        }

        public async Task Update(Note note)
        {
            _context.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Note note)
        {
            _context.Remove(note);
            await _context.SaveChangesAsync();
        }
    }
}
