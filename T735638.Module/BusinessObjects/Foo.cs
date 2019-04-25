using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T735638.Module.BusinessObjects
{
    [Persistent]
    public class Foo : XPObject
    {
        public Foo(Session session) : base(session)
        {
        }


        public string Bar { get; set; }
    }
}
