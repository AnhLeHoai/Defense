using System;

public enum TypeTabShopCharacter
{
    HAT = 0,
    SKIN = 1,
    PET = 2,
    SKILL = 3
}

public enum GameState
{
    LOBBY,
    REVIVE,
    IN_GAME,
    END_GAME,
}

public enum LevelType
{
    Bonus,
    Tutorial,
    NormalLevel,
}

public enum GameMode
{
    NONE,
    HIDE,
    SEEK
}

public enum IdPack
{
    NONE = -1,
    ONE = 0,
    HEAP = 1,
    PACK = 2,
    CLOTH_SACK = 3,
    IRON_CASE = 4,
    GOLDEN = 5,
    NO_ADS_PREMIUM = 6,
    NO_ADS_BASIC = 7
}

public enum Layer
{
    Default = 0,
    Player = 9,
    BrokenPieces = 10,
    BreakableObject = 11,
    FOV = 12
}

public enum CharacterAction
{
    Attack,
    Tremble,
    Victory,
    Run,
    Idle,
    Die,
    Action,
    Skill,
    Taunt,
    TauntOption,
    Jumping,
    StartJump,
    IsTauntAfterAttack,
    VictoryAction,
    IsLoopVictory,
    RandomValue,
    Jump
}

public enum LevelResult
{
    NotDecided,
    Win,
    Lose
}

public enum CollectibleType
{
    Gold,
    Speed,
    Key,
    Invisible,
    ReduceDetectRange,
    Stealth,
    ScaleUp,
}

[System.Flags]
public enum CollectibleStackType
{
    None = 0 << 0,
    Amount = 1 << 0,
    EffectiveTime = 2 << 0
}

public enum TypeHat
{
    EGG = 0,
    KUNG_FU = 1,
    DESTAP = 2,
    CORONA = 3,
    FLOWER = 4,
    BOX_HAT = 5,
    PULPO = 6,
    HORN = 7,
    AUDIFON = 8,
    Amoung_Us_Baby_2 = 9,
    Baloon_Hat = 10,
    Basic_Hat = 11,
    Gorrito_Hat = 12,
    Horn = 13,
    Kung_Fu_Hat = 14,
    Leaves_Hat = 15,
    Party_Hat = 16,
    Toilete_Paper_Hat = 17

}

public enum TypePet
{
    PET_1 = 0,
    PET_2 = 1,
    PET_3 = 2,
    PET_4 = 3,
    PET_5 = 4,
    PET_6 = 5,
}


[Flags]
public enum TypeUnlockSkin
{
    NONE = 0,
    SPIN = 1 << 1,
    COIN = 1 << 2,
    VIDEO = 1 << 3
}

public enum TypeEquipment
{
    HAT,
    SKIN,
    PET,
    SKILL
}

public enum TypeDialogReward
{
    LUCKY_WHEEL,
    END_GAME
}

public enum Skin
{
    SKIN_ROB = 0,
    SKIN_NOOB = 1,
    SKIN_NINJA = 2,
    SKIN_SPIDA = 3,
    SKIN_BADMON = 4,
    SKIN_CAPTAIN = 5,
    SKIN_IRON = 6,
    SKIN_DEAD_PULL = 7,
    SKIN_GOKU = 8,
    SKIN_JASON = 9,
    SKIN_SLENDERMAN = 10,
    SKIN_VENOM = 11,
    SKIN_MARIO = 12,
    SKIN_MABU = 13,
    SKIN_BIG6 = 14,
}

public enum TypeGift
{
    GOLD,
    MINI_CEW,
    X5_GOLD,
    REMOVE_ADS,
    HAT,
    SKIN,
    PET
}

public enum TypeItem
{
    Coin = 0,
    Key = 1

}

public enum TypeSoundIngame
{
    NONE = 0,
    PICK_COIN = 1,
    COLLECT_SPEED = 2
}

public enum Role
{
    CatMan,
    Sword,
    Assassin,
    Gunner,
    Hunter,
    Captain,
    Trapper
}

public enum Skill
{
    JUMP_AND_EAT = 0,
    KATANA = 1,
    KNIFE = 2,
    GUN = 3,
    BAZOOKA = 4,
    HAMMER = 5,
    TRAP = 6
}

public enum AttackType
{
    SHOULD_NOT_ATTACK,
    ATTACK_CHARACTER,
    BREAK_OBJECT
}

//public enum WeaponType
//{
//    None,
//    BearClaw,
//    CardClaw,
//    Claw1,
//    Claw2,
//    Claw3,
//    Claw4,
//    Claw5,
//    Claw6,
//    Claw7,
//    Claw8,
//    Claw9,
//    DemonClaw,
//    KarambitClaw,
//    MonsterClaw,
//    AlienGun,
//    Barrel,
//    Batana,
//    BatanaBomb,
//    Bazooka1,
//    Bazooka2,
//    Bazooka3,
//    Blade,
//    Blade1,
//    Blade2,
//    Bomb,
//    Bomb1,
//    Bomb2,
//    BuaDa,
//    CakeBomb,
//    Carrot,
//    CloudSword,
//    CowboyGun,
//    CupcakeBomb,
//    Dao4,
//    Dao5,
//    Destapador,
//    Glock,
//    Grenade,
//    Grenade1,
//    Grenade2,
//    GrenadeLauncher,
//    Gun,
//    Gun1,
//    Gun2,
//    Gun3,
//    Gun4,
//    Gun5,
//    Gun6,
//    Gun7,
//    GunLaze,
//    HaloRocket,
//    Hammer,
//    HammerIron,
//    HotDog,
//    Karambit9,
//    KiemDai1,
//    KiemDai2,
//    KiemDai3,
//    KiemDai4,
//    KiemDai5,
//    KiemDai6,
//    KiemDai7,
//    KiemDai8,
//    KiemNgan1,
//    KiemNgan2,
//    Knife1,
//    Knife2,
//    Knife3,
//    Knife4,
//    Knife5,
//    Knife6,
//    Knife7,
//    KnifeShark,
//    Kovani,
//    Launcher,
//    Launcher1,
//    Launcher2,
//    Launcher3,
//    Lolipop,
//    Mine,
//    Mjolnir,
//    NerfGun,
//    OldPhone,
//    Pan,
//    PaperKnives,
//    PirahnaCannon,
//    Pistola,
//    Pizza,
//    PlanBomb,
//    Rack,
//    Rocket,
//    ShovelIron,
//    StarBomb,
//    SteampunkGun,
//    Stick,
//    Sword1,
//    Tnt,
//    WarHammer,
//    WoodenSword,
//    Thuoc    
//}

public enum DieType
{
   BurstGun,
   Bomb,
   FlameGun,
   Cannon,
   Laser
}