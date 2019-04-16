using System;
using System.DirectoryServices;

namespace Autenticacion.Manager
{
    public class AutenticationManager
    {
        public DirectoryEntry GetDirectoryEntry()
        {
            DirectoryEntry de = new DirectoryEntry();
            de.Path = "LDAP://192.168.1.158/CN=Administrador;DC=Una.so.local";
            de.Username = @"so.local\Kaysera";
            de.Password = "Una-123";
            Console.WriteLine("se conecto, eres un dios kevin");
            
            return de;
        }

        public bool UserExists(string UserName)
        {
            DirectoryEntry de = ADHelper.GetDirectoryEntry();
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;
            deSearch.Filter = "(&(objectClass=user) (cn=" + UserName + "))";
            SearchResultCollection results = deSearch.FindAll();
            if (results.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidarUsuario(string nom, string cont)
        {
            return true;
        }
    }
}
