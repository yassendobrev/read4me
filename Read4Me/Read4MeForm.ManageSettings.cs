using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Read4Me
{
    partial class Read4MeForm
    {
        List<ComboBox> ComboboxesVoiceCB = new List<ComboBox>();
        List<ComboBox> ComboboxesRateCB = new List<ComboBox>();
        List<ComboBox> ComboboxesVolumeCB = new List<ComboBox>();
        List<CheckBox> CheckboxesCtrlCB = new List<CheckBox>();
        List<CheckBox> CheckboxesWinkeyCB = new List<CheckBox>();
        List<CheckBox> CheckboxesAltCB = new List<CheckBox>();
        List<TextBox> TextboxesHotkeyCB = new List<TextBox>();
        private void InitLists()
        {
            ComboboxesVoiceCB.Add(cbLang1);
            ComboboxesVoiceCB.Add(cbLang2);
            ComboboxesVoiceCB.Add(cbLang3);
            ComboboxesVoiceCB.Add(cbLang4);
            ComboboxesVoiceCB.Add(cbLang5);
            ComboboxesVoiceCB.Add(cbLang6);

            CheckboxesCtrlCB.Add(lCtrllang1);
            CheckboxesCtrlCB.Add(lCtrllang2);
            CheckboxesCtrlCB.Add(lCtrllang3);
            CheckboxesCtrlCB.Add(lCtrllang4);
            CheckboxesCtrlCB.Add(lCtrllang5);
            CheckboxesCtrlCB.Add(lCtrllang6);

            CheckboxesWinkeyCB.Add(lWinKeylang1);
            CheckboxesWinkeyCB.Add(lWinKeylang2);
            CheckboxesWinkeyCB.Add(lWinKeylang3);
            CheckboxesWinkeyCB.Add(lWinKeylang4);
            CheckboxesWinkeyCB.Add(lWinKeylang5);
            CheckboxesWinkeyCB.Add(lWinKeylang6);

            CheckboxesAltCB.Add(lAltlang1);
            CheckboxesAltCB.Add(lAltlang2);
            CheckboxesAltCB.Add(lAltlang3);
            CheckboxesAltCB.Add(lAltlang4);
            CheckboxesAltCB.Add(lAltlang5);
            CheckboxesAltCB.Add(lAltlang6);

            TextboxesHotkeyCB.Add(tbHKlang1);
            TextboxesHotkeyCB.Add(tbHKlang2);
            TextboxesHotkeyCB.Add(tbHKlang3);
            TextboxesHotkeyCB.Add(tbHKlang4);
            TextboxesHotkeyCB.Add(tbHKlang5);
            TextboxesHotkeyCB.Add(tbHKlang6);

            ComboboxesRateCB.Add(cbSRate1);
            ComboboxesRateCB.Add(cbSRate2);
            ComboboxesRateCB.Add(cbSRate3);
            ComboboxesRateCB.Add(cbSRate4);
            ComboboxesRateCB.Add(cbSRate5);
            ComboboxesRateCB.Add(cbSRate6);

            ComboboxesVolumeCB.Add(cbVolume1);
            ComboboxesVolumeCB.Add(cbVolume2);
            ComboboxesVolumeCB.Add(cbVolume3);
            ComboboxesVolumeCB.Add(cbVolume4);
            ComboboxesVolumeCB.Add(cbVolume5);
            ComboboxesVolumeCB.Add(cbVolume6);

            InitComboboxes();
        }

        private void ReadSettings()
        {
            string type;
            bool ctrl;
            bool winkey;
            bool alt;
            string voice;
            char key;
            int lang_num = 0;
            int srate = 0;
            int volume = 0;
            bool min_to_tray;
            bool read_selected_text;
            XmlReader reader;

            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\Read4Me.ini"))
            {
                reader = XmlReader.Create(Path.GetDirectoryName(Application.ExecutablePath) + "\\Read4Me.ini");
                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case "hotkey_general":
                                type = reader["type"];
                                ctrl = (reader["ctrl"] == "1") ? true : false;
                                winkey = (reader["winkey"] == "1") ? true : false;
                                alt = (reader["alt"] == "1") ? true : false;
                                if (reader["key"] != "")
                                {
                                    key = reader["key"][0];
                                }
                                else
                                {
                                    key = '\0';
                                }
                                SetViewSettings(type, ctrl, winkey, alt, key, "", 0, 0, 0, false, false, false, false);
                                RegisterHotkey(type, ctrl, winkey, alt, key, "", 0, 0);
                                break;

                            case "hotkey_speech":
                                type = reader["type"];
                                ctrl = (reader["ctrl"] == "1") ? true : false;
                                winkey = (reader["winkey"] == "1") ? true : false;
                                alt = (reader["alt"] == "1") ? true : false;
                                if (reader["key"] != "")
                                {
                                    key = reader["key"][0];
                                }
                                else
                                {
                                    key = '\0';
                                }
                                voice = reader["voice"];
                                if (reader["srate"] != "")
                                {
                                    srate = Int16.Parse(reader["srate"]);
                                }
                                else
                                {
                                    srate = int.MinValue;
                                }
                                if (reader["volume"] != "")
                                {
                                    volume = Int16.Parse(reader["volume"]);
                                }
                                else
                                {
                                    volume = int.MinValue;
                                }
                                SetViewSettings(type, ctrl, winkey, alt, key, voice, lang_num, srate, volume, false, false, false, false);
                                RegisterHotkey(type, ctrl, winkey, alt, key, voice, srate, volume);
                                lang_num++;
                                break;

                            case "batch_settings":
                                type = reader["type"];
                                voice = reader["voice"];
                                if (reader["srate"] != "")
                                {
                                    srate = Int16.Parse(reader["srate"]);
                                }
                                else
                                {
                                    srate = int.MinValue;
                                }
                                if (reader["volume"] != "")
                                {
                                    volume = Int16.Parse(reader["volume"]);
                                }
                                else
                                {
                                    volume = int.MinValue;
                                }
                                SetViewSettings(type, false, false, false, '\0', voice, 0, srate, volume, false, false, false, false);
                                lang_num++;
                                break;

                            case "win_bevaviour_settings":
                                type = reader["type"];
                                min_to_tray = (reader["min_to_tray"] == "1") ? true : false;
                                read_selected_text = (reader["read_selected_text"] == "1") ? true : false;
                                SetViewSettings(type, false, false, false, '\0', "", 0, 0, 0, false, false, min_to_tray, read_selected_text);
                                break;

                            case "other_settings":
                                type = reader["type"];
                                SetViewSettings(type, false, false, false, '\0', "", 0, 0, 0, false, false, false, false);
                                break;

                            default:
                                break;
                        }
                    }
                }
                reader.Close();
            }
            else
            {
                sWorkingStatus.Text = "No settings file found!";
            }
        }

        private void SetViewSettings(string type, bool ctrl, bool winkey, bool alt, char key, string voice, int lang_num, int srate, int volume, bool silence, bool silence_batch, bool min_to_tray, bool read_selected_text)
        {
            switch (type)
            {
                case "show_hide":
                    lCtrl0.Checked = ctrl;
                    lWinKey0.Checked = winkey;
                    lAlt0.Checked = alt;
                    if (key == '\0')
                    {
                        tbHK0.Text = "";
                    }
                    else
                    {
                        tbHK0.Text = key.ToString();
                    }
                    break;

                case "pause_resume":
                    lCtrl1.Checked = ctrl;
                    lWinKey1.Checked = winkey;
                    lAlt1.Checked = alt;
                    if (key == '\0')
                    {
                        tbHK1.Text = "";
                    }
                    else
                    {
                        tbHK1.Text = key.ToString();
                    }
                    break;

                case "previous_sentence":
                    lCtrl2.Checked = ctrl;
                    lWinKey2.Checked = winkey;
                    lAlt2.Checked = alt;
                    if (key == '\0')
                    {
                        tbHK2.Text = "";
                    }
                    else
                    {
                        tbHK2.Text = key.ToString();
                    }
                    break;

                case "next_sentence":
                    lCtrl3.Checked = ctrl;
                    lWinKey3.Checked = winkey;
                    lAlt3.Checked = alt;
                    if (key == '\0')
                    {
                        tbHK3.Text = "";
                    }
                    else
                    {
                        tbHK3.Text = key.ToString();
                    }
                    break;

                case "speech":
                    CheckboxesCtrlCB[lang_num].Checked = ctrl;
                    CheckboxesWinkeyCB[lang_num].Checked = winkey;
                    CheckboxesAltCB[lang_num].Checked = alt;
                    if (key == '\0')
                    {
                        TextboxesHotkeyCB[lang_num].Text = "";
                    }
                    else
                    {
                        TextboxesHotkeyCB[lang_num].Text = key.ToString();
                    }
                    ComboboxesVoiceCB[lang_num].SelectedIndex = cbLang1.FindStringExact(voice);
                    ComboboxesRateCB[lang_num].SelectedIndex = cbSRate1.FindStringExact(srate.ToString());
                    ComboboxesVolumeCB[lang_num].SelectedIndex = cbVolume1.FindStringExact(volume.ToString());
                    break;

                case "batch_settings":
                    cbVoiceBatch.SelectedIndex = cbVoiceBatch.FindStringExact(voice);
                    cbRateBatch.SelectedIndex = cbRateBatch.FindStringExact(srate.ToString());
                    cbVolumeBatch.SelectedIndex = cbVolumeBatch.FindStringExact(volume.ToString());
                    break;

                case "min_to_tray":
                    cbMinToTray.Checked = min_to_tray;
                    MinToTray = min_to_tray;
                    break;

                case "read_selected_text":
                    cbReadSelectedText.Checked = read_selected_text;
                    ReadSelectedText = read_selected_text;
                    break;

                case "other_settings":
                    break;

                default:
                    break;
            }
        }

        List<Hotkey> hotkeys = new List<Hotkey>();
        List<Keys> keycodes = new List<Keys>();
        List<Action> actions = new List<Action>();
        private bool RegisterHotkey(string type, bool ctrl, bool winkey, bool alt, char key, string voice, int srate, int volume)
        {
            // register hotkeys
            // http://bloggablea.wordpress.com/2007/05/01/global-hotkeys-with-net/

            hotkeys.Add(new Hotkey());
            int current_hotkey = hotkeys.Count - 1;
            hotkeys[current_hotkey].Control = ctrl;
            hotkeys[current_hotkey].Windows = winkey;
            hotkeys[current_hotkey].Alt = alt;
            hotkeys[current_hotkey].KeyCode = (Keys)(byte)char.ToUpper(key);
            int delegate_num = current_hotkey;

            Action todo_action = delegate { return; };
            switch (type)
            {
                case "show_hide":
                    todo_action = () => ToggleForm();
                    break;

                case "pause_resume":
                    todo_action = () => SpeechPause();
                    break;

                case "previous_sentence":
                    todo_action = () => SpeechSkip(-1);
                    break;

                case "next_sentence":
                    todo_action = () => SpeechSkip(1);
                    break;

                case "speech":
                    todo_action = () => ReadClipboard(voice, srate, volume);
                    break;
            }
            hotkeys[current_hotkey].Pressed += delegate { todo_action(); };

            if (!hotkeys[current_hotkey].GetCanRegister(this))
            {
                return false;
            }
            else
            {
                hotkeys[current_hotkey].Register(this);
                return true;
            }
        }

        private void UnregisterHotkeys()
        {
            for (int i = 0; i < hotkeys.Count; i++)
            {
                if (hotkeys[i].Registered)
                {
                    hotkeys[i].Unregister();
                }
            }
            hotkeys.Clear();
        }

        private void bApplyClick(object sender, EventArgs e)
        {
            WriteSettingsIni();
        }

        private void WriteSettingsIni()
        {
            StreamWriter FileWriter;
            try
            {
                FileWriter = new StreamWriter(Path.GetDirectoryName(Application.ExecutablePath) + "\\Read4Me.ini", false, Encoding.UTF8);
            }
            catch
            {
                return;
            }
            FileWriter.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            FileWriter.Write("<settings>\r\n");
            FileWriter.Write("    <hotkey_general type=\"show_hide\" ctrl=\"" + (lCtrl0.Checked ? "1" : "0") + "\" winkey=\"" + (lWinKey0.Checked ? "1" : "0") + "\" alt=\"" + (lAlt0.Checked ? "1" : "0") + "\" key=\"" + tbHK0.Text + "\"></hotkey_general>\r\n");
            FileWriter.Write("    <hotkey_general type=\"pause_resume\" ctrl=\"" + (lCtrl1.Checked ? "1" : "0") + "\" winkey=\"" + (lWinKey1.Checked ? "1" : "0") + "\" alt=\"" + (lAlt1.Checked ? "1" : "0") + "\" key=\"" + tbHK1.Text + "\"></hotkey_general>\r\n");
            FileWriter.Write("    <hotkey_general type=\"previous_sentence\" ctrl=\"" + (lCtrl2.Checked ? "1" : "0") + "\" winkey=\"" + (lWinKey2.Checked ? "1" : "0") + "\" alt=\"" + (lAlt2.Checked ? "1" : "0") + "\" key=\"" + tbHK2.Text + "\"></hotkey_general>\r\n");
            FileWriter.Write("    <hotkey_general type=\"next_sentence\" ctrl=\"" + (lCtrl3.Checked ? "1" : "0") + "\" winkey=\"" + (lWinKey3.Checked ? "1" : "0") + "\" alt=\"" + (lAlt3.Checked ? "1" : "0") + "\" key=\"" + tbHK3.Text + "\"></hotkey_general>\r\n");
            for (int i = 0; i < ComboboxesVoiceCB.Count; i++)
            {
                FileWriter.Write("    <hotkey_speech type=\"speech\" ctrl=\"" + (CheckboxesCtrlCB[i].Checked ? "1" : "0") + "\" winkey=\"" + (CheckboxesWinkeyCB[i].Checked ? "1" : "0") + "\" alt=\"" + (CheckboxesAltCB[i].Checked ? "1" : "0") + "\" key=\"" + TextboxesHotkeyCB[i].Text + "\" voice=\"" + ComboboxesVoiceCB[i].SelectedItem + "\" srate=\"" + ComboboxesRateCB[i].SelectedItem + "\" volume=\"" + ComboboxesVolumeCB[i].SelectedItem + "\"></hotkey_speech>\r\n");
            }
            FileWriter.Write("    <batch_settings type=\"batch_settings\" voice=\"" + cbVoiceBatch.SelectedItem + "\" srate=\"" + cbRateBatch.SelectedItem + "\" volume=\"" + cbVolumeBatch.SelectedItem + "\"></batch_settings>\r\n");
            FileWriter.Write("    <win_bevaviour_settings type=\"min_to_tray\" min_to_tray=\"" + (cbMinToTray.Checked ? "1" : "0") + "\"></win_bevaviour_settings>\r\n");
            FileWriter.Write("    <win_bevaviour_settings type=\"read_selected_text\" read_selected_text=\"" + (cbReadSelectedText.Checked ? "1" : "0") + "\"></win_bevaviour_settings>\r\n");
            FileWriter.Write("</settings>\r\n");
            FileWriter.Close();
            FileWriter.Dispose();
            UnregisterHotkeys();
            ReadSettings();
        }

        private void InitComboboxes()
        {
            for (int i = 0; i < ComboboxesRateCB.Count; i++)
            {
                for (int j = -10; j <= 10; j++)
                {
                    ComboboxesRateCB[i].Items.Add(j.ToString());
                    if (i == 0)
                        cbRateBatch.Items.Add(j.ToString());
                }
                ComboboxesRateCB[i].SelectedIndex = ComboboxesRateCB[i].FindStringExact(SpeechRateGlobal.ToString());
            }

            for (int i = 0; i < ComboboxesVolumeCB.Count; i++)
            {
                for (int j = 0; j <= 100; j=j+5)
                {
                    ComboboxesVolumeCB[i].Items.Add(j.ToString());
                    if (i == 0)
                        cbVolumeBatch.Items.Add(j.ToString());
                }
                ComboboxesVolumeCB[i].SelectedIndex = ComboboxesVolumeCB[i].FindStringExact(VolumeGlobal.ToString());
            }
        }

        void tbKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LWin || e.KeyCode == Keys.Menu)
            {
                // monitor modifiers "manually", since e.Modifiers does not contain winkey
                pKeys.Add(e.KeyCode);
            }

            string tbHotkey = "";
            if (e.KeyCode != Keys.ShiftKey && e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.LWin && e.KeyCode != Keys.Menu)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    foreach (Keys key in pKeys.PressedKeysList) // Loop through List with foreach
                    {
                        string KeyString = "";
                        if (key == Keys.ShiftKey)
                        {
                            KeyString = "Shift";
                        }
                        if (key == Keys.ControlKey)
                        {
                            KeyString = "Ctrl";
                        }
                        if (key == Keys.LWin)
                        {
                            KeyString = "WinKey";
                        }
                        if (key == Keys.Menu)
                        {
                            KeyString = "Alt";
                        }
                        tbHotkey += KeyString + "+";
                    }

                    if (char.IsLetter((char)e.KeyData) || char.IsDigit((char)e.KeyData))
                    {
                        tbKey.Text = tbHotkey + char.ConvertFromUtf32(e.KeyValue);
                    }
                });
            }
        }

        void tbKey_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LWin || e.KeyCode == Keys.Menu)
            {
                // monitor modifiers "manually", since e.Modifiers does not contain winkey
                pKeys.Remove(e.KeyCode);
            }
        }
    }

    class PressedKeys
    {
        public List<Keys> PressedKeysList;

        public PressedKeys()
        {
            PressedKeysList = new List<Keys>();
        }

        public void Add(Keys key)
        {
            if (!PressedKeysList.Contains(key))
            {
                PressedKeysList.Add(key);
            }
        }

        public void Remove(Keys key)
        {
            PressedKeysList.Remove(key);
        }
    }
}