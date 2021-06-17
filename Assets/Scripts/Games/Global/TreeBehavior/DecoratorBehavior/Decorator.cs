namespace Games.Global.TreeBehavior.DecoratorBehavior
{
    public abstract class Decorator : TreeNode
    {
        protected TreeNode child;

        protected Decorator(TreeNode child)
        {
            this.child = child;
        }
    }
}