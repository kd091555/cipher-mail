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
    public partial class ViewEmailForm : Form
    {

        string FormValue;
        string dateValue;
        string subjectvalue;
        string body;

        public ViewEmailForm(string _FormValue, string _dateValue, string _subjectvalue, string _body)
        {
            InitializeComponent();
            this.FormValue = _FormValue;
            this.dateValue = _dateValue;
            this.subjectvalue = _subjectvalue;
            this.body = _body;
        }

      
        private void ViewEmailForm_Load(object sender, EventArgs e)
        {
            this.FromTextValue.Text = this.FormValue;
            this.DateFormValue.Text = this.dateValue;
            this.SubjectFormValue.Text = this.subjectvalue;
            this.richTextBox1.Text = this.body;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
