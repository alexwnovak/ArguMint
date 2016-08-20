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

                     if ( string.IsNullOrEmpty( value ) )
                     {
                        throw new ArgumentErrorException( ArgumentErrorType.PrefixArgumentHasNoValue );
                     }

                     object convertedValue = ValueConverter.Convert( value, property.PropertyType );

                     if ( convertedValue == null )
                     {
                        throw new ArgumentErrorException( ArgumentErrorType.TypeMismatch );
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
                  string thisArgument = arguments[index].Trim();

                  if ( thisArgument == argumentString )
                  {
                     if ( index + 1 >= arguments.Length )
                     {
                        throw new ArgumentErrorException( ArgumentErrorType.PrefixArgumentHasNoValue );
                     }

                     string value = arguments[index + 1];
                     property.SetPropertyValue( argumentClass, value );
                  }
               }
            }
         }
      }
   }
}
