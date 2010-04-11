using System;
using System.Collections.Generic;

namespace JetBrains.Annotations
{
    public class NotNullAttribute : Attribute
    {
        
    }
    public class CanBeNullAttribute : Attribute
    {
        
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [BaseTypeRequired(new Type[] { typeof(Attribute) })]
    public sealed class BaseTypeRequiredAttribute : Attribute
    {
        private readonly Type[] baseTypes;

        public BaseTypeRequiredAttribute(params Type[] baseTypes)
        {
            this.baseTypes = baseTypes;
        }

        public IEnumerable<Type> BaseTypes 
        {
            get { return baseTypes; } 
        }
    }

}