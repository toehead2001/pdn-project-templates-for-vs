using PaintDotNet;
using PaintDotNet.Effects;
$if$ ($baseClass$ == PropertyBasedEffect)using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
$endif$using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace $safeprojectname$
{
    [PluginSupportInfo(typeof(PluginSupportInfo))]
    public class $safeprojectname$Plugin : $if$ ($baseClass$ == EffectTToken) Effect<$safeprojectname$ConfigToken> $else$ $baseClass$ $endif$
    {
        private int amount1 = 0;

        public $safeprojectname$Plugin()
            : base("$projectname$", null, SubmenuNames.Blurs, new EffectOptions { Flags = EffectFlags.Configurable })
        {
        }
$if$ ($baseClass$ == PropertyBasedEffect)
        private enum PropertyNames
        {
            Amount1
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            Property[] props = new Property[]
            {
                new Int32Property(PropertyNames.Amount1, 0, 10, 100)
            };

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.Amount1, ControlInfoPropertyNames.DisplayName, "Slider 1 Description");

            return configUI;
        }

        protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection props)
        {
            // Add help button to effect UI
            props[ControlInfoPropertyNames.WindowHelpContentType].Value = WindowHelpContentType.PlainText;
            props[ControlInfoPropertyNames.WindowHelpContent].Value = "$projectname$ v1.0\nCopyright ©$year$ by $username$\nAll rights reserved.";

            base.OnCustomizeConfigUIWindowProperties(props);
        }
$else$
        public override EffectConfigDialog CreateConfigDialog()
        {
            return new $safeprojectname$ConfigDialog();
        }
$endif$ $if$ ($baseClass$ == PropertyBasedEffect)
        protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.amount1 = newToken.GetProperty<Int32Property>(PropertyNames.Amount1).Value;

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }
$endif$ $if$ ($baseClass$ == Effect)
        protected override void OnSetRenderInfo(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            $safeprojectname$ConfigToken token = ($safeprojectname$ConfigToken)parameters;
            this.amount1 = token.Amount1;

            base.OnSetRenderInfo(parameters, dstArgs, srcArgs);
        }
$endif$ $if$ ($baseClass$ == EffectTToken)
        protected override void OnSetRenderInfo($safeprojectname$ConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.amount1 = newToken.Amount1;

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }
$endif$ $if$ ($baseClass$ == Effect)
        public override void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            if (length == 0) return;
            for (int i = startIndex; i < startIndex + length; ++i)
            {
                Render(dstArgs.Surface, srcArgs.Surface, rois[i]);
            }
        }
$else$
        protected override void OnRender(Rectangle[] renderRects, int startIndex, int length)
        {
            if (length == 0) return;
            for (int i = startIndex; i < startIndex + length; ++i)
            {
                Render(DstArgs.Surface, SrcArgs.Surface, renderRects[i]);
            }
        }
$endif$
        private void Render(Surface dst, Surface src, Rectangle rect)
        {
            // Delete any of these lines you don't need
            Rectangle selection = EnvironmentParameters.SelectionBounds;
            int CenterX = ((selection.Right - selection.Left) / 2) + selection.Left;
            int CenterY = ((selection.Bottom - selection.Top) / 2) + selection.Top;
            ColorBgra PrimaryColor = EnvironmentParameters.PrimaryColor;
            ColorBgra SecondaryColor = EnvironmentParameters.SecondaryColor;
            int BrushWidth = (int)EnvironmentParameters.BrushWidth;

            ColorBgra CurrentPixel;
            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                if (IsCancelRequested) return;
                for (int x = rect.Left; x < rect.Right; x++)
                {
                    CurrentPixel = (y % this.amount1 == 0) ? ColorBgra.Black : src[x, y];
                    // TODO: Add pixel processing code here
                    // Access RGBA values this way, for example:
                    // CurrentPixel.R = PrimaryColor.R;
                    // CurrentPixel.G = PrimaryColor.G;
                    // CurrentPixel.B = PrimaryColor.B;
                    // CurrentPixel.A = PrimaryColor.A;
                    dst[x, y] = CurrentPixel;
                }
            }
        }
    }
}
