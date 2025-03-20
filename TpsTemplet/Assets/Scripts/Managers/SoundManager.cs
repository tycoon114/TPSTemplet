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


    private Dictionary<string, AudioClip> bgmClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    //����ü�� ���� �ӽ÷� ��ųʸ��� ���� �� �� �ֵ��� ����
    [System.Serializable]
    public struct NamedAudioClip
    {
        public string name;
        public AudioClip clip;
    }

    public NamedAudioClip[] bgmClipList;
    public NamedAudioClip[] sfxClipList;

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

    //�Ÿ��� ȿ���� �Ҹ� ���� ����.... �ӽ� �������� �ۼ���
    public void PlaySfx(string name, Vector3 position)
    {
        if (sfxClips.ContainsKey(name))
        {
            AudioSource.PlayClipAtPoint(sfxClips[name], position);
        }
    }

    public void PlayButtonSfx(string name)
    {
        sfxSource.PlayOneShot(sfxClips[name]);
    }

    //bgm ����
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    //���� ����
    public void SetBGMVolueme(float volume)
    {
        bgmSource.volume = Mathf.Clamp(volume, 0, 1);
    }

    //sfx ����
    public void StopSfx()
    {
        sfxSource.Stop();
    }

    //���� ����
    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp(volume, 0, 1);
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
