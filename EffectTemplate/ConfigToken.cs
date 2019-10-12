using PaintDotNet.Effects;

namespace $safeprojectname$
{
    public class $safeprojectname$ConfigToken : EffectConfigToken
    {
        internal int Amount1 { get; set; }

        internal $safeprojectname$ConfigToken()
        {
            this.Amount1 = 10;
        }

        private $safeprojectname$ConfigToken($safeprojectname$ConfigToken copyMe)
        {
            this.Amount1 = copyMe.Amount1;
        }

        public override object Clone()
        {
            return new $safeprojectname$ConfigToken(this);
        }
    }
}
