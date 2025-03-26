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

    public Dictionary<string, ParticleSystem> explosionParticleDic = new Dictionary<string, ParticleSystem>();
    public Dictionary<string, ParticleSystem> gunFireParticleDic = new Dictionary<string, ParticleSystem>();
    public Dictionary<string, ParticleSystem> gunSmokeParticleDic = new Dictionary<string, ParticleSystem>();
    public Dictionary<string, ParticleSystem> gunHitParticleDic = new Dictionary<string, ParticleSystem>();

    public ParticleSystem explosionParticle;
    public ParticleSystem gunFireParticle;
    public ParticleSystem gunsmokeParticle;
    public ParticleSystem gunHitParticle;

    public int poolSize = 30;

    [System.Serializable]
    public struct NamedParticle
    {
        public string name;
        public ParticleSystem particle;
    }

    public NamedParticle[] explosionParticleList;
    public NamedParticle[] gunFireParticleList;
    public NamedParticle[] gunSmokeParticleList;
    public NamedParticle[] gunHitParticleList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeParticle();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeParticle()
    {
        //폭파 파티클
        foreach (var explosionParticle in explosionParticleList)
        {
            if (!explosionParticleDic.ContainsKey(explosionParticle.name))
            {
                explosionParticleDic.Add(explosionParticle.name, explosionParticle.particle);
            }
        }

        //총구 파티클
        foreach (var gunFireParticle in gunFireParticleList)
        {
            if (!gunFireParticleDic.ContainsKey(gunFireParticle.name))
            {
                gunFireParticleDic.Add(gunFireParticle.name, gunFireParticle.particle);
            }
        }

        //총구 파티클(연기)
        foreach (var gunSmokeParticle in gunSmokeParticleList)
        {
            if (!gunSmokeParticleDic.ContainsKey(gunSmokeParticle.name))
            {
                gunSmokeParticleDic.Add(gunSmokeParticle.name, gunSmokeParticle.particle);
            }
        }

        //피격 파티클
        foreach (var gunHitParticle in gunHitParticleList)
        {
            if (!gunHitParticleDic.ContainsKey(gunHitParticle.name))
            {
                gunHitParticleDic.Add(gunHitParticle.name, gunHitParticle.particle);
            }
        }
    }

    public void PlayGunFireParticle(string name, Vector3 position, Vector3 scale, Quaternion rotation)
    {
        if (gunFireParticleDic.ContainsKey(name))
        {
            ParticleSystem particle = Instantiate(gunFireParticleDic[name], position, rotation);
            particle.gameObject.transform.localScale = scale;
            particle.Play();
            Destroy(particle.gameObject, particle.main.duration);
        }
    }


    public void PlayGunHitParticle(string name, Vector3 position, Vector3 scale, Quaternion rotation)
    {
        if (gunHitParticleDic.ContainsKey(name))
        {
            ParticleSystem particle = Instantiate(gunHitParticleDic[name], position, rotation);
            particle.gameObject.transform.localScale = scale;
            particle.Play();
            Destroy(particle.gameObject, particle.main.duration);
        }
    }

    //파티클 중지
    public void StopParticle()
    {
        
    }



}
