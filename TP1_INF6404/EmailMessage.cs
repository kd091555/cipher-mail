using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TP1_INF6404
{
  public  class EmailMessage
    {

         // le constructeur
        public EmailMessage()
        {
        }


         // le constructeur avec instantiation du message au complet
        public EmailMessage(string MessageComplet)
        {
            this.MessageComplet = MessageComplet;      
        }


        // le constructeur avec instantiation de l'entete seulement
        public EmailMessage(string EntetedeMessage, int flag)
        {
            int o;
            o = flag;
            this.EntetedeMessage = EntetedeMessage;
        }



     string No="";   // email identifier
     string From = "";   // from field in the message
     string Subject = "";   // Subject field in the message
     string date = "";  // date field in the message
     string MessageComplet ="";      // Le Message au complet
  
     string EntetedeMessage = "";       //Entete du email



     //Cette fonction lit le message et y recherche des expressions regulieres pour separer 
     // les variables de l'entete, From, To, Date, Subject.
     // Il retourne une chaine de caractere
     private string[] MakeArray(string[] search, string message)
     {
         if (search != null && search.Length != 0)
         {
             StringBuilder terms = new StringBuilder("^(");
             for (uint i = 0; i < search.Length; ++i)
                 terms.Append(search[i] + '|');


             terms.Remove(terms.Length - 1, 1);
             terms.Append("):(.*)$");


             // terms.ToString() == "^(Date|To|From|Size):(.*)$" in our case
             return Regex.Split(message, terms.ToString(), RegexOptions.Multiline);
         }


         return null;
     }


     // if the search term is not found, its corresponding index in items is UNCHANGED
     // also, items should be defined as being at LEAST as big search.Length
     private void FindInOrder(string[] list, string[] search, ref string[] items)
     {
         if (list == null || search == null || items == null)
             return;


         for (uint i = 0, k; i < search.Length; ++i)
         {
             k = 0;
             while (k < list.Length)
             {
                 if (search[i] == list[k].Trim() && k + 1 < list.Length)
                 {
                     items[i] = list[k + 1].Trim();
                     break;
                 }


                 ++k;
             }
         }
     }



   public void initialiserCourriel (string  _No, string _From, string _Subject, string _date)      // A des fins d'affichage dans la grille
     {
         this.No = _No;
         this.From = _From;   // from field in the message
         this.Subject = _Subject;   // Subject field in the message
         this.date = _date;  // date field in the message
          

     }

   public string[] retournerTableauCHamp(string _No, string _From, string _Subject, string _date)
   {
          
         string[] tableau = new string[] { _No, _From,_Subject, _date };                          
         return tableau;
    
   }


    public string getEmailNo()
    {
     return this.No;
    }

     public void setEmailNo(string No)
    {
      this.No = No;
    }

     public string getFromField()
    {
     return this.From;
    }

     public void setFromField(string From)
    {
      this.From = From;
    }


     public string getSubject()
    {
     return this.Subject;
    }

     public void setSubject(string Subject)
    {
      this.Subject= Subject;
    }

    public string getEntireMessage()
    {
        return this.MessageComplet;
    }

    public void setEntireMessage(string MessageComplet)
    {
        this.MessageComplet = MessageComplet;
    }

  


     public string getDate()
    {
     return this.date;
    }

     public void setDate(string date)
    {
      this.date= date;
    }


     public string getEntete()
     {
         return this.EntetedeMessage;
     }

     public void setEntete(string EntetedeMessage)
     {
         this.EntetedeMessage = EntetedeMessage;
     }

 





    public void retrieveHeadersFields(string _No)
    {

        string[] search = new string[] { "", "From", "Subject", "Date" };
        string[] items = new string[search.Length];


        FindInOrder(MakeArray(search, this.EntetedeMessage), search, ref items);


   

        this.No = _No;
        this.From = items[1];
        this.Subject = items[2];
        this.date = items[3];
    }



    }
  }

