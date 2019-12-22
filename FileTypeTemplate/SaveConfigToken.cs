using PaintDotNet;
using System;
using System.Collections.Generic;

namespace $safeprojectname$
{
    [Serializable]
    internal class $safeprojectname$SaveConfigToken : SaveConfigToken
    {
        public bool Invert { get; set; }

        public $safeprojectname$SaveConfigToken()
        {
            Invert = false;
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
