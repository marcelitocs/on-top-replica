using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Properties;
using IWshRuntimeLibrary;

namespace OnTopReplica.SidePanels
{
    public partial class ManageScenariosPanel : SidePanel
    {
        public ManageScenariosPanel()
        {
            InitializeComponent();
        }

        public override void SetHost(MainForm host)
        {
            base.SetHost(host);
            PopulateScenarios();
        }

        private void PopulateScenarios()
        {
            listViewScenarios.Items.Clear();

            if (Settings.Default.SavedScenarios != null)
            {
                foreach (var scenario in Settings.Default.SavedScenarios)
                {
                    var item = new ListViewItem(scenario.Name);
                    item.Tag = scenario;
                    listViewScenarios.Items.Add(item);
                }
            }
        }

        private void btnCreateShortcut_Click(object sender, EventArgs e)
        {
            if (listViewScenarios.SelectedItems.Count == 0)
                return;

            var item = listViewScenarios.SelectedItems[0];
            var scenario = item.Tag as StoredScenario;
            if (scenario == null)
                return;

            var options = scenario.ToOptions();
            var arguments = StartupOptions.Factory.ToCommandLine(options);
            var shortcutPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                scenario.Name + ".lnk");

            try
            {
                CreateShortcut(shortcutPath, Application.ExecutablePath, arguments);
                MessageBox.Show("Shortcut created on your desktop.", "OnTopReplica", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create shortcut: " + ex.Message, "OnTopReplica", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewScenarios.SelectedItems.Count == 0)
                return;

            var item = listViewScenarios.SelectedItems[0];
            var scenario = item.Tag as StoredScenario;
            if (scenario == null)
                return;

            if (MessageBox.Show("Are you sure you want to delete the scenario '" + scenario.Name + "'?", "OnTopReplica", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Settings.Default.SavedScenarios.Remove(scenario);
                Settings.Default.Save();
                PopulateScenarios();
            }
        }

        private void CreateShortcut(string shortcutPath, string targetPath, string arguments)
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            shortcut.TargetPath = targetPath;
            shortcut.Arguments = arguments;
            shortcut.Save();
        }
    }
}
