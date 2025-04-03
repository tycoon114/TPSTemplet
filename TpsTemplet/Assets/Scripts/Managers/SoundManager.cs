using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    {
        get;
        private set;
    }

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    //�ϴ� ����� �ҽ� ������Ʈ�� sfx�� ���� ��� - �ʿ��ϸ� ���� ����� �ű⼭ ���� ������ ��
    public AudioSource walkSource;
    public AudioSource gunSource;
    public AudioSource skillSource;

    private Dictionary<string, AudioClip> bgmClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    private Dictionary<string, AudioClip> walkClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> gunClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> skillClips = new Dictionary<string, AudioClip>();

    //����ü�� ���� �ӽ÷� ��ųʸ��� ���� �� �� �ֵ��� ����
    [System.Serializable]
    public struct NamedAudioClip
    {
        public string name;
        public AudioClip clip;
    }

    public NamedAudioClip[] bgmClipList;
    public NamedAudioClip[] sfxClipList;    //ȯ�����̳� ȿ������ ���⿡

    //ȿ������ ������ ���Ƽ� ���� �����ϱ���� 2025-03-21
    public NamedAudioClip[] walkClipList;   //�ȴ� �Ҹ��� ���⿡
    public NamedAudioClip[] gunClipList;    //�ѼҸ��� ���⿡, ���ߵ� ���⿡
    public NamedAudioClip[] skilClipList;    //��ų ���� ȿ������ ���⿡


    private Coroutine currentBGMCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //���� �Ҵ��ϱ�
    void InitializeAudioClips()
    {
        //bgm �Ҵ�
        foreach (var bgm in bgmClipList)
        {
            if (!bgmClips.ContainsKey(bgm.name))
            {
                bgmClips.Add(bgm.name, bgm.clip);
            }
        }
        //ȿ���� �Ҵ�
        foreach (var sfx in sfxClipList)
        {
            if (!sfxClips.ContainsKey(sfx.name))
            {
                sfxClips.Add(sfx.name, sfx.clip);
            }
        }

        //�ȴ� �Ҹ� �Ҵ�
        foreach (var walk in walkClipList)
        {
            if (!walkClips.ContainsKey(walk.name))
            {
                walkClips.Add(walk.name, walk.clip);
            }
        }
        //�ѼҸ� �Ҵ�
        foreach (var gun in gunClipList)
        {
            if (!gunClips.ContainsKey(gun.name))
            {
                gunClips.Add(gun.name, gun.clip);
            }
        }
        //��ų�Ҹ� �Ҵ�
        foreach (var skill in skilClipList)
        {
            if (!skillClips.ContainsKey(skill.name))
            {
                skillClips.Add(skill.name, skill.clip);
            }
        }
    }

    public void PlayBGM(string name, float fadeDuration = 1.0f)
    {
        if (bgmClips.ContainsKey(name))
        {
            //�ڷ�ƾ ����
            if (currentBGMCoroutine != null)
            {
                StopCoroutine(currentBGMCoroutine);
            }
            currentBGMCoroutine = StartCoroutine(FadeOutBGM(fadeDuration, () =>
            {
                bgmSource.spatialBlend = 0f;
                bgmSource.clip = bgmClips[name];
                bgmSource.Play();
                currentBGMCoroutine = StartCoroutine(FadeInBGM(fadeDuration));
            }));
        }
    }



    //ȿ���� ���
    public void PlaySfx(string name)
    {
        sfxSource.PlayOneShot(sfxClips[name]);
    }

    // �ȴ� �Ҹ�
    public void PlayWalkSfx(string name)
    {
        //if (walkClips.ContainsKey(name))
        //{
        //    AudioSource.PlayClipAtPoint(walkClips[name], position);
        //}
        sfxSource.PlayOneShot(walkClips[name]);
    }

    //�� �Ҹ�
    public void PlayGunSfx(string name, Vector3 position)
    {
        if (gunClips.ContainsKey(name))
        {
            AudioSource.PlayClipAtPoint(gunClips[name], position);
        }
    }

    // ��ų �Ҹ�
    public void PlaySkillSfx(string name, Vector3 position)
    {
        if (skillClips.ContainsKey(name))
        {
            AudioSource.PlayClipAtPoint(skillClips[name], position);
        }
    }

    //���� ����
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp(volume, 0, 1);
        Debug.Log("���� �Ŵ��� BGM :  " + volume);
    }

    //���� ����
    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp(volume, 0, 1);
        gunSource.volume = Mathf.Clamp(volume, 0, 1);
        walkSource.volume = Mathf.Clamp(volume, 0, 1);
        skillSource.volume = Mathf.Clamp(volume, 0, 1);
        Debug.Log("���� �Ŵ��� sfx :  " + volume);
    }

    //bgm ����
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    //sfx ����
    public void StopSfx()
    {
        sfxSource.Stop();
    }



    //�߼Ҹ� ����
    public void StopWalkSfx()
    {
        walkSource.Stop();
    }

    //��ݼҸ� ����
    public void StopGunSfx()
    {
        gunSource.Stop();
    }

    //��ų �Ҹ� ����
    public void StopSkillSfx()
    {
        skillSource.Stop();
    }


    private IEnumerator FadeOutBGM(float duration, Action onFadeComplete)
    {
        float startVloume = bgmSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVloume, 0f, t / duration);
            yield return null;
        }
        bgmSource.volume = 0;
        onFadeComplete?.Invoke();
    }

    private IEnumerator FadeInBGM(float duration)
    {
        float startVloume = 0f;
        bgmSource.volume = 0f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVloume, 1f, t / duration);
            yield return null;
        }
        bgmSource.volume = 1.0f;

    }

}
