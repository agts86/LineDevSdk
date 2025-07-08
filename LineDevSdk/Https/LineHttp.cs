using System.Net.Http.Headers;
using LineDevSdk.DTOs.MessagingAPIs;
using Microsoft.Extensions.Configuration;

namespace LineDevSdk.Https;

public interface ILineHttp
{
    /// <summary>
    /// グルメAPIを実行して結果を取得する
    /// </summary>
    /// <param name="reply">返信内容</param>
    /// <returns>実行結果</returns>
    Task PostReplyAsync(Reply Reply, string endPointUrl, string token);
}   

/// <summary>
/// LineAPI用のHTTPクライアント
/// </summary>
public class LineHttp(HttpClient httpClient = null) : ILineHttp
{
    /// <summary>
    /// HttpClient
    /// </summary>
    private HttpAdapter Http { get; } = new HttpAdapter(httpClient ?? new HttpClient());
    
    /// <summary>
    /// 応答メッセージを送信する
    /// </summary>
    /// <param name="message">コンテンツ</param>
    public virtual async Task PostReplyAsync(Reply Reply, string endPointUrl, string token)
    {
        var auth = new AuthenticationHeaderValue("Bearer", token);
        await Http.PostAsync<object, Reply>(endPointUrl, Reply, auth);
    }
}
