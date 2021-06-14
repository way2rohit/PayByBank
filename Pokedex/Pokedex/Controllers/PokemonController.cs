using Microsoft.AspNetCore.Mvc;
using Pokedex.Application;
using Pokedex.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Pokedex.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PokemonController : ControllerBase
	{

		private readonly IAppProcessor _appProcessor;
		public PokemonController(IAppProcessor appProcessor)
		{
			_appProcessor = appProcessor;
		}


		/// <summary>
		/// Get Pokemon Info
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet("{name}")]
		public async Task<IActionResult> Get(string name)
		{
			try
			{
				if (String.IsNullOrEmpty(name))
				{
					return BadRequest($"Pokemon name should not be null or empty.");
				}

				GetPokemonInfoRequest request = new GetPokemonInfoRequest()
				{
					PockemonName = name
				};
				var result = await _appProcessor.Process(request);
				if (result.IsSuccess)
				{
					return Ok(result.Data);
				}
				else
				{
					if (result.ErrorCode == 404)
					{
						return NotFound(result.ErrorMessage);
					}
					else
					{
						return Problem(result.ErrorMessage);
					}
				}
			}
			catch
			{
				return Problem();
			}
		}

		/// <summary>
		/// Get Pokemon Translated Info
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet("translated/{name}")]
		public async Task<IActionResult> GetTranslated(string name)
		{
			try
			{
				if (String.IsNullOrEmpty(name))
				{
					return BadRequest($"Pokemon name should not be null or empty.");
				}

				GetPokemonInfoRequest request = new GetPokemonInfoRequest()
				{
					PockemonName = name,
					IsTranslationRequired = true
				};
				var result = await _appProcessor.Process(request);
				return Ok(result.Data);
			}
			catch
			{
				return Problem();
			}
		}
	}
}
