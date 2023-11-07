using GestorAlumnos_V1.Authorizaton;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace GestorAlumnos_V1.Authorization
{
    public class Operations
    {
        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement
        {
            Name = Constante.CreateAlumnoRoot
        };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement
        {
            Name = Constante.ReadAlumnoRoot
        };
        public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement
        {
            Name = Constante.UpdateAlumnoRoot
        };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement
        {
            Name = Constante.DeleteAlumnoRoot
        };
        public static OperationAuthorizationRequirement Approved = new OperationAuthorizationRequirement
        {
            Name = Constante.ApproveOperationRoot
        };
        public static OperationAuthorizationRequirement Rejected = new OperationAuthorizationRequirement
        {
            Name = Constante.RejectOperationRoot
        };
    }
}
