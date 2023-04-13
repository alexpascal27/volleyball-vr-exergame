using UnityEngine;

public class PassCannon : MonoBehaviour
{
    [SerializeField] public float maxPower = 10f;
    [SerializeField] public GameObject ballPrefab;
    [SerializeField] public Vector3 defaultRotation = new Vector3(0f, 0f, 0f);
    [SerializeField] public Transform shotPoint;
    // Outside position = Vector3(3.8, 3.2, 0.5)
    // Hitter highest hit while hitting ^ = Vector3(4, 1.5, 1.08)
    [SerializeField] public Vector3 basePassTargetPoint = new Vector3(0, 3.2f, 0.5f);
    [SerializeField] public Vector3 leewayFromBaseTargetPoint = new Vector3(3.8f, 0, 0);
    [SerializeField] public float timeToGetToTarget;

    public GameObject targetPrefab;
    public GameObject hitterPrefab;
    // a bit to the left, much lower and behind
    public Vector3 hitterPositionOffset = new Vector3(0.4f, -1.6f, 0.5f);
    
    public GameObject paddlePrefab;
    // higher and a bit behind
    public Vector3 paddlePositionOffset = new Vector3(0, 2f, 1f);
    public float timeBeforeHitToSpawnPaddle = 0.5f;
    
    [SerializeField, Range(0f, 1f)] public float percentageOfReceiveMisses = 0f;
    [SerializeField] public Vector3 maxReceiveErrorRange;
    [SerializeField, Range(0f, 1f)] public float percentageOfHitMisses = 0f;
    
    
    private float timeLeft;
    private bool canDeduct = false;
    private Vector3 targetPoint;
    private bool receiveMiss;
    private bool hitMiss;

    private void Update()
    {
        if (canDeduct && timeLeft > timeBeforeHitToSpawnPaddle)
        {
            timeLeft -= Time.deltaTime;
        }
        if(canDeduct && timeLeft <= timeBeforeHitToSpawnPaddle)
        {
            if (!receiveMiss || !hitMiss)
            {
               GameObject instantiatedPaddle = Instantiate(paddlePrefab, targetPoint + paddlePositionOffset, paddlePrefab.transform.rotation);
               Rigidbody paddleRb = instantiatedPaddle.GetComponent<Rigidbody>();
               Vector3 predictedVelocity = PredictVelocityGivenFinalPosition(paddleRb, targetPoint + new Vector3(0,0, 0.1f), timeBeforeHitToSpawnPaddle);
               paddleRb.velocity = predictedVelocity;
               canDeduct = false; 
            }
        }
    }
    
    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Ball"))
        {
            // Destroy ball and spawn new one to make receiving control easier
            Destroy(go);
            timeLeft = timeToGetToTarget;
            canDeduct = true;
            Shoot();
        }
    }

    void Shoot()
    {
        receiveMiss = DetermineIfToReceiveMiss();
        hitMiss = DetermineIfToHitMiss();
        targetPoint = GenerateTargetPointAlongNet();
        // Spawn ball
        GameObject instantiatedBall = Instantiate(ballPrefab, shotPoint.position, Quaternion.Euler(Vector3.zero));
        Rigidbody ballRb = instantiatedBall.GetComponent<Rigidbody>();
        
        Vector3 predictedVelocity = PredictVelocityGivenFinalPosition(ballRb, receiveMiss ? GenerateRandomTarget() : targetPoint, timeToGetToTarget);
        ballRb.velocity = predictedVelocity;
        
        if(targetPrefab) Instantiate(targetPrefab, targetPoint, targetPrefab.transform.rotation);
        Instantiate(hitterPrefab, targetPoint + hitterPositionOffset, hitterPrefab.transform.rotation);
    }

    Vector3 GenerateTargetPointAlongNet()
    {
        Vector3 offset = new Vector3(
            UnityEngine.Random.Range(-leewayFromBaseTargetPoint.x, leewayFromBaseTargetPoint.x),
            UnityEngine.Random.Range(-leewayFromBaseTargetPoint.y, leewayFromBaseTargetPoint.y),
            UnityEngine.Random.Range(-leewayFromBaseTargetPoint.z, leewayFromBaseTargetPoint.z));
        return basePassTargetPoint + offset;
    }

    Vector3 PredictVelocityGivenFinalPosition(Rigidbody rb, Vector3 r, float t)
    {
        // v0 = (r - r0 - 1/2at^2) / t              = rearrangement of kinematic equation
        Vector3 r0 = rb.position;
        Vector3 a = new Vector3(0, -9.8f, 0);

        return (r - r0 - (a * Mathf.Pow(t, 2)) / 2) / t;
    }
    
    bool DetermineIfToReceiveMiss()
    {
        // randomise between 0 and 1, and see if miss (between 0 and percentageOfMisses) or not
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        return randomValue <= percentageOfReceiveMisses;
    }
    
    bool DetermineIfToHitMiss()
    {
        // randomise between 0 and 1, and see if miss (between 0 and percentageOfMisses) or not
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        return randomValue <= percentageOfHitMisses;
    }
    
    Vector3 GenerateRandomTarget()
    {
        float x, y, z;
        x = UnityEngine.Random.Range(- maxReceiveErrorRange.x,  maxReceiveErrorRange.x);
        y = UnityEngine.Random.Range(- maxReceiveErrorRange.y,  maxReceiveErrorRange.y);
        z = UnityEngine.Random.Range(- maxReceiveErrorRange.z,  maxReceiveErrorRange.z);
        return new Vector3(x, y, z);
    }

}
