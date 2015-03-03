using System;

namespace Blargument
{
   [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
   public class ArgumentAttribute : Attribute
   {
   }
}
