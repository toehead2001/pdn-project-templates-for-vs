using PaintDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace $safeprojectname$
{
    internal partial class $safeprojectname$SaveConfigWidget : SaveConfigWidget $if$ ($baseClass$ == FileTypeTTokenTWidget) <$safeprojectname$Plugin, $safeprojectname$SaveConfigToken> $endif$
    {
        private CheckBox invertBox;

        public $safeprojectname$SaveConfigWidget()$if$ ($baseClass$ == FileTypeTTokenTWidget)
            : base(new $safeprojectname$Plugin())$endif$
        {
            invertBox = new CheckBox
            {
                AutoSize = true,
                Text = "Invert"
            };
            invertBox.CheckedChanged += tokenChanged;

            Controls.Add(invertBox);
        }
$if$ ($baseClass$ == FileTypeTTokenTWidget)
        protected override $safeprojectname$SaveConfigToken CreateTokenFromWidget()
        {
            return new $safeprojectname$SaveConfigToken
            {
                Invert = invertBox.Checked
            };
        }

        protected override void InitWidgetFromToken($safeprojectname$SaveConfigToken sourceToken)
        {
            invertBox.Checked = sourceToken.Invert;
        }
$else$
        protected override void InitTokenFromWidget()
        {
            $safeprojectname$SaveConfigToken token = ($safeprojectname$SaveConfigToken)this.Token;
            token.Invert = this.invertBox.Checked;
        }

        protected override void InitWidgetFromToken(SaveConfigToken sourceToken)
        {
            $safeprojectname$SaveConfigToken token = ($safeprojectname$SaveConfigToken)sourceToken;
            this.invertBox.Checked = token.Invert;
        }
$endif$
        private void tokenChanged(object sender, EventArgs e)
        {
            UpdateToken();
        }
    }
}
