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

    //일단 오디오 소스 컴포넌트는 sfx와 같이 사용 - 필요하면 따로 만들고 거기서 값을 수정할 것
    public AudioSource walkSource;
    public AudioSource gunSource;
    public AudioSource skillSource;

    private Dictionary<string, AudioClip> bgmClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    private Dictionary<string, AudioClip> walkClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> gunClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> skillClips = new Dictionary<string, AudioClip>();

    //구조체를 만들어서 임시로 딕셔너리를 관리 할 수 있도록 설정
    [System.Serializable]
    public struct NamedAudioClip
    {
        public string name;
        public AudioClip clip;
    }

    public NamedAudioClip[] bgmClipList;
    public NamedAudioClip[] sfxClipList;    //환경음이나 효과음은 여기에

    //효과음이 종류가 많아서 따로 관리하기로함 2025-03-21
    public NamedAudioClip[] walkClipList;   //걷는 소리는 여기에
    public NamedAudioClip[] gunClipList;    //총소리는 여기에, 폭발도 여기에
    public NamedAudioClip[] skilClipList;    //스킬 관련 효과음은 여기에


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

    //사운드 할당하기
    void InitializeAudioClips()
    {
        //bgm 할당
        foreach (var bgm in bgmClipList)
        {
            if (!bgmClips.ContainsKey(bgm.name))
            {
                bgmClips.Add(bgm.name, bgm.clip);
            }
        }
        //효과음 할당
        foreach (var sfx in sfxClipList)
        {
            if (!sfxClips.ContainsKey(sfx.name))
            {
                sfxClips.Add(sfx.name, sfx.clip);
            }
        }

        //걷는 소리 할당
        foreach (var walk in walkClipList)
        {
            if (!walkClips.ContainsKey(walk.name))
            {
                walkClips.Add(walk.name, walk.clip);
            }
        }
        //총소리 할당
        foreach (var gun in gunClipList)
        {
            if (!gunClips.ContainsKey(gun.name))
            {
                gunClips.Add(gun.name, gun.clip);
            }
        }
        //스킬소리 할당
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
            //코루틴 정지
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



    //효과음 재생
    public void PlaySfx(string name)
    {
        sfxSource.PlayOneShot(sfxClips[name]);
    }

    // 걷는 소리
    public void PlayWalkSfx(string name)
    {
        //if (walkClips.ContainsKey(name))
        //{
        //    AudioSource.PlayClipAtPoint(walkClips[name], position);
        //}
        sfxSource.PlayOneShot(walkClips[name]);
    }

    //총 소리
    public void PlayGunSfx(string name, Vector3 position)
    {
        if (gunClips.ContainsKey(name))
        {
            AudioSource.PlayClipAtPoint(gunClips[name], position);
        }
    }

    // 스킬 소리
    public void PlaySkillSfx(string name, Vector3 position)
    {
        if (skillClips.ContainsKey(name))
        {
            AudioSource.PlayClipAtPoint(skillClips[name], position);
        }
    }

    //볼륨 조절
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp(volume, 0, 1);
        Debug.Log("사운드 매니저 BGM :  " + volume);
    }

    //볼륨 조절
    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp(volume, 0, 1);
        gunSource.volume = Mathf.Clamp(volume, 0, 1);
        walkSource.volume = Mathf.Clamp(volume, 0, 1);
        skillSource.volume = Mathf.Clamp(volume, 0, 1);
        Debug.Log("사운드 매니저 sfx :  " + volume);
    }

    //bgm 중지
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    //sfx 중지
    public void StopSfx()
    {
        sfxSource.Stop();
    }



    //발소리 중지
    public void StopWalkSfx()
    {
        walkSource.Stop();
    }

    //사격소리 중지
    public void StopGunSfx()
    {
        gunSource.Stop();
    }

    //스킬 소리 중지
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
