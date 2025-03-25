using System.Collections.Generic;
using UnityEngine;
using static SoundManager;


public enum ParticleType
{
    Explosion,
    GunFire,
    GunSmoke,
    GunHitWall,
}

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance
    {
        get;
        private set;
    }

    private Dictionary<ParticleType, ParticleSystem> particleSystemDic = new Dictionary<ParticleType, ParticleSystem>();

    public Dictionary<string, ParticleSystem> gunShootParticleDIc = new Dictionary<string, ParticleSystem>();


    public ParticleSystem explosionParticle;
    public ParticleSystem gunFireParticle;
    public ParticleSystem gunsmokeParticle;
    public ParticleSystem gunHitWallParticle;

    public int poolSize = 30;

    [System.Serializable]
    public struct NamedParticle
    {
        public string name;
        public ParticleSystem particle;
    }
    

    public NamedParticle[] gunShootParticleList;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //파티클 추가
        AddParticle();

    }

    protected void AddParticle()
    {
        particleSystemDic.Add(ParticleType.Explosion, explosionParticle);
        particleSystemDic.Add(ParticleType.GunFire, gunFireParticle);
        particleSystemDic.Add(ParticleType.GunSmoke, gunsmokeParticle);
        particleSystemDic.Add(ParticleType.GunHitWall, gunHitWallParticle);
    }

    public void PariclePlay(ParticleType type, Vector3 position, Vector3 scale, Quaternion rotation)
    {
        ParticleSystem particle = Instantiate(particleSystemDic[type],position, rotation);
        particle.gameObject.transform.localScale = scale;
        particle.Play();
        Destroy(particle.gameObject, particle.main.duration);
    }

}
