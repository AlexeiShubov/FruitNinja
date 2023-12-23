public class HP
{
    public int CurrentHP;

    public HP(int startHPCount)
    {
        CurrentHP = startHPCount;
    }

    public void CorrectCountHP(int count)
    {
        CurrentHP += count;

        if (CurrentHP > 0) return;

        GameEvents.OnLossGameEvent.Invoke(true);
    }
}
