using System;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;




    [SerializeField] private float _speed = 3f;

    [SerializeField] private List<Engine> _allEnginesGroup;
    [SerializeField] private List<GameObject> _allEngines;


    #region Engines
    [Header("Move Speed")]
    [SerializeField] private float _frontSpeed = 4f;
    [SerializeField] private float _backSpeed = 2f;

    [SerializeField] private float _leftRightSpeed = 3f;
    [SerializeField] private float _upDownSpeed = 3f;

    [Header("Rotate Speed")]
    [SerializeField] private float _upDownRotationSpeed = 1f;
    [SerializeField] private float _leftRightRotationSpeed = 1f;
    [SerializeField] private float _clockwiseRotaionSpeed = 1f;

    [Header("Move Engines")]
    [SerializeField] private List<GameObject> _GoBackEngines;
    [SerializeField] private List<GameObject> _GoFrontEngine;

    [SerializeField] private List<GameObject> _GoDownEngines;
    [SerializeField] private List<GameObject> _GoUpEngines;

    [SerializeField] private List<GameObject> _GoLeftEngines;
    [SerializeField] private List<GameObject> _GoRightEngines;


    [Header("Rotate Engines")]
    [SerializeField] private List<GameObject> _RollLeftEngines;
    [SerializeField] private List<GameObject> _RollRightEngines;

    [SerializeField] private List<GameObject> _RollDownEngines;
    [SerializeField] private List<GameObject> _RollUpEngines;

    [SerializeField] private List<GameObject> _RollContClockEngines;
    [SerializeField] private List<GameObject> _RollClokUpEngines;


    #endregion


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _playerInput = GetComponent<PlayerInput>();

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Shot.performed += ShotAction_performed;


        #region engines        

        _allEngines = new List<GameObject>();
        _allEngines = _allEngines.Concat(_GoFrontEngine).Concat(_GoBackEngines).Concat(_GoUpEngines).Concat(_GoDownEngines).Concat(_GoLeftEngines).Concat(_GoRightEngines).ToList();
        _allEngines = _allEngines.Concat(_RollRightEngines).Concat(_RollLeftEngines).Concat(_RollUpEngines).Concat(_RollDownEngines).Concat(_RollClokUpEngines).Concat(_RollContClockEngines).ToList();
        _allEngines = _allEngines.Distinct().ToList();

        #endregion
    }


    public void ShotAction_performed(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }



    private void LateUpdate()
    {

        Vector3 direcion = _playerInputActions.Player.Movment.ReadValue<Vector3>();
        direcion = GetMovmentSpeedModfied(direcion);
        _rb.AddRelativeForce(direcion * _speed, ForceMode.Force);

        Vector3 roll = _playerInputActions.Player.Rotation.ReadValue<Vector3>();
        roll = GetRotationSpeedModfied(roll);
        _rb.AddRelativeTorque(roll * _speed, ForceMode.Force);

        IgniteEngines(direcion,  roll);
    }

    private void IgniteEngines(Vector3 direcion, Vector3 roll)
    {
        List<GameObject> engines = new List<GameObject>();
        if (direcion.z > 0)
        {
            engines.AddRange(_GoFrontEngine);
        }
        else if (direcion.z < 0)
        {
            engines.AddRange(_GoBackEngines);
        }

        if (direcion.x > 0)
        {
            engines.AddRange(_GoRightEngines);
        }
        else if (direcion.x < 0)
        {
            engines.AddRange(_GoLeftEngines);
        }

        if (direcion.y > 0)
        {
            engines.AddRange(_GoUpEngines);
        }
        else if (direcion.y < 0)
        {
            engines.AddRange(_GoDownEngines);
        }

        if (roll.x > 0)
        {
            engines.AddRange(_RollUpEngines);
        }
        else if (roll.x < 0)
        {
            engines.AddRange(_RollDownEngines);
        }
        if (roll.y > 0)
        {
            engines.AddRange(_RollRightEngines);
        }
        else if (roll.y < 0)
        {
            engines.AddRange(_RollLeftEngines);
        }
        if (roll.z > 0)
        {
            engines.AddRange(_RollContClockEngines);
        }
        else if (roll.z < 0)
        {
            engines.AddRange(_RollClokUpEngines);
        }


        List<GameObject> enginess = new List<GameObject>(_allEngines);

        enginess.RemoveAll(x => engines.Contains(x));

        foreach (var item in engines)
        {
            item.SetActive(true);
        }
        foreach (var item in enginess)
        {
            item.SetActive(false);
        }

    }

    private Vector3 GetMovmentSpeedModfied(Vector3 inputDirections)
    {
        if (inputDirections.z > 0)
        {
            inputDirections.z *= _frontSpeed;
        }
        else
        {
            inputDirections.z *= _backSpeed;
        }

        inputDirections.x *= _leftRightSpeed;
        inputDirections.y *= _upDownSpeed;

        return inputDirections;
    }

    private Vector3 GetRotationSpeedModfied(Vector3 inputRotations)
    {
        inputRotations.x *= _upDownRotationSpeed;
        inputRotations.y *= _leftRightRotationSpeed;
        inputRotations.z *= _clockwiseRotaionSpeed;

        return inputRotations;
    }





    private List<Engine> GetCorrestEngineList(List<GameObject> engineToList)
    {
        List<Engine> list = new List<Engine>();
        foreach (var item in engineToList)
        {
            list.Add(new Engine { engin = item, active = false });
        }

        _allEnginesGroup.AddRange(list);
        return list;
    }


    [Serializable]
    private class Engine
    {
        public GameObject engin;
        public bool active = true;

    }


}



