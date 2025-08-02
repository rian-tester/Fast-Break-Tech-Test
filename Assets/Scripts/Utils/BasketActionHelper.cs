using UnityEngine;
public static class BasketActionHelper
{
    public static Vector2 CalculateShot(
        Vector3 shooterPosition,
        Vector3 targetPosition,
        float accuracy,           // 0-100 range
        float baseShootPower = 1000f,
        float gravityScale = 9.81f,
        float ballRadius = 0.375f,  // Ball radius (0.75 scale * 0.5 sphere radius)
        float ballMass = 1f         // Ball mass for realistic physics
    )
    {
        float calculatedPower = 0f;
        float calculatedArc = 0f;
        Vector2 calculatedVar = new Vector2();

        Vector3 displacement = targetPosition - shooterPosition;
        
        Vector3 adjustedTarget = targetPosition + Vector3.up * ballRadius;
        displacement = adjustedTarget - shooterPosition;
        
        float horizontalDistance = new Vector3(displacement.x, 0, displacement.z).magnitude;
        float verticalDistance = displacement.y;
        
        Debug.Log($"=== BASKET HELPER DEBUG ===");
        Debug.Log($"Original Target: {targetPosition}");
        Debug.Log($"Adjusted Target: {adjustedTarget}");
        Debug.Log($"Horizontal Distance: {horizontalDistance}");
        Debug.Log($"Vertical Distance: {verticalDistance}");
        Debug.Log($"Ball Radius: {ballRadius}, Ball Mass: {ballMass}");
        
        if (horizontalDistance < 0.1f)
        {
            Debug.LogWarning("Distance too small, using fallback values");
            calculatedVar.x = baseShootPower * 0.3f;
            calculatedVar.y = baseShootPower * 0.5f;
            return calculatedVar;
        }
        
        float gravity = gravityScale;
        float optimalAngle = 50f * Mathf.Deg2Rad;
        
        float velocitySquared = (gravity * horizontalDistance * horizontalDistance) / 
                               (horizontalDistance * Mathf.Sin(2 * optimalAngle) - 
                                2 * verticalDistance * Mathf.Cos(optimalAngle) * Mathf.Cos(optimalAngle));
        
        Debug.Log($"Velocity Squared: {velocitySquared}");
        
        if (velocitySquared <= 0)
        {
            Debug.LogWarning("Invalid velocity calculation, using fallback");
            calculatedVar.x = baseShootPower * 0.5f;
            calculatedVar.y = baseShootPower * 0.6f;
            return calculatedVar;
        }
        
        float velocity = Mathf.Sqrt(Mathf.Abs(velocitySquared));
        
        float massMultiplier = Mathf.Sqrt(ballMass);
        calculatedPower = velocity * baseShootPower / (1000f * massMultiplier);
        calculatedArc = velocity * Mathf.Sin(optimalAngle) / massMultiplier;
        
        Debug.Log($"Velocity: {velocity}, Mass Multiplier: {massMultiplier}");
        Debug.Log($"Raw Power: {calculatedPower}, Raw Arc: {calculatedArc}");
        
        float accuracyDeviation = 1f;
        if (accuracy >= 99.9f)
        {
            accuracyDeviation = 1f;
        }
        else if (accuracy >= 95f)
        {
            accuracyDeviation = Random.Range(0.99f, 1.01f);
        }
        else if (accuracy >= 70f)
        {
            float deviation = Mathf.Lerp(0.15f, 0.01f, (accuracy - 70f) / 25f);
            accuracyDeviation = Random.Range(1f - deviation, 1f + deviation);
        }
        else
        {
            float deviation = Mathf.Lerp(0.4f, 0.15f, accuracy / 70f);
            accuracyDeviation = Random.Range(1f - deviation, 1f + deviation);
        }
        
        calculatedPower *= accuracyDeviation;
        calculatedArc *= accuracyDeviation;

        Debug.Log($"Accuracy: {accuracy}, Deviation: {accuracyDeviation}");
        Debug.Log($"Final Power: {calculatedPower}, Final Arc: {calculatedArc}");

        calculatedVar.x = calculatedPower;
        calculatedVar.y = calculatedArc;

        return calculatedVar;
    }
}
   
