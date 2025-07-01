using System.Reflection;

namespace LineDevSdk.Utilities;

/// <summary>
/// リフレクションを使ってポリモーフィズムを実現するクラス
/// </summary>
internal class Polymorphism
{
    /// <summary>
    /// ポリモーフィズムの配列を生成する
    /// </summary>
    /// <param name="obj">インスタンス引数</param>
    /// <typeparam name="T">基底型</typeparam>
    /// <returns>基底型配列</returns>
    internal static T[] CreatePolymorphismArray<T>(params object[] obj)
    {
        return [.. Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface)
            .Select(x => obj.Length == 0 ? (T)Activator.CreateInstance(x) : (T)Activator.CreateInstance(x, obj))];
    }
}
