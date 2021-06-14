using Pokedex.Domain.Models;
using System.Threading.Tasks;

namespace Pokedex.Application
{
    public interface IAppProcessor
    {
        public Task<BaseResponse<PokemonInfo>> Process(GetPokemonSpeicesRequest request);
    }
}
