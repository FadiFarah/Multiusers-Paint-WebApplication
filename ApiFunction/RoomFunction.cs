using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ApiFunctions.Models;
using ApiFunctions.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GroupPaintOnlineWebApp.ApiFunction
{
    public class RoomFunction
    {
        private readonly ICosmosDbService<Room> _roomRepository;
        private readonly HttpClient _httpClient;

        public RoomFunction(ICosmosDbService<Room> roomRepository, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        }
        [FunctionName("GetRooms")]
        public async Task<IActionResult> GetAllRoomsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "room")] HttpRequest req, ILogger log)
        {
            var allRooms = await _roomRepository.GetAllAsync();
            return new OkObjectResult(allRooms);
        }

        [FunctionName("GetRoomById")]
        public async Task<IActionResult> GetRoomAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "room/{id}")] HttpRequest req, string id, ILogger log)
        {
            var roomDetails = await _roomRepository.GetAsync(id);
            return new OkObjectResult(roomDetails);
        }

        [FunctionName("CreateRoom")]
        public async Task<IActionResult> CreateNewRoomAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "room")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Room room = JsonConvert.DeserializeObject<Room>(requestBody);
            var newRoom = new Room
            {
                id = room.id,
                CurrentUsers = room.CurrentUsers,
                RoomName = room.RoomName,
                IsPublic = room.IsPublic,
                Password = room.Password
            };
            var createdRoom = await _roomRepository.AddAsync(newRoom).ConfigureAwait(true);
            await _httpClient.PostAsJsonAsync("https://grouppaintonline-roomslisthubfunction.azurewebsites.net/api/roomscountchanged", "");
            return new OkObjectResult(createdRoom);
        }

        [FunctionName("DeleteRoomById")]
        public async Task<IActionResult> DeleteRoomByIdAsync([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "room/{id}")] HttpRequest req, string id, ILogger log)
        {
            var alla = await _roomRepository.DeleteAsync(id).ConfigureAwait(true);
            await _httpClient.PostAsJsonAsync("https://grouppaintonline-roomslisthubfunction.azurewebsites.net/api/roomscountchanged", "");
            return new OkObjectResult(alla);
        }

        [FunctionName("UpdateRoom")]
        public async Task<IActionResult> UpdateRoomAsync([HttpTrigger(AuthorizationLevel.Function, "put", Route = "room")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Room newRoom = JsonConvert.DeserializeObject<Room>(requestBody);
            var updatedRoom = await _roomRepository.UpdateAsync(newRoom);
            if (updatedRoom == null)
                return new NotFoundResult();
            await _httpClient.PostAsJsonAsync("https://grouppaintonline-roomslisthubfunction.azurewebsites.net/api/roomscountchanged", "");
            return new OkObjectResult(updatedRoom);
        }
    }
}
