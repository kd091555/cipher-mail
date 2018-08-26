using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;



namespace TP1_INF6404
{
   public class POP3
    {
       public enum etat_de_connection { deconnecte, AUTHORIZATION, TRANSACTION, UPDATE };

        string POPServer;    // POP host server name 
        string user;         // POP server username
        string pwd;          // POP server password
        string noPort;       // port number
        
        private TcpClient accesServeur;
        private NetworkStream Netstrm;
        private StreamReader streamLecture;   //cette classe fournit l acces au flux de donnees 

        private String Data;
        private byte[] sizeData;
        private String CRLF = "\r\n";   // caractere pour delimiter la fin de ligne

        public string commandeAffichee = "";
        public string reponseAffichee = "";
        

        public bool erreur;  //variable de controle
        public etat_de_connection etat = etat_de_connection.deconnecte;

        // le constructeur
        public POP3()
        {
        }

        //constructeur instancie les variables serveurs, username et password
        public POP3(string _server, string _user, string _pwd, string _noPort)
        {
            this.POPServer = _server;
            this.user = _user;
            this.pwd = _pwd;
            this.noPort = _noPort;
        }

 #region Gestion des  Fonctionnalites generales


       // Cette methode etablit la conection au serveur distant
        public string connect ()
        {
    
             try
             {       

                accesServeur = new TcpClient(POPServer, Convert.ToInt32(this.noPort));
            
            //Initialisation et connection au serveur POP3
            Netstrm = accesServeur.GetStream();

            streamLecture = new StreamReader(accesServeur.GetStream());

            //La session POP est maintenant dans un état d'authorisation
            etat = etat_de_connection.AUTHORIZATION;

             return (streamLecture.ReadLine());
             }
            catch (InvalidOperationException err)   // Il faut faire un DIalog Box. Verifie la validite de l'operation
             {
                
                return ("Error: " + err.ToString());
             }
           catch (FormatException err2)  // verifie si les parametres du serveur son entrees. Il faut faire un DIalog Box
           {
       
              
              return ("Error: " + err2.ToString());
             }

       
       }

        private void setCommande(string var1)
        {

            this.commandeAffichee = var1;
        }

        public string getCommande()
        {

            return this.commandeAffichee;
        }

        private void setReponse(string var1)
        {

            this.reponseAffichee = var1;
        }

        public string getReponse()
        {

            return this.reponseAffichee;
        }
 


        private string disconnect()
        {
            string temp = "Deconection etablie.";
            if (etat != etat_de_connection.deconnecte)
            {

                //close connection
                Netstrm.Close();
                streamLecture.Close();
                etat = etat_de_connection.deconnecte;
            }
            else
            {
                temp = "Non connecte.";
            }
            return (temp);
        }

       //Cette methode execute la requeste POP envoyee par le client POP
  
//      private void issue_command(string command)   
        private string issue_command(string command)   
        {
          
            Data = command + CRLF;
                    
            sizeData = System.Text.Encoding.ASCII.GetBytes(Data.ToCharArray());

          
            Netstrm.Write(sizeData, 0, sizeData.Length);

            return command;
            
        }

        //Cette methode lit  la  reponse a la requeste POP envoyee par le serveur
       // lorsque la reponse est sur une seule ligne
        public string read_single_line_response()   
         {
            //read the response of the pop server.  
             
            string temp;
            try
            {
                temp = streamLecture.ReadLine();
       
                was_pop_error(temp);
                 setReponse(temp); 
                return (temp);
            }
            catch (InvalidOperationException err)
            {
                return ("Error in read_single_line_response(): " + err.ToString());
            }

        }

        //Cette methode lit  la  reponse a la commande POP envoyee par le serveur
        // lorsque la reponse est sur plusieurs lignes
        private string read_multi_line_response()  
       {
           //lite la reponse  du serveur Pop. 

           string temp = "";
           string szTemp;

           try
           {
               szTemp = streamLecture.ReadLine();
               
               was_pop_error(szTemp);
               if (!erreur)
               {

                   while (szTemp != ".")
                   {
                       temp += szTemp + CRLF;
                       szTemp = streamLecture.ReadLine();
                   }
               }
               else
               {
                   temp = szTemp;
               }
               setReponse(temp); 
               return (temp);
           }
           catch (InvalidOperationException err)
           {
               return ("Error in read_multi_line_response(): " + err.ToString());
           }
       }



