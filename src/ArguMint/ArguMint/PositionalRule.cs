namespace ArguMint
{
   internal class PositionalRule : IArgumentRule
   {
      public void Match( object argumentClass, IMarkedProperty<ArgumentAttribute> property, string[] arguments )
      {
         if ( property.Attribute.Position == ArgumentPosition.Any )
         {
            foreach ( string argument in arguments )
            {
               if ( argument == property.Attribute.Argument )
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
               object convertedValue = ValueConverter.Convert( arguments[index], property.PropertyType );

               if ( convertedValue == null )
               {
                  ArgumentError.ThrowForTypeMismatch( property.PropertyName, property.PropertyType.Name );
               }

               property.SetPropertyValue( argumentClass, convertedValue );
            }
            else
            {
               ArgumentError.ThrowForArgumentMissing( property.PropertyName );;
            }
         }
      }
   }
}
