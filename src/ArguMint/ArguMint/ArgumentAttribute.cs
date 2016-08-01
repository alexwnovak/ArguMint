using System;

namespace ArguMint
{
   [AttributeUsage( AttributeTargets.Property )]
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
