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
		Task<BaseResponse<PokemonInfo>> GetPokemonInfo(GetPokemonInfoRequest request);

		/// <summary>
		/// GetTranslatedDescription
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<string> GetTranslatedDesc(GetTranslationRequest request);
	}
}
