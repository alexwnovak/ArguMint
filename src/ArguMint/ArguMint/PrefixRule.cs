namespace ArguMint
{
   internal class PrefixRule : IArgumentRule
   {
      public void Match( object argumentClass, IMarkedProperty<ArgumentAttribute> property, string[] arguments )
      {
         if ( !string.IsNullOrEmpty( property.Attribute.Argument ) )
         {
            if ( property.Attribute.Spacing == Spacing.None )
            {
               foreach ( string argument in arguments )
               {
                  if ( argument.StartsWith( property.Attribute.Argument ) )
                  {
                     string value = argument.Replace( property.Attribute.Argument, string.Empty );
                     property.SetPropertyValue( argumentClass, value );
                  }
               }
            }
            else if ( property.Attribute.Spacing == Spacing.Postfix )
            {
               for ( int index = 0; index < arguments.Length; index++ )
               {
                  if ( arguments[index] == property.Attribute.Argument )
                  {
                     string value = arguments[index + 1];
                     property.SetPropertyValue( argumentClass, value );
                  }
               }
            }
         }
      }
   }
}
