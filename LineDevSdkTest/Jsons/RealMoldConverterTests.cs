using System.Text.Json;
using LineDevSdk.Jsons;
using Xunit;

namespace LineDevSdkTest.Jsons
{
    public class RealMoldConverterTests
    {
        [Fact]
        public void Write_SerializeArray_Success()
        {
            // Arrange
            var arr = new int[] { 1, 2, 3 };
            var options = new JsonSerializerOptions();
            options.Converters.Add(new RealMoldConverter<int[]>());

            // Act
            var jsonArr = JsonSerializer.Serialize(arr, options);

            // Assert
            Assert.Equal("[1,2,3]", jsonArr);
        }

        [Fact]
        public void Read_ThrowsNotImplementedException()
        {
            // Arrange
            var json = "[1,2,3]";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new RealMoldConverter<int[]>());

            // Act & Assert
            Assert.Throws<NotImplementedException>(() =>
                JsonSerializer.Deserialize<int[]>(json, options));
        }
    }
}
