using BusinessLogic.Interfaces;
using Models;
using Models.Json.Team;
using Newtonsoft.Json;

namespace BusinessLogic.Managers
{
    public class GuildTeamManager : IGuildTeamManager
    {
        public IHttpClientFactory HttpClientFactory { get; set; }
        
        public async Task<TeamsBaseModel> GetTeams()
        {
            var httpClient = HttpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(Endpoints.Teams);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TeamsBaseModel>(result);
        }
    }
}
