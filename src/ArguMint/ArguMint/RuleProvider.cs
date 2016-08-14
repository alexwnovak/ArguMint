namespace ArguMint
{
   internal class RuleProvider : IRuleProvider
   {
      private static readonly IArgumentRule[] _allRules = DefineRules();

      private static IArgumentRule[] DefineRules()
      {
         return new IArgumentRule[]
         {
            new PositionalRule(),
            new PrefixRule()
         };
      }

      public IArgumentRule[] GetRules() => _allRules;
   }
}
