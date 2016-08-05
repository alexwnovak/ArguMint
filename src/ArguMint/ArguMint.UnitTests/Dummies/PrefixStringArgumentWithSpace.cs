namespace ArguMint.UnitTests.Dummies
{
   public class PrefixStringArgumentWithSpace
   {
      [Argument( "-filename", Spacing.Postfix )]
      public string FileName
      {
         get;
         set;
      }
   }
}
