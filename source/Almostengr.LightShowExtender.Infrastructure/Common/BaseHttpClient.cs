using Almostengr.LightShowExtender.DomainService.Common;
using System.Text.Json;
using System.Text;

namespace Almostengr.LightShowExtender.Infrastructure.Common;

public abstract class BaseHttpClient : IBaseHttpClient
{
    internal async Task<T> HttpGetAsync<T>(HttpClient httpClient, string route)
    {
        int attempt = 0;

    SendRequest:
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(route);
            await response.WasRequestSuccessfulAsync();
            string result = await response.Content.ReadAsStringAsync();

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(result, typeof(T));
            }

            return JsonSerializer.Deserialize<T>(result, SerializerOptions())!;
        }
        catch (ServerErrorException ex)
        {
            attempt = await HandleServerErrorAsync(attempt, ex);
            goto SendRequest;
        }
    }

    internal async Task HttpDeleteAsync(HttpClient httpClient, string route)
    {
        int attempt = 0;

    SendRequest:
        try
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(route);
            await response.WasRequestSuccessfulAsync();
        }
        catch (ServerErrorException ex)
        {
            attempt = await HandleServerErrorAsync(attempt, ex);
            goto SendRequest;
        }
    }

    internal async Task<X> HttpPutAsync<T, X>(HttpClient httpClient, string route, T transferObject) where T : BaseDto where X : BaseDto
    {
        int attempt = 0;

    SendRequest:
        try
        {
            StringContent content = SerializeRequestBody<T>(transferObject);
            HttpResponseMessage response = await httpClient.PutAsync(route, content);
            await response.WasRequestSuccessfulAsync();

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<X>(result, SerializerOptions())!;
        }
        catch (ServerErrorException ex)
        {
            attempt = await HandleServerErrorAsync(attempt, ex);
            goto SendRequest;
        }
    }

    internal async Task<T> HttpPutAsync<T>(HttpClient httpClient, string route, T transferObject) where T : BaseDto
    {
        int attempt = 0;

    SendRequest:
        try
        {
            StringContent content = SerializeRequestBody<T>(transferObject);
            HttpResponseMessage response = await httpClient.PutAsync(route, content);
            await response.WasRequestSuccessfulAsync();

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(result, SerializerOptions())!;
        }
        catch (ServerErrorException ex)
        {
            attempt = await HandleServerErrorAsync(attempt, ex);
            goto SendRequest;
        }
    }

    internal async Task<T> HttpPostAsync<T>(HttpClient httpClient, string route, T transferObject) where T : BaseDto
    {
        int attempt = 0;

    SendRequest:
        try
        {
            StringContent content = SerializeRequestBody<T>(transferObject);
            HttpResponseMessage response = await httpClient.PostAsync(route, content);
            await response.WasRequestSuccessfulAsync();

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(result, SerializerOptions())!;
        }
        catch (ServerErrorException ex)
        {
            attempt = await HandleServerErrorAsync(attempt, ex);
            goto SendRequest;
        }
    }

    private async Task<int> HandleServerErrorAsync(int attempt, ServerErrorException exception)
    {
        const int MAX_RETRIES = 3;
        if (attempt > MAX_RETRIES)
        {
            throw exception;
        }

        attempt++;
        await Task.Delay(TimeSpan.FromSeconds(attempt * 2));

        return attempt;
    }

    private StringContent SerializeRequestBody<T>(T transferObject) where T : BaseDto
    {
        var json = JsonSerializer.Serialize(transferObject);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    internal string GetUrlWithProtocol(string address)
    {
        const string http = "http://";

        if (!address.StartsWith(http) && !address.StartsWith("https://"))
        {
            address = http + address;
        }

        if (!address.EndsWith("/"))
        {
            address = address + "/";
        }

        return address;
    }

    private JsonSerializerOptions SerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }
}