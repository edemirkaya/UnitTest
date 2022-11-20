﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppLib.Services
{
    public  interface IIdentityValidator
    {
        public bool IsValid(string identityNumber);
        public bool CheckConnectionRemoteServer();
    }
}
