namespace Rioters {

    public enum NpcWorldState : byte {

        StaminaLevel,
        PoliceInRange,
        HasDestructiblesInRange,
        HasTakenDamage,
        HasDied
    }

    public enum NpcType : byte {

        Rioter,
        Police,
        MediaReporter

    }
}