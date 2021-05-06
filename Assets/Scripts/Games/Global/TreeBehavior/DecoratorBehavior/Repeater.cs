namespace Games.Global.TreeBehavior.DecoratorBehavior
{
    public class Repeater : Decorator
    {
        public Repeater(TreeNode child) : base(child)
        {
        }

        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            TreeStatus status = child.OnExecute(behaviorStatus);

            if (status != TreeStatus.RUNNING)
            {
                Reset();
                child.Reset();
            }

            return TreeStatus.SUCCESS;
        }

        protected override void OnReset()
        {
        }
    }
}