using SASELibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcSASE.Models
{
    public class SASEExplorer
    {
        public SASEAccountService accountService;
        public string accountname;

        public SASEExplorer(string name, string key)
        {
            accountService = new SASEAccountService(name, key);
            accountname = name;
        }
        public SASEExplorer() { }
    }
}