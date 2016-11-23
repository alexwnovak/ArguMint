namespace ArguMint
{
   internal class PrefixRule : IArgumentRule
   {
      public void Match( object argumentClass, IMarkedProperty<ArgumentAttribute> property, ArgumentToken[] arguments )
      {
         if ( !string.IsNullOrEmpty( property.Attribute.Argument ) )
         {
            if ( property.Attribute.Spacing == Spacing.None )
            {
               foreach ( var argument in arguments )
               {
                  if ( argument.Token == property.Attribute.Argument && property.PropertyType == typeof( bool ) )
                  {
                     property.SetPropertyValue( argumentClass, true );
                  }
                  else if ( argument.Token.StartsWith( property.Attribute.Argument ) )
                  {
                     string value = argument.Token.Replace( property.Attribute.Argument, string.Empty );

                     if ( string.IsNullOrEmpty( value ) )
                     {
                        ArgumentError.ThrowForPrefixArgumentHasNoValue( property.PropertyName );
                     }

                     object convertedValue = ValueConverter.Convert( value, property.PropertyType );

                     if ( convertedValue == null )
                     {
                        ArgumentError.ThrowForTypeMismatch( property.PropertyName, property.PropertyType.Name );
                     }

                     property.SetPropertyValue( argumentClass, convertedValue );
                  }
               }
            }
            else if ( property.Attribute.Spacing == Spacing.Postfix )
            {
               string argumentString = property.Attribute.Argument.Trim();

               for ( int index = 0; index < arguments.Length; index++ )
               {
                  string thisArgument = arguments[index].Token.Trim();

                  if ( thisArgument == argumentString )
                  {
                     if ( index + 1 >= arguments.Length )
                     {
                        ArgumentError.ThrowForPrefixArgumentHasNoValue( property.PropertyName );
                     }

                     string value = arguments[index + 1].Token;
                     property.SetPropertyValue( argumentClass, value );
                  }
               }
            }
         }
      }
   }
}
