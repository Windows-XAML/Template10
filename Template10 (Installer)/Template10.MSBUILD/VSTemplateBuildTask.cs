using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;
using System.Xml.Linq;
using System.Collections;
using System.Xml.XPath;
using System.Xml;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace Template10.MSBUILD
{

    /// <summary>
    /// A custom MS BUILD Task that generates a Project Template ZIP file
    /// and copies to the specified location. This is used to covert the existing
    /// Template 10 projects into project templates for deployment via the
    /// the VSIX.
    /// </summary>
    public class VSTemplateBuildTask : Microsoft.Build.Utilities.Task
    {
        #region ---- Private Variables ----------------

        private string tempFolder;
        private ItemFolder topFolder;
        bool helpFileReferenceExists = false;

        #endregion

        #region ---- public properties  -------
        /// <summary>
        /// Gets or sets the csproj file.
        /// </summary>
        /// <value>
        /// The csproj file.
        /// </value>
        [Required]
        public string CsprojFile { get; set; }

        /// <summary>
        /// Gets or sets the name of the zip.
        /// </summary>
        /// <value>
        /// The name of the zip.
        /// </value>
        [Required]
        public string ZipName { get; set; }

        /// <summary>
        /// Gets or sets the help URL.
        /// </summary>
        /// <value>
        /// The help URL.
        /// </value>
        [Required]
        public string HelpUrl { get; set; }


        /// <summary>
        /// Gets or sets the project description.
        /// </summary>
        /// <value>
        /// The project description.
        /// </value>
        [Required]
        public string ProjectDescription { get; set; }

        /// <summary>
        /// Gets or sets the preview image path.
        /// </summary>
        /// <value>
        /// The preview image path.
        /// </value>
        [Required]
        public string PreviewImagePath { get; set; }


        /// <summary>
        /// Gets or sets the name of the project friendly.
        /// </summary>
        /// <value>
        /// The name of the project friendly.
        /// </value>
        [Required]
        public string ProjectFriendlyName { get; set; }


        /// <summary>
        /// Gets or sets the source dir.
        /// </summary>
        /// <value>
        /// The source dir.
        /// </value>
        [Required]
        public string SourceDir { get; set; }

        public string TargetDir2 { get; set; }

        /// <summary>
        /// Gets or sets the target dir.
        /// </summary>
        /// <value>
        /// The target dir.
        /// </value>
        public string TargetDir { get; set; }

        #endregion


        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            helpFileReferenceExists = false;
            tempFolder = Path.Combine(TargetDir, Constants.TEMPFOLDER);
            if (Directory.Exists(tempFolder))
            {
                Directory.Delete(tempFolder, true);
            }

            string projectFolder = Path.GetDirectoryName(CsprojFile);
            CopyProjectFilesToTempFolder(projectFolder, tempFolder);
            ReplaceNamespace(tempFolder);
            FileHelper.DeleteKey(tempFolder);
            ProcessVSTemplate(tempFolder);
            OperateOnCsProj(tempFolder, CsprojFile);
            OperateOnManifest(Path.Combine(tempFolder, "Package.appxmanifest"));
            CopyEmbeddedFilesToOutput(tempFolder);
            string jsonProj = Path.Combine(tempFolder, Constants.PROJECTJSON);
            AddTemplate10Nuget(jsonProj);
            SetupHelpFile(Path.Combine(tempFolder, Constants.HELPHTML), HelpUrl);
            ZipFiles(tempFolder, ZipName, TargetDir);
            return true;
        }

        /// <summary>
        /// Replaces the namespace.
        /// </summary>
        /// <param name="tempFolder">The temporary folder.</param>
        private void ReplaceNamespace(string tempFolder)
        {
            string csprojXml = FileHelper.ReadFile(CsprojFile);
            string rootNamespace = GetExistingRootNamespace(csprojXml);
            var ext = new List<string> { ".cs", ".xaml"};
            var files = Directory.GetFiles(tempFolder, "*.*", SearchOption.AllDirectories).Where(s => ext.Any(e => s.EndsWith(e)));
            foreach (var file in files)
            {
                string text = FileHelper.ReadFile(file);
                //TODO: think about a safer way to do this... what if there is another use of RootNamespace string elsewhere... this will break the generated project.
                text = text.Replace(rootNamespace, "$safeprojectname$");
                FileHelper.WriteFile(file, text);
            }
        }

        /// <summary>
        /// Operates the on manifest.
        /// </summary>
        /// <param name="manifestFile">The manifest file.</param>
        private void OperateOnManifest(string manifestFile)
        {
            string manifestText = FileHelper.ReadFile(manifestFile);

            var replacements = new List<FindReplaceItem>();

            replacements.Add(new FindReplaceItem() { Pattern = "<mp:PhoneIdentity(.*?)/>", Replacement = @"<mp:PhoneIdentity PhoneProductId=""$$guid9$$"" PhonePublisherId=""00000000-0000-0000-0000-000000000000""/>" });
            replacements.Add(new FindReplaceItem() { Pattern = "<DisplayName>(.*?)</DisplayName>", Replacement = @"<DisplayName>$$projectname$$</DisplayName>" });
            replacements.Add(new FindReplaceItem() { Pattern = "<PublisherDisplayName>(.*?)</PublisherDisplayName>", Replacement = @"<PublisherDisplayName>$$XmlEscapedPublisher$$</PublisherDisplayName>" });
            replacements.Add(new FindReplaceItem() { Pattern = @"Executable=""(.*?)""", Replacement = @"Executable=""$$targetnametoken$$.exe""" });
            replacements.Add(new FindReplaceItem() { Pattern = @"EntryPoint=""(.*?)""", Replacement = @"EntryPoint=""$$safeprojectname$$.App""" });
            replacements.Add(new FindReplaceItem() { Pattern = @"DisplayName=""(.*?)""", Replacement = @"DisplayName=""$$projectname$$.App""" });
            replacements.Add(new FindReplaceItem() { Pattern = @"EntryPoint=""(.*?)""", Replacement = @"EntryPoint=""$$projectname$$.App""" });

            foreach (var item in replacements)
            {
                manifestText = Regex.Replace(manifestText, item.Pattern, item.Replacement);
            }

            manifestText = ReplaceIdentityNode(manifestText);

            FileHelper.WriteFile(manifestFile, manifestText);
        }

        /// <summary>
        /// Adds the template10 nuget.
        /// </summary>
        /// <param name="jsonProj">The json proj.</param>
        public void AddTemplate10Nuget(string jsonProj)
        {
            string txt = FileHelper.ReadFile(jsonProj);

            if (txt.Contains(Constants.TEMPLATE10))
            {
                return;
            }

            int startIndex = txt.IndexOf(@"""dependencies"": {");

            string template10Txt = Constants.TEMPLATE10PROJECTJSON;
            int insertIndex = txt.IndexOf("},", startIndex) - 4;
            txt = txt.Insert(insertIndex, "," + Environment.NewLine + template10Txt);
            FileHelper.WriteFile(jsonProj, txt);
        }

        /// <summary>
        /// Sets up the help file, basically changing the redirect.
        /// </summary>
        /// <param name="helpFile">The help file.</param>
        /// <param name="helpUrl">The help URL.</param>
        private void SetupHelpFile(string helpFile, string helpUrl)
        {
            if (!File.Exists(helpFile))
                return;

            string helpText = FileHelper.ReadFile(helpFile);
            helpText = helpText.Replace(Constants.HELPURL, helpUrl);
            FileHelper.WriteFile(helpFile, helpText);
        }

        /// <summary>
        /// Copies the project files to temporary folder.
        /// </summary>
        /// <param name="projectFolder">The project folder.</param>
        /// <param name="tempFolder">The temporary folder.</param>
        private void CopyProjectFilesToTempFolder(string projectFolder, string tempFolder)
        {
            FileHelper.DirectoryCopy(projectFolder, tempFolder, true);
        }

        /// <summary>
        /// Processes the vs template.
        /// </summary>
        /// <param name="tempFolder">The temporary folder.</param>
        private void ProcessVSTemplate(string tempFolder)
        {
            string xml = FileHelper.ReadFile(CsprojFile);
            string projectName = Path.GetFileName(CsprojFile);
            string projXml = GetProjectNode(xml, projectName);
            xml = Constants.VSTEMPLATETEXT.Replace(Constants.PROJECTNODE, projXml);
            xml = xml.Replace(Constants.TEMPLATENAME, ProjectFriendlyName);
            xml = xml.Replace(Constants.TEMPLATEDESCRIPTION, ProjectDescription);
            string previewFileName = Path.GetFileName(PreviewImagePath);
            xml = xml.Replace(Constants.PREVIEWIMAGEFILE, previewFileName);

            string filePath = Path.Combine(tempFolder, Constants.VSTEMPLATENAME);

            FileHelper.WriteFile(filePath, xml);
        }

        /// <summary>
        /// Zips the files.
        /// </summary>
        /// <param name="tempFolder">The temporary folder.</param>
        /// <param name="zipName">Name of the zip.</param>
        /// <param name="targetDir">The target dir.</param>
        private void ZipFiles(string tempFolder, string zipName, string targetDir)
        {
            string zipFileName = Path.Combine(targetDir, ZipName);

            if (File.Exists(zipFileName))
            {
                File.Delete(zipFileName);
            }
            ZipFile.CreateFromDirectory(tempFolder, zipFileName);

            //-- now second one...
            if (!string.IsNullOrWhiteSpace(TargetDir2))
            {
                string zipFileName2 = Path.Combine(TargetDir2, ZipName);
                File.Copy(zipFileName, zipFileName2, true);
            }

            //-- clean up the temporary folder
            Directory.Delete(tempFolder, true);
        }

        /// <summary>
        /// Copies the embedded files to output.
        /// </summary>
        /// <param name="targetDir">The target dir.</param>
        private void CopyEmbeddedFilesToOutput(string targetDir)
        {
            string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            foreach (var item in names)
            {
                using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(item))
                {
                    var targetFile = Path.Combine(targetDir, Path.GetFileName(item.Substring(item.LastIndexOf("EmbeddedFiles.") + 14)));

                    using (var fileStream = File.Create(targetFile))
                    {
                        s.Seek(0, SeekOrigin.Begin);
                        s.CopyTo(fileStream);
                    }
                }
            }

        }

        /// <summary>
        /// Operates the on cs proj.
        /// </summary>
        /// <param name="tempFolder">The temporary folder.</param>
        /// <param name="csprojFile">The csproj file.</param>
        private void OperateOnCsProj(string tempFolder, string csprojFile)
        {
            string fileName = Path.GetFileName(CsprojFile);
            string targetPath = Path.Combine(tempFolder, fileName);

            File.Copy(CsprojFile, targetPath, true);
            string csprojText = FileHelper.ReadFile(targetPath);

            var replacements = new List<FindReplaceItem>();

            replacements.Add(new FindReplaceItem() { Pattern = @"<PackageCertificateKeyFile>(.*?)</PackageCertificateKeyFile>", Replacement = @"<PackageCertificateKeyFile>$$projectname$$_TemporaryKey.pfx</PackageCertificateKeyFile>" });
            replacements.Add(new FindReplaceItem() { Pattern = "<RootNamespace>(.*?)</RootNamespace>", Replacement = "<RootNamespace>$$safeprojectname$$</RootNamespace>" });
            replacements.Add(new FindReplaceItem() { Pattern = "<AssemblyName>(.*?)</AssemblyName>", Replacement = "<AssemblyName>$$safeprojectname$$</AssemblyName>" });
            replacements.Add(new FindReplaceItem() { Pattern = @"<None Include=""(.*?)_TemporaryKey.pfx"" />", Replacement = @"<None Include=""$$projectname$$_TemporaryKey.pfx"" />" });
            replacements.Add(new FindReplaceItem() { Pattern = @"<ProjectGuid>(.*?)</ProjectGuid>", Replacement = @"<ProjectGuid>$guid1$</ProjectGuid>" });


            foreach (var item in replacements)
            {
                csprojText  = Regex.Replace(csprojText, item.Pattern, item.Replacement);
            }

            csprojText = RemoveItemNodeAround(@"csproj", csprojText);

            csprojText = AddHelpToCSProj(csprojText);

            FileHelper.WriteFile(targetPath, csprojText);
        }

        /// <summary>
        /// Adds the help to cs proj.
        /// </summary>
        /// <param name="csprojText">The csproj text.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string AddHelpToCSProj(string csprojText)
        {
            //<Content Include="Help.htm" />
            //    <Content Include="Properties\Default.rd.xml" />

            if (csprojText.ToLower().Contains("help.htm"))
            {
                return csprojText;
            }

            string findText = @"<Content Include=""Properties\Default.rd.xml"" />";
            string helpText = @"<Content Include=""Help.htm"" />";

            csprojText = csprojText.Replace(findText, helpText + findText);
            return csprojText;
        }

        /// <summary>
        /// Gets the existing root namespace.
        /// </summary>
        /// <param name="csprojxml">The csprojxml.</param>
        /// <returns></returns>
        private string GetExistingRootNamespace(string csprojxml)
        {
            XDocument xdoc;
            using (StringReader sr = new StringReader(csprojxml))
            {
                xdoc = XDocument.Load(sr, LoadOptions.None);
            }

            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            return xdoc.Descendants(ns + "RootNamespace").FirstOrDefault().Value;

        }

        /// <summary>
        /// Removes the item node around the specified text. This is used to remove the csproj reference in the csproj file which will be replaced by a project.json NuGet reference instead.
        /// </summary>
        /// <param name="findText">The find text.</param>
        /// <param name="csprojText">The csproj text.</param>
        /// <returns></returns>
        private string RemoveItemNodeAround(string findText, string csprojText)
        {
            if (!csprojText.Contains(findText))
            {
                return csprojText;
            }

            int findTextIndex, start, end;
            string firstHalf, lastHalf;

            findTextIndex = csprojText.IndexOf(findText);

            start = csprojText.Substring(0, findTextIndex).LastIndexOf("<ItemGroup>");
            end = csprojText.IndexOf("</ItemGroup>", findTextIndex);
            firstHalf = csprojText.Substring(0, start);
            lastHalf = csprojText.Substring(end + 12);
            return firstHalf + lastHalf;
        }

        /// <summary>
        /// Replaces the identity node.
        /// </summary>
        /// <param name="manifestText">The manifest text.</param>
        /// <returns></returns>
        private string ReplaceIdentityNode(string manifestText)
        {
            string findText = @"<Identity";
            if (!manifestText.Contains(findText))
            {
                return manifestText;
            }

            string identityReplacementText = @"<Identity
    Name=""$guid9$""
    Publisher = ""$XmlEscapedPublisherDistinguishedName$""
    Version = ""1.0.0.0"" /> ";

            int findTextIndex, start, end;
            string firstHalf, lastHalf;

            findTextIndex = manifestText.IndexOf(findText);

            start = findTextIndex;
            end = manifestText.IndexOf("/>", findTextIndex);
            firstHalf = manifestText.Substring(0, start);
            lastHalf = manifestText.Substring(end + 2);
            return firstHalf + identityReplacementText + lastHalf;
        }


        /// <summary>
        /// Gets the project node.
        /// </summary>
        /// <param name="csprojxml">The csprojxml.</param>
        /// <param name="projectFileName">Name of the project file.</param>
        /// <returns></returns>
        public string GetProjectNode(string csprojxml, string projectFileName)
        {
            string projectNodeStart = @"<Project TargetFileName=""$projectName"" File=""$projectName"" ReplaceParameters=""true"">";
            projectNodeStart = projectNodeStart.Replace("$projectName", projectFileName);
            //string projectName = GetProjectName(csprojxml);
            List<string> projectItems = GetProjectItems(csprojxml);

            //-- sorting for directories
            projectItems = SortProjectItems(projectItems);
            GetItemFolder(projectItems);
            string foldersString = SerializeFolder(topFolder);
            if (!helpFileReferenceExists)
            {
                foldersString = InsertHelp(foldersString);
            }

            using (StringWriter writer = new StringWriter())
            {
                writer.WriteLine(projectNodeStart);
                writer.WriteLine(foldersString);
                writer.WriteLine("</Project>");

                return writer.ToString();
            }

        }

        /// <summary>
        /// Inserts the help.
        /// </summary>
        /// <param name="foldersString">The folders string.</param>
        /// <returns></returns>
        private string InsertHelp(string foldersString)
        {
            string helpString = @"<ProjectItem ReplaceParameters=""false"" TargetFileName=""help.htm"" OpenInWebBrowser=""true"">help.htm</ProjectItem>";
            return helpString + foldersString;
        }

        /// <summary>
        /// Serializes the folder.
        /// </summary>
        /// <param name="topFolder">The top folder.</param>
        /// <returns></returns>
        private string SerializeFolder(ItemFolder topFolder)
        {
            string folderString = string.Empty;
            string projItemNodeTemplate = @"<ProjectItem ReplaceParameters = ""true"" TargetFileName=""$filename"">$filename</ProjectItem>";
            string folderItemNodeTemplate = @"<Folder Name=""$folderName"" TargetFolderName=""$folderName"" >";

            if (topFolder.FolderName != null)
            {
                folderString = folderItemNodeTemplate.Replace("$folderName", topFolder.FolderName);
            }

            foreach (var item in topFolder.Items)
            {
                if (IsHelpItem(item))
                { 
                    folderString = folderString + @"<ProjectItem ReplaceParameters=""false"" TargetFileName=""help.htm"" OpenInWebBrowser=""true"">help.htm</ProjectItem>";
                    helpFileReferenceExists = true;
                }
                else if (IsKeyProjectItemNode(item))
                    folderString = folderString + @"<ProjectItem ReplaceParameters=""false"" TargetFileName=""$projectname$_TemporaryKey.pfx"" BlendDoNotCreate=""true"">Application_TemporaryKey.pfx</ProjectItem>";
                else
                {
                    //-- now writing item.
                    if (!string.IsNullOrEmpty(item) && !item.Contains("csproj") && !item.Contains(".."))
                    {
                        folderString = folderString + projItemNodeTemplate.Replace("$filename", item);
                    }
                }
            }

            foreach (var folderItem in topFolder.Folders)
            {
                folderString = folderString + SerializeFolder(folderItem);
            }

            if (topFolder.FolderName != null)
            {
                folderString = folderString + "</Folder>";
            }

            return folderString;

        }

        /// <summary>
        /// Determines whether [is help item] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private bool IsHelpItem(string item)
        {
            return item.ToLower().Contains("help.htm");
        }

        /// <summary>
        /// Gets the item folder.
        /// </summary>
        /// <param name="projectItems">The project items.</param>
        private void GetItemFolder(List<string> projectItems)
        {
            topFolder = new ItemFolder();
            string[] stringSeparator = new string[] { @"\" };

            foreach (var item in projectItems)
            {
                var parts = item.Split(stringSeparator, StringSplitOptions.RemoveEmptyEntries);
                AddPartsToTopFolder(parts);
            }

        }

        /// <summary>
        /// Adds the parts to top folder.
        /// </summary>
        /// <param name="parts">The parts.</param>
        private void AddPartsToTopFolder(string[] parts)
        {
            AddPartsToFolder(topFolder, parts, 0);
        }

        /// <summary>
        /// Adds the parts to folder.
        /// </summary>
        /// <param name="currentFolder">The current folder.</param>
        /// <param name="parts">The parts.</param>
        /// <param name="partIndex">Index of the part.</param>
        private void AddPartsToFolder(ItemFolder currentFolder, string[] parts, int partIndex)
        {
            //-- empty folder
            if (partIndex >= parts.Length)
                return;

            string part = parts[partIndex];

            if (!IsFolder(part))
            {
                currentFolder.Items.Add(part);
                return;
            }

            var folder = currentFolder.Folders.FirstOrDefault(e => e.FolderName == part);

            if (folder == null)
            {
                folder = new ItemFolder() { FolderName = part };
                currentFolder.Folders.Add(folder);
            }
            AddPartsToFolder(folder, parts, ++partIndex);
        }

        /// <summary>
        /// Determines whether the specified part is folder.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <returns></returns>
        private bool IsFolder(string part)
        {
            return !part.Contains(".");
        }

        /// <summary>
        /// Sorts the project items.
        /// </summary>
        /// <param name="projectItems">The project items.</param>
        /// <returns></returns>
        private List<string> SortProjectItems(List<string> projectItems)
        {
            projectItems.Sort();

            var l2 = new List<string>();
            foreach (var item in projectItems)
            {
                if (!item.Contains(@"\"))
                    l2.Insert(0, item);
                else
                    l2.Add(item);

            }

            projectItems = l2;
            return projectItems;
        }


        /// <summary>
        /// Gets the name of the folder.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private string GetFolderName(string item)
        {
            int startIndex = item.IndexOf(@"\");
            return item.Substring(0, startIndex);
        }

        /// <summary>
        /// Determines whether [is key project item node] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private bool IsKeyProjectItemNode(string item)
        {
            return item.Contains(".pfx");
        }

        /// <summary>
        /// Gets the project items.
        /// </summary>
        /// <param name="csprojxml">The csprojxml.</param>
        /// <returns></returns>
        private List<string> GetProjectItems(string csprojxml)
        {
            List<string> files = new List<string>(); ;
            XDocument xdoc;
            using (StringReader sr = new StringReader(csprojxml))
            {
                xdoc = XDocument.Load(sr, LoadOptions.None);
            }

            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var items = xdoc.Descendants(ns + "ItemGroup");
            string itemString = string.Empty;
            foreach (var itemG in items)
            {
                foreach (var item in itemG.Elements())
                {
                    itemString = item.Attribute("Include").Value;
                    if (!string.IsNullOrEmpty(itemString) && !itemString.Contains("=") && !itemString.Contains(","))
                    {
                        files.Add(itemString);
                    }
                }
            }

            return files;

        }




    }
}
