[System.Serializable]
public struct BallShootingParams
{
    public float accuracy;
    public float flightTimeMultiplier;
    public float arcHeightMultiplier;
    public UnityEngine.Vector3 shooterPosition;
    public UnityEngine.Vector3 targetPosition;

    public BallShootingParams(float accuracy, float flightTimeMultiplier, float arcHeightMultiplier, UnityEngine.Vector3 shooterPosition, UnityEngine.Vector3 targetPosition)
    {
        this.accuracy = accuracy;
        this.flightTimeMultiplier = flightTimeMultiplier;
        this.arcHeightMultiplier = arcHeightMultiplier;
        this.shooterPosition = shooterPosition;
        this.targetPosition = targetPosition;
    }
}
