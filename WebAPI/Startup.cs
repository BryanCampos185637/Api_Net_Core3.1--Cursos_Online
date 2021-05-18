using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Authorization;
using Persistencia;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Aplicacion.Cursos;
using FluentValidation.AspNetCore;
using WebAPI.Middleware;
using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Aplicacion.Contratos;
using Seguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
using Microsoft.OpenApi.Models;
using Persistencia.Paginacion;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region controller
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));//valida que este autenticado para usar los metodos de los controles
            }).AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>()); //validaciones 
            #endregion

            #region mis servicios DbContext
            services.AddDbContext<CursosOnlineContext>(p =>
            {//se pasa la conexion de la base de datos
                p.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            #endregion

            #region conexion a la base de datos con dapper
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));
            services.AddOptions();
            #endregion

            #region configuracion de servicio IMediator
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);//dominio/curso/consulta
            #endregion

            #region configuracion de core identity
            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            #endregion

            #region identity role
            identityBuilder.AddRoles<IdentityRole>();
            identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario, IdentityRole>>();
            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();//va manejar login
            services.TryAddSingleton<ISystemClock, SystemClock>();
            #endregion

            #region inyeccion para la creacion de tokens
            services.AddScoped<IJwtGenerador, JwtGenerador>();
            #endregion

            #region seguridad utilizando los tokens
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=> {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            #endregion

            #region obtener el nombre del usuario
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();
            #endregion

            #region servicio de AutoMapper
            services.AddAutoMapper(typeof(Consulta.Manejador));
            #endregion

            #region conexion de dapper al arrancar la api
            services.AddTransient<IFactoryConnection, FactoryConnection>();
            services.AddScoped<IInstructor, InstructorRepositorio>();
            #endregion

            #region paginacion
            services.AddScoped<IPaginacion, PaginacionRepositorio>();
            #endregion

            #region swagger
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Servicios para mantenimientos de cursos",
                    Version = "v1"
                });
                p.CustomSchemaIds(p => p.FullName);
            });
            #endregion

            #region cors
            services.AddCors(p => p.AddPolicy("corsApp", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region UseSwagger
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v1/swagger.json", "Cusos online v1");
            });
            #endregion

            app.UseMiddleware<ManejadorErrorMiddleware>();//manejador de errores

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseCors("corsApp");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}