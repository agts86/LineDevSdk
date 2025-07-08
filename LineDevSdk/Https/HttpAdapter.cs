using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LineDevSdk.Https;

/// <summary>
/// http操作クラス
/// </summary>
internal class HttpAdapter(HttpClient httpClient)
{
    /// <summary>
    /// HttpClientオブジェクト
    /// </summary>
    private HttpClient HttpClient { get;} = httpClient;

    /// <summary>
    /// シリアライズオプション
    /// </summary>
    private JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

    /// <summary>
    /// Getリクエストをする
    /// </summary>
    /// <param name="url">リクエスト先</param>
    /// <param name="authenticationHeaderValue">basic認証</param>
    /// <returns>レスポンス結果</returns>
    internal async Task<T> GetAsync<T>(string url, AuthenticationHeaderValue authenticationHeaderValue = null) where T : class
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url)
        };
        request.Headers.Authorization = authenticationHeaderValue;
        return await SendAsync<T>(request);
    }

    /// <summary>
    /// Postリクエストをする
    /// </summary>
    /// <param name="url">リクエスト先</param>
    /// <param name="contentType">ContentType</param>
    /// <param name="authenticationHeaderValue">basic認証</param>
    /// <returns>レスポンス結果</returns>
    internal async Task<T> PostAsync<T, Tbody>(string url, Tbody body, AuthenticationHeaderValue authenticationHeaderValue = null)  where T : class
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = authenticationHeaderValue;
        return await SendAsync<T>(request);
    }

    /// <summary>
    /// Putリクエストをする
    /// </summary>
    /// <param name="url">リクエスト先</param>
    /// <param name="contentType">ContentType</param>
    /// <param name="authenticationHeaderValue">basic認証</param>
    /// <returns>レスポンス結果</returns>
    internal async Task<T> PutAsync<T, Tbody>(string url, Tbody body, AuthenticationHeaderValue authenticationHeaderValue = null)  where T : class
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(url),
            Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = authenticationHeaderValue;
        return await SendAsync<T>(request);
    }

    /// <summary>
    /// Deleteリクエストをする
    /// </summary>
    /// <param name="url">リクエスト先</param>
    /// <param name="contentType">ContentType</param>
    /// <param name="authenticationHeaderValue">basic認証</param>
    /// <returns>レスポンス結果</returns>
    internal async Task<T> DeleteAsync<T, Tbody>(string url, Tbody body, AuthenticationHeaderValue authenticationHeaderValue = null)  where T : class
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(url),
            Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = authenticationHeaderValue;
        return await SendAsync<T>(request);
    }

    /// <summary>
    /// リクエスト共通処理
    /// </summary>
    /// <returns>レスポンス結果</returns>
    private async Task<T> SendAsync<T>(HttpRequestMessage request) where T : class
    {
        var res = await HttpClient.SendAsync(request);
        if (!res.IsSuccessStatusCode)
        {
            var errorBody = await res.Content.ReadAsStringAsync();
            var message = $"Request failed: {(int)res.StatusCode} {res.StatusCode}\n" +
                          $"Request: {res.RequestMessage}\n" +
                          $"Response: {errorBody}";
            throw new HttpRequestException(message, null, res.StatusCode);
        }
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }
}
