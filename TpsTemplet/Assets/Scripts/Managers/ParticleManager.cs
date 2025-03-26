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
    public Dictionary<string, ParticleSystem> gunHitWallParticleDic = new Dictionary<string, ParticleSystem>();

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

    public NamedParticle[] explosionParticleList;
    public NamedParticle[] gunFireParticleList;
    public NamedParticle[] gunSmokeParticleList;
    public NamedParticle[] gunHitWallParticleList;

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
        //��ƼŬ �߰�
        AddParticle();
    }

    void InitializeParticle()
    {
        //���� ��ƼŬ
        foreach (var explosionParticle in explosionParticleList)
        {
            if (!explosionParticleDic.ContainsKey(explosionParticle.name))
            {
                explosionParticleDic.Add(explosionParticle.name, explosionParticle.particle);
            }
        }

        //�ѱ� ��ƼŬ
        foreach (var gunFireParticle in gunFireParticleList)
        {
            if (!gunFireParticleDic.ContainsKey(gunFireParticle.name))
            {
                gunFireParticleDic.Add(gunFireParticle.name, gunFireParticle.particle);
            }
        }

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
        ParticleSystem particle = Instantiate(particleSystemDic[type], position, rotation);
        particle.gameObject.transform.localScale = scale;
        particle.Play();
        Destroy(particle.gameObject, particle.main.duration);
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


    //��ƼŬ ����
    public void StopParticle()
    {
        
    }



}
