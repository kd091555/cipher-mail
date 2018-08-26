using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Text;

namespace TP1_INF6404
{
    static class Program
    {



        
      
   

       
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            //new wave


            Console.WriteLine("Testing.1...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           Application.Run(new Form1());   

            
          
        }
    }
}
