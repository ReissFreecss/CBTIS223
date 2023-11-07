using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace GestorAlumnos_V1.Authorizaton
{
    public class AdministratorsAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Usuario>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, Usuario resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            //Developer pueden hacer todas las acciones
            if (context.User.IsInRole(Constante.RootRole))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
