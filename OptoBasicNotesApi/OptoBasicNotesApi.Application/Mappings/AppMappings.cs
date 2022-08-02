using AutoMapper;
using Markdig;
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

            CreateMap<Note, AllNotesItemDto>()
                .ForMember(dto => dto.NoteBodyHtml, opt => opt.MapFrom(mdl => Markdown.ToHtml(mdl.NoteBody, null, null)));

            CreateMap<Category, CategoryDto>();

            CreateMap<NoteCategory, NoteCategoryDto>();
        }
    }
}
