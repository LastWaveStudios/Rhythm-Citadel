

namespace Gameplay
{
    public enum DamageType
    {
        None = 0,
        String = 1,
        Percusion = (1 << 1),
        Hybrid = String | Percusion,
        Wind = (1 << 2),
        TrueDamage = (1 << 3)
    }
}