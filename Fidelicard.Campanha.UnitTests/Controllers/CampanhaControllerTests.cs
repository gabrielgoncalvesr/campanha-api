using Fidelicard.Campanha.Controllers;
using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Core.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class CampanhaControllerTests
{
    private readonly Mock<ICampanhaService> _mockService;
    private readonly Mock<ILogger<CampanhaController>> _mockLogger;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly CampanhaController _controller;

    public CampanhaControllerTests()
    {
        _mockService = new Mock<ICampanhaService>();
        _mockLogger = new Mock<ILogger<CampanhaController>>();
        _mockConfiguration = new Mock<IConfiguration>();

        _controller = new CampanhaController(_mockService.Object, _mockLogger.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task ObterCampanha_ShouldReturnOk_WhenValidIdProvided()
    {
        // Arrange
        var campanha = new Campanhas { Nome = "Campanha Teste" };
        var expectedResponse = CampanhaResult.SucessoObterUsuario(campanha);

        _mockService.Setup(s => s.ConsultarCampanhaAsync(It.IsAny<int>()))
                    .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.ObterCampanha(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<CampanhaResult>(okResult.Value);

        Assert.Equal(campanha.Nome, actualResponse.Campanha.Nome);        
        Assert.Null(actualResponse.ErroProcessamento);
    }

    [Fact]
    public async Task ObterCampanha_ShouldReturnBadRequest_WhenInvalidIdProvided()
    {
        // Arrange
        var expectedResponse = CampanhaResult.DadosInvalidos(
            new ArgumentException("Id inválido"), "Código da campanha inválido"
        );

        _mockService.Setup(s => s.ConsultarCampanhaAsync(It.IsAny<int>()))
                    .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.ObterCampanha(0);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var actualResponse = Assert.IsType<CampanhaResult>(badRequestResult.Value);
                
        Assert.Equal("Código da campanha inválido", actualResponse.Mensagem);
    }

    [Fact]
    public async Task ObterCampanha_ShouldHandleServiceException()
    {
        // Arrange
        var exception = new Exception("Erro ao processar a solicitação.");
        _mockService.Setup(s => s.ConsultarCampanhaAsync(It.IsAny<int>()))
                    .ThrowsAsync(exception);

        // Act
        var result = await _controller.ObterCampanha(1);

        // Assert
        var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, internalServerErrorResult.StatusCode);

        var actualResponse = Assert.IsType<CampanhaResult>(internalServerErrorResult.Value);        
        Assert.Equal("Erro ao processar a solicitação.", actualResponse.Mensagem);
    }
}
