using ArguMint.TestCommon.Helpers;

namespace ArguMint.UnitTests
{
   internal abstract class TreeNode
   {
      public TreeNode[] Nodes
      {
         get;
      }

      public virtual void Match( object argumentClass, IMarkedProperty<ArgumentAttribute> property, string[] arguments )
      {
         foreach ( var node in Nodes )
         {
            node.Match( argumentClass, property, arguments );
         }
      }
      //public void Match( object argumentClass, IMarkedProperty<ArgumentAttribute> property, string[] arguments )
      //{
      //   foreach ( var node in Nodes )
      //   {
      //      node.Apply( argumentClass, property, arguments );
      //   }
      //}

      //protected abstract void Apply( object argumentClass, IMarkedProperty<ArgumentAttribute> property, string[] arguments );
   }

   internal class LowercaseNode : TreeNode
   {
      public override void Match( object argumentClass, IMarkedProperty<ArgumentAttribute> property, string[] arguments )
      {
         for ( int index = 0; index < arguments.Length; index++ )
         {
            arguments[index] = arguments[index].ToLower();
         }

         foreach ( var node in Nodes )
         {
            node.Match( argumentClass, property, arguments );
         }
      }
   }

   internal class TestRule : IArgumentRule
   {
      private TreeNode _rootNode;

      public TestRule()
      {
         _rootNode = new LowercaseNode();
      }

      public void Match( object arguentClass, IMarkedProperty<ArgumentAttribute> property, string[] arguments )
      {
         _rootNode.Match( arguentClass, property, arguments );
      }
   }

   public class RuleTests
   {
      public void TryTheThing()
      {
         var testRule = new TestRule();

         testRule.Match( null, null, ArrayHelper.Create( "One", "TWO", "three" ) );
      }
   }
}
