using Billing.Application.DTOs;
using Billing.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Api.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(
        IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceResponse>> Create(
        [FromBody] CreateInvoiceRequest request,
        CancellationToken cancellationToken)
    {
        var invoice = await _invoiceService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = invoice.Id },
            invoice);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InvoiceResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var invoice = await _invoiceService.GetByIdAsync(
            id,
            cancellationToken);

        if (invoice is null)
        {
            return NotFound();
        }

        return Ok(invoice);
    }

    [HttpPost("{id:guid}/print")]
    public async Task<ActionResult<InvoiceResponse>> Print(
        Guid id,
        CancellationToken cancellationToken)
    {
        var invoice = await _invoiceService.PrintAsync(
            id,
            cancellationToken);

        if (invoice is null)
        {
            return NotFound();
        }

        return Ok(invoice);
    }
}