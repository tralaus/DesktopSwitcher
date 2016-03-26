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
        List<HotKeyRegister> hotKeyToRegister = null;

        Keys registerKey = Keys.None;

        KeyModifiers registerModifiers = KeyModifiers.None;

        public MainForm()
        {
            InitializeComponent();

            // Register the hotkey.
            hotKeyToRegister = new List<HotKeyRegister>();

            var hotKey = new HotKeyRegister(this.Handle, 100,
                KeyModifiers.Alt, Keys.D1);
            hotKey.HotKeyPressed += new EventHandler(HotKeyPressed);
            hotKeyToRegister.Add(hotKey);
            
            hotKey = new HotKeyRegister(this.Handle, 101,
                KeyModifiers.Alt, Keys.D2);
            hotKey.HotKeyPressed += new EventHandler(HotKeyPressed);
            hotKeyToRegister.Add(hotKey);

            hotKey = new HotKeyRegister(this.Handle, 102,
                KeyModifiers.Alt, Keys.D3);
            hotKey.HotKeyPressed += new EventHandler(HotKeyPressed);
            hotKeyToRegister.Add(hotKey);

            mynotifyicon = new NotifyIcon
            {
                Icon = new Icon("Pretzel.ico"),
                Visible = true,
                BalloonTipText = "Desktop Switch",
                BalloonTipTitle = "Desktop Switch Title"
            };
            mynotifyicon.ShowBalloonTip(500);
            mynotifyicon.Click += (sender, args) => this.Visible = true;

            this.WindowState = FormWindowState.Minimized;
        }


        /// <summary>
        /// Handle the KeyDown of tbHotKey. In this event handler, check the pressed keys.
        /// The keys that must be pressed in combination with the key Ctrl, Shift or Alt,
        /// like Ctrl+Alt+T. The method HotKeyRegister.GetModifiers could check whether 
        /// "T" is pressed.
        /// </summary>
        private void tbHotKey_KeyDown(object sender, KeyEventArgs e)
        {
            // The key event should not be sent to the underlying control.
            e.SuppressKeyPress = true;

            // Check whether the modifier keys are pressed.
            if (e.Modifiers != Keys.None)
            {
                Keys key = Keys.None;
                KeyModifiers modifiers = HotKeyRegister.GetModifiers(e.KeyData, out key);

                // If the pressed key is valid...
                if (key != Keys.None)
                {
                    this.registerKey = key;
                    this.registerModifiers = modifiers;

                    // Display the pressed key in the textbox.
                    tbHotKey.Text = string.Format("{0}+{1}",
                        this.registerModifiers, this.registerKey);

                    // Enable the button.
                    btnRegister.Enabled = true;
                }
            }
        }


        /// <summary>
        /// Handle the Click event of btnRegister.
        /// </summary>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (ArgumentException argumentException)
            {
                MessageBox.Show(argumentException.Message);
            }
            catch (ApplicationException applicationException)
            {
                MessageBox.Show(applicationException.Message);
            }
        }

        /// <summary>
        /// Show a message box if the HotKeyPressed event is raised.
        /// </summary>
        void HotKeyPressed(object sender, EventArgs e)
        {
            CSRegisterHotkey.HotKeyRegister reg = (CSRegisterHotkey.HotKeyRegister) sender;

            Desktop desktop = Desktop.FromIndex((int)reg.Key - 49);
            desktop.MakeVisible();
        }

        private System.Windows.Forms.NotifyIcon mynotifyicon;


        private void frmMain_Resize(object sender, EventArgs e)
        {

            if (FormWindowState.Minimized == this.WindowState)
            {
                mynotifyicon.Visible = true;
                mynotifyicon.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                mynotifyicon.Visible = false;
            }
        }


        /// <summary>
        /// Handle the Click event of btnUnregister.
        /// </summary>
        private void btnUnregister_Click(object sender, EventArgs e)
        {
            // Dispose the hotKeyToRegister.
            if (hotKeyToRegister != null)
            {
            }

            // Update the UI.
            tbHotKey.Enabled = true;
            btnRegister.Enabled = true;
            btnUnregister.Enabled = false;
        }


        /// <summary>
        /// Dispose the hotKeyToRegister when the form is closed.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            if (hotKeyToRegister != null)
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
