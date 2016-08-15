using System;

namespace ArguMint
{
   internal class HandlerDispatcher : IHandlerDispatcher
   {
      private readonly ITypeInspector _typeInspector;

      public HandlerDispatcher( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      public void DispatchArgumentsOmitted( object argumentClass )
      {
         if ( argumentClass == null )
         {
            throw new ArgumentException( "Argument class must not be null" );
         }

         var markedMethods = _typeInspector.GetMarkedMethods<ArgumentsOmittedHandlerAttribute>( argumentClass.GetType() );

         if ( markedMethods != null )
         {
            int count = markedMethods.Length;

            if ( count == 1 )
            {
               markedMethods[0].Invoke( argumentClass );
            }
            else if ( count > 1 )
            {
               throw new ArgumentConfigurationException( $"Argument class can only have one ArgumentsOmittedHandler but found {count}" );
            }
         }
      }

      public void DispatchArgumentError( object argumentClass )
      {
         if ( argumentClass == null )
         {
            throw new ArgumentException( "Argument class must not be null" );
         }
      }
   }
}
