using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseManager<MusicMgr>
{
    private AudioSource bkMusic = null;
    private GameObject soundObj = null;
    private List<AudioSource> soundList = new List<AudioSource>();

    private float bkValue = 1;

    private float soundValue = 1;

    public MusicMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(Update);
    }

    /// <summary>
    /// ÿ֡��������Ƿ񲥷����
    /// </summary>
    private void Update()
    {
        for(int i = soundList.Count - 1; i >= 0; i--)
        {
            if (soundList[i].isPlaying)
            {

                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
                
            }
        }
    }

    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="name"></param>
    public void PlayBkMusic(string name)
    {
        if(bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BkMusic";
            bkMusic = obj.AddComponent<AudioSource>();
        }

        //�첽���ر������֣�������ɲ���
        ResMgr.GetInstance().LoadAsync<AudioClip>("Music/BK" + name, (clip) =>
        {
            bkMusic.clip = clip;
            bkMusic.volume = bkValue;
            bkMusic.loop = true;
            bkMusic.Play();
        });
    }

    /// <summary>
    /// �ı䱳�����ִ�С
    /// </summary>
    /// <param name="v"></param>
    public void ChangeBKValue(float v)
    {
        bkValue = v;
        if(bkMusic == null)
        {
            return;
        }
        bkMusic.volume = bkValue;

    }

    /// <summary>
    /// ��ͣ��������
    /// </summary>
    public void PauseBKMusic()
    {
        if (bkMusic == null)
        {
            return;
        }
        bkMusic.Pause();
    }

    /// <summary>
    /// ֹͣ���ű�������
    /// </summary>
    public void StopSound()
    {
        if (bkMusic == null)
        {
            return;
        }
        bkMusic.Stop();
    }

    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callBack"></param>
    public void PlaySound(string name,bool isLoop = false, GameObject gameObject = null,UnityAction<AudioSource> callBack = null)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }


       
        
        //�첽���ر������֣�������ɲ���
        ResMgr.GetInstance().LoadAsync<AudioClip>("Music/Sound/" + name, (clip) =>
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = soundValue;
            source.loop = isLoop;
            source.Play();
            soundList.Add(source);

            if(callBack != null)
            {
                callBack(source);
            }
        });
    }

    /// <summary>
    /// �ı���Ч�����С
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        for(int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = soundValue;
        }
    }

    /// <summary>
    /// ֹͣ������Ч
    /// </summary>
    /// <param name="source"></param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }



}
