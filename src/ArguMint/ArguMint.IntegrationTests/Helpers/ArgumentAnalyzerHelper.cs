using System;
using System.Reflection;

namespace ArguMint.TestCommon.Helpers
{
   public static class ArgumentAnalyzerHelper
   {
      public static object Analyze( ArgumentAnalyzer argumentAnalyzer, Type argumentClassType, string[] arguments )
      {
         var analyzeMethod = argumentAnalyzer.GetType().GetMethod( "Analyze", BindingFlags.Public | BindingFlags.Instance );
         var closedAnalyzeMethod = analyzeMethod.MakeGenericMethod( argumentClassType );

         var parameters = new object[]
         {
            arguments
         };

         try
         {
            return closedAnalyzeMethod.Invoke( argumentAnalyzer, parameters );
         }
         catch ( TargetInvocationException ex ) when ( ex.InnerException != null )
         {
            throw ex.InnerException;
         }
      }
   }
}
