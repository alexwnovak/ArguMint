namespace ArguMint
{
   internal class ArgumentToken
   {
      public string Token
      {
         get;
      }

      public bool IsMatched
      {
         get;
         set;
      }

      public ArgumentToken( string token )
      {
         Token = token;
      }
   }
}
