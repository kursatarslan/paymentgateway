using System;

namespace Merchant.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]   
    public class ReactEnvironmentVarAttribute : Attribute {
        public string VarName { get; set; }
        public ReactEnvironmentVarAttribute(string varName) 
            => VarName = varName;
        
    }
}