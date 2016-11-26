using System;
using System.Linq;

namespace ArguMint
{
   public class ArgumentAnalyzer
   {
      private readonly IHandlerDispatcher _handlerDispatcher;
      private readonly IRuleMatcher _ruleMatcher;

      private ArgumentAnalyzer()
      {
         _handlerDispatcher = new HandlerDispatcher( new TypeInspector() );
         _ruleMatcher = new RuleMatcher( new RuleProvider(), new TypeInspector() );
      }

      internal ArgumentAnalyzer( IHandlerDispatcher handlerDispatcher, IRuleMatcher ruleMatcher )
      {
         _handlerDispatcher = handlerDispatcher;
         _ruleMatcher = ruleMatcher;
      }

      public static T Analyze<T>( string[] arguments ) where T : class, new()
         => new ArgumentAnalyzer().AnalyzeCore<T>( arguments );

      internal T AnalyzeCore<T>( string[] arguments ) where T : class, new()
      {
         if ( arguments == null )
         {
            throw new ArgumentException( "Arguments must not be null", nameof( arguments ) );
         }

         var argumentClass = new T();

         if ( arguments.Length == 0 )
         {
            _handlerDispatcher.DispatchArgumentsOmitted( argumentClass );

            return argumentClass;
         }

         try
         {
            var argumentTokens = arguments.Select( s => new ArgumentToken( s ) ).ToArray();

            _ruleMatcher.Match( argumentClass, argumentTokens );
         }
         catch ( ArgumentErrorException ex )
         {
            _handlerDispatcher.DispatchArgumentError( argumentClass, new ArgumentError( ex.ErrorType, ex.Properties ) );
         }

         return argumentClass;
      }
   }
}
