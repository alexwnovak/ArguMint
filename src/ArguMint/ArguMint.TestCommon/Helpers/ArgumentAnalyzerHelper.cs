using System;
using System.Reflection;

namespace ArguMint.TestCommon.Helpers
{
   public static class ArgumentAnalyzerHelper
   {
      public static object Analyze<T>( Type argumentClassType, string[] arguments )
      {
         var analyzeMethod = typeof( T ).GetMethod( "Analyze", BindingFlags.Public | BindingFlags.Static );
         var closedAnalyzeMethod = analyzeMethod.MakeGenericMethod( argumentClassType );

         var parameters = new object[]
         {
            arguments
         };

         try
         {
            return closedAnalyzeMethod.Invoke( null, parameters );
         }
         catch ( TargetInvocationException ex ) when ( ex.InnerException != null )
         {
            throw ex.InnerException;
         }
      }
   }
}
