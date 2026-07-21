namespace Identity.Api.Contratos.Autenticacion;

public sealed record RegistrarUsuarioPeticion(
    string Nombre,
    string Apellidos,
    string Correo,
    string Contrasena,
    string ConfirmacionContrasena);
