using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Xunit;
using LineDevSdk.Configurations;
using Microsoft.Extensions.FileProviders;

namespace LineDevSdkTest.Configurations
{
    public class LineSignatureFilterTests
    {
        [Fact]
        public async Task OnActionExecutionAsync_ValidSignature_CallsNext()
        {
            // Arrange
            var secret = "test_secret";
            var body = "test_body";
            var signature = ComputeSignature(secret, body);

            var inMemorySettings = new Dictionary<string, string> {
                {"Line:ChannelSecret", secret}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var env = new WebHostEnvironment { EnvironmentName = Environments.Production };

            var context = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                    ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller: null
            );
            context.HttpContext.Request.Headers["x-line-signature"] = signature;
            context.HttpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            context.HttpContext.Request.Body.Position = 0;

            var filter = new LineSignatureFilter(configuration, env);
            var nextCalled = false;
            ActionExecutionDelegate next = () => { nextCalled = true; return Task.FromResult<ActionExecutedContext>(null); };

            // Act
            await filter.OnActionExecutionAsync(context, next);

            // Assert
            Assert.True(nextCalled);
            Assert.Null(context.Result);
        }

        [Fact]
        public async Task OnActionExecutionAsync_InvalidSignature_ReturnsUnauthorized()
        {
            // Arrange
            var secret = "test_secret";
            var body = "test_body";
            var signature = "invalid_signature";

            var inMemorySettings = new Dictionary<string, string> {
                {"Line:ChannelSecret", secret}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var env = new WebHostEnvironment { EnvironmentName = Environments.Production };

            var context = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                    ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller: null
            );
            context.HttpContext.Request.Headers["x-line-signature"] = signature;
            context.HttpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            context.HttpContext.Request.Body.Position = 0;

            var filter = new LineSignatureFilter(configuration, env);
            var nextCalled = false;
            ActionExecutionDelegate next = () => { nextCalled = true; return Task.FromResult<ActionExecutedContext>(null); };

            // Act
            await filter.OnActionExecutionAsync(context, next);

            // Assert
            Assert.False(nextCalled);
            Assert.IsType<UnauthorizedResult>(context.Result);
        }

        [Fact]
        public async Task OnActionExecutionAsync_Development_SkipsVerification()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string>();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var env = new WebHostEnvironment { EnvironmentName = Environments.Development };

            var context = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                    ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller: null
            );
            var filter = new LineSignatureFilter(configuration, env);
            var nextCalled = false;
            ActionExecutionDelegate next = () => { nextCalled = true; return Task.FromResult<ActionExecutedContext>(null); };

            // Act
            await filter.OnActionExecutionAsync(context, next);

            // Assert
            Assert.True(nextCalled);
            Assert.Null(context.Result);
        }

        private static string ComputeSignature(string secret, string body)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
            return Convert.ToBase64String(hash);
        }

        // テスト用WebHostEnvironment実装
        private class WebHostEnvironment : IWebHostEnvironment
        {
            public string EnvironmentName { get; set; } = string.Empty;
            public string ApplicationName { get; set; } = string.Empty;
            public string WebRootPath { get; set; } = string.Empty;
            public IFileProvider WebRootFileProvider { get; set; } = null!;
            public string ContentRootPath { get; set; } = string.Empty;
            public IFileProvider ContentRootFileProvider { get; set; } = null!;
        }
    }
}
