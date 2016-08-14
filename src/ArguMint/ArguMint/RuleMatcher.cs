using System;

namespace ArguMint
{
   internal class RuleMatcher : IRuleMatcher
   {
      private readonly ITypeInspector _typeInspector;

      public RuleMatcher( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      public void Match( object argumentClass, string[] arguments )
      {
         var markedProperties = _typeInspector.GetMarkedProperties<ArgumentAttribute>( argumentClass.GetType() );

         if ( markedProperties.Length == 0 )
         {
            throw new ArgumentConfigurationException( "Argument class must have argument properties denoted by ArgumentAttributes" );
         }
      }
   }
}
