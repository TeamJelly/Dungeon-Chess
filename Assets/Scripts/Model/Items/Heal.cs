namespace Model.Items
{
    /*        public enum TargetType // ��ų�� ���
        {
            NULL = -1,
            Any,            // ��� Ÿ�Ͽ� ��밡��
            NoUnit,         // ������ ���� ������ ��밡��, �̵� Ȥ�� ��ȯ�� ��ų�� ���
            Friendly,       // ��ȣ���� ���ֿ� ��밡�� (AI ��)
            Hostile,        // �������� ���ֿ� ��밡�� (AI ��)
        }
     */
    class Heal : Item
    {
        public Heal()
        {
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed_705");
            Color = UnityEngine.Color.green;
            Target = TargetType.Any;
        }

        public override void Use(Unit unit)
        {
            Common.Command.Heal(unit,10);
        }
    }
}