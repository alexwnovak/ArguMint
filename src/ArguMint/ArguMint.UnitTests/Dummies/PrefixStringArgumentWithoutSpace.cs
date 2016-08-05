namespace ArguMint.UnitTests.Dummies
{
   public class PrefixStringArgumentWithoutSpace
   {
      [Argument( "/f:", Spacing.None )]
      public string FileName
      {
         get;
         set;
      }
   }
}
