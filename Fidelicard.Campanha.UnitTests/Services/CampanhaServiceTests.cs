using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Models;
using Fidelicard.Campanha.Core.Service;
using Fidelicard.Campanha.Core.Result;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fidelicard.Campanha.UnitTests.Services
{
    public class CampanhaServiceTests
    {
        private readonly Mock<ICampanhaRepository> _mockRepository;
        private readonly Mock<ILogger<CampanhaService>> _mockLogger;
        private readonly CampanhaService _service;

        public CampanhaServiceTests()
        {
            _mockRepository = new Mock<ICampanhaRepository>();
            _mockLogger = new Mock<ILogger<CampanhaService>>();
            _service = new CampanhaService(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task ConsultarCampanhaAsync_QuandoCampanhaExiste_RetornaSucesso()
        {
            // Arrange
            var campanhaEsperada = new Campanhas
            {
                Id = 1,
                Nome = "Campanha Teste",
                Descricao = "Descrição teste",
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(30),
                Ativo = 1
            };

            _mockRepository.Setup(repo => repo.ObterPorIdAsync(1))
                .ReturnsAsync(campanhaEsperada);

            // Act
            var resultado = await _service.ConsultarCampanhaAsync(1);

            // Assert
            Assert.NotNull(resultado);
            //Assert.True(resultado.Sucesso);
            Assert.Equal(campanhaEsperada.Id, resultado.Campanha.Id);
            Assert.Equal(campanhaEsperada.Nome, resultado.Campanha.Nome);
        }

        [Fact]
        public async Task ConsultarCampanhaAsync_QuandoCampanhaNaoExiste_RetornaDadosInvalidos()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.ObterPorIdAsync(1))
                .ReturnsAsync((Campanhas)null);

            // Act
            var resultado = await _service.ConsultarCampanhaAsync(1);

            // Assert
            Assert.NotNull(resultado);
            //Assert.False(resultado.Sucesso);
            Assert.Contains("Campanha inexistente", resultado.Mensagem);
        }

        [Fact]
        public async Task CadastrarCampanhaAsync_QuandoDadosValidos_RetornaIdCampanha()
        {
            // Arrange
            var novaCampanha = new Campanhas
            {
                Nome = "Nova Campanha",
                Descricao = "Nova descrição",
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(30),
                Ativo = 1
            };

            _mockRepository.Setup(repo => repo.CadastrarAsync(It.IsAny<Campanhas>()))
                .ReturnsAsync(1);

            // Act
            var resultado = await _service.CadastrarCampanhaAsync(novaCampanha);

            // Assert
            Assert.Equal(1, resultado);
        }

        [Fact]
        public async Task AtualizarCampanhaAsync_QuandoCampanhaExiste_RetornaTrue()
        {
            // Arrange
            var campanha = new Campanhas
            {
                Id = 1,
                Nome = "Campanha Atualizada",
                Descricao = "Descrição atualizada",
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(30),
                Ativo = 1
            };

            _mockRepository.Setup(repo => repo.AtualizarAsync(It.IsAny<Campanhas>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _service.AtualizarCampanhaAsync(campanha);

            // Assert
            Assert.Equal(1, resultado);
        }

        [Fact]
        public async Task ConsultarCampanhasPorPeriodoAsync_QuandoExistemCampanhas_RetornaSucesso()
        {
            // Arrange
            var dataInicio = DateTime.Now;
            var dataFim = DateTime.Now.AddDays(30);
            var campanhas = new List<Campanhas>
            {
                new Campanhas
                {
                    Id = 1,
                    Nome = "Campanha 1",
                    DataInicio = dataInicio,
                    DataFim = dataFim,
                    Ativo = 1
                },
                new Campanhas
                {
                    Id = 2,
                    Nome = "Campanha 2",
                    DataInicio = dataInicio.AddDays(1),
                    DataFim = dataFim.AddDays(-1),
                    Ativo = 1
                }
            };

            _mockRepository.Setup(repo => repo.ListarAsync())
                .ReturnsAsync(campanhas);

            // Act
            var resultado = await _service.ConsultarCampanhasPorPeriodoAsync(dataInicio, dataFim);

            // Assert
            Assert.NotNull(resultado);
            //Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Campanhas);
            Assert.Equal(2, resultado.Campanhas.Count());
        }
    }
}
