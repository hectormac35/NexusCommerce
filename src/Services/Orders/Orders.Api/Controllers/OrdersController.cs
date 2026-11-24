using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Pedidos.Commands.ChangeOrderStatus;
using Orders.Application.Pedidos.Commands.CreateOrder;
using Orders.Application.Pedidos.Queries.GetOrderById;
using Orders.Application.Pedidos.Queries.GetOrders;

namespace Orders.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/pedidos")]
public sealed class OrdersController : ControllerBase
{
    private const string RolAdministrador = "Administrador";

    private readonly ISender _sender;
    private readonly IValidator<CreateOrderCommand> _validator;

    public OrdersController(
        ISender sender,
        IValidator<CreateOrderCommand> validator)
    {
        _sender = sender;
        _validator = validator;
    }

    /// <summary>Crea un pedido para el usuario autenticado.</summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(CreateOrderResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        if (ObtenerUsuarioId() is not { } clienteId)
        {
            return Unauthorized();
        }

        // El cliente del pedido es siempre quien hace la petición.
        var command = new CreateOrderCommand(
            clienteId,
            request.Lineas ?? []);

        var validationResult = await _validator.ValidateAsync(
            command,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(
                new ValidationProblemDetails(AgruparErrores(validationResult)));
        }

        var result = await _sender.Send(
            command,
            cancellationToken);

        if (result.Failure)
        {
            return Problem(
                title: "No se pudo crear el pedido",
                detail: result.Error,
                statusCode: StatusCodes.Status400BadRequest);
        }

        CreateOrderResponse response = result.Value!;

        return Created(
            $"/api/pedidos/{response.PedidoId}",
            response);
    }

    /// <summary>Administradores: todos los pedidos. Resto: solo los suyos.</summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<GetOrdersResponse>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        if (ObtenerUsuarioId() is not { } usuarioId)
        {
            return Unauthorized();
        }

        Guid? filtroCliente = User.IsInRole(RolAdministrador)
            ? null
            : usuarioId;

        var result = await _sender.Send(
            new GetOrdersQuery(filtroCliente),
            cancellationToken);

        return Ok(result.Value);
    }

    [HttpPatch("{pedidoId:guid}/estado")]
    [Authorize(Roles = RolAdministrador)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus(
        Guid pedidoId,
        [FromBody] ChangeOrderStatusRequest request,
        [FromServices] IValidator<ChangeOrderStatusCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = new ChangeOrderStatusCommand(
            pedidoId,
            request.NuevoEstado);

        var validationResult = await validator.ValidateAsync(
            command,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(
                new ValidationProblemDetails(AgruparErrores(validationResult)));
        }

        var result = await _sender.Send(
            command,
            cancellationToken);

        if (result.Failure)
        {
            if (result.Error == "Pedido no encontrado.")
            {
                return NotFound(
                    new
                    {
                        error = result.Error
                    });
            }

            return Problem(
                title: "No se pudo cambiar el estado del pedido",
                detail: result.Error,
                statusCode: StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }

    [HttpGet("{pedidoId:guid}")]
    [ProducesResponseType(
        typeof(GetOrderByIdResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid pedidoId,
        CancellationToken cancellationToken)
    {
        if (ObtenerUsuarioId() is not { } usuarioId)
        {
            return Unauthorized();
        }

        var result = await _sender.Send(
            new GetOrderByIdQuery(pedidoId),
            cancellationToken);

        // Un pedido de otra persona se trata igual que uno inexistente (no se revela que existe).
        if (result.Failure
            || (result.Value!.ClienteId != usuarioId && !User.IsInRole(RolAdministrador)))
        {
            return NotFound(
                new
                {
                    error = "Pedido no encontrado."
                });
        }

        return Ok(result.Value);
    }

    private Guid? ObtenerUsuarioId()
    {
        string? valor = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        return Guid.TryParse(valor, out Guid usuarioId)
            ? usuarioId
            : null;
    }

    private static Dictionary<string, string[]> AgruparErrores(
        FluentValidation.Results.ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToArray());
    }
}

public sealed record CreateOrderRequest(
    IReadOnlyCollection<CreateOrderItemRequest>? Lineas);

public sealed record ChangeOrderStatusRequest(
    string NuevoEstado);
