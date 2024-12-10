using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Project
{
    public static class clsUtil
    {
        public static string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static string RenameImage(string sourceFilePath)
        {
            // Generate a unique name for the copied file
            string uniqueFileName = GenerateGuid() + Path.GetExtension(sourceFilePath);
            return uniqueFileName;
        }

        public static string CreateFolder()
        {
            string destinationFolder = "C:\\DVLD People Images";

            if (!File.Exists(destinationFolder))
            {
                // Ensure the directory exists
                Directory.CreateDirectory(destinationFolder);
                return destinationFolder;
            }

            return "";
        }

        public static string CopyToFolder(string sourceFilePath)
        {
            string destinationFolder = CreateFolder();

            if(destinationFolder == "") 
            { 
                return ""; 
            }

            // Combine the destination folder and the unique file name
            string destinationFilePath = Path.Combine(destinationFolder, RenameImage(sourceFilePath));

            // Copy the file
            File.Copy(sourceFilePath, destinationFilePath);

            return destinationFilePath;
        }
    }
}
