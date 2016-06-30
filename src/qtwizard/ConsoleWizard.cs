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

using EnvDTE;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using QtProjectLib;
using System.Collections.Generic;
using System.IO;

namespace QtProjectWizard
{
    public class ConsoleWizard : IWizard
    {
        public void RunStarted(object automation, Dictionary<string, string> replacements,
            WizardRunKind runKind, object[] customParams)
        {
            var serviceProvider = new ServiceProvider(automation as IServiceProvider);
            var iVsUIShell = serviceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;

            iVsUIShell.EnableModeless(0);

            try {
                System.IntPtr hwnd;
                iVsUIShell.GetDialogOwnerHwnd(out hwnd);

                try {
                    var wizard = new WizardWindow(new List<WizardPage>() {
                        new IntroPage {
                            Data = data,
                            Header = @"Welcome to the Qt Console Application Wizard",
                            Message = @"This wizard generates a Qt console application "
                                + @"project. The application derives from QCoreApplication "
                                + @"and does not present a GUI." + System.Environment.NewLine
                                + System.Environment.NewLine + "To continue, click Next.",
                            PreviousButtonEnabled = false,
                            NextButtonEnabled = true,
                            FinishButtonEnabled = false,
                            CancelButtonEnabled = true
                        },
                        new ModulePage {
                            Data = data,
                            Header = @"Welcome to the Qt Console Application Wizard",
                            Message = @"Select the modules you want to include in your project. The "
                                + @"recommended modules for this project are selected by default.",
                            PreviousButtonEnabled = true,
                            NextButtonEnabled = false,
                            FinishButtonEnabled = new QtProjectEngine().IsModuleInstalled(@"QtCore"),
                            CancelButtonEnabled = true
                        }
                    })
                    {
                        Title = @"Qt Console Application Wizard"
                    };
                    WindowHelper.ShowModal(wizard, hwnd);
                    if (!wizard.DialogResult.HasValue || !wizard.DialogResult.Value)
                        throw new System.Exception("Unexpected wizard return value.");
                } catch (QtVSException exception) {
                    Messages.DisplayErrorMessage(exception.Message);
                    throw; // re-throw, but keep the original exception stack intact
                }

                var version = (automation as DTE).Version;
                replacements["$ToolsVersion$"] = version;

                var vm = QtVersionManager.The();
                var vi = new VersionInformation(vm.GetInstallPath(vm.GetDefaultVersion()));
                replacements["$Platform$"] = vi.GetVSPlatformName();

                replacements["$Keyword$"] = Resources.qtProjectKeyword;
                replacements["$ProjectGuid$"] = @"{B12702AD-ABFB-343A-A199-8E24837244A3}";
                replacements["$PlatformToolset$"] = version.Replace(".", "");
            } catch {
                try {
                    Directory.Delete(replacements["$destinationdirectory$"]);
                    Directory.Delete(replacements["$solutiondirectory$"]);
                } catch { }

                iVsUIShell.EnableModeless(1);
                throw new WizardBackoutException();
            }

            iVsUIShell.EnableModeless(1);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void ProjectFinishedGenerating(Project project)
        {
            var qtProject = QtProject.Create(project);

            QtVSIPSettings.SaveUicDirectory(project, null);
            QtVSIPSettings.SaveMocDirectory(project, null);
            QtVSIPSettings.SaveMocOptions(project, null);
            QtVSIPSettings.SaveRccDirectory(project, null);
            QtVSIPSettings.SaveLUpdateOnBuild(project);
            QtVSIPSettings.SaveLUpdateOptions(project, null);
            QtVSIPSettings.SaveLReleaseOptions(project, null);

            var vm = QtVersionManager.The();
            var qtVersion = vm.GetDefaultVersion();
            var vi = new VersionInformation(vm.GetInstallPath(qtVersion));
            if (vi.GetVSPlatformName() != "Win32")
                qtProject.SelectSolutionPlatform(vi.GetVSPlatformName());
            vm.SaveProjectQtVersion(project, qtVersion);

            qtProject.MarkAsQtProject("v1.0");
            qtProject.AddDirectories();

            var type = TemplateType.Application | TemplateType.ConsoleSystem;
            qtProject.WriteProjectBasicConfigurations(type, false);

            qtProject.AddModule(QtModule.Main);
            foreach (var module in data.Modules)
                qtProject.AddModule(QtModules.Instance.ModuleIdByName(module));
            qtProject.SetQtEnvironment(qtVersion);
            qtProject.Finish(); // Collapses all project nodes.
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            if (projectItem.FileCount >= 1) {
                var qtProject = QtProject.Create(projectItem.ContainingProject);
                for (short i = 0; i < projectItem.FileCount; ++i)
                    qtProject.AdjustWhitespace(projectItem.FileNames[i]);
            }
        }

        public void RunFinished()
        {
        }

        private readonly WizardData data = new WizardData()
        {
            DefaultModules = new List<string>() { @"QtCore" }
        };
    }
}
