namespace Games.Global.TreeBehavior.DecoratorBehavior
{
    public class AlwaysTrue : Decorator
    {
        public AlwaysTrue(TreeNode child) : base(child)
        {
        }

        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            TreeStatus status = child.Execute(behaviorStatus);

            return status == TreeStatus.RUNNING ? TreeStatus.RUNNING : TreeStatus.SUCCESS;
        }

        protected override void OnReset()
        {
            
        }
    }
}