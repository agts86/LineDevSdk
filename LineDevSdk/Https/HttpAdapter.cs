﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LineDevSdk.Https;

/// <summary>
/// http操作クラス
/// </summary>
public class HttpAdapter(HttpClient httpClient)
{
    /// <summary>
    /// HttpClientオブジェクト
    /// </summary>
    protected HttpClient HttpClient { get;} = httpClient;

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
    public async virtual Task<T> GetAsync<T>(string url, AuthenticationHeaderValue authenticationHeaderValue = null) where T : class
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
    public async virtual Task<T> PostAsync<T, Tbody>(string url, Tbody body, AuthenticationHeaderValue authenticationHeaderValue = null)  where T : class
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
    /// リクエスト共通処理
    /// </summary>
    /// <returns>レスポンス結果</returns>
    private async Task<T> SendAsync<T>(HttpRequestMessage request) where T : class
    {
        var res = await HttpClient.SendAsync(request);
        if (!res.IsSuccessStatusCode) throw new HttpRequestException(res.RequestMessage.ToString());
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }
}
