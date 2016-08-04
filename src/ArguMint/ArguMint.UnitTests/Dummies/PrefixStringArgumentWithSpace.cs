namespace ArguMint.UnitTests.Dummies
{
   public class PrefixStringArgumentWithSpace
   {
      [Argument( "/f:" ) ]
      public string FileName
      {
         get;
         set;
      }
   }
}
