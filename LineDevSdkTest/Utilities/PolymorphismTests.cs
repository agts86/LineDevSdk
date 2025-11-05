using LineDevSdk.Utilities;
using LineDevSdk.DTOs.Commons.Messages;

namespace LineDevSdkTest.Utilities;

public class PolymorphismTests
{
    [Fact]
    public void CreatePolymorphismArray_IMessage_ReturnsSdkConcreteTypes()
    {
        // Act
        var result = Polymorphism.CreatePolymorphismArray<IMessage>();
        var types = result.Select(x => x.GetType()).ToArray();

        // Assert
        Assert.Contains(typeof(TextMessage), types);
        Assert.Contains(typeof(LocationMessage), types);
        // 他にもIMessage実装型があればここにAssert追加可
        Assert.True(types.Length >= 2); // 実装型が増えた場合もOK
    }
}

