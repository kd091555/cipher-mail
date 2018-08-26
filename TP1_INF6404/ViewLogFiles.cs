using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TP1_INF6404
{
    public partial class ViewLogFiles : Form
    {

      
        public Form1 MyParentForm;

           //COnstructeur, Initalise le formulaire
        public ViewLogFiles( )
        {
            
        }

        //COnstructeur, Initalise le formulaire
        public ViewLogFiles(string TexteAfficher)
        {
            InitializeComponent(TexteAfficher);
        }

       
     
        private void label2_Click(object sender, EventArgs e)
        {

        }

        //enregistre les données qui ont été enregistrés dans les champs.  C est le bouton OK
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Ferme le formulaire et annule tout. C est le bouton CANCEL
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }


        

         private void textBox1_TextChanged(object sender, EventArgs e)
         {
              
         }


    }
}
