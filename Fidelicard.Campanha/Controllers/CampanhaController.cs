using Fidelicard.Campanha.Core.Interface;
using Fidelicard.Campanha.Core.Models;

using Fidelicard.Campanha.Core.Result;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;



namespace Fidelicard.Campanha.Controllers

{

    [ApiController]

    [Route("api/v{version:apiVersion}/fidelicard/campanha")]

    public class CampanhaController : ControllerBase

    {

        private readonly ICampanhaService _service;

        private readonly ILogger<CampanhaController> _logger;

        private readonly IConfiguration _configuration;



        public CampanhaController(ICampanhaService service,

           ILogger<CampanhaController> logger,

           IConfiguration configuration)

        {

            _service = service;

            _logger = logger;

            _configuration = configuration;

        }



        [HttpGet("obterCampanha")]

        [SwaggerResponse(StatusCodes.Status200OK, "Consultar campanha obtido com sucesso", typeof(CampanhasResponse))]

        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]

        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Não autorizado")]

        [SwaggerResponse(StatusCodes.Status403Forbidden, "Acesso negado")]

        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]

        public async Task<IActionResult> ObterCampanha([FromRoute] int idCampanha)

        {

            if (idCampanha == 0)

            {

                return BadRequest(new { Mensagem = "Código da campanha invalido" });

            }



            try

            {

                var response = await _service.ConsultarCampanhaAsync(idCampanha).ConfigureAwait(false);



                if (response == null)

                {

                    return StatusCode(StatusCodes.Status500InternalServerError,

                        new { Mensagem = "Erro ao obter Campanha. Tente novamente mais tarde." });

                }



                return Ok(response);               

            }

            catch (UnauthorizedAccessException authEx)

            {

                _logger.LogWarning("Falha de autenticação: {Message}", authEx.Message);

                return Unauthorized(new { Mensagem = "Acesso não autorizado." });

            }

            catch (Exception ex)

            {

                _logger.LogError("Erro inesperado ao obter Campanha: {Message} - STACKTRACE: {StackTrace}", ex.Message, ex.StackTrace);



                var errorDetails = new

                {

                    Mensagem = "Erro inesperado ao processar sua solicitação. Tente novamente mais tarde.",

                    Controle = new

                    {

                        Codigo = "USUARIO.500",

                        Descricao = "Erro no processamento de Obter Campanha"

                    }

                };



                return StatusCode(StatusCodes.Status500InternalServerError, errorDetails);

            }

        }



        [HttpGet]

        [Route("obterCampanha/{dataInicio}/{dataFim}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Campanhas obtidas com sucesso", typeof(IEnumerable<CampanhasResponse>))]

        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]

        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Não autorizado")]

        [SwaggerResponse(StatusCodes.Status403Forbidden, "Acesso negado")]

        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]

        public async Task<IActionResult> ObterCampanhasPorPeriodo([FromRoute] DateTime dataInicio, [FromRoute] DateTime dataFim)

        {

            if (dataInicio == DateTime.MinValue || dataFim == DateTime.MinValue)

            {

                return BadRequest(new { Mensagem = "As datas fornecidas são inválidas." });

            }



            if (dataInicio > dataFim)

            {

                return BadRequest(new { Mensagem = "A data de início não pode ser maior que a data de fim." });

            }



            try

            {

                var response = await _service.ConsultarCampanhasPorPeriodoAsync(dataInicio, dataFim).ConfigureAwait(false);



                if (response == null)

                {

                    return NotFound(new { Mensagem = "Nenhuma campanha encontrada para o período especificado." });

                }



                return Ok(response);

            }

            catch (UnauthorizedAccessException authEx)

            {

                _logger.LogWarning("Falha de autenticação: {Message}", authEx.Message);

                return Unauthorized(new { Mensagem = "Acesso não autorizado." });

            }

            catch (Exception ex)

            {

                _logger.LogError("Erro inesperado ao obter Campanhas: {Message} - STACKTRACE: {StackTrace}", ex.Message, ex.StackTrace);



                var errorDetails = new

                {

                    Mensagem = "Erro inesperado ao processar sua solicitação. Tente novamente mais tarde.",

                    Controle = new

                    {

                        Codigo = "CAMPANHAS.500",

                        Descricao = "Erro no processamento de Obter Campanhas"

                    }

                };



                return StatusCode(StatusCodes.Status500InternalServerError, errorDetails);

            }

        }



        [HttpPost("cadastro")]

        [SwaggerResponse(StatusCodes.Status200OK, "Campanha cadastrada com sucesso", typeof(CampanhasResponse))]

        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]

        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Não autorizado")]

        [SwaggerResponse(StatusCodes.Status403Forbidden, "Acesso negado")]

        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]

        public async Task<IActionResult> CadastrarCampanha([FromBody] Campanhas campanha)

        {

            if (campanha == null)

            {

                return BadRequest(new { Mensagem = "O corpo da requisição não pode ser nulo." });

            }



            try

            {

                var response = await _service.CadastrarCampanhaAsync(campanha).ConfigureAwait(false);



                if (response == 0)

                {

                    return StatusCode(StatusCodes.Status500InternalServerError,

                        new { Mensagem = "Erro ao cadastrar campanha. Tente novamente mais tarde." });

                }



                return Ok(response);

            }

            catch (ArgumentException argEx)

            {

                _logger.LogWarning("Validação falhou ao cadastrar campanha: {Message}", argEx.Message);

                return BadRequest(new { Mensagem = argEx.Message });

            }

            catch (UnauthorizedAccessException authEx)

            {

                _logger.LogWarning("Falha de autenticação: {Message}", authEx.Message);

                return Unauthorized(new { Mensagem = "Acesso não autorizado." });

            }

            catch (Exception ex)

            {

                _logger.LogError("Erro inesperado ao cadastrar campanha: {Message} - STACKTRACE: {StackTrace}", ex.Message, ex.StackTrace);



                var errorDetails = new

                {

                    Mensagem = "Erro inesperado ao processar sua solicitação. Tente novamente mais tarde.",

                    Controle = new

                    {

                        Codigo = "USUARIO.500",

                        Descricao = "Erro no processamento no cadastro de campanha"

                    }

                };



                return StatusCode(StatusCodes.Status500InternalServerError, errorDetails);

            }

        }



        [HttpPut("atualizarCampanha")]

        [SwaggerResponse(StatusCodes.Status200OK, "Campanha atualizada com sucesso", typeof(CampanhasResponse))]

        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]

        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Não autorizado")]

        [SwaggerResponse(StatusCodes.Status403Forbidden, "Acesso negado")]

        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno no servidor")]

        public async Task<IActionResult> AtualizarCampanha([FromBody] Campanhas campanha)

        {

            if (campanha == null)

            {

                return BadRequest(new { Mensagem = "O corpo da requisição não pode ser nulo." });

            }



            try

            {

                var response = await _service.AtualizarCampanhaAsync(campanha).ConfigureAwait(false);



                if (response == 0)

                {

                    return StatusCode(StatusCodes.Status500InternalServerError,

                        new { Mensagem = "Erro ao processar a atualização da campanha. Tente novamente mais tarde." });

                }



                return Ok(response);

            }

            catch (ArgumentException argEx)

            {

                _logger.LogWarning("Validação falhou ao processar campanha: {Message}", argEx.Message);

                return BadRequest(new { Mensagem = argEx.Message });

            }

            catch (UnauthorizedAccessException authEx)

            {

                _logger.LogWarning("Falha de autenticação: {Message}", authEx.Message);

                return Unauthorized(new { Mensagem = "Acesso não autorizado." });

            }

            catch (Exception ex)

            {

                _logger.LogError("Erro inesperado ao processar a atualização da campanha: {Message} - STACKTRACE: {StackTrace}", ex.Message, ex.StackTrace);



                var errorDetails = new

                {

                    Mensagem = "Erro inesperado ao processar sua solicitação. Tente novamente mais tarde.",

                    Controle = new

                    {

                        Codigo = "USUARIO.500",

                        Descricao = "Erro no processamento de Atualizar Campanha"

                    }

                };



                return StatusCode(StatusCodes.Status500InternalServerError, errorDetails);

            }

        }

    }

}

