using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlexaNG
{
    public partial class Form1 : Form
    {
        // Constants
        private const int TOTAL_TASKS = 40;
        private const string ASCII_ART_FILE = "flexa-ascii.dat";
        private const string SEVEN_ZIP_EXE = "7z.exe";
        private const string ERROR_LOG_FILE = "error.log";
        private const string COMPRESSION_ERROR_LOG = "compression_error.log";

        // Browser profile directories
        private const string CHROME_PROFILE_PATH = @"Google\Chrome\User Data\Default";
        private const string FIREFOX_PROFILE_PATH = @"Mozilla\Firefox\Profiles";
        private const string EDGE_PROFILE_PATH = @"Microsoft\Edge\User Data\Default";

        private bool errorsOccurred = false;
        private string errorFilePath = "";

        public Form1()
        {
            InitializeComponent();
            this.btn_proceed.Click += new System.EventHandler(this.btn_proceed_Click);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void UpdateStatus(string message)
        {
            if (lb_current == null || lb_current.IsDisposed)
                return;

            if (lb_current.InvokeRequired)
            {
                lb_current.Invoke(new Action(() => {
                    if (!lb_current.IsDisposed)
                        lb_current.Text = message;
                }));
            }
            else
            {
                lb_current.Text = message;
            }
        }

        private bool IsValidPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            try
            {
                // Check for invalid characters
                Path.GetFullPath(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                return false;

            // Basic validation - command should not contain dangerous characters
            return !command.Contains("..") && !command.Contains("/") && !command.Contains("\\");
        }

        private async void btn_proceed_Click(object sender, EventArgs e)
        {
            if (btn_proceed != null && !btn_proceed.IsDisposed)
                btn_proceed.Enabled = false;
            progressBar1.Value = 0;

            errorsOccurred = false;
            errorFilePath = "";

            string computerName = Environment.MachineName;
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string outputFolderPath = Path.Combine(
                Application.StartupPath,
                $"FlexaNG_logs_{computerName}_{currentDate}");

            if (!IsValidPath(outputFolderPath))
            {
                MessageBox.Show("Invalid output folder path. Please check the application directory.",
                    "FlexaNG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (btn_proceed != null && !btn_proceed.IsDisposed)
                    btn_proceed.Enabled = true;
                return;
            }

            try
            {
                Directory.CreateDirectory(outputFolderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot create output folder: {ex.Message}",
                    "FlexaNG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (btn_proceed != null && !btn_proceed.IsDisposed)
                    btn_proceed.Enabled = true;
                return;
            }

            var progress = new Progress<int>(percent => {
                if (progressBar1 != null && !progressBar1.IsDisposed)
                {
                    if (progressBar1.InvokeRequired)
                    {
                        progressBar1.Invoke(new Action(() => {
                            if (!progressBar1.IsDisposed)
                                progressBar1.Value = Math.Min(percent, 100);
                        }));
                    }
                    else
                    {
                        progressBar1.Value = Math.Min(percent, 100);
                    }
                }
            });

            await Task.Run(() => CollectData(outputFolderPath, progress));

            if (btn_proceed != null && !btn_proceed.IsDisposed)
                btn_proceed.Enabled = true;

            if (errorsOccurred && !string.IsNullOrEmpty(errorFilePath))
            {
                MessageBox.Show(
                    $"Log generation completed, but some errors occurred during the process.\nSee error.log for details.",
                    "FlexaNG",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Logs generated successfully!", "FlexaNG",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private string GetFileHeader()
        {
            StringBuilder header = new StringBuilder();

            string asciiArtPath = Path.Combine(Application.StartupPath, ASCII_ART_FILE);
            if (File.Exists(asciiArtPath))
            {
                header.AppendLine(File.ReadAllText(asciiArtPath));
            }
            else
            {
                header.AppendLine(@"--__FlexaNG__--");
            }

            header.AppendLine("=======================================");
            header.AppendLine("LOG GENERATED BY FlexaNG v.0.3");
            header.AppendLine("https://github.com/000rosiu/FlexaNG");
            header.AppendLine("=======================================");
            header.AppendLine();

            return header.ToString();
        }

        private void SaveLogWithHeader(string content, string outputPath)
        {
            string header = GetFileHeader();
            File.WriteAllText(outputPath, header + content);
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // Copy files
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                try
                {
                    string destFile = Path.Combine(destDir, Path.GetFileName(file));
                    File.Copy(file, destFile, true);
                }
                catch (Exception ex)
                {
                    errorsOccurred = true;
                    SaveLogWithHeader($"Error copying file {file}: {ex.Message}", errorFilePath);
                }
            }

            foreach (string dir in Directory.GetDirectories(sourceDir))
            {
                try
                {
                    string destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
                    CopyDirectory(dir, destSubDir);
                }
                catch (Exception ex)
                {
                    errorsOccurred = true;
                    SaveLogWithHeader($"Error copying directory {dir}: {ex.Message}", errorFilePath);
                }
            }
        }

        private void CollectData(string outputFolderPath, IProgress<int> progress)
        {
            int totalTasks = TOTAL_TASKS; 
            int completedTasks = 0;

            void UpdateProgress()
            {
                completedTasks++;
                int progressValue = Math.Min((int)((float)completedTasks / totalTasks * 100), 100);
                progress.Report(progressValue);
            }

            void RunAndSaveCommand(string command, string arguments, string outputFileName)
            {
                // Validate inputs
                if (!IsValidCommand(command))
                {
                    errorsOccurred = true;
                    errorFilePath = Path.Combine(outputFolderPath, ERROR_LOG_FILE);
                    SaveLogWithHeader($"Invalid command: {command}", errorFilePath);
                    return;
                }

                if (!IsValidPath(outputFolderPath))
                {
                    errorsOccurred = true;
                    errorFilePath = Path.Combine(outputFolderPath, ERROR_LOG_FILE);
                    SaveLogWithHeader($"Invalid output path: {outputFolderPath}", errorFilePath);
                    return;
                }

                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = command;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    SaveLogWithHeader(output, Path.Combine(outputFolderPath, outputFileName));
                }
                catch (Exception ex)
                {
                    errorsOccurred = true;
                    errorFilePath = Path.Combine(outputFolderPath, ERROR_LOG_FILE);
                    SaveLogWithHeader($"Error executing command {command} {arguments}: {ex.Message}", errorFilePath);
                }
            }

            try
            {
                // Version info
                UpdateStatus("Collecting system information...");
                RunAndSaveCommand("cmd.exe", "/c ver", "ver.log");
                UpdateProgress();

                // Volume info
                UpdateStatus("Collecting volume information...");
                RunAndSaveCommand("cmd.exe", "/c vol", "vol.log");
                UpdateProgress();

                // IP configuration
                UpdateStatus("Collecting IP configuration...");
                RunAndSaveCommand("ipconfig", "/all", "ipconfig.log");
                RunAndSaveCommand("ipconfig", "/displaydns", "ipconfig_dns.log");
                UpdateProgress();

                // User info
                UpdateStatus("Collecting user information...");
                RunAndSaveCommand("net", "user", "net-user.log");
                UpdateProgress();

                // File associations
                UpdateStatus("Collecting file associations...");
                RunAndSaveCommand("cmd.exe", "/c assoc", "assoc.log");
                UpdateProgress();

                // Environment variables
                UpdateStatus("Collecting environment variables...");
                RunAndSaveCommand("cmd.exe", "/c set", "set.log");
                UpdateProgress();

                // Compression info
                UpdateStatus("Collecting compression information...");
                RunAndSaveCommand("compact", "", "compact.log");
                UpdateProgress();

                // File types
                UpdateStatus("Collecting file types...");
                RunAndSaveCommand("cmd.exe", "/c ftype", "ftype.log");
                UpdateProgress();

                // Network connections
                UpdateStatus("Collecting network connections...");
                RunAndSaveCommand("netstat", "-an", "netstat-an.log");
                UpdateProgress();

                // Power configuration
                UpdateStatus("Collecting power configuration...");
                RunAndSaveCommand("powercfg", "/a", "powercfg.log");
                UpdateProgress();

                // Process list
                UpdateStatus("Collecting tasklist...");
                RunAndSaveCommand("tasklist", "", "tasklist.log");
                UpdateProgress();

                // BIOS info
                UpdateStatus("Collecting BIOS information...");
                RunAndSaveCommand("wmic", "bios get /all", "bios.log");
                UpdateProgress();

                // CPU info
                UpdateStatus("Collecting CPU information...");
                RunAndSaveCommand("wmic", "cpu get /all", "cpu.log");
                UpdateProgress();

                // System info
                UpdateStatus("Collecting system information...");
                RunAndSaveCommand("systeminfo", "", "systeminfo.log");
                UpdateProgress();

                // Timezone info
                UpdateStatus("Collecting timezone information...");
                RunAndSaveCommand("wmic", "timezone get /all", "timezone.log");
                UpdateProgress();

                // Graphics card info
                UpdateStatus("Collecting graphics card information...");
                RunAndSaveCommand("wmic", "path win32_videocontroller get /all", "graphics.log");
                UpdateProgress();

                // RAM info
                UpdateStatus("Collecting RAM information...");
                RunAndSaveCommand("wmic", "memorychip get /all", "ram.log");
                UpdateProgress();

                // Disk info
                UpdateStatus("Collecting disk information...");
                RunAndSaveCommand("wmic", "diskdrive get /all", "disk.log");
                UpdateProgress();

                // OS info
                UpdateStatus("Collecting OS information...");
                RunAndSaveCommand("wmic", "os get /all", "os.log");
                UpdateProgress();

                // Information about installed programs
                UpdateStatus("Collecting installed software information...");
                RunAndSaveCommand("wmic", "product get name,version", "installed_software.log");
                UpdateProgress();

                // Information about system services
                UpdateStatus("Collecting system services information...");
                RunAndSaveCommand("sc", "query", "services.log");
                UpdateProgress();

                // Information about drivers
                UpdateStatus("Collecting driver information...");
                RunAndSaveCommand("driverquery", "/v", "drivers.log");
                UpdateProgress();

                // Information about disk partitions
                UpdateStatus("Collecting disk partition information...");
                RunAndSaveCommand("wmic", "logicaldisk get caption,description,providername,volumename,size,freespace", "partitions.log");
                UpdateProgress();

                // Information about network interfaces
                UpdateStatus("Collecting network interface information...");
                RunAndSaveCommand("wmic", "nic get AdapterType,Name,Installed,MACAddress,NetConnectionID,Speed", "network_adapters.log");
                UpdateProgress();

                // Information about services launched
                UpdateStatus("Collecting running services information...");
                RunAndSaveCommand("net", "start", "running_services.log");
                UpdateProgress();

                // Information about user groups
                UpdateStatus("Collecting user groups information...");
                RunAndSaveCommand("net", "localgroup", "user_groups.log");
                UpdateProgress();

                // Information about user accounts
                UpdateStatus("Collecting user accounts information...");
                RunAndSaveCommand("wmic", "useraccount get name,sid,status,passwordrequired", "user_accounts.log");
                UpdateProgress();

                // Information about installed updates
                UpdateStatus("Collecting installed updates information...");
                RunAndSaveCommand("wmic", "qfe get hotfixid,description,installedby,installedon", "installed_updates.log");
                UpdateProgress();

                // Information on scheduled tasks
                UpdateStatus("Collecting scheduled tasks information...");
                RunAndSaveCommand("schtasks", "/query /fo LIST /v", "scheduled_tasks.log");
                UpdateProgress();

                // Information about PnP devices
                UpdateStatus("Collecting PnP devices information...");
                RunAndSaveCommand("wmic", "path Win32_PnPEntity get Caption,DeviceID,Manufacturer,PNPDeviceID", "pnp_devices.log");
                UpdateProgress();

                // Information about firewall configuration
                UpdateStatus("Collecting firewall configuration...");
                RunAndSaveCommand("netsh", "advfirewall show allprofiles", "firewall.log");
                UpdateProgress();

                // Generate directory tree if checkbox is checked
                if (check_tree.Checked)
                {
                    string treeCommand = "cmd.exe";
                    string treeArgs = $"/c tree C:\\ /F /A > \"{Path.Combine(outputFolderPath, "directory_tree.log")}\"";

                    Process process = new Process();
                    process.StartInfo.FileName = treeCommand;
                    process.StartInfo.Arguments = treeArgs;
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();

                    string treeFilePath = Path.Combine(outputFolderPath, "directory_tree.log");
                    if (File.Exists(treeFilePath))
                    {
                        string content = File.ReadAllText(treeFilePath);
                        SaveLogWithHeader(content, treeFilePath);
                    }
                }
                UpdateProgress();

                // Collect browser data if checkbox is checked
                if (check_browsers.Checked)
                {
                    string browserDataPath = Path.Combine(outputFolderPath, "browser_data");
                    Directory.CreateDirectory(browserDataPath);

                    StringBuilder browserInfo = new StringBuilder();

                    // Chrome
                    string chromePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        CHROME_PROFILE_PATH);

                    if (Directory.Exists(chromePath))
                    {
                        browserInfo.AppendLine("Google Chrome:");
                        browserInfo.AppendLine($"Profile path: {chromePath}");

                        string chromeBackupPath = Path.Combine(browserDataPath, "Chrome_Default");
                        Directory.CreateDirectory(chromeBackupPath);

                        try
                        {
                            string[] filesToCopy = {
                                "Bookmarks", "Cookies", "History", "Login Data", "Preferences",
                                "Web Data", "Favicons", "Shortcuts"
                            };

                            foreach (string file in filesToCopy)
                            {
                                string sourcePath = Path.Combine(chromePath, file);
                                if (File.Exists(sourcePath))
                                {
                                    File.Copy(sourcePath, Path.Combine(chromeBackupPath, file), true);
                                }
                            }

                            if (Directory.Exists(Path.Combine(chromePath, "Extensions")))
                            {
                                CopyDirectory(Path.Combine(chromePath, "Extensions"),
                                    Path.Combine(chromeBackupPath, "Extensions"));
                            }

                            browserInfo.AppendLine("Profile backup created successfully.");
                        }
                        catch (Exception ex)
                        {
                            browserInfo.AppendLine($"Error backing up Chrome profile: {ex.Message}");
                        }
                    }

                    // Firefox
                    string firefoxPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        FIREFOX_PROFILE_PATH);

                    if (Directory.Exists(firefoxPath))
                    {
                        browserInfo.AppendLine("\nMozilla Firefox:");
                        browserInfo.AppendLine($"Profiles path: {firefoxPath}");

                        string[] profiles = Directory.GetDirectories(firefoxPath);
                        foreach (string profile in profiles)
                        {
                            string profileName = Path.GetFileName(profile);
                            browserInfo.AppendLine($"Profile: {profileName}");

                            string firefoxBackupPath = Path.Combine(browserDataPath, "Firefox_" + profileName);
                            Directory.CreateDirectory(firefoxBackupPath);

                            try
                            {
                                string[] filesToCopy = {
                                    "places.sqlite", "cookies.sqlite", "formhistory.sqlite",
                                    "logins.json", "key4.db", "prefs.js", "addons.json"
                                };

                                foreach (string file in filesToCopy)
                                {
                                    string sourcePath = Path.Combine(profile, file);
                                    if (File.Exists(sourcePath))
                                    {
                                        File.Copy(sourcePath, Path.Combine(firefoxBackupPath, file), true);
                                    }
                                }

                                browserInfo.AppendLine("Profile backup created successfully.");
                            }
                            catch (Exception ex)
                            {
                                browserInfo.AppendLine($"Error backing up Firefox profile: {ex.Message}");
                            }
                        }
                    }

                    // Edge
                    string edgePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        EDGE_PROFILE_PATH);

                    if (Directory.Exists(edgePath))
                    {
                        browserInfo.AppendLine("\nMicrosoft Edge:");
                        browserInfo.AppendLine($"Profile path: {edgePath}");

                        string edgeBackupPath = Path.Combine(browserDataPath, "Edge_Default");
                        Directory.CreateDirectory(edgeBackupPath);

                        try
                        {
                            string[] filesToCopy = {
                                "Bookmarks", "Cookies", "History", "Login Data", "Preferences",
                                "Web Data", "Favicons", "Shortcuts"
                            };

                            foreach (string file in filesToCopy)
                            {
                                string sourcePath = Path.Combine(edgePath, file);
                                if (File.Exists(sourcePath))
                                {
                                    File.Copy(sourcePath, Path.Combine(edgeBackupPath, file), true);
                                }
                            }

                            if (Directory.Exists(Path.Combine(edgePath, "Extensions")))
                            {
                                CopyDirectory(Path.Combine(edgePath, "Extensions"),
                                    Path.Combine(edgeBackupPath, "Extensions"));
                            }

                            browserInfo.AppendLine("Profile backup created successfully.");
                        }
                        catch (Exception ex)
                        {
                            browserInfo.AppendLine($"Error backing up Edge profile: {ex.Message}");
                        }
                    }

                    SaveLogWithHeader(browserInfo.ToString(), Path.Combine(browserDataPath, "browser_profiles.log"));
                }
                UpdateProgress();

                // Compress folder if checkbox is checked
                if (check_makezip.Checked)
                {
                    string sevenZipPath = Path.Combine(Application.StartupPath, SEVEN_ZIP_EXE);

                    if (File.Exists(sevenZipPath))
                    {
                        string zipPath = outputFolderPath + ".zip";

                        ProcessStartInfo processInfo = new ProcessStartInfo();
                        processInfo.FileName = sevenZipPath;
                        processInfo.Arguments = $"a -tzip \"{zipPath}\" \"{outputFolderPath}\\*\"";
                        processInfo.WindowStyle = ProcessWindowStyle.Hidden;

                        Process process = Process.Start(processInfo);
                        process.WaitForExit();
                    }
                    else
                    {
                        SaveLogWithHeader("7z.exe not found in application directory. Compression was not performed.",
                            Path.Combine(outputFolderPath, COMPRESSION_ERROR_LOG));
                    }
                }
                UpdateProgress();
            }
            catch (Exception ex)
            {
                SaveLogWithHeader($"Error during data collection: {ex.Message}",
                    Path.Combine(outputFolderPath, "ERROR.log"));
            }
        }

        private void check_browsers_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
