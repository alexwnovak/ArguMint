using System;

namespace Blargument
{
   public static class ArgumentAnalyzer
   {
      public static T Analyze<T>( string[] arguments ) where T : class, new()
      {
         if ( arguments == null )
         {
            throw new ArgumentException( "Arguments must not be null", "arguments" );
         }

         throw new NotImplementedException();
      }
   }
}
