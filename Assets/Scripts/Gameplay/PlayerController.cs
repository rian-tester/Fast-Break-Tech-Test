using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : HeroBase
{
    [SerializeField]
    private float passingPower;
    [SerializeField]
    float playerAccuracy = 100f;
    [SerializeField]
    private float flightTimeMultiplier = 0.2f;
    [SerializeField]
    private float arcHeightMultiplier = 0.5f; 

    protected override void Update()
    {
        base.Update();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    public void OnPass(InputAction.CallbackContext context)
    {
        if (context.performed && characterState == CharacterState.Dribbling) 
            SetState(CharacterState.Passing);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && characterState == CharacterState.Dribbling) 
            SetState(CharacterState.Shooting);
    }

    protected override void OnStateChanged(CharacterState newState, CharacterState oldState)
    {
        switch (newState)
        {
            case CharacterState.EmptyHanded:
                if (controlledBall != null)
                {
                    controlledBall = null;
                }
                break;

            case CharacterState.Passing:
                if (controlledBall == null) return;
                controlledBall.SetState(BallState.Free);
                controlledBall.transform.SetParent(null);
                controlledBall.BallRigidbody.WakeUp();
                controlledBall.BallRigidbody.AddForce((transform.forward * passingPower) + (transform.up * passingPower/4), ForceMode.Acceleration); 
                controlledBall = null;
                SetState(CharacterState.EmptyHanded);

                break;

            case CharacterState.Shooting:
                if (controlledBall == null) return;
                
                GameManager gameManager = ServiceLocator.Get<GameManager>();
                if (gameManager != null)
                {
                    BasketballRing targetRing = gameManager.GetTargetRing(playerTeam);
                    if (targetRing != null)
                    {
                        ShootBall(targetRing);
                        Debug.Log($"{GetCharacterName()} shoots toward {targetRing.DefendingTeam} ring!");
                    }
                }
                break;

            default:
                break;
        }
    }
    public void TestReleaseBall()
    {
        if (controlledBall != null)
        {
            controlledBall.SetState(BallState.Free);
            controlledBall.transform.SetParent(null);
            controlledBall.BallRigidbody.WakeUp();
            controlledBall.BallRigidbody.AddForce(transform.forward * passingPower/10);
            controlledBall = null;
            SetState(CharacterState.EmptyHanded);
        }
    }

    private void ShootBall(BasketballRing targetRing)
    {
        BallController ballToShoot = controlledBall;
        ballToShoot.SetState(BallState.Free);
        ballToShoot.transform.SetParent(null);
        
        Vector3 startPosition = ballToShoot.transform.position;
        Vector3 ringTopPosition = targetRing.ShootingTarget.position + Vector3.up * 0.6f;
        Vector3 ringCenterPosition = targetRing.ShootingTarget.position;
        
        Vector3 finalTarget = ApplyAccuracyVariance(ringTopPosition, playerAccuracy);
        
        Debug.Log($"=== TRANSFORM SHOOTING DEBUG ===");
        Debug.Log($"Start Position: {startPosition}");
        Debug.Log($"Ring Top Target: {ringTopPosition}");
        Debug.Log($"Final Target (with accuracy): {finalTarget}");
        Debug.Log($"Player Accuracy: {playerAccuracy}");
        
        StartCoroutine(ShootBallCoroutine(ballToShoot, startPosition, finalTarget, ringCenterPosition));
        
        controlledBall = null;
        SetState(CharacterState.EmptyHanded);
    }
    
    private Vector3 ApplyAccuracyVariance(Vector3 perfectTarget, float accuracy)
    {
        if (accuracy >= 100f) return perfectTarget;
        
        float maxDeviation = Mathf.Lerp(2f, 0.01f, accuracy / 100f);
        Vector3 randomOffset = Random.insideUnitCircle * maxDeviation;
        return perfectTarget + new Vector3(randomOffset.x, 0, randomOffset.y);
    }
    
    private System.Collections.IEnumerator ShootBallCoroutine(BallController ball, Vector3 startPos, Vector3 targetPos, Vector3 ringCenter)
    {
        if (ball == null) yield break;
        
        ball.BallRigidbody.isKinematic = true;
        ball.BallRigidbody.useGravity = false;
        
        float distance = Vector3.Distance(startPos, targetPos);
        float flightTime = distance * flightTimeMultiplier;
        float arcHeight = (distance * arcHeightMultiplier) + (3f / Mathf.Max(distance, 1f));
        
        Debug.Log($"Flight Time: {flightTime}, Arc Height: {arcHeight}, Distance: {distance}");
        
        float elapsedTime = 0f;
        
        while (elapsedTime < flightTime)
        {
            float t = elapsedTime / flightTime;
            
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
            float heightOffset = Mathf.Sin(t * Mathf.PI) * arcHeight;
            currentPos.y += heightOffset;
            
            ball.transform.position = currentPos;
            ball.transform.Rotate(Vector3.right * 360f * Time.deltaTime);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        ball.transform.position = targetPos;
        
        bool hitRing = Vector3.Distance(targetPos, ringCenter + Vector3.up * 0.6f) < 0.5f;
        
        if (hitRing)
        {
            yield return new WaitForSeconds(0.1f);
            
            float dropTime = 0.3f;
            Vector3 dropStart = ball.transform.position;
            elapsedTime = 0f;
            
            while (elapsedTime < dropTime)
            {
                float t = elapsedTime / dropTime;
                ball.transform.position = Vector3.Lerp(dropStart, ringCenter, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            ball.transform.position = ringCenter;
        }
        
        ball.BallRigidbody.isKinematic = false;
        ball.BallRigidbody.useGravity = true;
        ball.SetState(BallState.Free);
    }
}