namespace Games.Global.TreeBehavior.CompositeBehavior
{
    public class Sequence : Composite
    {
        public Sequence(params TreeNode[] children) : base(children)
        {

        }

        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            TreeStatus currentStatus = children[currentChild].Execute(behaviorStatus);

            switch (currentStatus)
            {
                case TreeStatus.SUCCESS:
                    currentChild++;
                    break;
                case TreeStatus.FAILURE:
                    return TreeStatus.FAILURE;
            }

            if (currentChild == children.Count)
            {
                return TreeStatus.SUCCESS;
            }

            if (currentStatus == TreeStatus.SUCCESS)
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