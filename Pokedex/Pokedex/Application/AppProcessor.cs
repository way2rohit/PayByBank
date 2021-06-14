using Microsoft.Extensions.Logging;
using Pokedex.Domain;
using Pokedex.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Pokedex.Application
{
	public class AppProcessor : IAppProcessor
	{
		private readonly ILogger<AppProcessor> _logger;
		private readonly IService _service;
		public AppProcessor(ILogger<AppProcessor> logger, IService service)
		{
			_logger = logger;
			_service = service;
		}

		/// <summary>
		/// Process Request
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<BaseResponse<PokemonInfo>> Process(GetPokemonInfoRequest request)
		{
			BaseResponse<PokemonInfo> response = new BaseResponse<PokemonInfo>();
			try
			{
				//Get the Pokemon Speices details by name 
				var pokemonInfoResponse = await _service.GetPokemonInfo(request);

				if (pokemonInfoResponse.IsSuccess)
				{
					response.Data = pokemonInfoResponse.Data;

					//If translation is required then call GetGetTranslatedDesc
					if (request.IsTranslationRequired)
					{
						GetTranslationRequest getTranslationRequest = new GetTranslationRequest()
						{
							Habitat = pokemonInfoResponse.Data.Habitat,
							Text = pokemonInfoResponse.Data.Description,
						};
						response.Data.Description = await _service.GetTranslatedDesc(getTranslationRequest);
					}
				}
				else
				{
					response.ErrorCode = pokemonInfoResponse.ErrorCode;
					response.ErrorMessage = pokemonInfoResponse.ErrorMessage;
				}
			}
			catch (Exception ex)
			{
				//TODO Define custom error code - useful to montior events in datadog
				_logger.LogError("Error in AppProcessor.Process while processing request.", ex);
				response.ErrorCode = 9999;
				response.ErrorMessage = $"{ex.Message} {ex.InnerException}";
			}
			return response;
		}
	}
}
