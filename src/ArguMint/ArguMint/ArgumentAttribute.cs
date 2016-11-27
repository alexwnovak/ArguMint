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

      public Spacing Spacing
      {
         get;
      }

      public ArgumentPosition Position
      {
         get;
         set;
      }

      public ArgumentAttribute()
      {
      }

      public ArgumentAttribute( string argument )
      {
         Argument = argument;
      }

      public ArgumentAttribute( string argument, Spacing spacing )
         : this( argument )
      {
         if ( spacing < Spacing.None || spacing > Spacing.Postfix )
         {
            throw new ArgumentOutOfRangeException( nameof( spacing ), $"Spacing parameter value was out of range: {(int) spacing}" );
         }

         Spacing = spacing;
      }
   }
}
