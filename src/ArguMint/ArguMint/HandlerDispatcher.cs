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

      private IMarkedMethod<T>[] GetMarkedMethods<T>( object argumentClass ) where T : Attribute
      {
         if ( argumentClass == null )
         {
            throw new ArgumentException( "Argument class must not be null" );
         }

         var markedMethods = _typeInspector.GetMarkedMethods<T>( argumentClass.GetType() );

         if ( markedMethods?.Length > 1 )
         {
            throw new ArgumentConfigurationException( $"Argument class can only have one {typeof( T ).Name} but found {markedMethods.Length}" );
         }

         return markedMethods;
      }

      public void DispatchArgumentsOmitted( object argumentClass )
      {
         var markedMethods = GetMarkedMethods<ArgumentsOmittedHandlerAttribute>( argumentClass );

         if ( markedMethods?.Length == 1 )
         {
            markedMethods[0].Invoke( argumentClass );
         }
      }

      public void DispatchArgumentError( object argumentClass, ArgumentErrorType errorType )
      {
         var markedMethods = GetMarkedMethods<ArgumentErrorHandlerAttribute>( argumentClass );

         if ( markedMethods?.Length == 1 )
         {
            var parameterTypes = markedMethods[0].ParameterTypes;

            if ( parameterTypes.Length == 0 )
            {
               markedMethods[0].Invoke( argumentClass );
            }
            else if ( parameterTypes.Length == 1 && parameterTypes[0] == typeof( ArgumentErrorType ) )
            {
               markedMethods[0].Invoke( argumentClass, new object[] { errorType } );
            }
         }
      }
   }
}
