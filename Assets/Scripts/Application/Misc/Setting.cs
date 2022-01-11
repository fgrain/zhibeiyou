using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Slider bg, be, sv;
    public Button open, close;
    public GameObject setting;

    // Start is called before the first frame update
    private void Start()
    {
        if (!Game.Instance.sound.m_Bg.clip)
            StartCoroutine(PlayBG());
        StartCoroutine(setValue());
        if (open != null)
        {
            open.onClick.AddListener(() => OpenSetting());
        }
        if (close != null)
        {
            close.onClick.AddListener(() => ExitSetting());
        }

        bg.onValueChanged.AddListener((float value) => ChangeBGValue(value));
        be.onValueChanged.AddListener((float value) => ChangeEffectValue(value));
        sv.onValueChanged.AddListener((float value) => ChangeSoundValue(value));
    }

    public void ExitSetting()
    {
        setting.SetActive(false);
    }

    public void OpenSetting()
    {
        setting.SetActive(true);
    }

    public void ChangeBGValue(float value)
    {
        Game.Instance.sound.ChangeBgValue(value);
    }

    public void ChangeEffectValue(float value)
    {
        Game.Instance.sound.ChangeEffectValue(value);
    }

    public void ChangeSoundValue(float value)
    {
        Game.Instance.sound.ChangeSoundValue(value);
    }

    private IEnumerator PlayBG()
    {
        yield return new WaitForEndOfFrame();
        Game.Instance.sound.PlayBG("StartBG");
    }

    private IEnumerator setValue()
    {
        yield return new WaitForEndOfFrame();
        be.value = Game.Instance.sound.effectValue;
        bg.value = Game.Instance.sound.bgValue;
        sv.value = Game.Instance.sound.soundValue;
    }
}