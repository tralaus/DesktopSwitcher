/****************************** Module Header ******************************\
* Module Name:  MainForm.cs
* Project:	    CSRegisterHotkey
* Copyright (c) Microsoft Corporation.
* 
* This is the main form of this application. It is used to initialize the UI 
* and handle the events.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DesktopSwitch.Windows10Interop;

namespace CSRegisterHotkey
{
    public partial class MainForm : Form
    {
        readonly List<HotKeyRegister> _hotKeyToRegister = null;
        private readonly System.Windows.Forms.NotifyIcon _mynotifyicon;

        public MainForm()
        {
            InitializeComponent();

            List<Keys> desktops = new List<Keys> { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5 };

            // Register the hotkey.
            _hotKeyToRegister = new List<HotKeyRegister>();

            foreach (var desktop in desktops)
            {
                var hotKey = new HotKeyRegister(this.Handle, (int)desktop,
                KeyModifiers.Alt, desktop);
                hotKey.HotKeyPressed += new EventHandler(HotKeyPressed);
                _hotKeyToRegister.Add(hotKey);
            }

            _mynotifyicon = new NotifyIcon
            {
                Icon = new Icon("Pretzel.ico"),
                Visible = true
            };

            _mynotifyicon.Click += (sender, args) => this.Visible = true;

            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Show a message box if the HotKeyPressed event is raised.
        /// </summary>
        void HotKeyPressed(object sender, EventArgs e)
        {
            try
            {
                HotKeyRegister reg = (CSRegisterHotkey.HotKeyRegister)sender;

                Desktop desktop = Desktop.FromIndex((int)reg.Key - 49);
                desktop.MakeVisible();
            }
            catch (Exception)
            {
                _mynotifyicon.BalloonTipText = "Invalid Desktop";
                _mynotifyicon.BalloonTipTitle = "Error";
                _mynotifyicon.ShowBalloonTip(500);
            }

        }

        /// <summary>
        /// Dispose the hotKeyToRegister when the form is closed.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            if (_hotKeyToRegister != null)
            {
            }

            base.OnClosed(e);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
            }
        }
    }
}
