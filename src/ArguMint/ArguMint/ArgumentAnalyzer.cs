using System;

namespace ArguMint
{
   public class ArgumentAnalyzer
   {
      private readonly IHandlerDispatcher _handlerDispatcher;
      private readonly IRuleMatcher _ruleMatcher;

      public ArgumentAnalyzer()
      {
         _handlerDispatcher = new HandlerDispatcher( new TypeInspector() );
         _ruleMatcher = new RuleMatcher( new RuleProvider(), new TypeInspector() );
      }

      internal ArgumentAnalyzer( IHandlerDispatcher handlerDispatcher, IRuleMatcher ruleMatcher )
      {
         _handlerDispatcher = handlerDispatcher;
         _ruleMatcher = ruleMatcher;
      }

      public T Analyze<T>( string[] arguments ) where T : class, new()
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
            _ruleMatcher.Match( argumentClass, arguments );
         }
         catch ( ArgumentErrorException )
         {
            _handlerDispatcher.DispatchArgumentError( argumentClass );
         }

         return argumentClass;
      }
   }
}
