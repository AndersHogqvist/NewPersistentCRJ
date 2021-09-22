using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Timers;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using IniParser;
using IniParser.Model;
using IniParser.Exceptions;


namespace NewPersistentCRJ
{
    public partial class Form1 : Form
    {
        private const string APP_NAME = "PersistentCRJ";
        private const string APP_VERSION = "3.1";

        private FileIniDataParser parser;
        private IniData ini_data;
        private bool auto_save = false;
        private int start_event_no = 0x1FFF0;
        private bool is_online = false;
        private string test_lvar;
        private string original_data;
        private bool is_dirty = false;

        private Dictionary<string, int> random_lvars;

        Color original_color;

        private string status_text;
        private System.Timers.Timer status_timer;

        public Form1()
        {
            InitializeComponent();

            // Exit callback
            Application.ApplicationExit += new EventHandler(this.OnApplicationExitCB);

            parser = new FileIniDataParser();

            status_timer = new System.Timers.Timer(2000);
            status_timer.AutoReset = false;

            this.Text = APP_NAME + " " + APP_VERSION + " (not connected)";
            this.Update();

            autoSaveBtn.Enabled = false;
            loadBtn.Enabled = false;
            randomBtn.Enabled = false;
            saveBtn.Enabled = false;
            randomLvarsScroll.Enabled = false;

            toolTip.SetToolTip(this.saveBtn, "Save the current state to the file");
            toolTip.SetToolTip(this.autoSaveBtn, "If autosave is enabled the state will be saved to file whenever a value is changed");
            toolTip.SetToolTip(this.connectBtn, "Manually connect to the sim");
            toolTip.SetToolTip(this.loadBtn, "Load a saved state");
            toolTip.SetToolTip(this.randomBtn, "Set the controls in a random state");
            toolTip.SetToolTip(this.randomLvarsScroll, "The number of controls to randomize");
            toolTip.SetToolTip(this.exitBtn, "Exit PersistentCRJ");

            try
            {
                ini_data = parser.ReadFile("lvars.ini");

                original_data = ini_data.ToString();

                Int32.TryParse(ini_data["Config"]["Start_event_number"], out start_event_no);

                random_lvars = new Dictionary<string, int>();

                foreach (KeyData key in ini_data["Randomize"])
                {
                    random_lvars.Add(key.KeyName, Int32.Parse(key.Value));
                }

                foreach (KeyData key in ini_data["LVars"])
                {
                    test_lvar = key.KeyName;
                    break;
                }

                randomLvarsScroll.Minimum = 1;
                randomLvarsScroll.Maximum = random_lvars.Count;
                randomLvarsScroll.Value = random_lvars.Count;
                numOfCtrlLabel.Text = randomLvarsScroll.Value.ToString() + " controls";
            }
            catch (ParsingException e)
            {
                MessageBox.Show("Failed to load lvars.ini: \n" + e.Message);
                Process[] processes = Process.GetProcessesByName("PersistentCRJ");
                foreach (Process process in processes)
                {
                    process.Kill();
                }
            }

            try
            {
                fsuipcw_init(this.Handle, start_event_no, null);
                fsuipcw_registerUpdateCallback(ModuleUpdatedCB);
                fsuipcw_registerLvarUpdateCallbackByName(LvarUpdateCB);
                fsuipcw_start();
            }
            catch (DllNotFoundException e)
            {
                MessageBox.Show("Failed to load 'FSUIPC_WAPID.dll'!\nMake sure it's in the same folder as PersistentCRJ.exe.");
                Process[] processes = Process.GetProcessesByName("PersistentCRJ");
                foreach (Process process in processes)
                {
                    process.Kill();
                }
            }

            original_color = autoSaveBtn.ForeColor;
        }


        /**
         * Functions to manipulate the UI from callbacks
         */
        delegate void SetOnlineOfflineCB();
        private void SetOnlineOffline()
        {
            if (this.InvokeRequired)
            {
                SetOnlineOfflineCB cb = new SetOnlineOfflineCB(SetOnlineOffline);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                if (is_online)
                {
                    this.Text = APP_NAME + " " + APP_VERSION;
                    this.Update();

                    connectBtn.Enabled = false;
                    autoSaveBtn.Enabled = true;
                    loadBtn.Enabled = true;
                    randomBtn.Enabled = true;
                    randomLvarsScroll.Enabled = true;
                }
                else
                {
                    this.Text = APP_NAME + " " + APP_VERSION + " (not connected)";
                    this.Update();

                    connectBtn.Enabled = true;
                    autoSaveBtn.Enabled = false;
                    loadBtn.Enabled = false;
                    randomBtn.Enabled = false;
                    saveBtn.Enabled = false;
                    randomLvarsScroll.Enabled = false;
                }
            }
        }

