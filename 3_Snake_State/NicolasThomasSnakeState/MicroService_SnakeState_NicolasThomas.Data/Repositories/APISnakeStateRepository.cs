using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MicroService_SnakeState_NicolasThomas.Data.Models;

namespace MicroService_SnakeState_NicolasThomas.Data.Repositories
{
    public class APISnakeStateRepository : ISnakeStateRepository
    {
        private readonly string url;
        private readonly HttpClient client; //Fais les requetes http

        public APISnakeStateRepository(string url)
        {
            this.url = url;
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
        }
        public async Task<SnakeStateModel> EnvoieListPosition(SnakeStateModel model)
        {
            var response = await client.PostAsJsonAsync($"{url}/snakestate/on-move", model);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<SnakeStateModel?>();
            return null;
        }

        public async Task<SnakeStateModel> Subscribe(SnakeStateModel model)
        {
            var response = await client.PostAsJsonAsync($"{url}/snakestate/subscribe",model);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<SnakeStateModel?>();
            return null;
        }

        public async Task<SnakeStateModel> Unsubscribe(SnakeStateModel model)
        {
            var response = await client.PostAsJsonAsync($"{url}/snakestate/unsubscribe", model);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<SnakeStateModel?>();
            return null;
        }
    }
}
