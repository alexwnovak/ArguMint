namespace ArguMint
{
   internal class RuleMatcher : IRuleMatcher
   {
      private readonly IRuleProvider _ruleProvider;
      private readonly ITypeInspector _typeInspector;

      public RuleMatcher( IRuleProvider ruleProvider, ITypeInspector typeInspector )
      {
         _ruleProvider = ruleProvider;
         _typeInspector = typeInspector;
      }

      public void Match( object argumentClass, ArgumentToken[] arguments )
      {
         var markedProperties = _typeInspector.GetMarkedProperties<ArgumentAttribute>( argumentClass.GetType() );

         if ( markedProperties.Length == 0 )
         {
            throw new ArgumentConfigurationException( "Argument class must have argument properties denoted by ArgumentAttributes" );
         }

         foreach ( var markedProperty in markedProperties )
         {
            foreach ( var rule in _ruleProvider.GetRules() )
            {
               rule.Match( argumentClass, markedProperty, arguments );
            }
         }
      }
   }
}
