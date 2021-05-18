using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta: IRequest<UsuarioData>
        {
            public string UserName { get; set; }
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class EjecutaValidador: AbstractValidator<Ejecuta> 
        {
            public EjecutaValidador()
            {
                RuleFor(p => p.NombreCompleto).NotEmpty();
                RuleFor(p => p.UserName).NotEmpty();
                RuleFor(p => p.Email).NotEmpty();
                RuleFor(p => p.Password).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta,UsuarioData>
        {
            private readonly CursosOnlineContext context;
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGenerador;
            public Manejador(CursosOnlineContext _context, UserManager<Usuario> _userManager, IJwtGenerador _ijwtGenerador)
            {
                context = _context;
                userManager = _userManager;
                jwtGenerador = _ijwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var existe = await context.Users.Where(p => p.Email.Equals(request.Email)).AnyAsync();//deveulve bool
                if (existe)
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "El email ingresado ya esta en uso" });
                var existeUserName = await context.Users.Where(p => p.UserName.Equals(request.UserName)).AnyAsync();
                if (existeUserName)
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "El Nombre de usuario ingresado ya esta en uso" });
                var usuario = new Usuario
                {
                    NombreCompleto= request.NombreCompleto,
                    Email= request.Email,
                    UserName= request.UserName
                };
                var rpt = await userManager.CreateAsync(usuario, request.Password);
                if (rpt.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = jwtGenerador.CrearToken(usuario, null),
                        UserName = usuario.UserName,
                        Email = usuario.Email
                    };
                }
                else
                    throw new Exception("No se pudo ingresar el nuevo usuario");
            }
        }
    }
}