using UnityEngine;

public class AttackTypes : MonoBehaviour
{
    public GameObject ice, fire, nature;

    public GameObject WaterAttack()
    {
        return ice;
    }
    public GameObject FireAttack()
    {
        return fire;
    }
    public GameObject WindAttack()
    {
        return nature;
    }
    public GameObject NotMentioned()
    {
        return null;
    }
}
