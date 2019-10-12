using PaintDotNet.Effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace $safeprojectname$
{
    internal partial class $safeprojectname$ConfigDialog : EffectConfigDialog$if$ ($baseClass$ != Effect) <$safeprojectname$Plugin, $safeprojectname$ConfigToken> $endif$
    {
        public $safeprojectname$ConfigDialog()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            FinishTokenUpdate();
        }
$if$ ($baseClass$ == Effect)
        protected override void InitialInitToken()
        {
            theEffectToken = new $safeprojectname$ConfigToken();
        }

        protected override void InitTokenFromDialog()
        {
            $safeprojectname$ConfigToken token = ($safeprojectname$ConfigToken)EffectToken;
            token.Amount1 = this.trackBar1.Value;
        }

        protected override void InitDialogFromToken(EffectConfigToken effectTokenCopy)
        {
            $safeprojectname$ConfigToken token = ($safeprojectname$ConfigToken)effectTokenCopy;
            this.trackBar1.Value = token.Amount1;
        }
$else$
        protected override $safeprojectname$ConfigToken CreateInitialToken()
        {
            return new $safeprojectname$ConfigToken();
        }

        protected override void InitDialogFromToken($safeprojectname$ConfigToken effectTokenCopy)
        {
            this.trackBar1.Value = effectTokenCopy.Amount1;
        }

        protected override void LoadIntoTokenFromDialog($safeprojectname$ConfigToken writeValuesHere)
        {
            writeValuesHere.Amount1 = this.trackBar1.Value;
        }
$endif$
    }
}
