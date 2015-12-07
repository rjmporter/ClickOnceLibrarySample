using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace LightweightApp
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         _dllMapping["ClickOnceLibrary"] = "ClickOnceLibrary";
         _dllMapping[ "ClickOnceLib2" ] = "ClickOnceLib2";
         System.Windows.Forms.MessageBox.Show( "Attach Debugger Now" );
         SetRegistryKeys();
         AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Program.CurrentDomain_AssemblyResolve);

         Application.Run(new Form1());
      }
      internal static Dictionary<string, string> _dllMapping = new Dictionary<string, string>();
      internal static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
      {
         Assembly newAssembly = null;


         ApplicationDeployment deploy = ApplicationDeployment.CurrentDeployment;

         // Get the DLL name from the Name argument.
         string[] nameParts = args.Name.Split(',');
         string dllName = nameParts[0];
         string downloadGroupName = _dllMapping[dllName];

         try
         {
            MessageBox.Show("About to download the library: " + dllName);
            deploy.DownloadFileGroup(downloadGroupName);
         }
         catch (DeploymentException)
         {
            MessageBox.Show("Downloading file group failed. Group name: " + downloadGroupName + "; DLL name: " + args.Name);
            throw;
         }

         // Load the assembly.
         // Assembly.Load() doesn't work here, as the previous failure to load the assembly
         // is cached by the CLR. LoadFrom() is not recommended. Use LoadFile() instead.
         try
         {
            newAssembly = Assembly.LoadFile(Application.StartupPath + @"\" + dllName + ".dll");
         }
         catch (Exception)
         {
            throw;
         }


         return (newAssembly);
      }

      private static void SetRegistryKeys()
      {
         RegistryKey subKey = Registry.CurrentUser.OpenSubKey( "SOFTWARE" )?.OpenSubKey("classes", true);
         RegistryKey handlerKey = subKey?.CreateSubKey( "tsc-clickonce-test" );
         handlerKey?.SetValue( "URL Protocol", "URL:Camtasia Protocol", RegistryValueKind.String );
         RegistryKey defaultIconKey = handlerKey?.CreateSubKey( "DefaultIcon" );
         defaultIconKey?.SetValue("", "me.ico", RegistryValueKind.String);
         RegistryKey shellKey = handlerKey?.CreateSubKey( "shell" );
         RegistryKey shellOpenKey = shellKey?.CreateSubKey( "open" );
         RegistryKey shellOpenCommandKey = shellOpenKey?.CreateSubKey( "command" );
         shellOpenCommandKey?.SetValue("", @"cmd /c ""start https://www.techsmith.com/""");
      }
   }
}
