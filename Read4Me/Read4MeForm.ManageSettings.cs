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
        List<ComboBox> comboboxes_voice = new List<ComboBox>();
        List<ComboBox> comboboxes_srate = new List<ComboBox>();
        List<ComboBox> comboboxes_volume = new List<ComboBox>();
        List<CheckBox> checkboxes_ctrl_lang = new List<CheckBox>();
        List<CheckBox> checkboxes_winkey_lang = new List<CheckBox>();
        List<CheckBox> checkboxes_alt_lang = new List<CheckBox>();
        List<TextBox> textboxes_lang = new List<TextBox>();
        private void init_lists()
        {
            comboboxes_voice.Add(cbLang1);
            comboboxes_voice.Add(cbLang2);
            comboboxes_voice.Add(cbLang3);
            comboboxes_voice.Add(cbLang4);
            comboboxes_voice.Add(cbLang5);
            comboboxes_voice.Add(cbLang6);

            checkboxes_ctrl_lang.Add(lCtrllang1);
            checkboxes_ctrl_lang.Add(lCtrllang2);
            checkboxes_ctrl_lang.Add(lCtrllang3);
            checkboxes_ctrl_lang.Add(lCtrllang4);
            checkboxes_ctrl_lang.Add(lCtrllang5);
            checkboxes_ctrl_lang.Add(lCtrllang6);

            checkboxes_winkey_lang.Add(lWinKeylang1);
            checkboxes_winkey_lang.Add(lWinKeylang2);
            checkboxes_winkey_lang.Add(lWinKeylang3);
            checkboxes_winkey_lang.Add(lWinKeylang4);
            checkboxes_winkey_lang.Add(lWinKeylang5);
            checkboxes_winkey_lang.Add(lWinKeylang6);

            checkboxes_alt_lang.Add(lAltlang1);
            checkboxes_alt_lang.Add(lAltlang2);
            checkboxes_alt_lang.Add(lAltlang3);
            checkboxes_alt_lang.Add(lAltlang4);
            checkboxes_alt_lang.Add(lAltlang5);
            checkboxes_alt_lang.Add(lAltlang6);

            textboxes_lang.Add(tbHKlang1);
            textboxes_lang.Add(tbHKlang2);
            textboxes_lang.Add(tbHKlang3);
            textboxes_lang.Add(tbHKlang4);
            textboxes_lang.Add(tbHKlang5);
            textboxes_lang.Add(tbHKlang6);

            comboboxes_srate.Add(cbSRate1);
            comboboxes_srate.Add(cbSRate2);
            comboboxes_srate.Add(cbSRate3);
            comboboxes_srate.Add(cbSRate4);
            comboboxes_srate.Add(cbSRate5);
            comboboxes_srate.Add(cbSRate6);

            comboboxes_volume.Add(cbVolume1);
            comboboxes_volume.Add(cbVolume2);
            comboboxes_volume.Add(cbVolume3);
            comboboxes_volume.Add(cbVolume4);
            comboboxes_volume.Add(cbVolume5);
            comboboxes_volume.Add(cbVolume6);

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
                                SetViewSettings(type, ctrl, winkey, alt, key, "", 0, 0, 0, false, false);
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
                                SetViewSettings(type, ctrl, winkey, alt, key, voice, lang_num, srate, volume, false, false);
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
                                SetViewSettings(type, false, false, false, '\0', voice, 0, srate, volume, false, false);
                                lang_num++;
                                break;

                            case "other_settings":
                                type = reader["type"];
                                SetViewSettings(type, false, false, false, '\0', "", 0, 0, 0, false, false);
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

        private void SetViewSettings(string type, bool ctrl, bool winkey, bool alt, char key, string voice, int lang_num, int srate, int volume, bool silence, bool silence_batch)
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
                    checkboxes_ctrl_lang[lang_num].Checked = ctrl;
                    checkboxes_winkey_lang[lang_num].Checked = winkey;
                    checkboxes_alt_lang[lang_num].Checked = alt;
                    if (key == '\0')
                    {
                        textboxes_lang[lang_num].Text = "";
                    }
                    else
                    {
                        textboxes_lang[lang_num].Text = key.ToString();
                    }
                    comboboxes_voice[lang_num].SelectedIndex = cbLang1.FindStringExact(voice);
                    comboboxes_srate[lang_num].SelectedIndex = cbSRate1.FindStringExact(srate.ToString());
                    comboboxes_volume[lang_num].SelectedIndex = cbVolume1.FindStringExact(volume.ToString());
                    break;

                case "batch_settings":
                    cbVoiceBatch.SelectedIndex = cbVoiceBatch.FindStringExact(voice);
                    cbRateBatch.SelectedIndex = cbRateBatch.FindStringExact(srate.ToString());
                    cbVolumeBatch.SelectedIndex = cbVolumeBatch.FindStringExact(volume.ToString());
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
                    todo_action = () => SpeechStop();
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
            WriteSettings();
        }

        private void WriteSettings()
        {
            StreamWriter file_writer;
            try
            {
                file_writer = new StreamWriter(Path.GetDirectoryName(Application.ExecutablePath) + "\\Read4Me.ini", false, Encoding.UTF8);
            }
            catch
            {
                return;
            }
            file_writer.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            file_writer.Write("<settings>\r\n");
            file_writer.Write("    <hotkey_general type=\"show_hide\" ctrl=\"" + (lCtrl0.Checked ? "1" : "0") + "\" winkey=\"" + (lWinKey0.Checked ? "1" : "0") + "\" alt=\"" + (lAlt0.Checked ? "1" : "0") + "\" key=\"" + tbHK0.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"pause_resume\" ctrl=\"" + (lCtrl1.Checked ? "1" : "0") + "\" winkey=\"" + (lWinKey1.Checked ? "1" : "0") + "\" alt=\"" + (lAlt1.Checked ? "1" : "0") + "\" key=\"" + tbHK1.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"previous_sentence\" ctrl=\"" + (lCtrl2.Checked ? "1" : "0") + "\" winkey=\"" + (lWinKey2.Checked ? "1" : "0") + "\" alt=\"" + (lAlt2.Checked ? "1" : "0") + "\" key=\"" + tbHK2.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"next_sentence\" ctrl=\"" + (lCtrl3.Checked ? "1" : "0") + "\" winkey=\"" + (lWinKey3.Checked ? "1" : "0") + "\" alt=\"" + (lAlt3.Checked ? "1" : "0") + "\" key=\"" + tbHK3.Text + "\"></hotkey_general>\r\n");
            for (int i = 0; i < comboboxes_voice.Count; i++)
            {
                file_writer.Write("    <hotkey_speech type=\"speech\" ctrl=\"" + (checkboxes_ctrl_lang[i].Checked ? "1" : "0") + "\" winkey=\"" + (checkboxes_winkey_lang[i].Checked ? "1" : "0") + "\" alt=\"" + (checkboxes_alt_lang[i].Checked ? "1" : "0") + "\" key=\"" + textboxes_lang[i].Text + "\" voice=\"" + comboboxes_voice[i].SelectedItem + "\" srate=\"" + comboboxes_srate[i].SelectedItem + "\" volume=\"" + comboboxes_volume[i].SelectedItem + "\"></hotkey_speech>\r\n");
            }
            file_writer.Write("    <batch_settings type=\"batch_settings\" voice=\"" + cbVoiceBatch.SelectedItem + "\" srate=\"" + cbRateBatch.SelectedItem + "\" volume=\"" + cbVolumeBatch.SelectedItem + "\"></batch_settings>\r\n");
            file_writer.Write("</settings>\r\n");
            file_writer.Close();
            file_writer.Dispose();
            UnregisterHotkeys();
            ReadSettings();
        }

        private void InitComboboxes()
        {
            for (int i = 0; i < comboboxes_srate.Count; i++)
            {
                for (int j = -10; j <= 10; j++)
                {
                    comboboxes_srate[i].Items.Add(j.ToString());
                    if (i == 0)
                        cbRateBatch.Items.Add(j.ToString());
                }
            }

            for (int i = 0; i < comboboxes_srate.Count; i++)
            {
                for (int j = 0; j <= 100; j++)
                {
                    comboboxes_volume[i].Items.Add(j.ToString());
                    if (i == 0)
                        cbVolumeBatch.Items.Add(j.ToString());
                }
            }
        }
    }
}