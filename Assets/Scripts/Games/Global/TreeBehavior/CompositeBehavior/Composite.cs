using System.Collections.Generic;

namespace Games.Global.TreeBehavior.CompositeBehavior
{
    public abstract class Composite : TreeNode
    {
        protected List<TreeNode> children;
        protected int currentChild;

        protected Composite(params TreeNode[] nodes)
        {
            children = new List<TreeNode>();
            children.AddRange(nodes);
            currentChild = 0;
        }
    }
}