using Microsoft.AspNetCore.Mvc;
using Moq;
using Pokedex.Application;
using Pokedex.Controllers;
using Pokedex.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Tests
{
	public class PokemonControllerTest
	{

		private readonly Mock<IAppProcessor> _appProcessor;
		PokemonController _controller;
		public PokemonControllerTest()
		{
			_appProcessor = new Mock<IAppProcessor>();
			_controller = new PokemonController(_appProcessor.Object);
		}


		[Fact]
		public async Task Get_Pokemon_OK()
		{
			// Arrange
			var name = "steelix";

			_appProcessor.Setup(x => x.Process(It.IsAny<GetPokemonInfoRequest>())).Returns(Task.FromResult(new BaseResponse<PokemonInfo>()
			{
				Data = new PokemonInfo()
				{
					Description = "This is a test description.",
					Habitat = "cave",
					IsLegendary = true,
					Name = "steelix"
				}
			}));

			// Act
			var result = await _controller.Get(name);
			var okResult = result as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Fact]
		public async Task Get_Pokemon_NotFound()
		{
			// Arrange
			var name = "steelix";

			_appProcessor.Setup(x => x.Process(It.IsAny<GetPokemonInfoRequest>())).Returns(Task.FromResult(new BaseResponse<PokemonInfo>()
			{
				Data = null,
				ErrorCode = 404,
				ErrorMessage = "Not Found"

			}));

			// Act
			var result = await _controller.Get(name);
			var notfoundResult = result as NotFoundObjectResult;

			// Assert
			Assert.NotNull(notfoundResult);
			Assert.Equal(404, notfoundResult.StatusCode);

		}

		[Fact]
		public async Task Get_Pokemon_BadRequest()
		{
			// Arrange
			var name = "";

			_appProcessor.Setup(x => x.Process(It.IsAny<GetPokemonInfoRequest>())).Returns(Task.FromResult(new BaseResponse<PokemonInfo>()
			{
				Data = null,
				ErrorCode = 400,
				ErrorMessage = "Bad Request"

			}));

			// Act
			var result = await _controller.Get(name);
			var badRequestResult = result as BadRequestObjectResult;

			// Assert
			Assert.NotNull(badRequestResult);
			Assert.Equal(400, badRequestResult.StatusCode);

		}

		[Fact]
		public async Task Get_Pokemon_Translated_OK()
		{
			// Arrange
			var name = "steelix";

			_appProcessor.Setup(x => x.Process(It.IsAny<GetPokemonInfoRequest>())).Returns(Task.FromResult(new BaseResponse<PokemonInfo>()
			{
				Data = new PokemonInfo()
				{
					Description = "This is a test description.",
					Habitat = "cave",
					IsLegendary = true,
					Name = "steelix"
				}
			}));

			// Act
			var result = await _controller.GetTranslated(name);
			var okResult = result as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
		public async Task Get_Pokemon_Translated_BadRequest()
		{
			// Arrange
			var name = "";

			_appProcessor.Setup(x => x.Process(It.IsAny<GetPokemonInfoRequest>())).Returns(Task.FromResult(new BaseResponse<PokemonInfo>()
			{
				Data = null,
				ErrorCode = 400,
				ErrorMessage = "Bad Request"

			}));

			// Act
			var result = await _controller.GetTranslated(name);
			var badRequestResult = result as BadRequestObjectResult;

			// Assert
			Assert.NotNull(badRequestResult);
			Assert.Equal(400, badRequestResult.StatusCode);
		}

	}
}
