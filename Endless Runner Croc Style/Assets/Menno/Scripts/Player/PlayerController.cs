using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Serialization;

// Gemaakt door menno
public class PlayerController : MonoBehaviour
{
    #region Overall Settings

    [Header("The Overall Settings")]
    [Tooltip("Rigidbody player")]
    [SerializeField] private Rigidbody m_rb;

    [Tooltip("The 0 point to check calculations with")]
    private int m_zeroPoint;

    [Tooltip("The bool that checks if you are allowed to jump")]
    public bool TouchingWater = false;

    [Tooltip("Player data and info")]
    public PlayerData Data;

    #endregion

    #region Jump and Gravity Settings
    [Header("The jump and gravity settings")]
    [Tooltip("The gravity so that if the player jumps the gravity will work like the reall gravity in real life")]
    [SerializeField] private float m_gravity = -9.81f;

    [Tooltip("The jump force for the player when he is jumping")]
    public float JumpForce = 10;

    [Tooltip("The scale to scale the gravity")]
    public float GravityScale = 3f;

    #endregion

    #region Lane Settings
    [Header("Lane Settings")]
    [Tooltip("The of the lane your in")]
    public int LaneNumber = 1;

    [Tooltip("The players current x position")]
    [SerializeField] private float m_currentPos;

    [Tooltip("The chosen pos where the player needs to move to")]
    [SerializeField] private float m_chosenXPos = 0;
    
    [Tooltip(" Shows if the croc is already moving")]
    [SerializeField] private bool m_isMoving = false;

    #endregion

    #region Event Settings

    [Header("Event Settings")]
    public UnityEvent CollisionEvents;

    #endregion

    #region Crodile Attack

    [Header("Croc Settings")]
    [Tooltip("The time that takes for the croc to fully rotate 360 degrees")]
    public float TimeForTheCrocAttack = 2;

    [Tooltip("The time until a next spin is allowed by the player")]
    public float TheTimeUntilNextSpin = 5f;

    [Tooltip("The bool to check if the player is allowed to do a spin attack")]
    public bool AttackSpinIsAllowed = true;

    [Tooltip("If this is true then log or flower may be destroyed")]
    public bool DestroyObstacle = false;

    #endregion

    void Start()
    {
        // Gets the rigidbody of the player
        m_rb = GetComponent<Rigidbody>();

        // Sets the the current x pos of the player at the start of the game
        m_currentPos = transform.position.x;
     }

    void Update()
    {
        MoveInput();
        MovePlayerToLane(Data.TheTimeToMoveToLane, m_isMoving);
    }

    private void FixedUpdate()
    {
        Grav();
    }

    #region Movement of the player

