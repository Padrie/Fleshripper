using FMOD.Studio;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public Player player;

    public EventReference calmMusicRef;
    public EventReference combatMusicRef;
    
    private EventInstance calmMusic;
    private EventInstance combatMusic;

    private float calmVolume = 0;
    private float combatVolume = 0;

    public float musicBlendTime = 1;
    
    void Start()
    {
        
        calmMusic = RuntimeManager.CreateInstance(calmMusicRef);
        combatMusic = RuntimeManager.CreateInstance(combatMusicRef);
        
        calmMusic.start();
        combatMusic.setPaused(true);
        combatMusic.start();

        calmMusic.setVolume(0);
        combatMusic.setVolume(0);
    }
    
    void Update()
    {
        
        TransitionMusic();

        UpdateMusicVolume();
        
        CheckMuted();
        
    }

    public void TransitionMusic()
    {
        if (player.inCombat || player.inCombatArea)
        {
            calmVolume = math.lerp(calmVolume, 0, Time.deltaTime * musicBlendTime);
            combatVolume = math.lerp(combatVolume, 1, Time.deltaTime * musicBlendTime);
        } else if (!player.inCombat && !player.inCombatArea)
        {
            calmVolume = math.lerp(calmVolume, 1, Time.deltaTime * musicBlendTime);
            combatVolume = math.lerp(combatVolume, 0, Time.deltaTime * musicBlendTime);
        }
    }

    public void UpdateMusicVolume()
    {
        calmMusic.setVolume(calmVolume);
        combatMusic.setVolume(combatVolume);
    }

    public void CheckMuted()
    {
        if (calmVolume == 0)
        {
            calmMusic.setPaused(true);
        }
        else
        {
            calmMusic.setPaused(false);
        }
        
        if (combatVolume == 0)
        {
            combatMusic.setPaused(true);
        }
        else
        {
            combatMusic.setPaused(false);
        }
    }
    
}