       private void was_pop_error(string response)
       {
           //Cette fonction detecte si le server POP qui a emis une reponse
           // a rejete une erreur

           if (response.StartsWith("-"))
           {
               //if Si le premier caractere de la reponse est "-", alors le 
               //serveur POP a rencontre une erreur en executant la derniere 
               //commande envoyee par le client 
               erreur = true;
           }
           else
           {
               //success
               erreur = false;
           }
       }
#endregion


        #region  commandes  et requetes du server POP

       //Cette commande envoie le nom d'usager de l'addresse email pour la connection
       // Si tout fonctionne correctement, il est suppose afficher la commande sur la console,
       // sinon, il affiche a l'ecran que l'etat N'est pas une transaction
       public string USER()
       {    
           string temp;
           if (etat != etat_de_connection.AUTHORIZATION)
           {
               //the pop command USER is only valid in the AUTHORIZATION state
               temp = "Connection state not in AUTHORIZATION mode";
           }
           else
           {
               if (this.user != null)
               {
                   setCommande(issue_command("USER " + this.user));
                   temp = read_single_line_response();

                  
               }
               else
               {   //no user has been specified
                   temp = "No User specified.";
               }
           }
           setReponse(temp);  //
           return (temp);
       }


       // Cete fonction commande la transmission du mot de passe
       // Si tout fonctionne correctement, il est suppose afficher la commande sur la console,
       // sinon, il affiche a l'ecran que l'etat N'est pas une transaction
       public string PASS()
       {
           string _pwd = this.pwd;
           string temp;
           if (etat != etat_de_connection.AUTHORIZATION)
           {
               //the pop command PASS is only valid in the AUTHORIZATION state
               temp = "Connection state not = AUTHORIZATION";
           }
           else
           {
               if (_pwd != null)
               {
                    issue_command("PASS " + _pwd);
                   temp = read_single_line_response();

                   if (!erreur)
                   {
                       //transition to the Transaction state
                       etat = etat_de_connection.TRANSACTION;
                   }
               }
               else
               {
                   temp = "No Password set.";
               }
           }
           setCommande("PASS **********");
           setReponse(temp);  //
           return (temp);
       }


       // cette methode envoie la requete de suppression d'un message
       // le parametre d'entree est le numero de message

       public string DELE(int msg_number)
       {
           string temp;

           if (etat !=  etat_de_connection.TRANSACTION)
           {
               //DELE est valide seulement que l'etat de la connection
               // est dans un état de TRANSACTION. Il ne peut pas se faire en 
               // mode deconnecte

               temp = "Connection state not in TRANSACTION mode";
           }
           else
           {
               
              setCommande( issue_command("DELE " + msg_number.ToString()));
               temp = read_single_line_response();
           
           }
           setReponse(temp);  //
           return (temp);
       }

       // Cete méthode retourne des messages de courriels
       // Si tout fonctionne correctement, il est suppose afficher la commande sur la console,
       // sinon, il affiche a l'ecran que l'etat N'est pas une transaction
       public string LIST()
       {
           string temp = "";
           if (etat !=  etat_de_connection.TRANSACTION)
           {
               //LIST est valide seulement quand l'etat de la connection
               // est dans un état de TRANSACTION. Il ne peut pas se faire en 
               // mode deconnecte
               temp = "Connection state not in TRANSACTION mode";

            
           }
           else
           {
               setCommande (issue_command("LIST"));
               temp = read_multi_line_response();
               setReponse(temp);  //
           }
           return (temp);
       }

       // Cette méthode retourne le courriel specifie par son numero 
       // Si tout fonctionne correctement, il est suppose afficher la commande sur la console,
       // sinon, il affiche a l'ecran que l'etat N'est pas une transaction
       public string LIST(int msg_number)
       {
           string temp = "";

           if (etat != etat_de_connection.TRANSACTION)
           {
               //the pop command LIST is only valid in the TRANSACTION state
               temp = "Connection state not in TRANSACTION mode";

            
           }
           else
           {
               setCommande(issue_command("LIST " + msg_number.ToString()));
               temp = read_single_line_response();  //when the message number is supplied, expect a single line response
               setReponse(temp); //
           }
           return (temp);

       }