    private void MoveInput()
    {
        #region Spin Attack

        if (AttackSpinIsAllowed)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Starts the spin attack of the croc
                StartCoroutine(CrocSpinAttack(TimeForTheCrocAttack));

                // Sets the attack spin is allowed is set to false so you can not spam spins
                AttackSpinIsAllowed = false;
            }  
        }

        #endregion

        #region Allowed to jump

        if (TouchingWater)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {
                // Lets the player jump
                m_rb.AddForce(new Vector3(0, transform.position.y, 0) + Vector3.up * JumpForce, ForceMode.Impulse);

                // Sets the touching water bool to false so you wont be able to do more jumps while in air
                TouchingWater = false;
            }

        }

        #endregion

        #region Allowed to move to different lane

        if (m_isMoving)
            return;
        
        if (LaneNumber != 0)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // Then if the player comes closer to position then it snaps to the position
                LaneNumber--;
            }
        }
        if (LaneNumber != 2)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Sets the lane number of where you then have to move to on the x axis
                LaneNumber++;
            }
        }

        #endregion
    }

    // based of from this page : https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/

    private void MovePlayerToLane(float _duration, bool _isMoving)
    {
        // The Stores current pos of the player
        Vector3 _startPosition = transform.position;

        // The pos where the player needs to go to
        Vector3 _endPoint = new Vector3(GameManager.Instance.Paths[LaneNumber].PathPositionX, transform.position.y, transform.position.z);

        // Sets the x pos of the chosen position and if the croc is already moving or not
        m_chosenXPos = _endPoint.x;

        if (m_currentPos != m_chosenXPos)
        {
            _isMoving = true;
            // Moves the player to the right lane
            gameObject.transform.position = Vector3.Lerp(_startPosition, _endPoint, Time.deltaTime * _duration);
        }
        
        // If the calculation is Higher then 0
        if (m_chosenXPos - transform.position.x > m_zeroPoint)
        {
            // Then if the player comes closer to position then it snaps to the position
            if (m_chosenXPos - transform.position.x < Data.Distance)
            {
                SetPlayerPos(_endPoint);
                StartCoroutine(MovingAlllowed(Data.WaitingTimeToMove));
            }
        }
        // If the calculation is lower then 0
        else if (m_chosenXPos - transform.position.x < m_zeroPoint)
        {
            // Then if the player comes closer to position then it snaps to the position
            if (m_chosenXPos - transform.position.x > -Data.Distance)
            {
                SetPlayerPos(_endPoint);
                StartCoroutine(MovingAlllowed(Data.WaitingTimeToMove));
            }
        }

    }

    private IEnumerator MovingAlllowed(float _movingTime)
    {
        yield return new WaitForSeconds(_movingTime);
            
        m_isMoving = false;
        
        yield return null;
    }

    // based of from this page : https://answers.unity.com/questions/1575765/how-to-rotate-about-360-on-a-coroutine.html

    private IEnumerator CrocSpinAttack(float _duration)
    {
        // Sets the time to 0
        float _time = 0.0f;

        // Sets the rotation of the the player in degrees
        float _startRotation = transform.eulerAngles.y;

        // Sets the end rotation with the start + 360 degrees
        float _endRotation = _startRotation + 360.0f;

        // As long as you spin you are able to destroy a log
        DestroyObstacle = true;

        while (_time < _duration)
        {
            // Count the time that the player is rotating
            _time += Time.deltaTime;

            // Calculates the rotation
            float _yRotation = Mathf.Lerp(_startRotation, _endRotation, _time / _duration) % 360.0f;

            // Sets the transform rotation in degrees
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, _yRotation, transform.eulerAngles.z);

            yield return null;

        }

        // After the spin you are not allowed to be able to destroy a log so it is set to false
        DestroyObstacle = false;

        yield return new WaitForSeconds(TheTimeUntilNextSpin);

        AttackSpinIsAllowed = true;

    }

    private void SetPlayerPos(Vector3 _endPoint)
    {
        // Sets the player at the end fully at the right location
        transform.position = _endPoint;

        // Sets the current pos to the pos the player moved to
        m_currentPos = m_chosenXPos;
    }

    #endregion

    #region Gravity

    private void Grav()
    {
        // Calculates the gravity that works on the player
        Vector3 _gravity = m_gravity * GravityScale * m_rb.mass * Vector3.up;

        // Adds the gravity on to the player
        m_rb.AddForce(_gravity, ForceMode.Acceleration);
    }

    #endregion

    #region Collision

    private void OnCollisionEnter(Collision collision)
    {
        // Looks for the water so the player is able to jump
        if (collision.gameObject.GetComponent<WaterTag>())

            // Sets the water to true so the player is able to jump again
            TouchingWater = true;

        if (collision.gameObject.TryGetComponent<ObstacleTag01>(out ObstacleTag01 _obstacle))
        {
            if (DestroyObstacle)
            {
                // Returns the log in the spin attack to the right pool
                _obstacle.GetComponent<PoolItem>().ReturnToPool();
                return;
            }

            CollisionEvents.Invoke();
        }
    }

    #endregion

}
