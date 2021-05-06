using UnityEditor.Experimental.GraphView;

namespace Games.Global.TreeBehavior
{
    public enum TreeStatus
    {
        SUCCESS,
        FAILURE,
        RUNNING
    }

    public abstract class BehaviorStatus
    {
        
    }
    
    public abstract class TreeNode
    {
        private bool starting = true;
        private int ticks = 0;

        public TreeStatus Execute(BehaviorStatus behaviorStatus)
        {
            TreeStatus status = OnExecute(behaviorStatus);

            ticks++;
            starting = false;

            if (status != TreeStatus.RUNNING)
            {
                Reset();
            }

            return status;
        }

        public void Reset()
        {
            starting = false;
            ticks = 0;
            OnReset();
        }

        public abstract TreeStatus OnExecute(BehaviorStatus behaviorStatus);
        protected abstract void OnReset();
    }
}