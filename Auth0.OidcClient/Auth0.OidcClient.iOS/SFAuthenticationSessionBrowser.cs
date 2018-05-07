using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using IdentityModel.OidcClient.Browser;
using SafariServices;
using UIKit;

namespace Auth0.OidcClient.iOS
{
{
    public class SFAuthenticationSessionBrowser : IBrowser
    {
        SFAuthenticationSession _sf;

        public SFAuthenticationSessionBrowser()
        {
            Debug.WriteLine("ctor");
        }

        public Task<BrowserResult> InvokeAsync(BrowserOptions options)
        {
            var wait = new TaskCompletionSource<BrowserResult>();

            _sf = new SFAuthenticationSession(
                new NSUrl(options.StartUrl),
                options.EndUrl,
                (callbackUrl, error) =>
                {
                    if (error != null)
                    {
                        var errorResult = new BrowserResult
                        {
                            ResultType = BrowserResultType.UserCancel,
                            Error = error.ToString()
                        };

                        wait.SetResult(errorResult);
                    }
                    else
                    {
                        var result = new BrowserResult
                        {
                            ResultType = BrowserResultType.Success,
                            Response = callbackUrl.AbsoluteString
                        };

                        wait.SetResult(result);
                    }
                });

            _sf.Start();
            return wait.Task;
        }
    }
}