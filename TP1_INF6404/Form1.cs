using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;

namespace TP1_INF6404
{

    
    public partial class Form1 : Form
    {

    
        private String POPServerName;       // Le nom du serveur POP
        private String POPUsername;         // le username du serveur POP
        private String POPPassword;         //  le mot de passe du serveur POP
        private String PopPort;             // le numero de port du serveur POP
       
        string NumeroMessage = "";          //le numéro de Courriel.Tres utile pour la selection du message

        string NombreTotaleEmails = "";   // Le nombre total de Emails dans la boite de reception 
        string str = " ";                    // Commandes affichees a l'ecran

        List<EmailMessage> listObjects = new List<EmailMessage>(); // La liste de emails

        public Form1()
        {
            InitializeComponent();
         
        }


       

        // Cette chaine de caracteres lit la ligne commande resultante de STAT
        // Puis elle en ressort le premier numero qui est le nombre de emails
        private string getNumberEmails(string ChaineLue)
        {
            string s = ChaineLue;
            String [] words = s.Split(' ');

            if (ChaineLue != "")
            {
             string nombreTotal = words[1];
            return nombreTotal;
            }  
            else 
            {
                return ("ERREUR, pas de chaine Lue");
            }
       }
 
        // Cette méthode s'occupe d'afficher la liste des courriels dans la grille lorsque
        // le bouton GetMail a été cliqué
        private void afficherListeMessages(string POPServerName, string POPUsername, string POPPassword, string PopPort)
         {
             POP3 popClient = new POP3(POPServerName, POPUsername, POPPassword, PopPort);
            
             if ((POPServerName == "") || (POPUsername == "") || (POPPassword == "") || (PopPort == ""))
            {
             var result = MessageBox.Show("Veuillez vérifier vos paramètres de connection et que vous êtes bien connectés à Internet", "Confirmation",
                         MessageBoxButtons.OK,
                        MessageBoxIcon.Question);
             }
           else
            {
                label3.Visible = true;

                //nettoie ala grille pour l'affichage
               dataGridView1.Rows.Clear();

                // deletes all items in the List of objects of email class
                listObjects.RemoveAll(delegate(EmailMessage item) { return item.getEmailNo() != ""; });



                str += popClient.connect() + "\n";

                str += popClient.USER() + "\n";
                str += popClient.getCommande() + "\n\r";
                str += popClient.getReponse() + "\n\r";
                str += popClient.PASS() + "\n";

                str += popClient.getCommande() + "\n\r";
                str += popClient.getReponse() + "\n\r";

                str += popClient.STAT() + "\n";
                str += popClient.getCommande() + "\n\r";
                str += popClient.getReponse() + "\n\r";



              NombreTotaleEmails = getNumberEmails(popClient.STAT());

            this.NbMessages.Text = NombreTotaleEmails;
             string[] ch;

          //   Les emails sont classés du plus récent vers le plus ancien. juste les 5 derniers
                     for (int j = Convert.ToInt32(NombreTotaleEmails); j >=  Convert.ToInt32(NombreTotaleEmails)-5; j--) 
                        {

                           string messageLue = popClient.RETR(j);                   //retrait du message
                           
                          EmailMessage courrielT = new EmailMessage(messageLue);  //instanciation de l'entete du message
                          courrielT.setEntete(popClient.TOP(j));
                         str += popClient.getCommande() + "\n\r";
                         str += popClient.getReponse() + "\n\r";
                        courrielT.retrieveHeadersFields(j.ToString());

           
                   ch = courrielT.retournerTableauCHamp(courrielT.getEmailNo(), courrielT.getFromField(), courrielT.getSubject(), courrielT.getDate());


            
                              dataGridView1.Rows.Add(ch);
                            
                              listObjects.Add(courrielT);
                          
                            
                        }




             str += popClient.QUIT() + "\n";
             str += popClient.getCommande() + "\n\r";
             str += popClient.getReponse() + "\n\r";
             
           
            // Le bouton Delete est maintenant activé. On peut supprimer des messages.
             this.button3.Enabled = true;
             NumeroMessage = "";
            }
        }
 

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void accountSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

             
             
         }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e )
        {

            // Ceci est la méthode pour télécharger les couriels
            POPServerName = this.textBox1.Text;       // Le nom du serveur POP
            POPUsername = this.textBox2.Text;          // le username du serveur POP
            POPPassword = this.textBox3.Text;          //  le mot de passe du serveur POP
            PopPort = this.textBox4.Text;              // le numero de port du serveur POP

             afficherListeMessages(POPServerName, POPUsername, POPPassword, PopPort);
           
         }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        


     

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {            
        
        }



       private  void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
                                                                         

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            

           }


        private void button2_Click(object sender, EventArgs e)
        {

            SMTPForm envoyerEmail = new SMTPForm();
            // settingsForm.MdiParent = this;
            envoyerEmail.ShowDialog();
          
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
             // Le bouton delete a été cliqué         
	 


            POPServerName = this.textBox1.Text;       // Le nom du serveur POP
            POPUsername = this.textBox2.Text;          // le username du serveur POP
            POPPassword = this.textBox3.Text;          //  le mot de passe du serveur POP
            PopPort = this.textBox4.Text;              // le numero de port du serveur POP

             if (NumeroMessage=="")
            {
                DialogResult dialogResult = MessageBox.Show("Veuillez sélectionner la date", "Confirmation",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Question);
             }
            else
            {

            POP3 popClient = new POP3(POPServerName, POPUsername, POPPassword, PopPort);

            str += popClient.connect() + "\n\r";
            str += popClient.getCommande() + "\n\r";
            str += popClient.getReponse() + "\n\r";

            str += popClient.USER() + "\n";
            str += popClient.getCommande() + "\n\r";
            str += popClient.getReponse() + "\n\r";

            str += popClient.PASS() + "\n";
            str += popClient.getCommande() + "\n\r";
            str += popClient.getReponse() + "\n\r";

           
            string msg = "Supprimer Message " + NumeroMessage + " ?";


            DialogResult dialogResult = MessageBox.Show(msg, "Confirmation",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                //Supprimer Le Message concerne

                // Le bouton Delete est maintenant désactivé. 
                this.button3.Enabled = false;
                dataGridView1.Rows.Clear();

                // deletes all items in the List of objects of email class
                listObjects.RemoveAll(delegate(EmailMessage item) { return item.getEmailNo() != ""; });


                str += popClient.DELE(Convert.ToInt32(NumeroMessage)) + "\n";
                str += popClient.getCommande() + "\n\r";
                str += popClient.getReponse() + "\n\r";


              

            }
            else 
            {               
             }
         
            str += popClient.QUIT() + "\n\r";
            str += popClient.getCommande() + "\n\r";
            str += popClient.getReponse() + "\n\r";

            // Le bouton Delete est maintenant activé. 
            this.button3.Enabled = true;
            NumeroMessage = "";
            afficherListeMessages(POPServerName, POPUsername, POPPassword, PopPort);
                
	    }
           
          
       
	    }


        //Cette methode affiche la liste des messages dans une grille


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        

            // Pour s'assurer qu'on ne clique pas sur le tableau
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex != 1)
                {
                    if (!string.IsNullOrEmpty(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                    {
                         NumeroMessage = dataGridView1.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();

                         Console.WriteLine(Convert.ToInt32(NombreTotaleEmails));
                         Console.WriteLine(Convert.ToInt32(NumeroMessage));
                      
                    // La liste est indexée comme un tableau en ordre croissant : 0,1,2,3
                    // Le numéro de email est indexée en ordre décroissant du no de email : 30,29,28,27


                      EmailMessage courrielLu = listObjects[Convert.ToInt32(NombreTotaleEmails) - Convert.ToInt32(NumeroMessage)];

                   

                       ViewEmailForm AfficherEmail = new ViewEmailForm(courrielLu.getFromField(), courrielLu.getDate(), courrielLu.getSubject(), courrielLu.getEntireMessage());

                        AfficherEmail.ShowDialog();


                    }
                }
            }
        }

        private void getMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ceci est la méthode pour télécharger les couriels
            POPServerName = this.textBox1.Text;       // Le nom du serveur POP
            POPUsername = this.textBox2.Text;          // le username du serveur POP
            POPPassword = this.textBox3.Text;          //  le mot de passe du serveur POP
            PopPort = this.textBox4.Text;              // le numero de port du serveur POP

            afficherListeMessages(POPServerName, POPUsername, POPPassword, PopPort);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void createMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SMTPForm envoyerEmail = new SMTPForm();
            // settingsForm.MdiParent = this;
            envoyerEmail.ShowDialog();
            
        }

        private void viewLogFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewLogFiles commandes = new ViewLogFiles(str);
            commandes.ShowDialog();   
        }

      

       

      
      
       
    }
}
