using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCar : MonoBehaviour {

    public GameObject _childMesh;
    public GameObject _rotatingPivot;

    public Material _dissolveMaterial;

    public float _normalAcceleration;
	public float _boostAcceleration;
    public float _turningSpeed;
	public float _maxVelocityStandar = 1;
	public float _maxVelocityBoost = 1;
    public LayerMask _layerMask;
	public AnimationCurve _hoverCurve;
	public Animator _anim;
	public float _dragOnTheAir;
	public float _dragOnTheFloor;

    private Rigidbody _rb;
    private float _pedal;
	private float _acceleration;
	private float _maxVelocity = 100;
    private const float ORG_MAL_VEL = 100f;
    private float _rotx;
    private float _roty;
    private float _rotz;

    private Vector3 _startingForward;

    private float _lapTime = 0;
    private float _lastLapTime = 0;
    private float _lapNumber = 0;
    private bool _raceStarted = false;

    private float _life = 50;
    private float _turbo = 70;

    public GameObject _lifeContainer;
    public GameObject _turboContainer;
    private Transform[] _lifeCubes;
    private Transform[] _turboCubes;

    public  bool isBoosting = false;
    public  int playerNumber;

    public int _state = 0;
    public const int ALIVE = 0;
    public const int DEAD = 1;

    [HideInInspector] public bool hasDamage;
    public bool AIship = false;
    AISteering _AIComponent;

    // Use this for initialization
    void Start ()
    {
        
        if (AIship)
        {
            _AIComponent = GetComponent<AISteering>();
            _AIComponent.UpdateDirections(_rotatingPivot.transform.forward, transform.position, transform.up);
        }
        _rb = GetComponent<Rigidbody>();
        _startingForward = transform.forward;


        ///lifebar and turbobar
        ///get reference
        _lifeCubes = _lifeContainer.GetComponentsInChildren<Transform>();
        _turboCubes = _turboContainer.GetComponentsInChildren<Transform>();

        ///set active, full life and full turbo
        ///starts at 1 because the container itself is index 0
        for (int i = 1; i < _turboCubes.Length; i++)
        {
            _turboCubes[i].gameObject.SetActive(true);
    }

        for (int i2 = 1; i2 < _lifeCubes.Length; i2++)
        {
            _lifeCubes[i2].gameObject.SetActive(true);
        }
    }

	
	// Update is called once per frame
	void Update ()
    {
        UpdateLifeBar();
        UpdateTurboBar();

        //TODO: CODE IS REPEATED 3 TIMES, THERE ARE BETTER WAYS
        if (!AIship)
        {
            if (getPlayerNumber() == 1)
            {
                InputInterface.UserImput player1;
                player1.HorizontalAxis = Input.GetAxis("Horizontal");
                player1.VerticalAxis = Input.GetAxis("Vertical");
                player1.Boost = Input.GetKey(KeyCode.Space);

                _pedal = player1.VerticalAxis/*Input.GetAxis("Vertical")*/;
                _roty += /*Input.GetAxis("Horizontal")*/player1.HorizontalAxis * _turningSpeed * Time.deltaTime;

                if (player1.HorizontalAxis * 1 > 0)
                {
                    _anim.SetInteger("Turning", 1);
                }
                else if (player1.HorizontalAxis * 1 < 0)
                {
                    _anim.SetInteger("Turning", -1);
                }
                else
                {
                    _anim.SetInteger("Turning", 0);
                }

                if (player1.Boost)
                {
                    isBoosting = true;
                    _acceleration = _boostAcceleration;
                    _maxVelocity = _maxVelocityBoost;
                }
                else
                {
                    isBoosting = false;
                    _acceleration = _normalAcceleration;
                    _maxVelocity = _maxVelocityStandar;
                }
            }
            else if (getPlayerNumber() == 2)
            {
                InputInterface.UserImput player2;
                player2.HorizontalAxis = Input.GetAxis("HorizontalP2");
                player2.VerticalAxis = Input.GetAxis("VerticalP2");
                player2.Boost = Input.GetKey(KeyCode.G);

                _pedal = player2.VerticalAxis;
                _roty += player2.HorizontalAxis * _turningSpeed * Time.deltaTime;

                if (player2.HorizontalAxis * 1 > 0)
                {
                    _anim.SetInteger("Turning", 1);
                }
                else if (player2.HorizontalAxis * 1 < 0)
                {
                    _anim.SetInteger("Turning", -1);
                }
                else
                {
                    _anim.SetInteger("Turning", 0);
                }

                if (player2.Boost)
                {
                    isBoosting = true;
                    _acceleration = _boostAcceleration;
                    _maxVelocity = _maxVelocityBoost;
                }
                else
                {
                    isBoosting = false;
                    _acceleration = _normalAcceleration;
                    _maxVelocity = _maxVelocityStandar;
                }
            }
        }
        else
        {
            _AIComponent.UpdateDirections(_rotatingPivot.transform.forward, transform.position, transform.up);
            InputInterface.UserImput AI = _AIComponent.GetInput();

            _pedal = AI.VerticalAxis/*Input.GetAxis("Vertical")*/;
            _roty += /*Input.GetAxis("Horizontal")*/AI.HorizontalAxis * _turningSpeed * Time.deltaTime;

            if (AI.HorizontalAxis * 1 > 0)
            {
                _anim.SetInteger("Turning", 1);
            }
            else if (AI.HorizontalAxis * 1 < 0)
            {
                _anim.SetInteger("Turning", -1);
            }
            else
            {
                _anim.SetInteger("Turning", 0);
            }

            if (AI.Boost)
            {
                isBoosting = true;
                _acceleration = _boostAcceleration;
                _maxVelocity = _maxVelocityBoost;
            }
            else
            {
                isBoosting = false;
                _acceleration = _normalAcceleration;
                _maxVelocity = _maxVelocityStandar;
            }
        }
      





        //lap time
        if (_raceStarted)
        {
            _lapTime += Time.deltaTime;
        }

        //Manual Blow-up for testing
        if ((Input.GetKeyDown(KeyCode.B) || _life <= 0f) && _state == ALIVE)
        {
            BlowUp();
        }

        //Manual Re-Spawn for testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReSpawn();
        }
    }

    // FixedUpdate is called every fixed framerate frame
    void FixedUpdate()
    {
        _rotatingPivot.transform.localRotation = Quaternion.Euler(new Vector3(_rotx, _roty, _rotz));

		Vector3 upDirection = transform.up;


        //encuentro un punto por encima del player, independiente de su rotación
        Vector3 pointAbovePod = transform.position + transform.up.normalized * 5f;
        //casteo un rayo desde por encima del player hacia abajo del player, que ignora al player
        RaycastHit hitInfo;
        Physics.Raycast(pointAbovePod, -transform.up, out hitInfo, 10f, _layerMask);
        Debug.DrawRay(pointAbovePod, -transform.up * hitInfo.distance, Color.red);

        if (hitInfo.collider != null)
        {
            Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.blue);

            upDirection = hitInfo.normal;

            RaycastHit hit;
            Physics.Raycast(_childMesh.transform.position, _rotatingPivot.transform.forward, out hit, 10f, _layerMask);
            Debug.DrawRay(_childMesh.transform.position, _rotatingPivot.transform.forward * hit.distance, Color.cyan);
            Debug.DrawRay(hit.point, hit.normal, Color.blue);

            if (hit.collider != null) {
                upDirection += hit.normal;
                upDirection /= 2;
            }


            _rb.velocity += _rotatingPivot.transform.forward * _pedal * _acceleration;
            Debug.DrawRay(transform.position, _rb.velocity.normalized * 5f);
            //clamp of velocity
            if (_rb.velocity.magnitude > _maxVelocity)
                _rb.velocity = Vector3.Lerp(_rb.velocity, _rb.velocity.normalized * _maxVelocity, 20f *Time.deltaTime);


			if (hitInfo.collider.gameObject.tag == "No Gravity") {
				_rb.useGravity = false;
                _rb.MovePosition(Vector3.Lerp(_rb.position, hitInfo.point + upDirection * 2, (_rb.velocity.X0Z().magnitude + 100) * 0.05f * Time.deltaTime));
                //_rb.AddForce(Vector3.Lerp(Vector3.zero, transform.up * -200f, hitInfo.distance / 10f), ForceMode.Impulse);

                //_rb.AddForce(Vector3.Lerp(hitInfo.normal * 200f, Vector3.zero, hitInfo.distance / 10f), ForceMode.Impulse);
            } 
			else 
			{
				_rb.useGravity = true;
				_rb.AddForce(Vector3.Lerp(hitInfo.normal * 200f, Vector3.zero, hitInfo.distance / 10f), ForceMode.Impulse);
			}

            Vector3 projectedStartingForward = _startingForward.normalized - (Vector3.Dot(_startingForward.normalized, upDirection.normalized)*upDirection.normalized/(upDirection.magnitude * upDirection.magnitude));

            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(projectedStartingForward, upDirection),
                5 * Time.deltaTime);

            _startingForward = transform.forward;

        

			Debug.DrawRay(transform.position, upDirection, Color.cyan);

            _rb.drag = _dragOnTheFloor;

        }
        else
        {

			transform.rotation = Quaternion.Lerp(transform.rotation,
				Quaternion.LookRotation(transform.forward,Vector3.up),
				5* Time.deltaTime);
			_rb.useGravity = true;
			_rb.drag = _dragOnTheAir;
        }
	
    }

    public float GetLapTime()
    {
        return _lapTime;
    }

    public float GetLastLapTime()
    {
        return _lastLapTime;
    }

    public float GetLapNumber()
    {
        return _lapNumber;
    }
    
    public float GetLife()
    {
        return _life;
    }
    
    public void UpdateLifeBar()
    {

        float maxLife = _lifeCubes.Length;
        float cubesActivated = (int)(_life * maxLife / 100);

        ///starts at 1 because the container itself is index 0
        for (int i = 1; i < _lifeCubes.Length; i++)
        {
            _lifeCubes[i].gameObject.SetActive(false);
        }
        for (int i = 1; i <= cubesActivated; i++)
        {
            _lifeCubes[i].gameObject.SetActive(true);
        }

    }

    public void UpdateTurboBar()
    {

        float maxTurbo = _turboCubes.Length;
        float cubesActivated = (int)(_turbo * maxTurbo / 100);

        ///starts at 1 because the container itself is index 0
        for (int i = 1; i < _turboCubes.Length; i++)
        {
            _turboCubes[i].gameObject.SetActive(false);
        }
        for (int i = 1; i <= cubesActivated; i++)
        {
            _turboCubes[i].gameObject.SetActive(true);
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Start")
        {
            if (!_raceStarted)
            {
                _raceStarted = true;
            }
            else
            {
                _lapNumber += 1;
            }

            _lastLapTime = _lapTime;
            _lapTime = 0;
        }

    }

    public void setPlayerNumber(int _playerNumber)
    {
        playerNumber = _playerNumber;
    }

    public int getPlayerNumber()
    {
        return playerNumber ;
    }

    private void BlowUp()
    {
        foreach (Transform _child in _childMesh.transform)
        {
            _child.gameObject.AddComponent<Rigidbody>();
            _child.gameObject.GetComponent<BoxCollider>().enabled = true;
            Rigidbody _childRb = _child.gameObject.GetComponent<Rigidbody>();
            _childRb.velocity = _rb.velocity + new Vector3(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f));
            CShipPart _childScript = _child.GetComponent<CShipPart>();
            if (_childScript._dissolvesBool)
            {
                _childScript.Dissolve(_dissolveMaterial);
            }
        }

        _state = DEAD;
    }

    private void ReSpawn()
    {
        foreach (Transform _child in _childMesh.transform)
        {
            _child.gameObject.GetComponent<BoxCollider>().enabled = false;
            Rigidbody _childRb = _child.gameObject.GetComponent<Rigidbody>();
            _childRb.velocity = Vector3.zero;
            Destroy(_childRb);
        }

        foreach (Transform _child in _childMesh.transform)
        {
            CShipPart _childScript = _child.GetComponent<CShipPart>();
            _child.localPosition = _childScript._originalPosition;
            _child.localRotation = _childScript._originalRotation;
            _childScript.Reset();
        }

        _state = ALIVE;
        _life = 50f;
    }

    public void Collision(bool _firstCrash)
    {
        float _minDmg = 1f * Time.deltaTime;
        float _maxDmg = 15f * Time.deltaTime;

        if (_firstCrash)
        {
            _maxDmg = 2 * _maxDmg;
        }

        float _damageReceived = Mathf.Lerp(_minDmg, _maxDmg, _rb.velocity.magnitude / ORG_MAL_VEL);

        _life = _life - _damageReceived;
    }

    void OnCollisionEnter(Collision col)
    {
        Collision(true);
        hasDamage = true;
    }

    void OnCollisionStay(Collision col)
    {
        Collision(false);
        hasDamage = false;
    }
}
