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
        public async Task<DefaultResponse<PokemonInfo>> Process(GetPokemonSpeicesRequest request)
        {
            DefaultResponse<PokemonInfo> response = new DefaultResponse<PokemonInfo>();
            try
            {
                //Get the Pokemon Speices details by name 
                var pokemonInfoResponse = await _service.GetPokemonInfo(request);

                if (pokemonInfoResponse.IsSuccess)
                {
                    response.Data = pokemonInfoResponse.Data;
                    if (request.IsTranslationRequired)
                    {
                        GetTranslationRequest _getTranslationRequest = new GetTranslationRequest()
                        {
                            Habitat = pokemonInfoResponse.Data.Habitat,
                            Text = pokemonInfoResponse.Data.Description,
                        };
                        var translatedDescResponse = await _service.GetTranslatedDesc(_getTranslationRequest);
                        if (translatedDescResponse.IsSuccess)
                        {                                                   
                            response.Data.Description = translatedDescResponse.Data;
                        }
                        else
                        {
                            response.ErrorCode = translatedDescResponse.ErrorCode;
                            response.ErrorMessage = translatedDescResponse.ErrorMessage;
                        }
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
                _logger.LogError("Error in AppProcessor.Process while processing request.", ex);
                response.ErrorCode = 9999;
                response.ErrorMessage = $"{ex.Message} {ex.InnerException}";
            }
            return response;
        }
    }
}
