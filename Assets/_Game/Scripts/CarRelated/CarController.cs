using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.CarRelated
{
    public class CarController : MonoBehaviour
    {
        private State _currentState;

        private SpriteRenderer _spriteRenderer;
        public GameController GameController { private set; get; }
        public Rigidbody2D Rb { get; private set; }
        public List<Vector2> AllPositions { get; } = new List<Vector2>();

        public Vector2 EndingPosition { get; private set; }

        public bool IsPlayed { get; set; }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            IsPlayed = false;
            GameController = GameObject.Find("Ground").GetComponent<GameController>();
            EndingPosition = GameController.RowsValue[GameController.CurrentCar].ColumnValue[1];
            Rb = GetComponent<Rigidbody2D>();
            //At the start of the game object set the state to Idle
            SetState(new IdleState(this));
        }
        private void Update()
        {
            //To act Tick() as Update
            _currentState.Tick();
        }
        private void FixedUpdate()
        {
            //To act FixedTick() as FixedUpdate
            _currentState.FixedTick();
        }
        private void OnCollisionEnter2D(Collision2D col)
        {
            //Make sure we are at Movement State, we only want to check collisions with others, at this state
            if (_currentState.GetType().Name != "MovementState") return;

            if (col.gameObject.CompareTag("Obstacle") || col.gameObject.CompareTag("Car"))
            {
                //Reset
                ResetActivator();
            }
        }
        public void SetState(State state)
        {    // State setter
            _currentState?.OnStateExit();
            _currentState = state;
            _currentState.OnStateEnter();
        }
        public void ResetActivator()
        {
            //Changing this one making the game strange, If we don't clear the past positions, it will show the car's mistakes also.
            if (!GameController.IsOptionMode) {AllPositions.Clear();}
            GameController.ResetMethod();
        }
        public void SpriteOnOff(bool value)
        {
            _spriteRenderer.enabled = value;
        }
    }
}
