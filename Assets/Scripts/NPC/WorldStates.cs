namespace Rioters {

    public enum NpcWorldState : byte {

        StaminaLevel,
        PoliceInRange,
        HasDestructiblesInRange,
        TargetInAttackRange

    }

    public enum NpcType : byte {

        Rioter,
        Police,
        MediaReporter

    }
}