       //cette fonction teste l'état de la connection. elle ne fait rien. Elle retourne OK.
       // Si tout fonctionne correctement, il est suppose afficher la commande sur la console,
       // sinon, il affiche a l'ecran que l'etat N'est pas une transaction
       public string NOOP()
       {
           string temp;
           if (etat !=  etat_de_connection.TRANSACTION)
           {
               //the pop command NOOP is only valid in the TRANSACTION state
               temp = "Connection state not = TRANSACTION";
           }
           else
           {
               issue_command("NOOP");
               temp = read_single_line_response();
               setReponse(temp); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!nondosnfosndfs
           }
           return (temp);

       }

      

      

       // La fonction QUIT envoie la requete et deconnecte la transmission
       // Si tout fonctionne correctement, il est suppose afficher la commande sur la console,
       // sinon, il affiche a l'ecran que l'etat N'est pas une transaction
       public string QUIT()
       {
           // La fonction QUIT envoie la requete et deconnecte la transmission

           string temp;

           if (etat != etat_de_connection.deconnecte)
           {
               setCommande(issue_command("QUIT"));
               temp = read_single_line_response();
               temp += CRLF + disconnect();


           }
           else
           {
               temp = "Non connecte";

           }
           setReponse(temp); //!
           return (temp);
       }


       // Cette commande va afficher le message concerne. Le email au complet
       // Si tout fonctionne correctement, il est suppose afficher la commande sur la console,
       // sinon, il affiche a l'ecran que l'etat N'est pas une transaction
       public string RETR(int msg)
       {
           string temp = "";
           if (etat != etat_de_connection.TRANSACTION)
           {
               //the pop command RETR is only valid in the TRANSACTION state
               temp = "Connection state not in TRANSACTION mode";

            
           }
           else
           {
               // retrieve mail with number mail parameter
               setCommande(issue_command("RETR " + msg.ToString()));
               temp = read_multi_line_response();
           
      
           }
           setReponse(temp);  //
           return (temp);

       }


       // Cette commande va afficher l'enete du message concerne. 
       // Si tout fonctionne correctement, il est suppose afficher les 6 premieres lignes de l'entete,
       // sinon, il affiche a l'ecran que l'etat N'est pas une transaction. Ceci est utilise pour recuperer les details
       // Sujet, date, from, 
       public string TOP(int msg)
       {
           string temp = "";
           if (etat != etat_de_connection.TRANSACTION)
           {
               //the pop command RETR is only valid in the TRANSACTION state
               temp = "Connection state not in TRANSACTION mode";


           }
           else
           {
               // retrieve mail with number mail parameter
               setCommande (issue_command("TOP " + msg.ToString() + "  6"));
               temp = read_multi_line_response();
       

           }
           setReponse(temp);  // afficher les commandes envoyées ainsi que les réponses reçues
           return (temp);

       }




       // Cette commande va enlever la selection du message pour etre supprime
       // La commande QUIT ne pourra plus supprimer de message 
   
       public string RSET()
       {
           string temp;
           if (etat != etat_de_connection.TRANSACTION)
           {
               //the pop command STAT is only valid in the TRANSACTION state
               temp = "Connection state not = TRANSACTION";
           }
           else
           {
               issue_command("RSET");
               temp = read_single_line_response();

             
           }
           return (temp);

       }

       //Cette commande affiche le statut de la boite de reception
       // Il retourne le nombre de messages avec le nombre total d'octets dans la 
       // boite de reception
       // Si tout fonctionne correctement, il est suppose afficher la commande sur la console,
       // sinon, il affiche a l'ecran que l'etat N'est pas une transaction
       public string STAT()
       {
           string temp;
           if (etat ==  etat_de_connection.TRANSACTION)
           {
               setCommande( issue_command("STAT"));
               temp = read_single_line_response();
               setReponse(temp); // afficher les commandes envoyées ainsi que les réponses reçues
               return (temp);
           }
           else
           {
               //the pop command STAT is only valid in the TRANSACTION state
               return ("Connection state not = TRANSACTION");
           }
       }
        #endregion

    }
}
