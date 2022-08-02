using AutoMapper;
using OptoBasicNotes.Core.Models;
using OptoBasicNotes.Core.Models.DTOs;

namespace OptoBasicNotes.Core.Mappings
{
    public class AppMappings : Profile
    {
        /// <summary>
        /// Create Automapper profiles to easily move dtos to models
        /// </summary>
        public AppMappings()
        {
            CreateMap<AllNotesItemDto, AllNotesItemModel>();
            CreateMap<NoteDto, NoteModel>();
            CreateMap<CategoryDto, CategoryModel>();
            CreateMap<NoteCategoryDto, NoteCategoryModel>();
        }
    }
}
