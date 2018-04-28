using System;
using System.IO;
using Xunit;
using Roadkill.Core;
using Roadkill.Text;
using Roadkill.Text.Text;
using Roadkill.Text.Text.CustomTokens;
using Roadkill.Text.Text.TextMiddleware;

namespace Roadkill.Tests.Unit.Text.TextMiddleware
{
    public class CustomTokenMiddlewareTests
    {
        [Fact]
        public void should_clean_html_using_sanitizer()
        {
            string markdown = @"@@warningbox:ENTER YOUR CONTENT HERE

here is some more content

@@";

            string expectedHtml = @"<div class=""alert alert-warning"">ENTER YOUR CONTENT HERE

here is some more content

</div><br style=""clear:both""/>";

            var pagehtml = new PageHtml() { Html = markdown };

            var appSettings = new TextSettings();
            appSettings.CustomTokensPath = Path.Combine(Directory.GetCurrentDirectory(), "Unit", "Text", "CustomTokens", "customvariables.xml");

            var customTokenParser = new CustomTokenParser(appSettings);
            var middleware = new CustomTokenMiddleware(customTokenParser);

            // Act
            PageHtml actualPageHtml = middleware.Invoke(pagehtml);

            actualPageHtml.Html = actualPageHtml.Html.Replace(Environment.NewLine, "");
            expectedHtml = expectedHtml.Replace(Environment.NewLine, "");

            // Assert
            Assert.Equal(expectedHtml, actualPageHtml.Html);
        }
    }
}