namespace ArguMint
{
   internal interface IArgumentRule
   {
      void Match( object argumentClass, IMarkedProperty<ArgumentAttribute> property, string[] arguments );
   }
}
