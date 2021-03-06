using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pokedex.Domain.Extensions;
using Pokedex.Domain.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokedex.Domain
{
	public class Service : IService
	{
		private readonly ILogger<Service> _logger;
		private readonly HttpClient _httpClient;
		private readonly AppSettings _appSettings;

		public Service(ILogger<Service> logger, HttpClient httpClient, IOptions<AppSettings> appSettings)
		{
			_logger = logger;
			_httpClient = httpClient;
			_appSettings = appSettings.Value;
		}

		/// <summary>
		/// GetPokemon Information
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<BaseResponse<PokemonInfo>> GetPokemonInfo(GetPokemonInfoRequest request)
		{
			BaseResponse<PokemonInfo> response = new BaseResponse<PokemonInfo>();
			try
			{
				string url = $"{_appSettings.PokemonSpeciesEndpoint}{request.PockemonName.ToLower()}";
				using (var result = await _httpClient.GetAsync(url))
				{
					string apiResponse = await result.Content.ReadAsStringAsync();
					if (result.IsSuccessStatusCode)
					{
						var pokemonSpeices = JsonConvert.DeserializeObject<PokemonSpeices>(apiResponse);
						response.Data = new PokemonInfo()
						{
							Name = pokemonSpeices?.name,
							Habitat = pokemonSpeices?.habitat?.name,
							IsLegendary = pokemonSpeices.is_legendary,
							Description = pokemonSpeices?.flavor_text_entries[0]?.flavor_text
						};
					}
					else
					{
						response.ErrorCode = (int)result.StatusCode;
						response.ErrorMessage = $"{apiResponse} Pockemon Name: {request.PockemonName}";
					}
				}
			}
			catch (HttpRequestException ex)
			{
				//TODO Define custom error code - useful to montior events in datadog
				_logger.LogError("Error in Service.GetPokemonSpeices while processing request.", ex);
				response.ErrorCode = 400;//TODO Custum ErrorCode
				response.ErrorMessage = $"{ex.Message} {ex.InnerException} {ex.Data}";
			}
			catch (ArgumentNullException ex)
			{
				//TODO Define custom error code - useful to montior events in datadog
				_logger.LogError("Error in Service.GetPokemonSpeices while processing request.", ex);
				response.ErrorCode = 404;//TODO Custum ErrorCode
				response.ErrorMessage = $"{ex.ParamName} {ex.Message} {ex.InnerException} {ex.Data}";
			}
			catch (Exception ex)
			{
				//TODO Define custom error code - useful to montior events in datadog
				_logger.LogError("Error in Service.GetPokemonSpeices while processing request.", ex);
				response.ErrorCode = 9999;//TODO Custum ErrorCode
				response.ErrorMessage = $"{ex.Message} {ex.InnerException} {ex.Data}";
			}

			return response;
		}


		/// <summary>
		/// GetTranslatedDescription
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<string> GetTranslatedDesc(GetTranslationRequest request)
		{
			try
			{
				//If Habitat = null then it will throw object ref error and still return the standard description
				string url = (request.Habitat.Equals("cave", StringComparison.InvariantCultureIgnoreCase)) ||
								(request.IsLegendary)
											   ? $"{_appSettings.YodaTranslatorEndpoint}?text={request.Text.RemoveEscapeChars()}"
											   : $"{_appSettings.ShakespeareTranslatorEndpoint}?text={request.Text.RemoveEscapeChars()}";

				var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
				using (var result = await _httpClient.GetAsync(url))
				{
					string apiResponse = await result.Content.ReadAsStringAsync();
					if (result.IsSuccessStatusCode)
					{
						var apiResponseMessage = JsonConvert.DeserializeObject<Translation>(apiResponse);
						return apiResponseMessage.contents.translated;
					}
					else
					{
						//if you can't translate for whatever reason return standard description
						return request.Text;
					}
				}

			}
			catch
			{
				//if you can't translate for whatever reason return standard description
				return (!string.IsNullOrEmpty(request.Text)) ? request.Text : "No description found.";
			}
		}
		 
	}
}
