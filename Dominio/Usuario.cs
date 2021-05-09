using Microsoft.AspNetCore.Identity;

namespace Dominio
{
    public class Usuario : IdentityUser//heredamos para usar Core security
    {
        public string NombreCompleto { get; set; }
    }
}