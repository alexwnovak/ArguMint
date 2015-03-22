using System;

namespace ArguMint
{
   [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
   public class ArgumentAttribute : Attribute
   {
      private readonly string _argument;
      public string Argument
      {
         get
         {
            return _argument;
         }
      }

      public ArgumentAttribute( string argument )
      {
         _argument = argument;
      }
   }
}
