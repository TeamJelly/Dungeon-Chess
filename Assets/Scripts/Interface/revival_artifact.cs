class revival_artifact : IOnDie
{
    public void OnDie(Unit user, ref bool isDead)
    {
        if (isDead == true)
        {
            user.CurHP = 1;
            isDead = false;
            user.Artifacts.Remove("revival_artifact");
        }
    }
}