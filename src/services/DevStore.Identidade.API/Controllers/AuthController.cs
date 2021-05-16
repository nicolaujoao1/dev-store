using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DevStore.Core.Messages.Integration;
using DevStore.Identidade.API.Models;
using DevStore.Identidade.API.Services;
using DevStore.MessageBus;
using DevStore.WebAPI.Core.Controllers;

namespace DevStore.Identidade.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly AuthenticatioDevStorervice _authenticatioDevStorervice;
        private readonly IMessageBus _bus;

        public AuthController(
            AuthenticatioDevStorervice authenticatioDevStorervice,
            IMessageBus bus)
        {
            _authenticatioDevStorervice = authenticatioDevStorervice;
            _bus = bus;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _authenticatioDevStorervice.UserManager.CreateAsync(user, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                var clienteResult = await RegistrarCliente(usuarioRegistro);

                if (!clienteResult.ValidationResult.IsValid)
                {
                    await _authenticatioDevStorervice.UserManager.DeleteAsync(user);
                    return CustomResponse(clienteResult.ValidationResult);
                }

                return CustomResponse(await _authenticatioDevStorervice.GerarJwt(usuarioRegistro.Email));
            }

            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _authenticatioDevStorervice.SignInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha,
                false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await _authenticatioDevStorervice.GerarJwt(usuarioLogin.Email));
            }

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou Senha incorretos");
            return CustomResponse();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
        {
            var usuario = await _authenticatioDevStorervice.UserManager.FindByEmailAsync(usuarioRegistro.Email);

            var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(
                Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

            try
            {
                return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
            }
            catch
            {
                await _authenticatioDevStorervice.UserManager.DeleteAsync(usuario);
                throw;
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                AdicionarErroProcessamento("Refresh Token inválido");
                return CustomResponse();
            }

            var token = await _authenticatioDevStorervice.ObterRefreshToken(Guid.Parse(refreshToken));

            if (token is null)
            {
                AdicionarErroProcessamento("Refresh Token expirado");
                return CustomResponse();
            }

            return CustomResponse(await _authenticatioDevStorervice.GerarJwt(token.Username));
        }
    }
}