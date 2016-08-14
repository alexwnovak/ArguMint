namespace ArguMint
{
   internal interface IArgumentRule
   {
      void Match( object arguentClass, IMarkedProperty<ArgumentAttribute> property, string[] arguments );
   }
}
