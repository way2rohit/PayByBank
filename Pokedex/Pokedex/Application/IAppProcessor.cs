using Pokedex.Domain.Models;
using System.Threading.Tasks;

namespace Pokedex.Application
{
    public interface IAppProcessor
    {
        public Task<DefaultResponse<PokemonInfo>> Process(GetPokemonSpeicesRequest request);
    }
}
