using UnityEngine;

namespace _Game.Scripts.CarRelated
{
    public class IdleState : State
    {
        public IdleState(CarController carController) : base(carController) {}

        public override void Tick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //If we came here from the first start, go for movement state.
                if (!Controller.IsPlayed)
                {
                    Controller.SetState(new MovementState(Controller));
                }
                // Else, just loop over according to recorded positions.
                else
                {
                    Controller.SetState(new LoopState(Controller));
                }
            }
        }
    }
}
