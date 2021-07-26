using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoSingleton<Sound>
{
    public AudioSource m_Bg;
    public AudioSource m_effect;

    public AudioClip bgclip;
    public AudioClip effclip;
    public string ResourceDir = "";

    public float soundValue = 1;
    public float bgValue = 1;
    public float effectValue = 1;

    protected override void Awake()
    {
        base.Awake();
        m_Bg = gameObject.AddComponent<AudioSource>();
        m_Bg.playOnAwake = false;
        m_Bg.loop = true;

        m_effect = gameObject.AddComponent<AudioSource>();
        m_Bg.clip = bgclip;
        m_effect.clip = effclip;
    }

    //播放背景音乐
    public void PlayBG(string audioName, float value = 1)
    {
        value = m_Bg.volume = soundValue * bgValue;
        string oldName = "";
        if (m_Bg.clip)
        {
            oldName = m_Bg.clip.name;
        }
        if (oldName != audioName)
        {
            //加载资源
            string path = ResourceDir + "/" + audioName;
            bgclip = Resources.Load<AudioClip>(path);

            //播放
            if (bgclip)
            {
                m_Bg.volume = value;
                m_Bg.clip = bgclip;
                m_Bg.Play();
            }
        }
        else
        {
            //加载资源
            string path = ResourceDir + "/" + oldName;
            bgclip = Resources.Load<AudioClip>(path);

            //播放
            if (bgclip)
            {
                m_Bg.volume = value;
                m_Bg.clip = bgclip;
                m_Bg.Play();
            }
        }
    }

    //播放音效
    public void PlayEffect(string audioName, float value = 1)
    {
        value = soundValue * effectValue;
        //加载资源
        string path = ResourceDir + "/" + audioName;
        effclip = Resources.Load<AudioClip>(path);

        //播放
        if (effclip)
        {
            m_effect.volume = value;
            m_effect.PlayOneShot(effclip);
        }
    }

    //停止播放音效
    public void StopPlayEffect()
    {
        m_effect.Stop();
    }

    //停止所有音乐
    public void StopAllSound()
    {
        if (m_Bg)
        {
            m_Bg.Stop();
        }
        if (m_effect)
        {
            m_effect.Stop();
        }
    }

    public void ChangeSoundValue(float sValue)
    {
        soundValue = sValue;
        m_Bg.volume = soundValue * bgValue;
        m_effect.volume = soundValue * effectValue;
    }

    public void ChangeEffectValue(float eValue)
    {
        effectValue = eValue;
        m_effect.volume = soundValue * effectValue;
    }

    public void ChangeBgValue(float bValue)
    {
        bgValue = bValue;
        m_Bg.volume = soundValue * bgValue;
        m_Bg.clip = m_Bg.clip;
    }
}