﻿

#pragma checksum "C:\Users\sam71\Source\Repos\sigdp\GDP\GDPClient\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D8ACAD5C540E294EC416BEF031991FE8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GDPClient
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 26 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.settingsBtn_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 27 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.logoutBtn_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 48 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).DoubleTapped += this.userEntrieslv_DoubleTapped;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 150 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.cancelBtnAS_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 84 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.cancelBtn_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

