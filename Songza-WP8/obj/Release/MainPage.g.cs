﻿#pragma checksum "C:\Users\Andrew\documents\visual studio 2012\Projects\Songza-WP8\Songza-WP8\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F97B1551695C2A4FA7121731CC4945E2"
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
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ProgressBar Progress;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal Microsoft.Phone.Controls.ListPicker Day;
        
        internal Microsoft.Phone.Controls.ListPicker Period;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.LongListSelector ScenarioList;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Songza-WP8;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Progress = ((System.Windows.Controls.ProgressBar)(this.FindName("Progress")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.Day = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("Day")));
            this.Period = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("Period")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.ScenarioList = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("ScenarioList")));
        }
    }
}

