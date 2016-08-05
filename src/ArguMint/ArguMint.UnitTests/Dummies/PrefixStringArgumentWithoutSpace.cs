namespace ArguMint.UnitTests.Dummies
{
   public class PrefixStringArgumentWithoutSpace
   {
      [Argument( "/f:" ) ]
      public string FileName
      {
         get;
         set;
      }
   }
}
