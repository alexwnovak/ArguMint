using System;

namespace ArguMint
{
   internal static class ArgumentPositionExtensions
   {
      public static int ToIndex( this ArgumentPosition argumentPosition )
      {
         if ( argumentPosition < ArgumentPosition.Any || argumentPosition > ArgumentPosition.Tenth )
         {
            throw new ArgumentOutOfRangeException( nameof( argumentPosition ), $"Argument position parameter value was out of range: {(int) argumentPosition}" );
         }

         return (int) argumentPosition - (int) ArgumentPosition.First;
      }
   }
}
