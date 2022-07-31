using AutoMapper;
using OptoBasicNotesApi.Application.DTOs;
using OptoBasicNotesApi.Core.Models;

namespace OptoBasicNotesApi.Application.Mappings
{
    public  class AppMappings : Profile
    {
        /// <summary>
        /// Create Automapper profiles to easily move models to dtos
        /// </summary>
        public AppMappings()
        {
            CreateMap<Note, NoteDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<NoteCategory, NoteCategoryDto>();
        }
    }
}
