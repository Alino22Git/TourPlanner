using Microsoft.Web.WebView2.Wpf;
using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Threading.Tasks;

namespace TourPlannerBusinessLayer.Wrappers
{
    public class WebView2Wrapper : IWebView2Wrapper
    {
        private readonly WebView2 _webView2;

        public WebView2Wrapper(WebView2 webView2)
        {
            _webView2 = webView2;
        }

        public async Task CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat imageFormat, Stream imageStream)
        {
            await _webView2.CoreWebView2.CapturePreviewAsync(imageFormat, imageStream);
        }
    }
}
