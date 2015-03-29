namespace Cspec.Framework
{
    using System;

    public class PendingScenariosAttribute : Attribute
    {
        private readonly string text;

        public PendingScenariosAttribute(string text)
        {
            this.text = text;
        }

        public override string ToString()
        {
            return this.text;
        }
    }
}