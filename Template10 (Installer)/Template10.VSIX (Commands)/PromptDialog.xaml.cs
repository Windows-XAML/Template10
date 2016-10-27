using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Template10.VSIX.Commands.Templates;

namespace Template10.VSIX.Commands
{
    public partial class PromptDialog : DialogWindow
    {
        #region Fields

        private Project _project;
        private IServiceProvider serviceProvider;
        private readonly Guid guid_microsoft_csharp_editor = new Guid("{A6C744A8-0E4A-4FC6-886A-064283054674}");
        private readonly Guid guid_microsoft_csharp_editor_with_encoding = new Guid("{08467b34-b90f-4d91-bdca-eb8c8cf3033a}");
        private readonly Guid guid_microsoft_xaml_editor = new Guid("{32CC8DFA-2D70-49b2-94CD-22D57349B778}");
    #endregion Fields

    #region Constructors

    public PromptDialog(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;

            Title = Properties.Resources.PromptDialogTitle;
            pageNameLabel.Text = Properties.Resources.PromptDialog_PagaNameHeaderText;
            useModel_checkBox.Content = Properties.Resources.PromptDialog_UseModelCheckBoxContent;
            generateButton.Content = Properties.Resources.PrompDialog_GenerateButtonContent;
            cancelButton.Content = Properties.Resources.PrompDialog_CancelButtonContent;
            _project = GetSelectedProject();
        }

        #endregion Constructors

        #region Methods

        private static void CreateDirIfNotExists(string viewsPath)
        {
            if (!Directory.Exists(viewsPath))
            {
                Directory.CreateDirectory(viewsPath);
            }
        }

        private void GenerateModel(string projectPath, string pageName, Dictionary<string, object> dictOptions, List<ProjectItem> items)
        {
            var model = new Model();
            model.Session = dictOptions;
            model.Initialize();

            var modelText = model.TransformText();

            var modelsPath = $"{projectPath}\\Models";

            CreateDirIfNotExists(modelsPath);

            var modelFileName = $"{modelsPath}\\{pageName}Model.cs";
            File.WriteAllText(modelFileName, modelText);

            var projectItem = GetProjectItemFor(items, "Models");
            var modelFile = projectItem.ProjectItems.AddFromFile(modelFileName);
            OpenProjectItemInView(modelFile, guid_microsoft_csharp_editor, VSConstants.LOGVIEWID.Code_guid);
        }

        private void GenerateView(string projectPath, string pageName, Dictionary<string, object> dictOptions, List<ProjectItem> items)
        {
            var viewXaml = new ViewXaml();
            viewXaml.Session = dictOptions;
            viewXaml.Initialize();
            var viewXamlText = viewXaml.TransformText();

            var viewCs = new ViewCs();
            viewCs.Session = dictOptions;
            viewCs.Initialize();
            var viewCsText = viewCs.TransformText();

            var viewsPath = $"{projectPath}\\Views";
            CreateDirIfNotExists(viewsPath);

            var pageFileName = $"{viewsPath}\\{pageName}.xaml";
            var pageCsFileName = $"{viewsPath}\\{pageName}.xaml.cs";

            File.WriteAllText(pageFileName, viewXamlText);
            File.WriteAllText(pageCsFileName, viewCsText);

            var projectItem = GetProjectItemFor(items, "Views");
            var pageXamlFile = projectItem.ProjectItems.AddFromFile(pageFileName);
            OpenProjectItemInView(pageXamlFile, guid_microsoft_xaml_editor, VSConstants.LOGVIEWID.Designer_guid);


            var pageCsFile = projectItem.ProjectItems.AddFromFile(pageCsFileName);
            OpenProjectItemInView(pageCsFile, guid_microsoft_csharp_editor, VSConstants.LOGVIEWID.Code_guid);
        }

        private void GenerateViewModel(string projectPath, string pageName, Dictionary<string, object> dictOptions, List<ProjectItem> items)
        {
            var viewModel = new ViewModel();
            viewModel.Session = dictOptions;
            viewModel.Initialize();
            var viewModelText = viewModel.TransformText();

            var viewModelsPath = $"{projectPath}\\ViewModels";
            CreateDirIfNotExists(viewModelsPath);

            var viewModelFileName = $"{viewModelsPath}\\{pageName}ViewModel.cs";
            File.WriteAllText(viewModelFileName, viewModelText);

            var projectItem = GetProjectItemFor(items, "ViewModels");
            var viewModelFile = projectItem.ProjectItems.AddFromFile(viewModelFileName);
            OpenProjectItemInView(viewModelFile, guid_microsoft_csharp_editor, VSConstants.LOGVIEWID.Code_guid);

        }

        private ProjectItem GetProjectItemFor(List<ProjectItem> items, string itemName)
        {
            ProjectItem projectItem;
            if (!items.Any(p => p.Name == itemName))
            {
                projectItem = _project.ProjectItems.AddFolder(itemName);
            }
            else
            {
                projectItem = _project.ProjectItems.Item(itemName);
            }

            return projectItem;
        }

        private Project GetSelectedProject()
        {
            IntPtr hierarchyPointer, selectionContainerPointer;
            Object selectedObject = null;
            IVsMultiItemSelect multiItemSelect;
            uint projectItemId;

            IVsMonitorSelection monitorSelection =
                    (IVsMonitorSelection)Package.GetGlobalService(
                    typeof(SVsShellMonitorSelection));

            monitorSelection.GetCurrentSelection(out hierarchyPointer,
                                                 out projectItemId,
                                                 out multiItemSelect,
                                                 out selectionContainerPointer);

            IVsHierarchy selectedHierarchy = Marshal.GetTypedObjectForIUnknown(
                                                 hierarchyPointer,
                                                 typeof(IVsHierarchy)) as IVsHierarchy;

            if (selectedHierarchy != null)
            {
                ErrorHandler.ThrowOnFailure(selectedHierarchy.GetProperty(
                                                  projectItemId,
                                                  (int)__VSHPROPID.VSHPROPID_ExtObject,
                                                  out selectedObject));
            }

            if (selectionContainerPointer != IntPtr.Zero)
            {
                Marshal.Release(hierarchyPointer);
                Marshal.Release(selectionContainerPointer);
            }

            return selectedObject as Project;
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            var projectPath = Path.GetDirectoryName(_project.FullName);

            var pageName = pageName_TextBox.Text;
            var @namespace = _project.Properties.Item("RootNamespace").Value as string;
            bool hasModel = useModel_checkBox.IsChecked ?? false;

            var options = new BuildOptions
            {
                HasModel = hasModel,
                PageName = pageName,
                SolutionNamespace = @namespace
            };

            var dictOptions = new Dictionary<string, object>
            {
                { "Parameters", options }
            };

            var items = new List<ProjectItem>();

            foreach (ProjectItem item in _project.ProjectItems)
            {
                items.Add(item);
            }

            GenerateView(projectPath, pageName, dictOptions, items);
            GenerateViewModel(projectPath, pageName, dictOptions, items);

            if (hasModel)
            {
                GenerateModel(projectPath, pageName, dictOptions, items);
            }

            Close();
        }

        #endregion Methods

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenProjectItemInView(ProjectItem projectItem, Guid editorGuid, Guid logicalViewGuid)
        {
            string fullPath;
            IVsWindowFrame windowFrame;

            fullPath = projectItem.get_FileNames(0);

            windowFrame = VsShellUtilities.OpenDocumentWithSpecificEditor(serviceProvider, fullPath,
               editorGuid, logicalViewGuid);

            if (windowFrame != null)
            {
                windowFrame.Show();
            }
        }

    }
}
