using Billing.Application.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace Billing.Infrastructure.Clients;

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _httpClient;

    public InventoryClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ProductExistsAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"api/products/{productId}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;
    }

    public async Task<bool> RemoveStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            Quantity = quantity
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"api/products/{productId}/stock/out",
            request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            var error = await response.Content
                .ReadFromJsonAsync<ProblemDetailsResponse>(
                    cancellationToken);

            throw new Billing.Application.Exceptions
                .InsufficientStockException(
                    error?.Detail ?? "Insufficient stock.");
        }

        response.EnsureSuccessStatusCode();

        return true;
    }

    public async Task<bool> AddStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            Quantity = quantity
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"api/products/{productId}/stock/in",
            request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;
    }

    private sealed class ProblemDetailsResponse
    {
        public string? Detail { get; set; }
    }
}