using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Pokedex.Domain;
using Pokedex.Domain.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Tests.Domain
{
    public class ServiceTest
    {
        private readonly Mock<ILogger<Service>> _logger;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly Mock<IOptions<AppSettings>> _appSettings;
        Service _service;

        public ServiceTest()
        {
            _logger = new Mock<ILogger<Service>>();
            _appSettings = new Mock<IOptions<AppSettings>>();
            _handlerMock = new Mock<HttpMessageHandler>();

            _appSettings.Setup(x => x.Value).Returns(new AppSettings()
            {
                ShakespeareTranslatorEndpoint = "https://api.funtranslations.com/translate/shakespeare.json",
                PokemonSpeciesEndpoint = "https://pokeapi.co/api/v2/pokemon-species/",
                YodaTranslatorEndpoint = "https://api.funtranslations.com/translate/yoda.json"
            });
        }

        [Fact]
        public async Task GetPokemonSpeices_Returns_OK()
        {
            //Arrange
            GetPokemonInfoRequest request = new GetPokemonInfoRequest()
            {
                PockemonName = "steelix",
                IsTranslationRequired = true
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(MockData.GetPokemonSpeices()),
            };

            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var httpClient = new HttpClient(_handlerMock.Object);

            //Act
            _service = new Service(_logger.Object, httpClient, _appSettings.Object);

            //Assert
            var result = await _service.GetPokemonInfo(request);

            Assert.True(result.IsSuccess);
            Assert.True(result.Data != null);
            Assert.True(String.IsNullOrEmpty(result.ErrorMessage));
            Assert.Equal("steelix", result.Data.Name);
            Assert.Equal("cave", result.Data.Habitat);
            Assert.True(result.Data.IsLegendary);
            Assert.Equal("This is a test flavor_text Value", result.Data.Description);

        }

        [Fact]
        public async Task GetPokemonSpeices_Returns_NotFound()
        {
            //Arrange
            GetPokemonInfoRequest request = new GetPokemonInfoRequest()
            {
                PockemonName = "notfound",
                IsTranslationRequired = true
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(MockData.GetErrorResponse()),
            };

            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var httpClient = new HttpClient(_handlerMock.Object);

            //Act
            _service = new Service(_logger.Object, httpClient, _appSettings.Object);

            //Assert
            var result = await _service.GetPokemonInfo(request);

            Assert.False(result.IsSuccess);
            Assert.True(result.ErrorCode == 404);
            Assert.False(String.IsNullOrEmpty(result.ErrorMessage));
            Assert.True(result.Data == null);

        }

        [Fact]
        public async Task GetTranslatedDesc_Yoda_OK()
        {
            //Arrange
            GetTranslationRequest request = new GetTranslationRequest()
            {
                Habitat = "cave",
                IsLegendary = true,
                Text = "This is a test description."
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(MockData.GetTranslatedDesc()),
            };

            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var httpClient = new HttpClient(_handlerMock.Object);

            //Act
            _service = new Service(_logger.Object, httpClient, _appSettings.Object);

            //Assert
            var result = await _service.GetTranslatedDesc(request);

            Assert.True(result.IsSuccess);
            Assert.True(result.ErrorCode == 0);
            Assert.True(String.IsNullOrEmpty(result.ErrorMessage));
            Assert.True(result.Data != null);
            Assert.Equal("This is a test description.", request.Text);
            Assert.Equal("A test description,  'this is.'", result.Data);

        }

        [Fact]
        public async Task GetTranslatedDesc_Shakespeare_OK()
        {
            //Arrange
            GetTranslationRequest request = new GetTranslationRequest()
            {
                Habitat = "not-cave",
                IsLegendary = false,
                Text = "This is a test description."
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(MockData.GetTranslatedDesc()),
            };

            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var httpClient = new HttpClient(_handlerMock.Object);

            //Act
            _service = new Service(_logger.Object, httpClient, _appSettings.Object);

            //Assert
            var result = await _service.GetTranslatedDesc(request);

            Assert.True(result.IsSuccess);
            Assert.True(result.ErrorCode == 0);
            Assert.True(String.IsNullOrEmpty(result.ErrorMessage));
            Assert.True(result.Data != null);
            Assert.Equal("This is a test description.", request.Text);
            Assert.Equal("A test description,  'this is.'", result.Data);

        }

        [Fact]
        public async Task GetTranslatedDesc_NotFound()
        {
            //Arrange
            GetTranslationRequest request = new GetTranslationRequest()
            {
                Habitat = "cave",
                IsLegendary = true,
                Text = "This is a test description."
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(MockData.GetErrorResponse()),
            };

            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var httpClient = new HttpClient(_handlerMock.Object);

            //Act
            _service = new Service(_logger.Object, httpClient, _appSettings.Object);

            //Assert
            var result = await _service.GetTranslatedDesc(request);

            Assert.True(result.IsSuccess);
            Assert.True(result.ErrorCode != 404);
            Assert.Equal("This is a test description.", result.Data);
         }
    }

}
