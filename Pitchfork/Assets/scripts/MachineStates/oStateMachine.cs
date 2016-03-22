using UnityEngine;

public enum MachineState { Entry, Active, Exit, Exited }

public abstract class oStateMachine : MonoBehaviour
{
    public MachineState State;

    public void DoUpdate() {
        switch(State)
        {
            case MachineState.Entry:
                Debug.Log("Start: " + this.GetType());

                State = MachineState.Active;
                Entry();
                break;
            case MachineState.Active:
                Active();
                break;
            case MachineState.Exit:
                State = MachineState.Exited;
                Exit();
                break;
            case MachineState.Exited:
                break;
        }
    }
    public abstract void Entry();
    public abstract void Active();
    public abstract void Exit();
}
