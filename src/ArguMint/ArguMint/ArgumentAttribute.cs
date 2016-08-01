using System;

namespace ArguMint
{
   [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
   public class ArgumentAttribute : Attribute
   {
      public string Argument
      {
         get;
      }

      public ArgumentAttribute( string argument )
      {
         Argument = argument;
      }
   }
}
