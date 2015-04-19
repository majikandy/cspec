namespace Cspec.Framework
{
    using System;

    public abstract class FeatureAttribute : Attribute, IAcceptanceAttribute
    {
        protected FeatureAttribute(string text)
        {
            this.Text = text;
        }

        public string Text { get; set; }
    }
}