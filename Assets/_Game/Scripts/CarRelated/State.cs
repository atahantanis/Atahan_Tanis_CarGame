
namespace _Game.Scripts.CarRelated
{
    public abstract class State
    {
        protected readonly CarController Controller;
        public abstract void Tick();

        public virtual void FixedTick(){}
        public virtual void OnStateEnter(){}
        public virtual void OnStateExit(){}

        protected State(CarController carController)
        {
            Controller = carController;
        }
    }
}
