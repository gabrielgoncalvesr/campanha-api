using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.IntegrationTests;
using Microsoft.Extensions.DependencyInjection;

public class CampanhaServiceIntegrationTests : IClassFixture<ServiceProviderFixture>
{

    private readonly ICampanhaRepository _campanhaRepository;
    private readonly ICampanhaService _campanhaService;    

    public CampanhaServiceIntegrationTests(ServiceProviderFixture fixture)
    {
        _campanhaRepository = fixture.ServiceProvider.GetRequiredService<ICampanhaRepository>();
        _campanhaService = fixture.ServiceProvider.GetRequiredService<ICampanhaService>();        
    }

    [Fact]
    public async Task ConsultarCampanhaAsync_DeveRetornarSucesso_QuandoCampanhaExiste()
    {
        // Arrange
        int idCampanha = 1;

        // Insira uma campanha no repositório para o teste.
        await _campanhaRepository.CadastrarAsync(new Campanhas
        {
            Id = idCampanha,
            Nome = "Campanha de Teste",
            Descricao = "Campanha de Teste",
            Tipo = "Natal",
            Regras = "PIX",
            Premio = "10%",
            MaxParticipantes = 100,
            QuantidadePontos = 10,
            DataInicio = new DateTime(2024, 12, 01),
            DataFim = new DateTime(2025, 01, 05),
            Ativo = 1,
            ImagemUrl = "",
            DataCadastro = DateTime.Now
        });

        // Act
        var result = await _campanhaService.ConsultarCampanhaAsync(idCampanha);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Campanha);
        Assert.Equal("Campanha de Teste", result.Campanha.Nome);
    }

    [Fact]
    public async Task ConsultarCampanhaAsync_DeveRetornarDadosInvalidos_QuandoCampanhaNaoExiste()
    {
        // Arrange
        int idCampanhaInexistente = 999;

        // Act
        var result = await _campanhaService.ConsultarCampanhaAsync(idCampanhaInexistente);

        // Assert
        Assert.NotNull(result);
        Assert.Contains($"Campanha inexistente pelo código informado: {idCampanhaInexistente}", result.Mensagem);
    }

    [Fact]
    public async Task ConsultarCampanhaAsync_DeveRegistrarErro_QuandoOcorreExcecao()
    {
        // Arrange
        int idCampanha = -1; // Simula um valor que cause erro no repositório.        

        // Act
        var result = await _campanhaService.ConsultarCampanhaAsync(idCampanha);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Campanha inexistente pelo código informado: -1", result.Mensagem);
    }
}