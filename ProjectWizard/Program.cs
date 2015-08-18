using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://msdn.microsoft.com/en-us/library/ms185301.aspx
namespace ProjectWizard
{
    public class Program : IWizard
    {
        WizardForm _form;

        // Runs custom wizard logic before opening an item in the template.
        public void BeforeOpeningFile(global::EnvDTE.ProjectItem projectItem) { /* nothing */ }
        // Runs custom wizard logic when a project has finished generating.
        public void ProjectFinishedGenerating(global::EnvDTE.Project project) { /* nothing */ }
        // Runs custom wizard logic when the wizard has completed all tasks.
        public void RunFinished() { /* nothing */ }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="automationObject">An Object parameter that can be cast to the root _DTE object, to enable you to customize the project.</param>
        /// <param name="replacementsDictionary">A Dictionary<TKey, TValue> parameter that contains a collection of all pre-defined parameters in the template. For more information on template parameters, see Template Parameters.</param>
        /// <param name="runKind">A WizardRunKind parameter that contains information about what kind of template is being used.</param>
        /// <param name="customParams">An Object array that contains a set of parameters passed to the wizard by Visual Studio.</param>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _form = new WizardForm();
            _form.ShowDialog();

            var result = _form.Choice;
            // TODO
        }

        #region item templates

        // Indicates whether the specified project item should be added to the project.
        public bool ShouldAddProjectItem(string filePath) { return true; }
        // Runs custom wizard logic when a project item has finished generating.
        public void ProjectItemFinishedGenerating(global::EnvDTE.ProjectItem projectItem) { /* nothing */ }

        #endregion
    }
}
