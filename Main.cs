using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetFwTypeLib;

namespace Lolchat_offline
{
    public partial class Main : Form
    {
        public Main()
        {
            if (GetFirewallRuleByName("lolchat") == null)
            {
                CreateRuleLolchat();
            }

            Thread.Sleep(2000);
            InitializeComponent();

            try
            {
                INetFwRule lolchatRule = GetFirewallRuleByName("lolchat");

                btnActive.Text = lolchatRule.Enabled ? "Desactivar" : "Activar";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se debe ejecutar como administrador.");
            }
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            try
            {
                INetFwRule lolchatRule = GetFirewallRuleByName("lolchat");

                lolchatRule.Enabled = !lolchatRule.Enabled;
                btnActive.Text = lolchatRule.Enabled ? "Desactivar" : "Activar";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se debe ejecutar como administrador.");
            }

        }
 

        private INetFwRule GetFirewallRuleByName(string ruleName)
        {
            try
            {
                // Crear una instancia de la política del firewall
                Type netFwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(netFwPolicy2Type);

                // Obtener la colección de reglas
                INetFwRules fwRules = fwPolicy2.Rules;

                // Iterar sobre las reglas para encontrar la que coincide
                foreach (INetFwRule rule in fwRules)
                {
                    if (rule.Name == ruleName)
                    {
                        return rule; // Retorna la regla si la encuentra
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se debe ejecutar como administrador.");
            }
            return null; // Retorna null si no se encontró la regla
        }

        private void CreateRuleLolchat()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;

            string comando = "netsh advfirewall firewall add rule name=\"lolchat\" dir=out remoteport=5223 protocol=TCP action=block enable=no";
            process.StartInfo.Arguments =  $"/C {comando}";

            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear regla.");
            }
        }
    }
}
