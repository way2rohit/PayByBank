using Pokedex.Domain.Models;
using System.Threading.Tasks;

namespace Pokedex.Domain
{
    public interface IService
    {
        /// <summary>
        /// GetPokemonInformation
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DefaultResponse<PokemonInfo>> GetPokemonInfo(GetPokemonSpeicesRequest request);

        /// <summary>
        /// GetTranslatedDescription
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DefaultResponse<string>> GetTranslatedDesc(GetTranslationRequest request);
    }
}
