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
         var markedMethods = _typeInspector.GetMarkedMethods<ArgumentsOmittedHandlerAttribute>( argumentClass.GetType() );

         if ( markedMethods?.Length == 1 )
         {
            markedMethods[0].Invoke( argumentClass );
         }
      }
   }
}
