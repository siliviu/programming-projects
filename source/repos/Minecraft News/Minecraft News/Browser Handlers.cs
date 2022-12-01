using CefSharp;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace Minecraft_News
{
    public class CustomIRequestHandler : IRequestHandler
    {
        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            if (!Form.linksOpenInSystemBrowser || request.Url.StartsWith("data:") || request.Url.Contains("youtube.com/embed") || request.Url.StartsWith("largehtml://") || request.Url.Contains("clips.twitch.tv/embed?"))
                return false;
            Process.Start(request.Url);
            return true;
        }
        public bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return false;
        }

        public bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
        {
            return false;
        }

        public CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            callback.Dispose();
            return CefReturnValue.Continue;
        }

        public bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            callback.Dispose();
            return false;
        }

        public bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return true;
        }

        public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
        }

        public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            return false;
        }

        public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            callback.Dispose();
            return false;
        }

        public void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {
        }

        public void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {
        }

        public void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
        }

        public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl)
        {
        }

        public bool OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return false;
        }


        public IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return null;
        }


        public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
        }

        public bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, System.Security.Cryptography.X509Certificates.X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            callback.Dispose();
            return false;
        }
        bool IRequestHandler.CanGetCookies(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
        {
            return false;
        }
        bool IRequestHandler.CanSetCookie(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, CefSharp.Cookie cookie)
        {
            return false;
        }
    }

    public class CustomILifeSpanHandler : ILifeSpanHandler
    {
        bool ILifeSpanHandler.OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            Process.Start(targetUrl);
            newBrowser = null;
            return true;
        }
        void ILifeSpanHandler.OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
        }
        bool ILifeSpanHandler.DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return false;
        }
        void ILifeSpanHandler.OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
        }
    }

    public class CustomIContextMenuHandler : IContextMenuHandler
    {
        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            if (parameters.TypeFlags.HasFlag(ContextMenuType.Selection))
            {
                model.AddItem((CefMenuCommand)26502, "Search");
            }
            else if (parameters.TypeFlags.HasFlag(ContextMenuType.Media))
            {
                model.Clear();
                model.AddItem((CefMenuCommand)26503, "Go to source URL");
                model.AddItem((CefMenuCommand)26504, "Copy source URL");
                model.AddSeparator();
                model.AddItem((CefMenuCommand)26505, "Save file");
                if (parameters.MediaType == ContextMenuMediaType.Image)
                    model.AddItem((CefMenuCommand)26506, "Copy image");
            }
            else if (parameters.TypeFlags.HasFlag(ContextMenuType.Link))
            {
                model.Clear();
                model.AddItem((CefMenuCommand)26507, "Open link");
                model.AddItem((CefMenuCommand)26508, "Copy link");
            }
            else model.Clear();

        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            if (commandId == (CefMenuCommand)26502)
            {
                Process.Start("https://www.google.com/search?q=" + parameters.SelectionText.Replace(" ", "+"));
                return true;
            }
            if (commandId == (CefMenuCommand)26503)
            {
                Process.Start(parameters.SourceUrl);
                return true;
            }
            if (commandId == (CefMenuCommand)26504)
            {
                Clipboard.SetText(parameters.SourceUrl);
                return true;
            }
            if (commandId == (CefMenuCommand)26505)
            {
                using (WebClient temp = new WebClient())
                {
                    using (SaveFileDialog dialog = new SaveFileDialog())
                    {
                        if (parameters.MediaType == ContextMenuMediaType.Image)
                            dialog.Filter = "Image files (*.png, *.jpg, *.gif, ...) |  *.png; *.gif; *.jpg; *.jpeg; *.jpe; *.jfif;";
                        if (parameters.MediaType == ContextMenuMediaType.Video)
                            dialog.Filter = "Video files (*.mp4, *wmv, ...) | *.mp4; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; *.mkv; *.mov; *.mp2; *.mp2v; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm; *.dat;";
                        if (parameters.MediaType == ContextMenuMediaType.Audio)
                            dialog.Filter = "Audio files (*.mp3; *.wav; *.wma) | *.mp3; *.wav; *.wma;";
                        DialogResult result = dialog.ShowDialog();
                        if (result == DialogResult.OK)
                            temp.DownloadFile(parameters.SourceUrl, dialog.FileName);
                    }
                }
                return true;
            }
            if (commandId == (CefMenuCommand)26506)
            {
                WebRequest request = WebRequest.Create(parameters.SourceUrl);
                WebResponse response = request.GetResponse();
                System.IO.Stream responseStream =
                    response.GetResponseStream();
                Clipboard.SetImage(new Bitmap(responseStream));
                return true;
            }
            if (commandId == (CefMenuCommand)26507)
            {
                Process.Start(parameters.LinkUrl);
                return true;
            }
            if (commandId == (CefMenuCommand)26508)
            {
                Clipboard.SetText(parameters.LinkUrl);
                return true;
            }
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {

        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}
