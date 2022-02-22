using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataLoader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
        private readonly string _sizeLimitInBytes = "50,000,000"; // 50 MB.

        private readonly string _charsToReplace = "“=>\", ”=>\", ‘=>', ’=>'";

        private List<string> _items_All = new List<string>();
        private List<string> _items_Split = new List<string>();
        private List<string> _items_Replace = new List<string>();

        private CancellationTokenSource _cancelationTokenSource_All;
        private CancellationTokenSource _cancelationTokenSource_Split;
        private CancellationTokenSource _cancelationTokenSource_Replace;
        public MainWindow()
        {
            InitializeComponent();
            InputReplace_All.Text = _charsToReplace;
            InputReplace_Replace.Text = _charsToReplace;
            InputSplit_Split.Text = _sizeLimitInBytes;
            InputSplit_All.Text = _sizeLimitInBytes;
        }

        private void DropFilesFolders(object sender, DragEventArgs e)
        {
            var type = ((StackPanel)sender).Tag.ToString();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var label = string.Join('\n', files);
                switch (type)
                {
                    case "All":
                        FilesAndFoldersLabel_All.Content = label;
                        _items_All = files.ToList();
                        break;
                    case "Split":
                        FilesAndFoldersLabel_Split.Content = label;
                        _items_Split = files.ToList();
                        break;
                    case "Replace":
                        FilesAndFoldersLabel_Replace.Content = label;
                        _items_Replace = files.ToList();
                        break;
                }
            }
        }
        private async void Process_Click(object sender, RoutedEventArgs e)
        {
            var type = ((Button)sender).Tag.ToString();
            var _items = new List<string>();
            var _sizeLimitInBytes = 0;
            var generatedFolderName = $"__Processed__ ({type})";
            var processingLabel = "Processing...";
            var cancelLabel = "Cancel";
            var processLabel = "Start";
            var cancellationToken = new CancellationTokenSource();

            switch (type)
            {
                case "All":
                    if (Process_All.Content == cancelLabel)
                    {
                        _cancelationTokenSource_All.Cancel();
                        return;
                    }
                    Status_All.Content = processingLabel;
                    Process_All.Content = cancelLabel;
                    _cancelationTokenSource_All = new CancellationTokenSource();
                    cancellationToken = _cancelationTokenSource_All;
                    _items = _items_All;
                    _sizeLimitInBytes = int.Parse(InputSplit_All.Text.Replace(",", ""));
                    break;
                case "Split":
                    if (Process_Split.Content == cancelLabel)
                    {
                        _cancelationTokenSource_Split.Cancel();
                        return;
                    }
                    Status_Split.Content = processingLabel;
                    Process_Split.Content = cancelLabel;

                    _cancelationTokenSource_Split = new CancellationTokenSource();
                    cancellationToken = _cancelationTokenSource_Split;
                    _items = _items_Split;
                    _sizeLimitInBytes = int.Parse(InputSplit_Split.Text.Replace(",", ""));
                    break;
                case "Replace":
                    if (Process_Replace.Content == cancelLabel)
                    {
                        _cancelationTokenSource_Replace.Cancel();
                        return;
                    }
                    Status_Replace.Content = processingLabel;
                    Status_Replace.Content = cancelLabel;
                    _cancelationTokenSource_Replace = new CancellationTokenSource();
                    cancellationToken = _cancelationTokenSource_Replace;
                    _items = _items_Replace;
                    break;
            }

            if (_items.Count() == 0)
            {
                Cancel(type, processLabel, cancellationToken);
            }

            await Task.Delay(51);


            foreach (var itemPath in _items)
            {
                string dirPath = Path.GetDirectoryName(itemPath);

                string destDirPath = Path.Combine(dirPath, generatedFolderName);
                if (Directory.Exists(destDirPath))
                {
                    Directory.Delete(destDirPath, true);
                }
            }

            foreach (var itemPath in _items)
            {
                FileAttributes attr = File.GetAttributes(itemPath);

                var filePaths = new List<string> { itemPath };

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    filePaths = Directory.GetFiles(itemPath).ToList();
                }

                string dirPath = Path.GetDirectoryName(itemPath);

                string destDirPath = Path.Combine(dirPath, generatedFolderName);
                if (!Directory.Exists(destDirPath))
                {
                    Directory.CreateDirectory(destDirPath);
                }

                foreach (var filePath in filePaths)
                {
                    if (
                        _cancelationTokenSource_All is not null && _cancelationTokenSource_All.IsCancellationRequested ||
                        _cancelationTokenSource_Split is not null && _cancelationTokenSource_Split.IsCancellationRequested||
                        _cancelationTokenSource_Replace is not null && _cancelationTokenSource_Replace.IsCancellationRequested)
                    {
                        Cancel(type, processLabel, cancellationToken);
                        return;
                    }

                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    string fileExtension = Path.GetExtension(filePath);
                    if (fileExtension.ToUpper() != ".CSV")
                    {
                        continue;
                    }
                    var content = await File.ReadAllTextAsync(filePath);

                    var processedContent = content;

                    var fileInfo = new FileInfo(filePath);
                    var fileLength = fileInfo.Length;
                    if (type == "Replace" || type == "All")
                    {
                        var input = InputReplace_All.Text;
                        if (type == "Replace")
                        {
                            input = InputReplace_Replace.Text;

                        }
                        processedContent = await ProcessContentAsync(content, input);

                        fileLength = processedContent.Length;
                    }


                    var processedContents = new List<string> { processedContent };

                    if (type == "Split" || type == "All")
                    {
                        if (fileLength > _sizeLimitInBytes)
                        {
                            var numberOfFiles = Math.Ceiling(fileLength / (_sizeLimitInBytes * 0.95));

                            processedContents = await SplitContentAsync(processedContent, (int)numberOfFiles);
                        }
                    }

                    for (int index = 0; index < processedContents.Count; index++)
                    {
                        var cont = processedContents[index];

                        var newFilePath = Path.Combine(destDirPath, $"{fileNameWithoutExtension}__{index}{fileExtension}");

                        if (processedContents.Count == 1)
                        {
                            newFilePath = Path.Combine(destDirPath, $"{fileNameWithoutExtension}{fileExtension}");
                        }

                        File.WriteAllText(newFilePath, cont, System.Text.Encoding.UTF8);
                    }
                }


                switch (type)
                {
                    case "All":
                        Status_All.Content = "Done.";
                        Process_All.Content = processLabel;
                        break;
                    case "Split":
                        Status_Split.Content = "Done.";
                        Process_Split.Content = processLabel;
                        break;
                    case "Replace":
                        Status_Replace.Content = "Done.";
                        Process_Replace.Content = processLabel;
                        break;
                }
            }
        }

        private void Cancel(string type, string processLabel, CancellationTokenSource tokenSource)
        {
            switch (type)
            {
                case "All":
                    Status_All.Content = "Cancelled.";
                    Process_All.Content = processLabel;
                    break;
                case "Split":
                    Status_Split.Content = "Cancelled.";
                    Process_Split.Content = processLabel;
                    break;
                case "Replace":
                    Status_Replace.Content = "Cancelled.";
                    Process_Replace.Content = processLabel;
                    break;
            }

        }
        public async Task<string> ProcessContentAsync(string content, string input)
        {
            var processedContent = content;

            var processedInputs = input.Split(",").Select(x =>
            {
                if (x.Contains("=>"))
                {
                    return x.Split("=>");
                }
                else
                {
                    return null;
                }

            }).Where(x => x is not null).ToList();

            foreach (var inp in processedInputs)
            {

                var from = inp[0].Trim();
                var to = inp[1].Trim();
                if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                {
                    processedContent = processedContent.Replace(from, to);
                }
            }
            //var processedContent = Regex
            //    .Replace(content, @"[\u2018\u2019]", "'");

            //processedContent = Regex
            //    .Replace(processedContent, @"[\u201C\u201D]", "\"");

            return processedContent;
        }

        public async Task<List<string>> SplitContentAsync(string content, int numberOfSubContents)
        {
            var results = new List<string>();

            //var splitted = content.Split('\n');
            //var linesCount = splitted.Count();

            //var childLines = (int)Math.Ceiling((decimal)linesCount / numberOfSubContents);

            //for (var index = 0; index < numberOfSubContents; index++)
            //{
            //    var partial = splitted.Skip(index * childLines).Take(childLines);
            //    var final = string.Join('\n', partial).Trim();

            //    if (final != "")
            //    {
            //        results.Add(final);
            //    }
            //}

            //return results;


            var reg = @"\n";
            var contentLength = content.Length;
            var subLength = Math.Ceiling((decimal)contentLength / numberOfSubContents);
            var matches = Regex.Matches(content, reg);
            var totalMatchCount = matches.Count();

            var lastIndex = contentLength - 1;

            var remaining = numberOfSubContents;


            for (var index = 0; index < totalMatchCount; index++)
            {
                var i = totalMatchCount - index - 1;
                var indexTarget = subLength * (remaining - 1);
                var match = matches[i];
                var matchIndex = match.Index;
                if (matchIndex <= indexTarget)
                {
                    var splitted = content.Substring(matchIndex, lastIndex - matchIndex);
                    lastIndex = matchIndex;
                    if (splitted.Length > 0)
                    {
                        results.Add(splitted.Trim());
                        remaining--;
                    }
                }
            }

            if (lastIndex > 0)
            {
                var splitted = content.Substring(0, lastIndex);
                results.Add(splitted);
            }

            results.Reverse();

            return results;
        }


    }
}
