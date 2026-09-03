using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Pedidos.Commands.CreateOrder;
using Orders.Application.Pedidos.Commands.ChangeOrderStatus;
using Orders.Application.Pedidos.Queries.GetOrderById;
using Orders.Application.Pedidos.Queries.GetOrders;

namespace Orders.Api.Controllers;

[ApiController]
[Route("api/pedidos")]
public sealed class OrdersController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IValidator<CreateOrderCommand> _validator;

    public OrdersController(
        ISender sender,
        IValidator<CreateOrderCommand> validator)
    {
        _sender = sender;
        _validator = validator;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(CreateOrderResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(
            command,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            Dictionary<string, string[]> errors = validationResult.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(error => error.ErrorMessage)
                        .Distinct()
                        .ToArray());

            return BadRequest(
                new ValidationProblemDetails(errors));
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


    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<GetOrdersResponse>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetOrdersQuery(),
            cancellationToken);

        return Ok(result.Value);
    }


    [HttpPatch("{pedidoId:guid}/estado")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
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
            Dictionary<string, string[]> errors = validationResult.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(error => error.ErrorMessage)
                        .Distinct()
                        .ToArray());

            return BadRequest(
                new ValidationProblemDetails(errors));
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
        var result = await _sender.Send(
            new GetOrderByIdQuery(pedidoId),
            cancellationToken);

        if (result.Failure)
        {
            return NotFound(
                new
                {
                    error = result.Error
                });
        }

        return Ok(result.Value);
    }
}


public sealed record ChangeOrderStatusRequest(
    string NuevoEstado);
