using System.Linq;

namespace ArguMint.UnitTests.Helpers
{
   internal static class TokenHelper
   {
      public static ArgumentToken[] CreateArray( params string[] tokens )
         => tokens.Select( s => new ArgumentToken( s ) ).ToArray();
   }
}
