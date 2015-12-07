using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Deployment.Application;
using Microsoft.Samples.ClickOnceOnDemand;
using System.Security.Permissions;

namespace LightweightApp
{



   public partial class Form1 : Form
   {

      [SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
      public Form1()
      {
         InitializeComponent();
      }

      private void getAssemblyButton_Click(object sender, EventArgs e)
      {
         var dc = new DefferedLibClass();
         MessageBox.Show("Message: " + dc.Message);

         var dc2 = new DefferedLibClass2();
         MessageBox.Show(dc2.Message);
      }

      private void Form1_Load(object sender, EventArgs e)
      {

      }
   }
}

