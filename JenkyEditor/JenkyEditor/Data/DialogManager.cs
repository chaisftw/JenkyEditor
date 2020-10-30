using System;
using System.IO;
using System.Windows.Forms;

using Microsoft.Xna.Framework.Graphics;

namespace JenkyEditor
{
    public class DialogManager
    {
        public void SelectPng(string initialDirectory, string destination)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = initialDirectory;
            dlg.Filter = "Image Files|*.png;";
            dlg.Title = "Add tile image to project";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if(File.Exists(destination))
                {
                    File.Copy(dlg.FileName, destination, true);
                }

            }
        }

        public string GetProjectPath(string initialDirectory)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Select Project Folder";
            dlg.SelectedPath = initialDirectory;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.SelectedPath;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetImagePath(string initialDirectory)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = initialDirectory;
            dlg.Filter = "Image Files|*.png;";
            dlg.Title = "Add tile image to project";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