        /**
         * Functions set text on satus label
         */
        delegate void SetStatusLabelTextCB();
        private void SetStatusLabelText()
        {
            if (this.InvokeRequired)
            {
                SetStatusLabelTextCB cb = new SetStatusLabelTextCB(SetStatusLabelText);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                if (status_timer.Enabled)
                {
                    status_timer.Enabled = false;
                }
                statusLabel.Text = status_text;
                statusLabel.Update();
                status_timer.Elapsed += StatusTextRemoveCB;
                status_timer.AutoReset = true;
                status_timer.Enabled = true;
            }
        }

        /**
         * Function to enable/disable save button based on if there's something to save
         */
        delegate void ToggleSaveBtnCB();
        private void ToggleSaveBtn()
        {
            if (this.InvokeRequired)
            {
                ToggleSaveBtnCB cb = new ToggleSaveBtnCB(ToggleSaveBtn);
                this.Invoke(cb, new object[] { });
            }
            else
            {
                saveBtn.Enabled = is_dirty;

                if (is_dirty)
                {
                    saveBtn.Text = "Save to file";
                }
                else
                {
                    saveBtn.Text = "State saved";
                }

                saveBtn.Update();
            }
        }

        /**
         * Checks if Lvars are available by trying to read the value of the first Lvar in the list.
         * Calls SetOnlineOffline to update UI accordingly
         */
        private bool check_fsuipcw_running()
        {
            if (is_online)
            {
                if (fsuipcw_getLvarIdFromName(test_lvar) < 0)
                {
                    is_online = false;
                }
            }

            SetOnlineOffline();

            return is_online;
        }

        /**
         * Saves to file, copies ini_data to original_data, sets is_dirty to false
         * and toggles saveBtn
         */
        private void save_to_file()
        {
            parser.WriteFile("lvars.ini", ini_data);
            original_data = ini_data.ToString();
            is_dirty = false;
            ToggleSaveBtn();
        }

        /**
         * Callback called when LVars are loaded and ready. Runs through
         * all Lvars in the file and checks their values agains those in
         * the sim. If anyone differ it sets is_dirty to true.
         */
        void ModuleUpdatedCB()
        {
            is_online = true;

            SetOnlineOffline();

            try
            {
                Dictionary<string, double> values = new Dictionary<string, double>();
                fsuipcw_getLvarValues(values.Add);

                foreach (KeyData key in ini_data["LVars"])
                {
                    fsuipcw_flagLvarForUpdateCallbackByName(key.KeyName);

                    if (!is_dirty)
                    {
                        int stored_value;
                        Int32.TryParse(key.Value, out stored_value);

                        double value;
                        if (values.TryGetValue(key.KeyName, out value))
                        {
                            if (stored_value != (int)value)
                            {
                                SetStatusLabelText();
                                is_dirty = true;
                            }
                        }
                    }
                }
                ToggleSaveBtn();
            }
            catch (ParsingException e)
            {
                MessageBox.Show("Failed to register LVar update callback: \n" + e.Message);
                Process[] processes = Process.GetProcessesByName("PersistentCRJ");
                fsuipcw_end();
                foreach (Process process in processes)
                {
                    process.Kill();
                }
            }
        }

        /**
         * Callback for Lvar updates. If autosave is activated it will
         * save all Lvars passed to it to the file.
         */
        void LvarUpdateCB(string[] lvarNames, double[] values)
        {
            int value = (int)values[0];
            ini_data["LVars"][lvarNames[0]] = value.ToString();

            if (auto_save)
            {
                status_text = "Auto saved '" + lvarNames[0] + "'";
                SetStatusLabelText();
                save_to_file();
            }
            else
            {
                is_dirty = ini_data.ToString() != original_data;
                ToggleSaveBtn();
            }
        }

        /**
         * Callback to exit gracefully if application is not closed
         * by pressing Exit button
         */
        private void OnApplicationExitCB(object sender, EventArgs e)
        {
            exitBtn_Click(sender, e);
        }

        /**
         * Callback to remove status text after certain time
         */
        void StatusTextRemoveCB(object sender, EventArgs e)
        {
            status_timer.Enabled = false;
            status_text = "";
            SetStatusLabelText();
        }

        /**
         * Save button callback
         */
        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (!check_fsuipcw_running())
            {
                return;
            }

            save_to_file();

            status_text = "State saved";
            SetStatusLabelText();
        }

