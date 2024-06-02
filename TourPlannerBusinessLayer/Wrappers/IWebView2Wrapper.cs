using Microsoft.Web.WebView2.Core;

namespace TourPlannerBusinessLayer.Wrappers
{
    public interface IWebView2Wrapper
    {
        Task CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat imageFormat, Stream imageStream);
    }
}
