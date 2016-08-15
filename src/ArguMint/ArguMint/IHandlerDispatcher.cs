namespace ArguMint
{
   internal interface IHandlerDispatcher
   {
      void DispatchArgumentsOmitted( object argumentClass );
      void DispatchArgumentError( object argumentClass );
   }
}
