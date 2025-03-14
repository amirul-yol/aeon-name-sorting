using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

namespace aeon_name_sorting
{
    public partial class Form1 : Form
    {
        private const string MAILER_PATTERN = @"(_M_N_|_Sup_Only_)";
        private readonly OpenFileDialog openFileDialog;

        public Form1()
        {
            InitializeComponent();
            
            // Initialize OpenFileDialog
            openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files|*.txt",
                Multiselect = true,
                Title = "Select Emboss/Mailer Files"
            };

            // Initialize ProgressBar
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
        }

        private async void btnSelectFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                // Disable button during processing
                btnSelectFiles.Enabled = false;
                progressBar.Value = 0;
                lblError.Text = "";

                var files = openFileDialog.FileNames;
                if (files.Length == 0)
                {
                    ShowError("No files selected.");
                    return;
                }

                // Group files by type
                var embossFiles = new List<string>();
                var mailerFiles = new List<string>();

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    if (Regex.IsMatch(fileName, MAILER_PATTERN))
                        mailerFiles.Add(file);
                    else
                        embossFiles.Add(file);
                }

                // Process each group
                var totalFiles = embossFiles.Count + mailerFiles.Count;
                var processedFiles = 0;

                // Process emboss files
                foreach (var file in embossFiles)
                {
                    var outputDir = Path.Combine(Path.GetDirectoryName(file)!, "sorted-outputs");
                    Directory.CreateDirectory(outputDir);
                    
                    UpdateStatus($"Processing emboss file: {Path.GetFileName(file)}");
                    await ProcessFile(file, outputDir);
                    processedFiles++;
                    UpdateProgress((int)((float)processedFiles / totalFiles * 100));
                }

                // Process mailer files
                foreach (var file in mailerFiles)
                {
                    var outputDir = Path.Combine(Path.GetDirectoryName(file)!, "sorted-outputs");
                    Directory.CreateDirectory(outputDir);
                    
                    UpdateStatus($"Processing mailer file: {Path.GetFileName(file)}");
                    await ProcessFile(file, outputDir);
                    processedFiles++;
                    UpdateProgress((int)((float)processedFiles / totalFiles * 100));
                }

                UpdateStatus("Processing complete!");
            }
            catch (Exception ex)
            {
                ShowError($"Error: {ex.Message}");
            }
            finally
            {
                btnSelectFiles.Enabled = true;
            }
        }

        private async Task ProcessFile(string inputFile, string outputDir)
        {
            var fileName = Path.GetFileName(inputFile);
            var outputFile = Path.Combine(outputDir, 
                Path.GetFileNameWithoutExtension(fileName) + "-sorted.txt");

            // Read all lines
            var lines = await File.ReadAllLinesAsync(inputFile);

            // Split lines into sequence and data parts
            var splitLines = lines.Select(line =>
            {
                var firstDot = line.IndexOf('.');
                return new
                {
                    OriginalSequence = line.Substring(0, firstDot + 1),
                    Data = line.Substring(firstDot + 1),
                    OriginalLine = line
                };
            }).ToArray();

            // Sort based on names while keeping original data
            Array.Sort(splitLines, (a, b) =>
            {
                var nameA = ExtractName(a.OriginalLine);
                var nameB = ExtractName(b.OriginalLine);
                return string.Compare(nameA, nameB, StringComparison.OrdinalIgnoreCase);
            });

            // Recombine with sequential numbers
            var sortedLines = splitLines.Select((x, index) =>
            {
                var newSequence = $"{(index + 1):000000}.";
                return newSequence + x.Data;
            }).ToArray();

            // Write the sorted lines
            await File.WriteAllLinesAsync(outputFile, sortedLines);
        }

        private string ExtractName(string line)
        {
            try
            {
                // Name field starts at column 61
                return line.Substring(60).Trim();
            }
            catch
            {
                return "";
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            UpdateStatus("Error occurred!");
        }

        private void UpdateStatus(string status)
        {
            lblStatus.Text = status;
            Application.DoEvents();
        }

        private void UpdateProgress(int value)
        {
            progressBar.Value = Math.Min(100, Math.Max(0, value));
            Application.DoEvents();
        }
    }
}
