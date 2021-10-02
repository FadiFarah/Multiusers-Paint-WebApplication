using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ApiFunctions.Models;
using ApiFunctions.Services.Interfaces;
using System.Net.Http;

namespace ApiFunctions
{
    public class PaintingFunction
    {
        private readonly ICosmosDbService<Painting> _paintingRepository;
        private readonly HttpClient _httpClient;

        public PaintingFunction(ICosmosDbService<Painting> paintingRepository, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _paintingRepository = paintingRepository ?? throw new ArgumentNullException(nameof(paintingRepository));
        }
        [FunctionName("GetPaintings")]
        public async Task<IActionResult> GetAllPaintingsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "painting")] HttpRequest req, ILogger log)
        {
            var allPaintings = await _paintingRepository.GetAllAsync();
            return new OkObjectResult(allPaintings);
        }

        [FunctionName("GetPaintingById")]
        public async Task<IActionResult> GetPaintingAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "painting/{id}")] HttpRequest req, string id, ILogger log)
        {
            var paintingDetails = await _paintingRepository.GetAsync(id);
            return new OkObjectResult(paintingDetails);
        }

        [FunctionName("CreatePainting")]
        public async Task<IActionResult> CreateNewPaintingAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "painting")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Painting painting = JsonConvert.DeserializeObject<Painting>(requestBody);
            var newPainting = new Painting
            {
                id = painting.id,
                ContributedUsers = painting.ContributedUsers,
                ImageURL = painting.ImageURL,
                PaintingName = painting.PaintingName,
                RoomDetails = painting.RoomDetails,
                CreatorId = painting.CreatorId,
                ShapesDetails = painting.ShapesDetails
            };
            var createdPainting = await _paintingRepository.AddAsync(newPainting);
            return new OkObjectResult(createdPainting);
        }

        [FunctionName("DeletePaintingById")]
        public async Task<IActionResult> DeletePaintingByIdAsync([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "painting/{id}")] HttpRequest req, string id, ILogger log)
        {
            await _paintingRepository.DeleteAsync(id);
            return new OkResult();
        }

        [FunctionName("UpdatePainting")]
        public async Task<IActionResult> UpdatePaintingAsync([HttpTrigger(AuthorizationLevel.Function, "put", Route = "painting")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Painting newPainting = JsonConvert.DeserializeObject<Painting>(requestBody);
            var updatedPainting = await _paintingRepository.UpdateAsync(newPainting);
            if (updatedPainting == null)
                return new NotFoundResult();
            await _httpClient.PostAsJsonAsync("https://grouppaintonline-roomhubfunction.azurewebsites.net/api/sendpaintingupdates", updatedPainting);
            return new OkObjectResult(updatedPainting);
        }
    }
}
