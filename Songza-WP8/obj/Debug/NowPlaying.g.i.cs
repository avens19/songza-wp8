﻿#pragma checksum "C:\Users\Andrew\Documents\Visual Studio 2012\Projects\Songza-WP8\Songza-WP8\NowPlaying.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "64E0EBE4C222EE83C9C0EE11C90A4A36"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Songza_WP8 {
    
    
    public partial class NowPlaying : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.StackPanel CurrentTrack;
        
        internal System.Windows.Controls.Image Cover;
        
        internal System.Windows.Controls.Image PlayPause;
        
        internal System.Windows.Controls.Image Next;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Songza-WP8;component/NowPlaying.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.CurrentTrack = ((System.Windows.Controls.StackPanel)(this.FindName("CurrentTrack")));
            this.Cover = ((System.Windows.Controls.Image)(this.FindName("Cover")));
            this.PlayPause = ((System.Windows.Controls.Image)(this.FindName("PlayPause")));
            this.Next = ((System.Windows.Controls.Image)(this.FindName("Next")));
        }
    }
}

