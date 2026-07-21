namespace Identity.Api.Contratos.Autenticacion;

public sealed record IniciarSesionPeticion(
    string Correo,
    string Contrasena);
