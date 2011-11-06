using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Read4Me
{
    partial class Read4MeForm
    {
        List<ComboBox> comboboxes_lang = new List<ComboBox>();
        List<CheckBox> checkboxes_ctrl_lang = new List<CheckBox>();
        List<CheckBox> checkboxes_winkey_lang = new List<CheckBox>();
        List<CheckBox> checkboxes_alt_lang = new List<CheckBox>();
        List<TextBox> textboxes_lang = new List<TextBox>();
        private void init_lists()
        {
            comboboxes_lang.Add(cbLang1);
            comboboxes_lang.Add(cbLang2);
            comboboxes_lang.Add(cbLang3);
            comboboxes_lang.Add(cbLang4);
            comboboxes_lang.Add(cbLang5);
            comboboxes_lang.Add(cbLang6);

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
        }

        private void ReadSettings()
        {
            string type;
            bool ctrl;
            bool winkey;
            bool alt;
            string langid;
            string voice;
            char key;
            int lang_num = 0;

            try
            {
                XmlReader reader = XmlReader.Create(Path.GetDirectoryName(Application.ExecutablePath) + "\\Read4Me.ini");
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
                                SetViewSettings(type, ctrl, winkey, alt, key, "", "", 0);
                                RegisterHotkey(type, ctrl, winkey, alt, key, "", "");
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
                                langid = reader["langid"];
                                voice = reader["voice"];
                                SetViewSettings(type, ctrl, winkey, alt, key, langid, voice, lang_num);
                                RegisterHotkey(type, ctrl, winkey, alt, key, langid, voice);
                                lang_num++;
                                break;

                            default:
                                break;
                        }
                    }
                }
                reader.Close();
            }
            catch
            {
                sWorkingStatus.Text = "No settings file found!";
            }
        }

        private void SetViewSettings(string type, bool ctrl, bool winkey, bool alt, char key, string langid, string voice, int lang_num)
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
                    comboboxes_lang[lang_num].SelectedIndex = cbLang1.FindStringExact(voice);
                    break;

                default:
                    break;
            }
        }

        List<Hotkey> hotkeys = new List<Hotkey>();
        List<Keys> keycodes = new List<Keys>();
        List<Action> actions = new List<Action>();
        private bool RegisterHotkey(string type, bool ctrl, bool winkey, bool alt, char key, string langid, string voice)
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
                    todo_action = () => ReadClipboard(langid, voice);
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
                hotkeys[i].Unregister();
            }
            hotkeys.Clear();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            file_writer = new StreamWriter(Path.GetDirectoryName(Application.ExecutablePath) + "\\Read4Me.ini", false, Encoding.UTF8);
            file_writer.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            file_writer.Write("<settings>\r\n");
            file_writer.Write("    <hotkey_general type=\"show_hide\" ctrl=\"" + ((lCtrl0.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey0.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt0.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK0.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"pause_resume\" ctrl=\"" + ((lCtrl1.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey1.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt1.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK1.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"previous_sentence\" ctrl=\"" + ((lCtrl2.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey2.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt2.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK2.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"next_sentence\" ctrl=\"" + ((lCtrl3.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey3.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt3.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK3.Text + "\"></hotkey_general>\r\n");
            for (int i = 0; i < comboboxes_lang.Count; i++)
            {
                if ((textboxes_lang[i].Text != "") && ((string)comboboxes_lang[i].SelectedItem != ""))
                {
                    file_writer.Write("    <hotkey_speech type=\"speech\" ctrl=\"" + ((checkboxes_ctrl_lang[i].Checked == true) ? "1" : "0") + "\" winkey=\"" + ((checkboxes_winkey_lang[i].Checked == true) ? "1" : "0") + "\" alt=\"" + ((checkboxes_alt_lang[i].Checked == true) ? "1" : "0") + "\" key=\"" + textboxes_lang[i].Text + "\" langid=\"402\" voice=\"" + comboboxes_lang[i].SelectedItem + "\"></hotkey_speech>\r\n");
                }
            }
            file_writer.Write("</settings>\r\n");
            file_writer.Close();
            UnregisterHotkeys();
            ReadSettings();
        }
    }
}