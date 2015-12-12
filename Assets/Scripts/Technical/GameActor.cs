/// <summary>
/// Game Actor that can be hit.
/// </summary>
public interface GameActor
{
    /// <summary>
    /// Hit this actor and do damage to it.
    /// </summary>
    /// <param name="damage"></param>
    void Hit (int damage);
}
