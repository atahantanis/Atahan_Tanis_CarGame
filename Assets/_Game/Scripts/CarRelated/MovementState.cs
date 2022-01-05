using UnityEngine;

namespace _Game.Scripts.CarRelated
{
    public class MovementState : State
    {
        public MovementState(CarController carController) : base(carController) {}

        public override void OnStateEnter()
        {
            //Set the first position to prevent the errors that can show up when we set the starting and ending position same.
            Controller.AllPositions.Add(Controller.transform.position);
        }
        public override void Tick()
        {
            CheckEndDistance();
            if (Input.anyKey)
            {
                //Rotation, Input.GetAxisRaw makes the game arcade rather than Input.GetAxis
                var target = new Vector3(0, 0, -Input.GetAxisRaw("Horizontal"));
                Controller.transform.Rotate(target * (Time.deltaTime * GameController.RotationSpeed));
            }
        }
        public override void FixedTick()
        {
            var transform = Controller.transform;
            var target = transform.up * (Time.deltaTime * GameController.CarSpeed);
            var totalPosition = transform.position + target;
            //Save the positions for LoopState
            Controller.AllPositions.Add(totalPosition);
            Controller.Rb.MovePosition(totalPosition);
        }
        private void CheckEndDistance()
        {
            var gameController = Controller.GameController;
            var position = Controller.transform.position;
            var groundScale = gameController.CurrentGroundScale / 2;
            //Checking the is our car on the ground.
            if (-groundScale.x<=position.x && position.x<= groundScale.x && -groundScale.y<=position.y && position.y<=groundScale.y)
            {
                //Checking if we are close enough to the end positions.
                if (Vector2.Distance(position,Controller.EndingPosition)< 1f)
                {
                    //Next to Reset
                    Controller.IsPlayed = true;
                    gameController.ResetSpawnNextCar();
                }
            }
            else { Controller.ResetActivator(); }
        }
    }
}
