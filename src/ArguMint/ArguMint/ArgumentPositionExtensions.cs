namespace ArguMint
{
   internal static class ArgumentPositionExtensions
   {
      public static int ToIndex( this ArgumentPosition argumentPosition )
         => (int) argumentPosition - (int) ArgumentPosition.First;
   }
}
