using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.MSBUILD
{
    internal class Constants
    {
        internal const string HELPURL = "$helpurl";

        internal const string TEMPLATE10PROJECTJSON = @"    ""Template10"": ""1.1.*""";

        internal const string NEWTONSOFT_PROJECTJSON = @"""Newtonsoft.Json"": ""7.0.1""";

        internal const string TEMPLATE10 = "Template10";

        internal const string PROJECTJSON = "project.json";

        internal const string HELPHTML = "Help.htm";

        internal const string TEMPFOLDER = "Temp";

        internal const string PROJECTNODE = "$projectNode";

        internal const string TEMPLATENAME = "$templateName";

        internal const string TEMPLATEDESCRIPTION = "$templateDescription";

        internal const string PREVIEWIMAGEFILE = "$previewImageFile";

        internal const string VSTEMPLATENAME = "MyTemplate.vstemplate";

        //TODO: move this to a separate file... or embedded resource?
        internal const string VSTEMPLATETEXT = @"<VSTemplate Version=""3.0.0"" xmlns=""http://schemas.microsoft.com/developer/vstemplate/2005"" Type=""Project"">
  <TemplateData>
    <Name>$templateName</Name>
    <Description>$templateDescription</Description>
    <ProjectType>CSharp</ProjectType>
    <ProjectSubType>Template10</ProjectSubType>
    <SortOrder>0</SortOrder>
    <TemplateID>Microsoft.CS.WinRT.UAP.BlankApplication</TemplateID>
    <TemplateGroupID>WinRT-Managed</TemplateGroupID>
    <CreateNewFolder>true</CreateNewFolder>
    <DefaultName>WindowsApp</DefaultName>
    <ProvideDefaultName>true</ProvideDefaultName>
    <LocationField>Enabled</LocationField>
    <TargetPlatformName>Windows</TargetPlatformName>
    <RequiredPlatformVersion>6.1.0</RequiredPlatformVersion>
    <EnableLocationBrowseButton>true</EnableLocationBrowseButton>
    <Icon>__TemplateIcon.png</Icon>
    <PreviewImage>$previewImageFile</PreviewImage>
    <NumberOfParentCategoriesToRollUp>0</NumberOfParentCategoriesToRollUp>
  </TemplateData>
  <TemplateContent PreferedSolutionConfiguration=""Debug|x86"">
$projectNode
</TemplateContent>
  <WizardExtension>
    <Assembly>Microsoft.VisualStudio.WinRT.TemplateWizards, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</Assembly>
    <FullClassName>Microsoft.VisualStudio.WinRT.TemplateWizards.CreateProjectCertificate.Wizard</FullClassName>
  </WizardExtension>
  <WizardExtension>
    <Assembly>NuGet.VisualStudio.Interop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</Assembly>
    <FullClassName>NuGet.VisualStudio.TemplateWizard</FullClassName>
  </WizardExtension>
  <WizardExtension>
    <Assembly>Microsoft.VisualStudio.Universal.TemplateWizards, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</Assembly>
    <FullClassName>Microsoft.VisualStudio.Universal.TemplateWizards.PlatformVersion.Wizard</FullClassName>
  </WizardExtension>
  <WizardData>
    <packages repository=""registry"" keyName=""NETCoreSDK"" isPreunzipped=""true"">
      <package id=""Microsoft.NETCore.UniversalWindowsPlatform"" version=""5.0.0"" skipAssemblyReferences=""false"" />
    </packages>
  </WizardData>
</VSTemplate>";

    }
}
