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

namespace Duplicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSourceDirectory_Click(object sender, RoutedEventArgs e)
        {
            txtSourceDirectory.Text = OpenDirectoryDialog();
        }

        private void btnTargetDirectory_Click(object sender, RoutedEventArgs e)
        {
            txtTargetDirectory.Text = OpenDirectoryDialog();
        }

        private string OpenDirectoryDialog()
        {
            Gat.Controls.OpenDialogView openDialog = new Gat.Controls.OpenDialogView();
            Gat.Controls.OpenDialogViewModel vm = (Gat.Controls.OpenDialogViewModel)openDialog.DataContext;

            vm.IsDirectoryChooser = true;
            vm.IsSaveDialog = false;
            
            bool? result = vm.Show();
            if (result == true)
            {
                return vm.SelectedFolder.Path;
            }
            
            return string.Empty;
        }

        private void btnDuplicate_Click(object sender, RoutedEventArgs e)
        {
            DirectoryCopy(txtSourceDirectory.Text, 
                txtTargetDirectory.Text, true);

            lblMessage.Content = "Duplicate complete!";
        }

        // https://msdn.microsoft.com/en-us/library/bb762914(v=vs.110).aspx
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
