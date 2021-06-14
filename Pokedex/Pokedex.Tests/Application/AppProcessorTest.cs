using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Application;
using Pokedex.Domain;
using Pokedex.Domain.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Tests.Application
{
	public class AppProcessorTest
	{
		private readonly Mock<ILogger<AppProcessor>> _logger;
		private readonly Mock<IService> _service;
		AppProcessor _appProcessor;
		public AppProcessorTest()
		{
			_logger = new Mock<ILogger<AppProcessor>>();
			_service = new Mock<IService>();
			_appProcessor = new AppProcessor(_logger.Object, _service.Object);
		}

		[Fact]
		public async Task Process_No_Translation_Success()
		{
			//Assign
			GetPokemonInfoRequest request = new GetPokemonInfoRequest()
			{
				PockemonName = "steelix"
			};
			_service.Setup(x => x.GetPokemonInfo(It.IsAny<GetPokemonInfoRequest>())).Returns(Task.FromResult(new BaseResponse<PokemonInfo>()
			{
				Data = new PokemonInfo()
				{
					Description = "This is a test description.",
					Habitat = "cave",
					IsLegendary = true,
					Name = "steelix"
				}
			}));

			//Act
			var result = await _appProcessor.Process(request);

			//Assert
			Assert.True(result.IsSuccess);
			Assert.True(result.Data != null);
			Assert.True(String.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal("steelix", result.Data.Name);
			Assert.Equal("cave", result.Data.Habitat);
			Assert.True(result.Data.IsLegendary);
			Assert.Equal("This is a test description.", result.Data.Description);
			_service.Verify(x => x.GetPokemonInfo(It.IsAny<GetPokemonInfoRequest>()), Times.Once);
			_service.Verify(x => x.GetTranslatedDesc(It.IsAny<GetTranslationRequest>()), Times.Never);
		}

		[Fact]
		public async Task Process_Translation_Success()
		{
			//Assign
			GetPokemonInfoRequest request = new GetPokemonInfoRequest()
			{
				IsTranslationRequired = true,
				PockemonName = "steelix"
			};
			_service.Setup(x => x.GetPokemonInfo(It.IsAny<GetPokemonInfoRequest>())).Returns(Task.FromResult(new BaseResponse<PokemonInfo>()
			{
				Data = new PokemonInfo()
				{
					Description = "This is a test description.",
					Habitat = "cave",
					IsLegendary = true,
					Name = "steelix"
				}
			}));
			_service.Setup(x => x.GetTranslatedDesc(It.IsAny<GetTranslationRequest>())).Returns(Task.FromResult("A test description,  'this is.'"));

			//Act
			var result = await _appProcessor.Process(request);

			//Assert
			Assert.True(result.IsSuccess);
			Assert.True(result.Data != null);
			Assert.True(String.IsNullOrEmpty(result.ErrorMessage));
			Assert.Equal("steelix", result.Data.Name);
			Assert.Equal("cave", result.Data.Habitat);
			Assert.True(result.Data.IsLegendary);
			Assert.Equal("A test description,  'this is.'", result.Data.Description);
			_service.Verify(x => x.GetPokemonInfo(It.IsAny<GetPokemonInfoRequest>()), Times.Once);
			_service.Verify(x => x.GetTranslatedDesc(It.IsAny<GetTranslationRequest>()), Times.Once);
		}

		//TODO Add Test cases for exceptions
	}
}
