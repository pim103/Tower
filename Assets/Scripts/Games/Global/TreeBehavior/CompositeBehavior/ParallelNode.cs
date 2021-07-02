namespace Games.Global.TreeBehavior.CompositeBehavior
{
    public class ParallelNode : Composite
    {
        private TreeStatus saveStatus = TreeStatus.SUCCESS;
        
        public ParallelNode(params TreeNode[] children) : base(children)
        {
            
        }
        
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            TreeStatus currentStatus = children[currentChild].Execute(behaviorStatus);
            currentChild++;

            if (currentStatus == TreeStatus.FAILURE)
            {
                saveStatus = TreeStatus.FAILURE;
            }
            
            return currentChild == children.Count ? saveStatus : OnExecute(behaviorStatus);
        }

        protected override void OnReset()
        {
            currentChild = 0;
            foreach (TreeNode node in children)
            {
                node.Reset();
            }
        }
    }
}