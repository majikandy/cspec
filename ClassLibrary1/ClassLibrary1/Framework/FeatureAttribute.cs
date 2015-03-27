namespace CSpec.Framework
{
    using System;

    public abstract class FeatureAttribute : Attribute, DocGen.IAcceptanceAttribute
    {
        protected FeatureAttribute(string text)
        {
            this.Text = text;
        }

        public string Text { get; set; }
    }
}