using System;
using System.Collections.Generic;
using _Game.Scripts.CarRelated;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    
    //We need to give this class to serializable spec to save the data that we set from editor.
    [Serializable]
    public class ColumnData
    {
        [SerializeField]
        private Vector2[] column = new Vector2[GameController.ColumnNumber];
        public Vector2[] ColumnValue
        {
            get => column;
            set => column = value;
        }
        public float Direction { get; private set; }
        public void DirectionSetter(float centerGround)
        {
            // Car at bottom half of ground. Look Up
            if (column[0].y<centerGround)
            {
                Direction = 0;
            }
            //Car at upper half of ground.Look down
            else
            {
                Direction = -180;
            }
        }
    }
    public class GameController : MonoBehaviour
    {
        public GameObject carPrefab;

        public const int RotationSpeed = 375;
        public const int CarSpeed = 12;
        public const int ColumnNumber = 2;

        private const int TotalCarLimit = 8;

        [SerializeField]
        private ColumnData[] rows = new ColumnData[TotalCarLimit];
        public ColumnData[] RowsValue => rows;

        //Because of 2Dimensional Arrays cant serialize, used jagged arrays
        /*public readonly Vector2[,] StartEndPoints = new Vector2[8,2];*/

        //This boolean is described at CarController class=>ResetActivator method.
        [SerializeField] [HideInInspector]
        private bool isOptionMode;
        public bool IsOptionMode => isOptionMode;
        public Vector2 CurrentGroundScale { get; private set; }
        public int CurrentCar { get; private set; }
        //The red box that shows us the ending point at the Play Mode.
        private GameObject _endPointSquare;
        //For Instantiated cars.
        private readonly List<GameObject> _carList = new List<GameObject>();

        private void Start()
        {
            _endPointSquare = GameObject.Find("EndPointSquare");
            CurrentGroundScale = transform.localScale;

            ////Instantiate all the cars at the beginning to better performance=> object pooling
            for (var i = 0; i < TotalCarLimit; i++)
            {
                // Set the direction according to start position.
                RowsValue[i].DirectionSetter(transform.position.y);
                var car =Instantiate(carPrefab, Vector3.zero, Quaternion.Euler(0,0,RowsValue[i].Direction));
                _carList.Add(car);
            }
            //Prepare the first car for action.
            var firstCar = _carList[0];
            firstCar.SetActive(true);
            firstCar.transform.position = RowsValue[CurrentCar].ColumnValue[0];
            _endPointSquare.GetComponent<SpriteRenderer>().enabled = true;
            _endPointSquare.transform.position = RowsValue[CurrentCar].ColumnValue[1];
        }
        //Reset the old cars' positions and set the new one.
        public void ResetSpawnNextCar()
        {
            //Finish and move on to next Scene
            if (CurrentCar==TotalCarLimit-1)
            {
                var currentScene = SceneManager.GetActiveScene().buildIndex;
                if (currentScene == SceneManager.sceneCountInBuildSettings-1)
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
            else
            {
                // For Old cars
                for (var z = 0; z <= CurrentCar; z++)
                {
                    var selectedCar = _carList[z].transform;
                    var selectedComponent = _carList[z].GetComponent<CarController>();
                    selectedCar.position = RowsValue[z].ColumnValue[0];
                    selectedCar.rotation = Quaternion.Euler(0,0,RowsValue[CurrentCar].Direction);
                    selectedComponent.SpriteOnOff(false);
                    selectedComponent.SetState(new IdleState(selectedComponent));
                }
                //New car
                CurrentCar++;
                _carList[CurrentCar].transform.position = RowsValue[CurrentCar].ColumnValue[0];
                _carList[CurrentCar].gameObject.SetActive(true);
                _endPointSquare.transform.position = RowsValue[CurrentCar].ColumnValue[1];
            }
        }
        //Reset the all cars and start again without adding new car.
        public void ResetMethod()
        {
            for (var z = 0; z <= CurrentCar; z++)
            {
                var selectedCar = _carList[z].transform;
                var selectedComponent = _carList[z].GetComponent<CarController>();
                selectedCar.position = RowsValue[z].ColumnValue[0];
                selectedCar.rotation = Quaternion.Euler(0,0,RowsValue[CurrentCar].Direction);
                //Set the all cars state to Idle.
                selectedComponent.SetState(new IdleState(selectedComponent));
                // Don't change the current cars' visibility to give the effect that we just see current car at the beginning.
                if (z!=CurrentCar)
                {
                    selectedComponent.SpriteOnOff(false);
                }
            }
        }
    }
}
