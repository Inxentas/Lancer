using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents
{
    // character
    public static CharacterLoadedEvent characterLoaded = new CharacterLoadedEvent();
    public static CharacterKilledEvent characterKilled = new CharacterKilledEvent();
    public static CharacterResurrectEvent characterResurrect = new CharacterResurrectEvent();
    public static CharacterResurrectRequestEvent characterResurrectRequest = new CharacterResurrectRequestEvent();
    // entities
    public static NavMeshEntityLoadedEvent navMeshEntityLoaded = new NavMeshEntityLoadedEvent();
    public static NavMeshEntityHarmEvent navMeshEntityHarm = new NavMeshEntityHarmEvent();
    public static NavMeshEntityKillEvent navMeshEntityKill = new NavMeshEntityKillEvent();
    // user interface
    public static VolumeChangeMasterEvent volumeChangeMaster = new VolumeChangeMasterEvent();
    public static VolumeChangeBGMEvent volumeChangeBGM = new VolumeChangeBGMEvent();
    public static VolumeChangeSFXEvent volumeChangeSFX = new VolumeChangeSFXEvent();
    // save data
    public static SaveDataPropagateEvent saveDataPropagate = new SaveDataPropagateEvent();
    public static SettingsDataPropagateEvent settingsDataPropagate = new SettingsDataPropagateEvent();
    // audio
    public static BackGroundMusicRequestEvent backGroundMusicRequest = new BackGroundMusicRequestEvent();
}
// character
public class CharacterLoadedEvent : UnityEvent<Character> { }
public class CharacterKilledEvent : UnityEvent<Character> { }
public class CharacterResurrectEvent : UnityEvent<Character> { }
public class CharacterResurrectRequestEvent : UnityEvent { }
// entities
public class NavMeshEntityLoadedEvent : UnityEvent<NavMeshEntity> { }
public class NavMeshEntityKillEvent : UnityEvent<NavMeshEntity> { }
public class NavMeshEntityHarmEvent : UnityEvent<NavMeshEntity> { }

// user interface
public class VolumeChangeMasterEvent : UnityEvent<float> { };
public class VolumeChangeBGMEvent : UnityEvent<float> { };
public class VolumeChangeSFXEvent : UnityEvent<float> { };
// save data
public class SaveDataPropagateEvent : UnityEvent<SaveData> { };
public class SettingsDataPropagateEvent : UnityEvent<SettingsData> { };
// audio
public class BackGroundMusicRequestEvent : UnityEvent<AudioClip> { };