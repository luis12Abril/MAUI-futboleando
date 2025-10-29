using futboleandoEntities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Service
{
    public class LoginService
    {
        public LoginService()
        {

        }

        public async Task<bool> login(LoginCLS oLoginCLS)
        {
            if (oLoginCLS.nombreusuario.Equals("luis") && oLoginCLS.contra.Equals("Labt1970"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
