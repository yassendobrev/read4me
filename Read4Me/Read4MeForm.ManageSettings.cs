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
        List<ComboBox> comboboxes_lang = new List<ComboBox>();
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

            comboboxes_lang.Add(cbLangid1);
            comboboxes_lang.Add(cbLangid2);
            comboboxes_lang.Add(cbLangid3);
            comboboxes_lang.Add(cbLangid4);
            comboboxes_lang.Add(cbLangid5);
            comboboxes_lang.Add(cbLangid6);

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
            string lang;
            string langid;
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
                                SetViewSettings(type, ctrl, winkey, alt, key, "", "", 0, 0, 0);
                                RegisterHotkey(type, ctrl, winkey, alt, key, "", "", 0, 0);
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
                                lang = reader["lang"];
                                langid = (string)langids[lang];
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
                                SetViewSettings(type, ctrl, winkey, alt, key, lang, voice, lang_num, srate, volume);
                                RegisterHotkey(type, ctrl, winkey, alt, key, langid, voice, srate, volume);
                                lang_num++;
                                break;

                            case "batch_settings":
                                type = reader["type"];
                                lang = reader["lang"];
                                langid = (string)langids[lang];
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
                                SetViewSettings(type, false, false, false, '\0', lang, voice, 0, srate, volume);
                                lang_num++;
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

        private void SetViewSettings(string type, bool ctrl, bool winkey, bool alt, char key, string lang, string voice, int lang_num, int srate, int volume)
        {
            switch (type)
            {
                case "show_hide":
                    lCtrl0.Checked = ctrl;
                    lWinKey0.Checked = winkey;
                    lAlt0.Checked = alt;
                    tbHK0.Text = key.ToString();
                    break;

                case "pause_resume":
                    lCtrl1.Checked = ctrl;
                    lWinKey1.Checked = winkey;
                    lAlt1.Checked = alt;
                    tbHK1.Text = key.ToString();
                    break;

                case "previous_sentence":
                    lCtrl2.Checked = ctrl;
                    lWinKey2.Checked = winkey;
                    lAlt2.Checked = alt;
                    tbHK2.Text = key.ToString();
                    break;

                case "next_sentence":
                    lCtrl3.Checked = ctrl;
                    lWinKey3.Checked = winkey;
                    lAlt3.Checked = alt;
                    tbHK3.Text = key.ToString();
                    break;

                case "speech":
                    checkboxes_ctrl_lang[lang_num].Checked = ctrl;
                    checkboxes_winkey_lang[lang_num].Checked = winkey;
                    checkboxes_alt_lang[lang_num].Checked = alt;
                    textboxes_lang[lang_num].Text = key.ToString();
                    comboboxes_voice[lang_num].SelectedIndex = cbLang1.FindStringExact(voice);
                    comboboxes_lang[lang_num].SelectedIndex = cbLangid1.FindStringExact(lang);
                    comboboxes_srate[lang_num].SelectedIndex = cbSRate1.FindStringExact(srate.ToString());
                    comboboxes_volume[lang_num].SelectedIndex = cbVolume1.FindStringExact(volume.ToString());
                    break;

                case "batch_settings":
                    cbVoiceBatch.SelectedIndex = cbVoiceBatch.FindStringExact(voice);
                    cbLanguageBatch.SelectedIndex = cbLanguageBatch.FindStringExact(lang);
                    cbRateBatch.SelectedIndex = cbRateBatch.FindStringExact(srate.ToString());
                    cbVolumeBatch.SelectedIndex = cbVolumeBatch.FindStringExact(volume.ToString());
                    break;

                default:
                    break;
            }
        }

        List<Hotkey> hotkeys = new List<Hotkey>();
        List<Keys> keycodes = new List<Keys>();
        List<Action> actions = new List<Action>();
        private bool RegisterHotkey(string type, bool ctrl, bool winkey, bool alt, char key, string langid, string voice, int srate, int volume)
        {
            // register hotkeys
            // http://bloggablea.wordpress.com/2007/05/01/global-hotkeys-with-net/

            // langid: http://www.usb.org/developers/docs/USB_LANGIDs.pdf
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
                    todo_action = () => ReadClipboard(langid, voice, srate, volume);
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
            file_writer.Write("    <hotkey_general type=\"show_hide\" ctrl=\"" + ((lCtrl0.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey0.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt0.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK0.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"pause_resume\" ctrl=\"" + ((lCtrl1.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey1.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt1.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK1.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"previous_sentence\" ctrl=\"" + ((lCtrl2.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey2.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt2.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK2.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"next_sentence\" ctrl=\"" + ((lCtrl3.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey3.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt3.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK3.Text + "\"></hotkey_general>\r\n");
            for (int i = 0; i < comboboxes_voice.Count; i++)
            {
                // if ((textboxes_lang[i].Text != "") && ((string)comboboxes_lang[i].SelectedItem != "" && ((string)comboboxes_voice[i].SelectedItem != "")))
                // {
                    file_writer.Write("    <hotkey_speech type=\"speech\" ctrl=\"" + ((checkboxes_ctrl_lang[i].Checked == true) ? "1" : "0") + "\" winkey=\"" + ((checkboxes_winkey_lang[i].Checked == true) ? "1" : "0") + "\" alt=\"" + ((checkboxes_alt_lang[i].Checked == true) ? "1" : "0") + "\" key=\"" + textboxes_lang[i].Text + "\" lang=\"" + comboboxes_lang[i].SelectedItem + "\" voice=\"" + comboboxes_voice[i].SelectedItem + "\" srate=\"" + comboboxes_srate[i].SelectedItem + "\" volume=\"" + comboboxes_volume[i].SelectedItem + "\"></hotkey_speech>\r\n");
                // }
            }
            file_writer.Write("    <batch_settings type=\"batch_settings\" lang=\"" + cbLanguageBatch.SelectedItem + "\" voice=\"" + cbVoiceBatch.SelectedItem + "\" srate=\"" + cbRateBatch.SelectedItem + "\" volume=\"" + cbVolumeBatch.SelectedItem + "\"></batch_settings>\r\n");
            file_writer.Write("</settings>\r\n");
            file_writer.Close();
            UnregisterHotkeys();
            ReadSettings();
        }

        private void InitComboboxes()
        {
            langids.Add("Afrikaans", "436");
            langids.Add("Albanian", "41c");
            langids.Add("Arabic (Saudi Arabia)", "401");
            langids.Add("Arabic (Iraq)", "801");
            langids.Add("Arabic (Egypt)", "c01");
            langids.Add("Arabic (Libya)", "1001");
            langids.Add("Arabic (Algeria)", "1401");
            langids.Add("Arabic (Morocco)", "1801");
            langids.Add("Arabic (Tunisia)", "1c01");
            langids.Add("Arabic (Oman)", "2001");
            langids.Add("Arabic (Yemen)", "2401");
            langids.Add("Arabic (Syria)", "2801");
            langids.Add("Arabic (Jordan)", "2c01");
            langids.Add("Arabic (Lebanon)", "3001");
            langids.Add("Arabic (Kuwait)", "3401");
            langids.Add("Arabic (U.A.E.)", "3801");
            langids.Add("Arabic (Bahrain)", "3c01");
            langids.Add("Arabic (Qatar)", "4001");
            langids.Add("Armenian", "42b");
            langids.Add("Assamese", "44d");
            langids.Add("Azeri (Latin)", "42c");
            langids.Add("Azeri (Cyrillic)", "82c");
            langids.Add("Basque", "42d");
            langids.Add("Belarussian", "423");
            langids.Add("Bengali", "445");
            langids.Add("Bulgarian", "402");
            langids.Add("Burmese", "455");
            langids.Add("Catalan", "403");
            langids.Add("Chinese (Taiwan)", "404");
            langids.Add("Chinese (PRC)", "804");
            langids.Add("Chinese (Hong Kong SAR, PRC)", "c04");
            langids.Add("Chinese (Singapore)5", "1004");
            langids.Add("Chinese (Macau SAR)", "1404");
            langids.Add("Croatian", "41a");
            langids.Add("Czech", "405");
            langids.Add("Danish", "406");
            langids.Add("Dutch (Netherlands)", "413");
            langids.Add("Dutch (Belgium)", "813");
            langids.Add("English (United States)", "409");
            langids.Add("English (United Kingdom)", "809");
            langids.Add("English (Australian)", "c09");
            langids.Add("English (Canadian)", "1009");
            langids.Add("English (New Zealand)", "1409");
            langids.Add("English (Ireland)", "1809");
            langids.Add("English (South Africa)", "1c09");
            langids.Add("English (Jamaica)", "2009");
            langids.Add("English (Caribbean)", "2409");
            langids.Add("English (Belize)", "2809");
            langids.Add("English (Trinidad)", "2c09");
            langids.Add("English (Zimbabwe)", "3009");
            langids.Add("English (Philippines)", "3409");
            langids.Add("Estonian", "425");
            langids.Add("Faeroese", "438");
            langids.Add("Farsi", "429");
            langids.Add("Finnish", "40b");
            langids.Add("French (Standard)", "40c");
            langids.Add("French (Belgian)", "80c");
            langids.Add("French (Canadian)", "c0c");
            langids.Add("French (Switzerland)", "100c");
            langids.Add("French (Luxembourg)", "140c");
            langids.Add("French (Monaco)", "180c");
            langids.Add("Georgian", "437");
            langids.Add("German (Standard)", "407");
            langids.Add("German (Switzerland)", "807");
            langids.Add("German (Austria)", "c07");
            langids.Add("German (Luxembourg)", "1007");
            langids.Add("German (Liechtenstein)", "1407");
            langids.Add("Greek", "408");
            langids.Add("Gujarati", "447");
            langids.Add("Hebrew", "40d");
            langids.Add("Hindi", "439");
            langids.Add("Hungarian", "40e");
            langids.Add("Icelandic", "40f");
            langids.Add("Indonesian", "421");
            langids.Add("Italian (Standard)", "410");
            langids.Add("Italian (Switzerland)", "810");
            langids.Add("Japanese", "411");
            langids.Add("Kannada", "44b");
            langids.Add("Kashmiri (India)", "860");
            langids.Add("Kazakh", "43f");
            langids.Add("Konkani", "457");
            langids.Add("Korean", "412");
            langids.Add("Korean (Johab)", "812");
            langids.Add("Latvian", "426");
            langids.Add("Lithuanian", "427");
            langids.Add("Lithuanian (Classic)", "827");
            langids.Add("Macedonian6", "42f");
            langids.Add("Malay (Malaysian)", "43e");
            langids.Add("Malay (Brunei Darussalam)", "83e");
            langids.Add("Malayalam", "44c");
            langids.Add("Manipuri", "458");
            langids.Add("Marathi", "44e");
            langids.Add("Nepali (India)", "861");
            langids.Add("Norwegian (Bokmal)", "414");
            langids.Add("Norwegian (Nynorsk)", "814");
            langids.Add("Oriya", "448");
            langids.Add("Polish", "415");
            langids.Add("Portuguese (Brazil)", "416");
            langids.Add("Portuguese (Standard)", "816");
            langids.Add("Punjabi", "446");
            langids.Add("Romanian", "418");
            langids.Add("Russian", "419");
            langids.Add("Sanskrit", "44f");
            langids.Add("Serbian (Cyrillic)", "c1a");
            langids.Add("Serbian (Latin)", "81a");
            langids.Add("Sindhi", "459");
            langids.Add("Slovak", "41b");
            langids.Add("Slovenian", "424");
            langids.Add("Spanish (Traditional Sort)", "40a");
            langids.Add("Spanish (Mexican)", "80a");
            langids.Add("Spanish (Modern Sort)", "c0a");
            langids.Add("Spanish (Guatemala)", "100a");
            langids.Add("Spanish (Costa Rica)", "140a");
            langids.Add("Spanish (Panama)", "180a");
            langids.Add("Spanish (Dominican Republic)", "1c0a");
            langids.Add("Spanish (Venezuela)", "200a");
            langids.Add("Spanish (Colombia)", "240a");
            langids.Add("Spanish (Peru)", "280a");
            langids.Add("Spanish (Argentina)", "2c0a");
            langids.Add("Spanish (Ecuador)", "300a");
            langids.Add("Spanish (Chile)", "340a");
            langids.Add("Spanish (Uruguay)", "380a");
            langids.Add("Spanish (Paraguay)", "3c0a");
            langids.Add("Spanish (Bolivia)", "400a");
            langids.Add("Spanish (El Salvador)", "440a");
            langids.Add("Spanish (Honduras)", "480a");
            langids.Add("Spanish (Nicaragua)", "4c0a");
            langids.Add("Spanish (Puerto Rico)", "500a");
            langids.Add("Sutu", "430");
            langids.Add("Swahili (Kenya)", "441");
            langids.Add("Swedish", "41d");
            langids.Add("Swedish (Finland)", "81d");
            langids.Add("Tamil", "449");
            langids.Add("Tatar (Tatarstan)", "444");
            langids.Add("Telugu", "44a");
            langids.Add("Thai", "41e");
            langids.Add("Turkish", "41f");
            langids.Add("Ukrainian", "422");
            langids.Add("Urdu (Pakistan)", "420");
            langids.Add("Urdu (India)", "820");
            langids.Add("Uzbek (Latin)", "443");
            langids.Add("Uzbek (Cyrillic)", "843");
            langids.Add("Vietnamese", "42a");

            for (int i = 0; i < comboboxes_lang.Count; i++)
            {
                foreach (DictionaryEntry entry in langids)
                {
                    comboboxes_lang[i].Items.Add(entry.Key);
                    if (i == 0)
                        cbLanguageBatch.Items.Add(entry.Key);
                }
            }

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