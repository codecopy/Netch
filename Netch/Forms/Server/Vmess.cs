﻿using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Netch.Utils;

namespace Netch.Forms.Server
{
    public partial class VMess : Form
    {
        public int Index;

        public VMess(int index = -1)
        {
            InitializeComponent();

            Index = index;
        }

        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var cbx = sender as ComboBox;
            if (cbx != null)
            {
                e.DrawBackground();

                if (e.Index >= 0)
                {
                    var sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    var brush = new SolidBrush(cbx.ForeColor);

                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText as SolidBrush;
                    }

                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }

        private void VMess_Load(object sender, EventArgs e)
        {
            ConfigurationGroupBox.Text = i18N.Translate(ConfigurationGroupBox.Text);
            RemarkLabel.Text = i18N.Translate(RemarkLabel.Text);
            AddressLabel.Text = i18N.Translate(AddressLabel.Text);
            UserIDLabel.Text = i18N.Translate(UserIDLabel.Text);
            AlterIDLabel.Text = i18N.Translate(AlterIDLabel.Text);
            EncryptMethodLabel.Text = i18N.Translate(EncryptMethodLabel.Text);
            TransferProtocolLabel.Text = i18N.Translate(TransferProtocolLabel.Text);
            FakeTypeLabel.Text = i18N.Translate(FakeTypeLabel.Text);
            HostLabel.Text = i18N.Translate(HostLabel.Text);
            PathLabel.Text = i18N.Translate(PathLabel.Text);
            QUICSecurityLabel.Text = i18N.Translate(QUICSecurityLabel.Text);
            QUICSecretLabel.Text = i18N.Translate(QUICSecretLabel.Text);
            TLSSecureCheckBox.Text = i18N.Translate(TLSSecureCheckBox.Text);
            UseMuxCheckBox.Text = i18N.Translate(UseMuxCheckBox.Text);
            ControlButton.Text = i18N.Translate(ControlButton.Text);

            foreach (var encrypt in Global.EncryptMethods.VMess)
            {
                EncryptMethodComboBox.Items.Add(encrypt);
            }

            foreach (var protocol in Global.TransferProtocols)
            {
                TransferProtocolComboBox.Items.Add(protocol);
            }

            foreach (var fake in Global.FakeTypes)
            {
                FakeTypeComboBox.Items.Add(fake);
            }

            foreach (var security in Global.EncryptMethods.VMessQUIC)
            {
                QUICSecurityComboBox.Items.Add(security);
            }

            if (Index != -1)
            {
                RemarkTextBox.Text = Global.Settings.Server[Index].Remark;
                AddressTextBox.Text = Global.Settings.Server[Index].Hostname;
                PortTextBox.Text = Global.Settings.Server[Index].Port.ToString();
                UserIDTextBox.Text = Global.Settings.Server[Index].UserID;
                AlterIDTextBox.Text = Global.Settings.Server[Index].AlterID.ToString();
                EncryptMethodComboBox.SelectedIndex = Global.EncryptMethods.VMess.IndexOf(Global.Settings.Server[Index].EncryptMethod);
                TransferProtocolComboBox.SelectedIndex = Global.TransferProtocols.IndexOf(Global.Settings.Server[Index].TransferProtocol);
                FakeTypeComboBox.SelectedIndex = Global.FakeTypes.IndexOf(Global.Settings.Server[Index].FakeType);
                HostTextBox.Text = Global.Settings.Server[Index].Host;
                PathTextBox.Text = Global.Settings.Server[Index].Path;
                QUICSecurityComboBox.SelectedIndex = Global.EncryptMethods.VMessQUIC.IndexOf(Global.Settings.Server[Index].QUICSecure);
                QUICSecretTextBox.Text = Global.Settings.Server[Index].QUICSecret;
                TLSSecureCheckBox.Checked = Global.Settings.Server[Index].TLSSecure;
                UseMuxCheckBox.Checked = Global.Settings.Server[Index].UseMux;
            }
            else
            {
                EncryptMethodComboBox.SelectedIndex = 0;
                TransferProtocolComboBox.SelectedIndex = 0;
                FakeTypeComboBox.SelectedIndex = 0;
                QUICSecurityComboBox.SelectedIndex = 0;
            }
        }

        private void VMess_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.MainForm.Show();
        }

        private void ControlButton_Click(object sender, EventArgs e)
        {
            if (!Regex.Match(PortTextBox.Text, "^[0-9]+$").Success)
            {
                return;
            }
            if (AlterIDTextBox.Text == "")
            {
                MessageBoxX.Show(i18N.Translate("Please fill in alterID"));
                return;
            }
            if (Index == -1)
            {
                Global.Settings.Server.Add(new Models.Server
                {
                    Remark = RemarkTextBox.Text,
                    Type = "VMess",
                    Hostname = AddressTextBox.Text,
                    Port = int.Parse(PortTextBox.Text),
                    UserID = UserIDTextBox.Text,
                    AlterID = int.Parse(AlterIDTextBox.Text),
                    EncryptMethod = EncryptMethodComboBox.Text,
                    TransferProtocol = TransferProtocolComboBox.Text,
                    FakeType = FakeTypeComboBox.Text,
                    Host = HostTextBox.Text,
                    Path = PathTextBox.Text,
                    QUICSecure = QUICSecurityComboBox.Text,
                    QUICSecret = QUICSecretTextBox.Text,
                    TLSSecure = TLSSecureCheckBox.Checked,
                    UseMux = UseMuxCheckBox.Checked
                });
            }
            else
            {
                Global.Settings.Server[Index] = new Models.Server
                {
                    Remark = RemarkTextBox.Text,
                    Type = "VMess",
                    Hostname = AddressTextBox.Text,
                    Port = int.Parse(PortTextBox.Text),
                    UserID = UserIDTextBox.Text,
                    AlterID = int.Parse(AlterIDTextBox.Text),
                    EncryptMethod = EncryptMethodComboBox.Text,
                    TransferProtocol = TransferProtocolComboBox.Text,
                    FakeType = FakeTypeComboBox.Text,
                    Host = HostTextBox.Text,
                    Path = PathTextBox.Text,
                    QUICSecure = QUICSecurityComboBox.Text,
                    QUICSecret = QUICSecretTextBox.Text,
                    TLSSecure = TLSSecureCheckBox.Checked,
                    UseMux = UseMuxCheckBox.Checked,
                    Country = null
                };
            }

            Configuration.Save();
            MessageBoxX.Show(i18N.Translate("Saved"));
            Global.MainForm.InitServer();
            Close();
        }
    }
}
