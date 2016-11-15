/****************************************************************************
**
** Copyright (C) 2016 The Qt Company Ltd.
** Contact: https://www.qt.io/licensing/
**
** This file is part of the Qt VS Tools.
**
** $QT_BEGIN_LICENSE:GPL-EXCEPT$
** Commercial License Usage
** Licensees holding valid commercial Qt licenses may use this file in
** accordance with the commercial license agreement provided with the
** Software or, alternatively, in accordance with the terms contained in
** a written agreement between you and The Qt Company. For licensing terms
** and conditions see https://www.qt.io/terms-conditions. For further
** information use the contact form at https://www.qt.io/contact-us.
**
** GNU General Public License Usage
** Alternatively, this file may be used under the terms of the GNU
** General Public License version 3 as published by the Free Software
** Foundation with exceptions as appearing in the file LICENSE.GPL3-EXCEPT
** included in the packaging of this file. Please review the following
** information to ensure the GNU General Public License requirements will
** be met: https://www.gnu.org/licenses/gpl-3.0.html.
**
** $QT_END_LICENSE$
**
****************************************************************************/

using Microsoft.Win32;
using QtProjectLib;
using System;
using System.IO;
using System.Windows.Forms;

namespace QtVsTools
{
    public class AddQtVersionDialog : Form
    {
        private Label label1;
        private Label label2;
        private Button okButton;
        private Button cancelButton;
        private TextBox nameBox;
        private TextBox pathBox;
        private Button browseButton;
        private bool nameBoxDirty;
        private Timer errorTimer;
        private Label errorLabel;
        private string lastErrorString = string.Empty;

        public AddQtVersionDialog()
        {
            InitializeComponent();

            label1.Text = SR.GetString("AddQtVersionDialog_VersionName");
            label2.Text = SR.GetString("AddQtVersionDialog_Path");
            okButton.Text = SR.GetString(SR.OK);
            cancelButton.Text = SR.GetString(SR.Cancel);

            okButton.Click += okButton_Click;
            nameBox.TextChanged += DataChanged;
            pathBox.TextChanged += DataChanged;
            browseButton.Click += browseButton_Click;

            errorTimer = new Timer();
            errorTimer.Tick += errorTimer_Tick;
            errorTimer.Interval = 3000;

            KeyPress += AddQtVersionDialog_KeyPress;
            Shown += AddQtVersionDialog_Shown;
        }

        private void AddQtVersionDialog_Shown(object sender, EventArgs e)
        {
            Text = SR.GetString("AddQtVersionDialog_Title");
        }

        void errorTimer_Tick(object sender, EventArgs e)
        {
            errorLabel.Text = lastErrorString;
        }

        void AddQtVersionDialog_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27) {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            okButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            nameBox = new System.Windows.Forms.TextBox();
            pathBox = new System.Windows.Forms.TextBox();
            browseButton = new System.Windows.Forms.Button();
            errorLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            //
            // label1
            //
            label1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            label1.Location = new System.Drawing.Point(8, 16);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(88, 24);
            label1.TabIndex = 6;
            label1.Text = "Version name:";
            //
            // label2
            //
            label2.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            label2.Location = new System.Drawing.Point(8, 48);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(88, 23);
            label2.TabIndex = 5;
            label2.Text = "Path:";
            //
            // okButton
            //
            okButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            okButton.Enabled = false;
            okButton.Location = new System.Drawing.Point(143, 104);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.TabIndex = 3;
            okButton.Text = "&OK";
            //
            // cancelButton
            //
            cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(224, 104);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.TabIndex = 4;
            cancelButton.Text = "&Cancel";
            //
            // nameBox
            //
            nameBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            nameBox.Location = new System.Drawing.Point(96, 16);
            nameBox.Name = "nameBox";
            nameBox.Size = new System.Drawing.Size(200, 20);
            nameBox.TabIndex = 0;
            //
            // pathBox
            //
            pathBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            pathBox.Location = new System.Drawing.Point(96, 48);
            pathBox.Name = "pathBox";
            pathBox.Size = new System.Drawing.Size(176, 20);
            pathBox.TabIndex = 1;
            //
            // browseButton
            //
            browseButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            browseButton.Location = new System.Drawing.Point(272, 48);
            browseButton.Name = "browseButton";
            browseButton.Size = new System.Drawing.Size(24, 20);
            browseButton.TabIndex = 2;
            browseButton.Text = "...";
            //
            // errorLabel
            //
            errorLabel.AutoSize = true;
            errorLabel.ForeColor = System.Drawing.Color.Red;
            errorLabel.Location = new System.Drawing.Point(8, 71);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new System.Drawing.Size(0, 13);
            errorLabel.TabIndex = 0;
            //
            // AddQtVersionDialog
            //
            AcceptButton = okButton;
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(304, 134);
            Controls.Add(errorLabel);
            Controls.Add(browseButton);
            Controls.Add(pathBox);
            Controls.Add(nameBox);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            KeyPreview = true;
            Name = "AddQtVersionDialog";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Add New Qt Version";
            ResumeLayout(false);
            PerformLayout();

        }
        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            var vm = QtVersionManager.The();
            VersionInformation versionInfo = null;
            try {
                versionInfo = new VersionInformation(pathBox.Text);
            } catch (Exception exception) {
                if (nameBox.Text == "$(QTDIR)") {
                    var defaultVersion = vm.GetDefaultVersion();
                    versionInfo = vm.GetVersionInfo(defaultVersion);
                } else {
                    Messages.DisplayErrorMessage(exception.Message);
                    return;
                }
            }

