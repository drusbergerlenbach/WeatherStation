using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

using WeatherStationAPIProxy.Services;

namespace WeatherStationAPIProxy.Controllers
{
    [ApiController]
    [Route("WeatherForecastProxy")]
    public class WeatherForecastController : ControllerBase
    {  
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpClientService _httpClientService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, HttpClientService httpClientService)
        {
            _logger = logger;
            _httpClientService = httpClientService;
        }

        [Authorize]
        [AuthorizeForScopes(Scopes = new string[] { "api://c9b20e5d-9354-440d-b7cc-0f882cfae85d/Weather.Read" })]
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            try
            {
                var scopeRequiredByApi = new string[] { "Weather.Read" };
                HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

                var data = await this._httpClientService.GetApiDataAsync();
                var weatherForecastResponse = data.ToObject<List<WeatherForecast>>();
                return weatherForecastResponse;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Exception occured.", ex);
                return StatusCode(500);
            }
        }
    }
}