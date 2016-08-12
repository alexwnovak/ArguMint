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

         if ( markedMethods?.Length == 1 )
         {
            markedMethods[0].Invoke( argumentClass );
         }
      }
   }
}
