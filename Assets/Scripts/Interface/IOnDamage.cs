interface IOnDamage
{
    public void OnBeforeDamage(Unit user, ref int damage);
    public void OnAfterDamage(Unit user, ref int damage);
}