        /**
         * Exit button callback (also called from application exit callback)
         */
        private void exitBtn_Click(object sender, EventArgs e)
        {
            if (check_fsuipcw_running() == false)
            {
                fsuipcw_end();
                System.Windows.Forms.Application.Exit();
            }

            if (auto_save == false && is_dirty)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to save current the state to file?", "PersistentCRJ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    parser.WriteFile("lvars.ini", ini_data);
                }
            }

            fsuipcw_end();
            System.Windows.Forms.Application.Exit();
        }

        /**
         * Load button callback. Sends the stored value of each LVar
         * in lvars.ini LVars section to the sim.
         */
        private void loadBtn_Click(object sender, EventArgs e)
        {
            if (!check_fsuipcw_running())
            {
                return;
            }

            IniData new_ini_data = parser.ReadFile("lvars.ini");

            foreach (KeyData data in new_ini_data["LVars"])
            {
                try
                {
                    int id = fsuipcw_getLvarIdFromName(data.KeyName);
                    if (id >= 0)
                    {
                        fsuipcw_setLvarAsShort(id, Int32.Parse(data.Value));
                    }
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Failed to parse " + data.KeyName + ": " + ex.Message);
                    return;
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show("Failed to parse " + data.KeyName + ": " + ex.Message);
                    return;
                }
            }

            ini_data = new_ini_data;
            original_data = ini_data.ToString();
            is_dirty = false;
            ToggleSaveBtn();
            status_text = "State loaded";
            SetStatusLabelText();
        }

        /**
         * Randomize button callback. Reads a 
         */
        private void randomBtn_Click(object sender, EventArgs e)
        {
            if (!check_fsuipcw_running())
            {
                return;
            }

            Random rnd = new Random();

            if (randomLvarsScroll.Value == random_lvars.Count)
            {
                foreach (KeyValuePair<string, int> entry in random_lvars)
                {
                    int id = fsuipcw_getLvarIdFromName(entry.Key);
                    if (id >= 0)
                    {
                        fsuipcw_setLvarAsShort(id, rnd.Next(0, entry.Value + 1));
                    }
                }
            }
            else
            {
                List<int> used_lvars = new List<int>();
                int rand_index = 0;
                int num_of_vars = random_lvars.Count;
                for (int ix=0; ix<randomLvarsScroll.Value; ++ix)
                {
                    do
                    {
                        rand_index = rnd.Next(0, num_of_vars);
                    }
                    while (used_lvars.Contains(rand_index));

                    used_lvars.Add(rand_index);

                    KeyValuePair<string, int> entry = random_lvars.ElementAt(rand_index);
                    int id = fsuipcw_getLvarIdFromName(entry.Key);
                    if (id >= 0)
                    {
                        fsuipcw_setLvarAsShort(id, rnd.Next(0, entry.Value + 1));
                    }
                }
            }


            status_text = randomLvarsScroll.Value.ToString() + " controls randomized";
            SetStatusLabelText();
        }

        private void autoSaveBtn_Click(object sender, EventArgs e)
        {
            if (!check_fsuipcw_running())
            {
                return;
            }

            auto_save = !auto_save;

            if (auto_save)
            {
                autoSaveBtn.ForeColor = Color.Green;
                loadBtn.Enabled   = false;
                randomBtn.Enabled = false;
                saveBtn.Enabled = false;
                randomLvarsScroll.Enabled = false;
            }
            else
            {
                autoSaveBtn.ForeColor = original_color;
                loadBtn.Enabled   = true;
                randomBtn.Enabled = true;
                randomLvarsScroll.Enabled = true;
                ToggleSaveBtn();
            }
        }

        /**
         * Connect button callback
         */
        private void connectBtn_Click(object sender, EventArgs e)
        {
            fsuipcw_start();
            fsuipcw_reload();
        }

        /**
         * Random slider callback.
         */
        private void randomLvarsScroll_Scroll(object sender, EventArgs e)
        {
            numOfCtrlLabel.Text = randomLvarsScroll.Value.ToString() + " controls";
        }


        /**
          * DLL function import
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void logger(string lvarName);

        [DllImport("FSUIPC_WAPID.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void fsuipcw_init(IntPtr hWnd, int startEventNo, logger log_cb);

        [DllImport("FSUIPC_WAPID.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void fsuipcw_start();

        [DllImport("FSUIPC_WAPID.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int fsuipcw_getLvarIdFromName(string lvarName);

        [DllImport("FSUIPC_WAPID.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void fsuipcw_reload();

        [DllImport("FSUIPC_WAPID.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void fsuipcw_end();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void lvarValueRecvFunc(string lvarName, double value);

        [DllImport("FSUIPC_WAPID.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void fsuipcw_getLvarValues(lvarValueRecvFunc callback);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void fsuipcUpdateCallback();

        [DllImport("FSUIPC_WAPID.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void fsuipcw_registerUpdateCallback(fsuipcUpdateCallback cb);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void lvarUpdateCallback(string[] lvarName, double[] value);

        [DllImport("FSUIPC_WAPID.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void fsuipcw_registerLvarUpdateCallbackByName(lvarUpdateCallback callback);

        [DllImport("FSUIPC_WAPID.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void fsuipcw_flagLvarForUpdateCallbackByName(string lvarName);

        [DllImport("FSUIPC_WAPID.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void fsuipcw_setLvarAsShort(int lvarID, int value);

    }
}
