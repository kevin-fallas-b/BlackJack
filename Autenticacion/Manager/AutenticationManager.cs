using System;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;


namespace Autenticacion.Manager
{
    public class AutenticationManager
    {
        static void Main()
        {

        }

        public bool Autenticar(string usuario, string contra)
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain, "UNA", "administrador", "Una123");

            if (context.ValidateCredentials(usuario, contra))
            {
                Console.WriteLine("autentico");
                return true;
            }
            else
            {
                Console.WriteLine("no autentico");
                return false;
            }
        }
    }
}
