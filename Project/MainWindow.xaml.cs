using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Text_Holder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    /*
     * Todo 
     * 
    */
    public partial class MainWindow : Window
    {
        private readonly string fileStorage = @"C:\Program Files\TextHolder\TextData";
        private string currentFile = "default.txt";
        public MainWindow()
        {
            InitializeComponent();
        }
        private List<string> Search(string keyword)
        {
            keyword=keyword.ToLower();
            List<string> lines = new List<string>();
            using (StreamReader file = new StreamReader(fileStorage + @"\" + currentFile))
            {
                string currentLine;
                while ((currentLine = file.ReadLine()) != null)
                {
                    if (currentLine.ToLower().Contains(keyword))
                    {
                        lines.Add(currentLine);
                    }
                }
            }
            return lines;
        }

        private void RefreshFunction()
        {
            using (StreamReader file = new StreamReader(fileStorage + @"\" + currentFile))
            {
                OutputBox.Text = file.ReadToEnd();
                CurrentFileDisplay.Text = "Current File: " + currentFile;
            }
        }

        private void AppendButton_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter file = new StreamWriter(fileStorage + @"\" + currentFile, append : true))
            {
                file.WriteLine(InputBox.Text);
            }
            RefreshFunction();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            List<string> linesToKeep = new List<string>();
            List<string> linesToDelete = new List<string>();
            List<string> originalList = new List<string>();
            using (StreamReader file = new StreamReader(fileStorage + @"\" + currentFile)) // creates three lists. one of the whole file itself, and the other dividing the file into delete and keep.
            {
                string currentLine;
                while ((currentLine = file.ReadLine()) != null)
                {
                    originalList.Add(currentLine);
                    if (currentLine.ToLower().Contains(InputBox.Text.ToLower()))
                    {
                        linesToDelete.Add(currentLine);
                    } else
                    {
                        linesToKeep.Add(currentLine);
                    }
                }
            }
            if (linesToDelete.Count >= 2)
            {
                MessageBoxResult result = MessageBox.Show(linesToDelete.Count + " items have been found. Would you like to delete all of them? Select no to delete them on a case-by-case basis.", "Delete Multiple Entries", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                
                if (result == MessageBoxResult.No) // delete case-by-case
                {
                    int currentIndex = 0;
                    int offset = 0;
                    foreach (string line in linesToDelete)
                    {
                        MessageBoxResult marginalResult = MessageBox.Show("Delete \"" + line + "\"?", "Delete Individual Item", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (marginalResult == MessageBoxResult.No)
                        {
                            currentIndex = originalList.FindIndex(currentIndex, x => x.Equals(line)) - offset + 1;
                            linesToKeep.Insert(currentIndex - 1, line);
                        } 
                        else if (marginalResult == MessageBoxResult.Yes) {
                            offset++;
                        } 
                        else // cancel
                        {
                            for (int i = currentIndex; i < originalList.Count; i++)
                            {
                                linesToKeep.Add(originalList[i]);
                            }
                            break;
                        }
                    }
                }
                if (result != MessageBoxResult.Cancel)
                {
                    using (StreamWriter file = new StreamWriter(fileStorage + @"\" + currentFile))
                    {
                        foreach (string line in linesToKeep)
                        {
                            file.WriteLine(line);
                        }
                    }
                }
            }
            else if (linesToDelete.Count == 1)
            {
                using (StreamWriter file = new StreamWriter(fileStorage + @"\" + currentFile))
                {
                    foreach (string line in linesToKeep)
                    {
                        file.WriteLine(line);
                    }
                }
            } else
            {
                InputBox.Text = "No such entry exists";
             }
            RefreshFunction();
        }
        
       
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshFunction();
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> text = Search(SearchInput.Text);
            string finalText = "";
            foreach (string current in text)
            {
                finalText += current + "\n";
            }
            OutputBox.Text = finalText;
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            string inputtedFile = FileInput.Text;
            if (!inputtedFile.Contains(".txt"))
            {
                inputtedFile += ".txt";
            }
            if (File.Exists(fileStorage + @"\" + inputtedFile))
            {
                FileInput.Text = "A file with that name already exists";
            }
            else
            {
                File.Create(fileStorage + @"\" + inputtedFile).Close();
                currentFile = inputtedFile;
                RefreshFunction();
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            string inputtedFile = FileInput.Text;
            if (!inputtedFile.Contains(".txt"))
            {
                inputtedFile += ".txt";
            }
            if (File.Exists(fileStorage + @"\" + inputtedFile))
            {
                currentFile = inputtedFile;
                RefreshFunction();
            } else
            {
                FileInput.Text = "No such file exists";
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = currentFile; // Default file name
            dlg.InitialDirectory = fileStorage;
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            dlg.CheckFileExists = true;

            // Show open file dialog box
            bool? wentThrough = dlg.ShowDialog();

            if (wentThrough == true)
            {
                FileInput.Text = dlg.SafeFileName;
                currentFile = dlg.SafeFileName;
                RefreshFunction();
            } else
            {
                FileInput.Text = "Invalid file";
            }
            
        }
    }
}
