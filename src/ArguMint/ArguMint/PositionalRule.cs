namespace ArguMint
{
   internal class PositionalRule : IArgumentRule
   {
      public void Match( object argumentClass, IMarkedProperty<ArgumentAttribute> property, ArgumentToken[] arguments )
      {
         if ( property.Attribute.Position == ArgumentPosition.Any )
         {
            foreach ( var argument in arguments )
            {
               if ( !argument.IsMatched && argument.Token == property.Attribute.Argument )
               {
                  if ( property.PropertyType == typeof( bool ) )
                  {
                     property.SetPropertyValue( argumentClass, true );
                     break;
                  }
               }
            }
         }
         else
         {
            int index = property.Attribute.Position.ToIndex();

            if ( arguments.Length >= index + 1 )
            {
               object convertedValue = ValueConverter.Convert( arguments[index].Token, property.PropertyType );

               if ( convertedValue == null )
               {
                  ArgumentError.ThrowForTypeMismatch( property.PropertyName, property.PropertyType.Name );
               }

               property.SetPropertyValue( argumentClass, convertedValue );
               arguments[index].IsMatched = true;
            }
            else
            {
               ArgumentError.ThrowForArgumentMissing( property.PropertyName );
            }
         }
      }
   }
}
