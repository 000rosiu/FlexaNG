using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
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
            
            // Custom progress bar styling
            progressBar1.ForeColor = Color.FromArgb(100, 87, 193);
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

            // Allow only specific commands or .exe files without path traversal
            string cmdLower = command.ToLower();
            
            // Check for path traversal attempts
            if (command.Contains(".."))
                return false;
            
            // Allow commands without path separators, or standard Windows commands
            if (!command.Contains("\\") && !command.Contains("/"))
                return true;
                
            // Allow full paths only if they don't contain suspicious patterns
            return !command.Contains("..");
        }

        private async void btn_proceed_Click(object sender, EventArgs e)
        {
            // Disable button during operation
            btn_proceed.Enabled = false;
            progressBar1.Value = 0;
            errorsOccurred = false;

            // Warn about tree operation
            if (check_tree.Checked)
            {
                var result = MessageBox.Show(
                    "Warning: Generating a directory tree of the entire C: drive can take a very long time.\n\nContinue?",
                    "FlexaNG - Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    check_tree.Checked = false;
                }
            }

            // Prepare output folder
            string computerName = Environment.MachineName;
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string outputFolderPath = Path.Combine(
                Application.StartupPath,
                $"FlexaNG_logs_{computerName}_{currentDate}");

            try
            {
                Directory.CreateDirectory(outputFolderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot create output folder: {ex.Message}", "FlexaNG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btn_proceed.Enabled = true;
                return;
            }

            var progress = new Progress<int>(percent => {
                if (progressBar1 != null && !progressBar1.IsDisposed)
                    progressBar1.Value = Math.Min(percent, 100);
            });

            // Run collection in background
            await Task.Run(() => CollectData(
                outputFolderPath,
                progress,
                check_tree.Checked,
                check_browsers.Checked,
                check_makezip.Checked));

            // Re-enable button
            btn_proceed.Enabled = true;

            // Show results
            if (errorsOccurred && !string.IsNullOrEmpty(errorFilePath))
            {
                string sizeInfo = GetFolderSizeInfo(outputFolderPath);
                MessageBox.Show(
                    $"Logs generated with some errors.\n\n{sizeInfo}\n\nCheck error.log for details.",
                    "FlexaNG",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                string sizeInfo = GetFolderSizeInfo(outputFolderPath);
                MessageBox.Show(
                    $"Logs generated successfully!\n\n{sizeInfo}",
                    "FlexaNG",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private string GetFolderSizeInfo(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    return "Folder size: unknown";

                long totalSize = 0;
                int fileCount = 0;

                var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    try
                    {
                        totalSize += new FileInfo(file).Length;
                        fileCount++;
                    }
                    catch { }
                }

                string sizeText;
                if (totalSize < 1024)
                    sizeText = $"{totalSize} B";
                else if (totalSize < 1024 * 1024)
                    sizeText = $"{totalSize / 1024.0:F2} KB";
                else if (totalSize < 1024 * 1024 * 1024)
                    sizeText = $"{totalSize / (1024.0 * 1024):F2} MB";
                else
                    sizeText = $"{totalSize / (1024.0 * 1024 * 1024):F2} GB";

                return $"Total size: {sizeText}\nFiles: {fileCount}";
            }
            catch
            {
                return "Folder size: unknown";
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
            header.AppendLine("LOG GENERATED BY FlexaNG v.0.4");
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
            // Initialize errorFilePath if not set
            if (string.IsNullOrEmpty(errorFilePath))
            {
                errorFilePath = Path.Combine(Path.GetDirectoryName(destDir) ?? "", ERROR_LOG_FILE);
            }

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

        private bool CopyFileWithRetry(string sourceFile, string destFile, int maxRetries = 3, int delayMs = 500)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    File.Copy(sourceFile, destFile, true);
                    return true;
                }
                catch (IOException) when (i < maxRetries - 1)
                {
                    // File might be locked, wait and retry
                    Thread.Sleep(delayMs);
                }
                catch (Exception ex)
                {
                    errorsOccurred = true;
                    if (!string.IsNullOrEmpty(errorFilePath))
                    {
                        SaveLogWithHeader($"Error copying file {sourceFile}: {ex.Message}", errorFilePath);
                    }
                    return false;
                }
            }
            return false;
        }

        private void CollectData(string outputFolderPath, IProgress<int> progress,
            bool includeTree, bool includeBrowsers, bool includeZip)
        {
            errorFilePath = Path.Combine(outputFolderPath, ERROR_LOG_FILE);
            
            int totalTasks = 32;
            if (includeTree) totalTasks++;
            if (includeBrowsers) totalTasks++;
            if (includeZip) totalTasks++;
                
            int completedTasks = 0;

            void ReportProgress()
            {
                completedTasks++;
                progress.Report(Math.Min((completedTasks * 100) / totalTasks, 100));
            }

            void Exec(string cmd, string args, string outFile)
            {
                try
                {
                    UpdateStatus($"Running: {cmd}");
                    
                    var psi = new ProcessStartInfo
                    {
                        FileName = cmd,
                        Arguments = args,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using (var proc = Process.Start(psi))
                    {
                        string output = proc.StandardOutput.ReadToEnd();
                        string errors = proc.StandardError.ReadToEnd();
                        proc.WaitForExit();

                        if (!string.IsNullOrEmpty(errors))
                        {
                            output += "\n\n=== ERRORS ===\n" + errors;
                            errorsOccurred = true;
                        }

                        SaveLogWithHeader(output, Path.Combine(outputFolderPath, outFile));
                    }
                }
                catch (Exception ex)
                {
                    errorsOccurred = true;
                    string errorMsg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Command failed:\n" +
                                     $"  Command: {cmd}\n" +
                                     $"  Arguments: {args}\n" +
                                     $"  Output file: {outFile}\n" +
                                     $"  Error: {ex.GetType().Name} - {ex.Message}\n" +
                                     $"  Stack trace: {ex.StackTrace}\n" +
                                     new string('-', 80);
                    SaveLogWithHeader(errorMsg, errorFilePath);
                    UpdateStatus($"Error: {cmd}");
                }
            }

            try
            {
                // System commands
                Exec("cmd.exe", "/c ver", "ver.log"); ReportProgress();
                Exec("cmd.exe", "/c vol", "vol.log"); ReportProgress();
                Exec("ipconfig", "/all", "ipconfig.log");
                Exec("ipconfig", "/displaydns", "ipconfig_dns.log"); ReportProgress();
                Exec("net", "user", "net-user.log"); ReportProgress();
                Exec("cmd.exe", "/c assoc", "assoc.log"); ReportProgress();
                Exec("cmd.exe", "/c set", "set.log"); ReportProgress();
                Exec("compact", "", "compact.log"); ReportProgress();
                Exec("cmd.exe", "/c ftype", "ftype.log"); ReportProgress();
                Exec("netstat", "-an", "netstat-an.log"); ReportProgress();
                Exec("powercfg", "/a", "powercfg.log"); ReportProgress();
                Exec("tasklist", "", "tasklist.log"); ReportProgress();
                Exec("wmic", "bios get /all", "bios.log"); ReportProgress();
                Exec("wmic", "cpu get /all", "cpu.log"); ReportProgress();
                
                UpdateStatus("Running systeminfo (may take a while)...");
                Exec("systeminfo", "", "systeminfo.log"); ReportProgress();
                
                Exec("wmic", "timezone get /all", "timezone.log"); ReportProgress();
                Exec("wmic", "path win32_videocontroller get /all", "graphics.log"); ReportProgress();
                Exec("wmic", "memorychip get /all", "ram.log"); ReportProgress();
                Exec("wmic", "diskdrive get /all", "disk.log"); ReportProgress();
                Exec("wmic", "os get /all", "os.log"); ReportProgress();
                Exec("wmic", "product get name,version", "installed_software.log"); ReportProgress();
                Exec("sc", "query", "services.log"); ReportProgress();
                Exec("driverquery", "/v", "drivers.log"); ReportProgress();
                Exec("wmic", "logicaldisk get caption,description,providername,volumename,size,freespace", "partitions.log"); ReportProgress();
                Exec("wmic", "nic get AdapterType,Name,Installed,MACAddress,NetConnectionID,Speed", "network_adapters.log"); ReportProgress();
                Exec("net", "start", "running_services.log"); ReportProgress();
                Exec("net", "localgroup", "user_groups.log"); ReportProgress();
                Exec("wmic", "useraccount get name,sid,status,passwordrequired", "user_accounts.log"); ReportProgress();
                Exec("wmic", "qfe get hotfixid,description,installedby,installedon", "installed_updates.log"); ReportProgress();
                Exec("schtasks", "/query /fo LIST /v", "scheduled_tasks.log"); ReportProgress();
                Exec("wmic", "path Win32_PnPEntity get Caption,DeviceID,Manufacturer,PNPDeviceID", "pnp_devices.log"); ReportProgress();
                Exec("netsh", "advfirewall show allprofiles", "firewall.log"); ReportProgress();

                // Tree
                if (includeTree)
                {
                    try
                    {
                        UpdateStatus("Generating directory tree...");
                        string treeFile = Path.Combine(outputFolderPath, "directory_tree.log");
                        var psi = new ProcessStartInfo("cmd.exe", $"/c tree C:\\ /F /A > \"{treeFile}\"")
                        {
                            UseShellExecute = true,
                            CreateNoWindow = true
                        };
                        var p = Process.Start(psi);
                        p.WaitForExit();
                        
                        if (File.Exists(treeFile))
                        {
                            string content = File.ReadAllText(treeFile);
                            SaveLogWithHeader(content, treeFile);
                        }
                        else
                        {
                            errorsOccurred = true;
                            string treeError = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Directory tree generation failed:\n" +
                                             $"  Output file not created: {treeFile}\n" +
                                             $"  Process exit code: {p.ExitCode}\n" +
                                             new string('-', 80);
                            SaveLogWithHeader(treeError, errorFilePath);
                        }
                    }
                    catch (Exception treeEx)
                    {
                        errorsOccurred = true;
                        string treeError = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Directory tree exception:\n" +
                                         $"  Error: {treeEx.GetType().Name} - {treeEx.Message}\n" +
                                         $"  Stack trace: {treeEx.StackTrace}\n" +
                                         new string('-', 80);
                        SaveLogWithHeader(treeError, errorFilePath);
                    }
                    ReportProgress();
                }

                // Browsers
                if (includeBrowsers)
                {
                    UpdateStatus("Collecting browser data...");
                    string browserDataPath = Path.Combine(outputFolderPath, "browser_data");
                    Directory.CreateDirectory(browserDataPath);
                    StringBuilder browserInfo = new StringBuilder();

                    // Chrome
                    string chromePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), CHROME_PROFILE_PATH);
                    if (Directory.Exists(chromePath))
                    {
                        browserInfo.AppendLine("Google Chrome:");
                        browserInfo.AppendLine($"  Profile path: {chromePath}");
                        string chromeBackup = Path.Combine(browserDataPath, "Chrome_Default");
                        Directory.CreateDirectory(chromeBackup);
                        
                        try
                        {
                            foreach (var file in new[] { "Bookmarks", "Cookies", "History", "Login Data", "Preferences", "Web Data", "Favicons", "Shortcuts" })
                            {
                                string src = Path.Combine(chromePath, file);
                                if (File.Exists(src))
                                {
                                    CopyFileWithRetry(src, Path.Combine(chromeBackup, file));
                                    browserInfo.AppendLine($"  ✓ {file}");
                                }
                                else
                                {
                                    browserInfo.AppendLine($"  - {file} (not found)");
                                }
                            }
                            
                            string extDir = Path.Combine(chromePath, "Extensions");
                            if (Directory.Exists(extDir))
                            {
                                CopyDirectory(extDir, Path.Combine(chromeBackup, "Extensions"));
                                browserInfo.AppendLine($"  ✓ Extensions");
                            }
                                
                            browserInfo.AppendLine("  Backup completed.");
                        }
                        catch (Exception ex) 
                        { 
                            errorsOccurred = true;
                            browserInfo.AppendLine($"  Error: {ex.Message}");
                            string chromeError = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Chrome backup failed:\n" +
                                               $"  Profile: {chromePath}\n" +
                                               $"  Backup: {chromeBackup}\n" +
                                               $"  Error: {ex.GetType().Name} - {ex.Message}\n" +
                                               $"  Stack trace: {ex.StackTrace}\n" +
                                               new string('-', 80);
                            SaveLogWithHeader(chromeError, errorFilePath);
                        }
                    }
                    else
                    {
                        browserInfo.AppendLine("Google Chrome: Not installed or profile not found");
                        browserInfo.AppendLine($"  Expected path: {chromePath}");
                    }

                    // Firefox
                    string firefoxPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FIREFOX_PROFILE_PATH);
                    if (Directory.Exists(firefoxPath))
                    {
                        browserInfo.AppendLine("\nMozilla Firefox:");
                        browserInfo.AppendLine($"  Profiles path: {firefoxPath}");
                        foreach (string profile in Directory.GetDirectories(firefoxPath))
                        {
                            string profileName = Path.GetFileName(profile);
                            browserInfo.AppendLine($"  Profile: {profileName}");
                            string firefoxBackup = Path.Combine(browserDataPath, "Firefox_" + profileName);
                            Directory.CreateDirectory(firefoxBackup);
                            
                            try
                            {
                                foreach (var file in new[] { "places.sqlite", "cookies.sqlite", "formhistory.sqlite", "logins.json", "key4.db", "prefs.js", "addons.json" })
                                {
                                    string src = Path.Combine(profile, file);
                                    if (File.Exists(src))
                                    {
                                        CopyFileWithRetry(src, Path.Combine(firefoxBackup, file));
                                        browserInfo.AppendLine($"    ✓ {file}");
                                    }
                                    else
                                    {
                                        browserInfo.AppendLine($"    - {file} (not found)");
                                    }
                                }
                                browserInfo.AppendLine($"  {profileName}: Backup completed.");
                            }
                            catch (Exception ex) 
                            { 
                                errorsOccurred = true;
                                browserInfo.AppendLine($"  {profileName}: Error - {ex.Message}");
                                string ffError = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Firefox backup failed:\n" +
                                               $"  Profile: {profile}\n" +
                                               $"  Backup: {firefoxBackup}\n" +
                                               $"  Error: {ex.GetType().Name} - {ex.Message}\n" +
                                               $"  Stack trace: {ex.StackTrace}\n" +
                                               new string('-', 80);
                                SaveLogWithHeader(ffError, errorFilePath);
                            }
                        }
                    }
                    else
                    {
                        browserInfo.AppendLine("\nMozilla Firefox: Not installed or profile not found");
                        browserInfo.AppendLine($"  Expected path: {firefoxPath}");
                    }

                    // Edge
                    string edgePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), EDGE_PROFILE_PATH);
                    if (Directory.Exists(edgePath))
                    {
                        browserInfo.AppendLine("\nMicrosoft Edge:");
                        browserInfo.AppendLine($"  Profile path: {edgePath}");
                        string edgeBackup = Path.Combine(browserDataPath, "Edge_Default");
                        Directory.CreateDirectory(edgeBackup);
                        
                        try
                        {
                            foreach (var file in new[] { "Bookmarks", "Cookies", "History", "Login Data", "Preferences", "Web Data", "Favicons", "Shortcuts" })
                            {
                                string src = Path.Combine(edgePath, file);
                                if (File.Exists(src))
                                {
                                    CopyFileWithRetry(src, Path.Combine(edgeBackup, file));
                                    browserInfo.AppendLine($"  ✓ {file}");
                                }
                                else
                                {
                                    browserInfo.AppendLine($"  - {file} (not found)");
                                }
                            }
                            
                            string extDir = Path.Combine(edgePath, "Extensions");
                            if (Directory.Exists(extDir))
                            {
                                CopyDirectory(extDir, Path.Combine(edgeBackup, "Extensions"));
                                browserInfo.AppendLine($"  ✓ Extensions");
                            }
                                
                            browserInfo.AppendLine("  Backup completed.");
                        }
                        catch (Exception ex) 
                        { 
                            errorsOccurred = true;
                            browserInfo.AppendLine($"  Error: {ex.Message}");
                            string edgeError = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Edge backup failed:\n" +
                                             $"  Profile: {edgePath}\n" +
                                             $"  Backup: {edgeBackup}\n" +
                                             $"  Error: {ex.GetType().Name} - {ex.Message}\n" +
                                             $"  Stack trace: {ex.StackTrace}\n" +
                                             new string('-', 80);
                            SaveLogWithHeader(edgeError, errorFilePath);
                        }
                    }
                    else
                    {
                        browserInfo.AppendLine("\nMicrosoft Edge: Not installed or profile not found");
                        browserInfo.AppendLine($"  Expected path: {edgePath}");
                    }

                    SaveLogWithHeader(browserInfo.ToString(), Path.Combine(browserDataPath, "browser_profiles.log"));
                    ReportProgress();
                }

                // Compression
                if (includeZip)
                {
                    UpdateStatus("Compressing to ZIP...");
                    string zipPath = outputFolderPath + ".zip";
                    
                    try
                    {
                        // If ZIP exists, create with unique timestamp instead of deleting
                        if (File.Exists(zipPath))
                        {
                            zipPath = outputFolderPath + $"_{DateTime.Now:HHmmss}.zip";
                        }
                        
                        ZipFile.CreateFromDirectory(outputFolderPath, zipPath, CompressionLevel.Optimal, false);
                        UpdateStatus($"ZIP created: {Path.GetFileName(zipPath)}");
                    }
                    catch (Exception zipEx)
                    {
                        errorsOccurred = true;
                        string zipError = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ZIP compression failed:\n" +
                                        $"  ZIP path: {zipPath}\n" +
                                        $"  ZIP exists: {File.Exists(zipPath)}\n" +
                                        $"  Source folder: {outputFolderPath}\n" +
                                        $"  Source exists: {Directory.Exists(outputFolderPath)}\n" +
                                        $"  Error: {zipEx.GetType().Name} - {zipEx.Message}\n" +
                                        $"  Stack trace: {zipEx.StackTrace}\n";
                        
                        string sevenZipPath = Path.Combine(Application.StartupPath, SEVEN_ZIP_EXE);
                        if (File.Exists(sevenZipPath))
                        {
                            zipError += $"  Trying 7z.exe fallback: {sevenZipPath}\n";
                            try
                            {
                                var psi = new ProcessStartInfo(sevenZipPath, $"a -tzip \"{zipPath}\" \"{outputFolderPath}\\*\"")
                                {
                                    WindowStyle = ProcessWindowStyle.Hidden
                                };
                                var p = Process.Start(psi);
                                p.WaitForExit();
                                zipError += $"  7z.exe exit code: {p.ExitCode}\n";
                            }
                            catch (Exception ex7z)
                            {
                                zipError += $"  7z.exe also failed: {ex7z.Message}\n";
                            }
                        }
                        else
                        {
                            zipError += $"  7z.exe not found at: {sevenZipPath}\n";
                        }
                        
                        zipError += new string('-', 80);
                        SaveLogWithHeader(zipError, errorFilePath);
                    }
                    ReportProgress();
                }
                
                UpdateStatus("Completed!");
            }
            catch (Exception ex)
            {
                errorsOccurred = true;
                string criticalError = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] CRITICAL ERROR in CollectData:\n" +
                                      $"  Error type: {ex.GetType().Name}\n" +
                                      $"  Message: {ex.Message}\n" +
                                      $"  Stack trace:\n{ex.StackTrace}\n" +
                                      $"  Output folder: {outputFolderPath}\n";
                
                if (ex.InnerException != null)
                {
                    criticalError += $"  Inner exception: {ex.InnerException.GetType().Name}\n" +
                                   $"  Inner message: {ex.InnerException.Message}\n";
                }
                
                criticalError += new string('=', 80);
                
                SaveLogWithHeader(criticalError, errorFilePath);
                UpdateStatus($"Critical error: {ex.Message}");
            }
        }

        private void check_browsers_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
