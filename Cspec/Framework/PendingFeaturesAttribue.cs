namespace Cspec.Framework
{
    using System;

    public class Criteria : Attribute
    {
        public Criteria(string[] criteria)
        {
            this.Value = criteria;
        }

        public string[] Value { get; private set; }
    }
}