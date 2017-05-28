using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISteering: MonoBehaviour {

    public GameObject _waypointContainer;
    List<CWaypoint> _waypoints;
    public CWaypoint _currentWaypoint;
    Vector3 _carRotation;
    Vector3 _carPosition;
    Vector3 _forwardDirection;
    Vector3 _upDirection;
    bool _accelerating = true;
    bool _boosting;
    bool _riding = true;
    public bool GoRight { private set;  get; }
    public bool GoLeft { private set; get; }
    InputInterface.UserImput _output;

    private void Awake()
    {
        //TODO: Implement the manager for the waypoints
        //_waypoints = CWaypointManager.Inst.GetWaypoints()
        _output = new InputInterface.UserImput();
        _waypoints = new List<CWaypoint>();
        for (int i = 0; i < _waypointContainer.transform.childCount; i++)
        {
            _waypoints.Add(_waypointContainer.transform.GetChild(i).GetComponent<CWaypoint>());
        }
        StartCoroutine(DoDrive());
        StartCoroutine(SteerTowardsWaypoint());
       
    }

    public void UpdateDirections(Vector3 forward, Vector3 position, Vector3 up)
    {
        _carPosition = position;
        _forwardDirection = forward;
        _upDirection = up;
    }

    public InputInterface.UserImput GetInput()
    {
       
        return _output;
    }

    IEnumerator DoDrive()
    {
        _currentWaypoint = _waypoints[0];
        while (_riding)
	    {
            Debug.Log((_currentWaypoint.WorldPosition - _carPosition).magnitude);
            _accelerating = true;
            if ((_currentWaypoint.WorldPosition - _carPosition).magnitude < 40f)
            {
                Debug.Log("changes waypoint");
                int indx = _waypoints.IndexOf(_currentWaypoint) + 1;
                if (indx < _waypoints.Count)
                    _currentWaypoint = _waypoints[indx];
                else
                    _currentWaypoint = _waypoints[0];               
            }
          
            yield return null;
	    }
        _accelerating = false;
    }

    public static float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }


    void CalculateSteering1()
    {
       
        Vector3 vec_1 = _forwardDirection.normalized;
        Vector3 vec_2 = (_currentWaypoint.WorldPosition - _forwardDirection).normalized;

        float crossProductSign = Vector3.Dot(Vector3.Cross(vec_1, vec_2), _upDirection.normalized);
       // Debug.Log(crossProductSign);
        Debug.DrawRay(_carPosition, Vector3.Cross(vec_1, vec_2).normalized * 5, Color.magenta);
        Debug.DrawLine(_carPosition, _currentWaypoint.WorldPosition);

        _output.HorizontalAxis = crossProductSign * 6;
       
    }

    IEnumerator SteerTowardsWaypoint()
    {
        while (_accelerating)
        {

            CalculateSteering1();
            _output.VerticalAxis = (_accelerating ? 1 : 0);
            yield return new WaitForSeconds(0.1f);
        }
        
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
