using System;

namespace PayDay2SaveView
{
    public class ArgumentAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; }

        public ArgumentAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}