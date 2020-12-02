using System;
using Model.Enums;

namespace Model.Annotations
{
    public class HasPermission : Attribute
    {
        public Permissions Permission { get; set; }

        public bool HasMaxValue { get; set; }

        private Permissions maxValue;

        public Permissions MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                HasMaxValue = true;
            }
        }
    }
}
