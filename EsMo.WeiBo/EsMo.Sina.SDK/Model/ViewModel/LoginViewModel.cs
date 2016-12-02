﻿using EsMo.Common;
using EsMo.Common.UI;
using EsMo.Sina.Model.Person;
using EsMo.Sina.SDK.Service;
using EsMo.Sina.SDK.Utils;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsMo.Sina.SDK.Model
{
    public class LoginViewModel : BaseViewModel
    {
        #region ctor

        ILoginService loginService;
        IWebView webView;
        bool fillAccount;
        const string FillAccount = "fillAccount()";
        public LoginViewModel(ILoginService loginService, IAccountService accountService)
        {
            this.loginService = loginService;
        }
        #endregion

        #region prop
        public IWebView WebView
        {
            get
            {
                return webView;
            }

            set
            {
                if (this.webView != null)
                    this.webView.LoadFinished -= WebView_LoadFinished;
                webView = value;
                if (this.webView != null)
                    this.webView.LoadFinished += WebView_LoadFinished;
            }
        }
        #endregion

        #region methods

        Task<string> GetLoginPage()
        {
            return this.loginService.GetAuthPage(null, null);
        }
      
        public override void Appearing()
        {
            this.WebView.LoadHtmlString(GetLoginPage().Result, GlobalURI.SinaApi.ToSchemaHttps());
        }
        private void WebView_LoadFinished(object sender, EventArgs e)
        {
            if(!fillAccount)
            {
                this.webView.EvalJavaScript(FillAccount);
                this.fillAccount = true;
            }
            else if(this.webView.Uri.ToString()== "https://api.weibo.com/oauth2/authorize")
            {
                //authorize
            }
            else 
            {
                this.ShowViewModel<StartupViewModel>(new Dictionary<string, string> { { StartupViewModel.LoginUrl, this.WebView.Uri.ToString() } });
            }
        }
        #endregion
    }
}