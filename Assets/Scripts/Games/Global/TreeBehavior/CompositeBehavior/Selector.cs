namespace Games.Global.TreeBehavior.CompositeBehavior
{
    public class Selector : Composite
    {
        public Selector(params TreeNode[] children) : base(children)
        {
            
        }


        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            TreeStatus currentStatus = children[currentChild].Execute(behaviorStatus);

            switch (currentStatus)
            {
                case TreeStatus.SUCCESS:
                    return TreeStatus.SUCCESS;
                case TreeStatus.FAILURE:
                    currentChild++;
                    break;
            }

            if (currentChild == children.Count)
            {
                return TreeStatus.FAILURE;
            }

            if (currentStatus == TreeStatus.FAILURE)
            {
                return OnExecute(behaviorStatus);
            }

            return TreeStatus.RUNNING;
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