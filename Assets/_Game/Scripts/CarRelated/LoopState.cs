using UnityEngine;

namespace _Game.Scripts.CarRelated
{
    public class LoopState : State
    {
        public LoopState(CarController carController) : base(carController) {}

        private const int SpriteDefaultRotation= 90;

        private int _counter;

        public override void OnStateEnter()
        {
            Controller.SpriteOnOff(true);
        }
        public override void OnStateExit()
        {
            _counter = 0;
        }
        public override void Tick()
        {
            //Debug related usage.
        }
        public override void FixedTick()
        {
            //If this car finish movement wait the main car to reset.
            if (_counter== Controller.AllPositions.Count - 1) return;
            var nextPosition = Controller.AllPositions[_counter];
            //Change rotation according to next position, to reduce list allocation.
            #region RotationChanger
            var direction = (Vector2)Controller.transform.position - nextPosition ;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += SpriteDefaultRotation;
            Controller.transform.rotation = Quaternion.Euler(0,0,angle);
            #endregion
            Controller.Rb.MovePosition(nextPosition);
            _counter++;
        }
    }
}