            var makefileGenerator = versionInfo.GetQMakeConfEntry("MAKEFILE_GENERATOR");
            if (makefileGenerator != "MSVC.NET" && makefileGenerator != "MSBUILD") {
                MessageBox.Show(SR.GetString("AddQtVersionDialog_IncorrectMakefileGenerator",
                    makefileGenerator), null, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                return;
            }
            vm.SaveVersion(nameBox.Text, pathBox.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DataChanged(object sender, EventArgs e)
        {
            errorLabel.Text = string.Empty;
            errorTimer.Stop();
            errorTimer.Start();
            var name = nameBox.Text.Trim();
            var path = pathBox.Text;

            if (sender == nameBox)
                nameBoxDirty = true;

            if (!nameBoxDirty) {
                string str;
                if (path.EndsWith("\\", StringComparison.Ordinal))
                    str = path.Substring(0, path.Length - 1);
                else
                    str = path;

                var pos = str.LastIndexOf('\\');
                name = str.Substring(pos + 1);
                nameBox.TextChanged -= DataChanged;
                nameBox.Text = name;
                nameBox.TextChanged += DataChanged;
            }

            pathBox.Enabled = name != "$(QTDIR)";
            browseButton.Enabled = pathBox.Enabled;

            if (name.Length < 1 || (name != "$(QTDIR)" && path.Length < 1)) {
                okButton.Enabled = false;
                return;
            }

            if (name != "$(QTDIR)") {
                try {
                    var di = new DirectoryInfo(pathBox.Text);
                    if (!di.Exists) {
                        lastErrorString = string.Empty;
                        okButton.Enabled = false;
                        return;
                    }
                } catch {
                    lastErrorString = SR.GetString("AddQtVersionDialog_InvalidDirectory");
                    okButton.Enabled = false;
                    return;
                }

                var fi = new FileInfo(pathBox.Text + "\\lib\\libqtmain.a");
                if (!fi.Exists)
                    fi = new FileInfo(pathBox.Text + "\\lib\\libqtmaind.a");
                if (fi.Exists) {
                    lastErrorString = SR.GetString("AddQtVersionDialog_MingwQt");
                    okButton.Enabled = false;
                    return;
                }

                fi = new FileInfo(pathBox.Text + "\\bin\\qmake.exe");
                if (!fi.Exists) {
                    lastErrorString = SR.GetString("AddQtVersionDialog_NotFound", fi.FullName);
                    okButton.Enabled = false;
                    return;
                }
            }

            var found = false;
            foreach (var s in QtVersionManager.The().GetVersions()) {
                if (nameBox.Text == s) {
                    lastErrorString = SR.GetString("AddQtVersionDialog_VersionAlreadyPresent");
                    found = true;
                    break;
                }
            }
            okButton.Enabled = !found;
            if (!found)
                lastErrorString = string.Empty;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using (var fd = new FolderBrowserDialog()) {
                fd.Description = SR.GetString("SelectQtPath");
                fd.SelectedPath = RestoreLastSelectedPath();
                if (fd.ShowDialog() == DialogResult.OK) {
                    pathBox.Text = fd.SelectedPath;
                    SaveLastSelectedPath(fd.SelectedPath);
                }
            }
        }

        private static string RestoreLastSelectedPath()
        {
            try {
                var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\"
                    + Resources.registryPackagePath, false);
                if (key != null)
                    return (string) key.GetValue("QtVersionLastSelectedPath");
            } catch {
            }

            return string.Empty;
        }

        private static void SaveLastSelectedPath(string path)
        {
            var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\"
                + Resources.registryPackagePath);
            if (key != null)
                key.SetValue("QtVersionLastSelectedPath", path);
        }
    }
}
