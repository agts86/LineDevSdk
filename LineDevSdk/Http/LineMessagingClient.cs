using System.Net.Http.Headers;
using LineDevSdk.DTO.MessagingAPIs;

namespace LineDevSdk.Http;

public interface ILineMessagingClient
{
    /// <summary>
    /// グルメAPIを実行して結果を取得する
    /// </summary>
    /// <param name="reply">返信内容</param>
    /// <returns>実行結果</returns>
    Task PostReplyAsync(Reply Reply, string token);
}   

/// <summary>
/// LineAPI用クライアント
/// </summary>
public class LineMessagingClient(HttpClient httpClient = null) : ILineMessagingClient
{
    /// <summary>
    /// デフォルトURL
    /// </summary>
    /// <value></value>
    public string DefaultUrl { get; } = "https://api.line.me";

    /// <summary>
    /// HttpClient
    /// </summary>
    private HttpAdapter Http { get; } = new HttpAdapter(httpClient ?? new HttpClient());
    
    /// <summary>
    /// 応答メッセージを送信する
    /// </summary>
    /// <param name="message">コンテンツ</param>
    public async Task PostReplyAsync(Reply Reply, string token)
    {
        var auth = new AuthenticationHeaderValue("Bearer", token);
        var endpoint = $"{DefaultUrl}/v2/bot/message/reply";
        await Http.PostAsync<object, Reply>(endpoint, Reply, auth);
    }
}
