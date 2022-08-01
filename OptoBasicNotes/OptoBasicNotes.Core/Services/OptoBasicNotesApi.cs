using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OptoBasicNotes.Core.Interfaces;
using OptoBasicNotes.Core.Models;
using OptoBasicNotes.Core.Models.DTOs;
using RestSharp;

namespace OptoBasicNotes.Core.Services
{
    public class OptoBasicNotesApi : IOptoBasicNotesApi
    {
        private readonly RestClient _restClient;
        private readonly ILogger<OptoBasicNotesApi> _logger;
        private readonly IMapper _mapper;

        public OptoBasicNotesApi(ILogger<OptoBasicNotesApi> logger, IConfiguration configuration, IMapper mapper)
        {
            //setup rest client.
            //  Not going as far as factory as we are single page and this service will be registered as a singleton.
            var baseUrl = configuration["OptoBasicNotesApiBaseUrl"];
            _restClient = new RestClient(baseUrl);

            _logger = logger;
            _mapper = mapper;
        }

        #region Categories 

        public async Task<IList<CategoryModel>> GetAllCategoriesAsync() =>
            _mapper.Map<List<CategoryModel>>(await ExecuteRequestAsync<List<CategoryDto>>("/categories", Method.Get));

        public async Task<CategoryModel> CreateCategoryAsync(string categoryName) =>
            _mapper.Map<CategoryModel>(await ExecuteRequestAsync<CategoryDto>("/categories", Method.Post, new CreateCategoryDto { CategoryName = categoryName }));

        #endregion

        #region Notes 

        public async Task<IList<NoteModel>> GetAllNotesAsync() =>
            _mapper.Map<List<NoteModel>>(await ExecuteRequestAsync<List<NoteDto>>("/notes", Method.Get));

        public async Task<NoteModel> GetNoteAsync(int id) =>
            _mapper.Map<NoteModel>(await ExecuteRequestAsync<NoteDto>($"/notes/{id}", Method.Get));

        public async Task<NoteModel> GetNoteConvertedMarkdownAsync(int id) =>
            _mapper.Map<NoteModel>(await ExecuteRequestAsync<NoteDto>($"/notes/{id}/html", Method.Get));

        public async Task<NoteModel> CreateNoteAsync(string noteBody, IList<int> noteCategoryIds) =>
            _mapper.Map<NoteModel>(await ExecuteRequestAsync<NoteDto>("/notes", Method.Post, new CreateUpdateNoteDto
            {
                NoteBody = noteBody,
                NoteCategoryIds = noteCategoryIds
            }));

        public async Task UpdateNoteAsync(int id, string noteBody, IList<int> noteCategoryIds) =>
            await ExecuteRequestAsync<bool>($"/notes/{id}", Method.Put, new CreateUpdateNoteDto
            {
                NoteBody = noteBody,
                NoteCategoryIds = noteCategoryIds
            });

        public async Task DeleteNoteAsync(int id) =>
            await ExecuteRequestAsync<bool>($"/notes/{id}", Method.Delete);

        #endregion

        private async Task<T> ExecuteRequestAsync<T> (string requestUrl, Method method, object body = null)
        {
            var request = new RestRequest(requestUrl, method);
            
            //some method require a json body.  so if this has been passed in put this int he body.
            if (body != null)
                request.AddJsonBody (body);

            var response = await _restClient.ExecuteAsync (request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (response.Content != null) return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                _logger.LogWarning($"ExecuteRequestAsync failed to get OK response.  Content = {response?.Content}  StatusCode = {response?.StatusCode}");
            }

            return default;
        }
    }
}
