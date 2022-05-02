using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AudioMenuHelper : MonoBehaviour
{
    public GameObject audioTextCheck;

    public AudioMixer Main;
    //public AudioMixerGroup MAIN;
    //public AudioMixerGroup BGM;
    //public AudioMixerGroup BGS;

    [SerializeField] Animator audioAnimator;

    [SerializeField] Slider MainVolumeSlider;
    [SerializeField] Slider BGMVolumeSlider;
    [SerializeField] Slider BGSVolumeSlider;

    [SerializeField] TextMeshProUGUI MainVolumeNumber;
    [SerializeField] TextMeshProUGUI BGMVolumeNumber;
    [SerializeField] TextMeshProUGUI BGSVolumeNumber;

    private bool flag;

    int temp; //to convert volume from float to int for display purposes

    // Start is called before the first frame update
    void Start()
    {
        //debug
        MainVolumeSlider.value = 1;
        MainVolumeNumber.text = "100";

        BGMVolumeSlider.value = 1;
        BGMVolumeNumber.text = "100";

        BGSVolumeSlider.value = 1;
        BGSVolumeNumber.text = "100";
    }

    // Update is called once per frame
    void Update()
    {
        if (audioTextCheck.GetComponent<TextMeshProUGUI>().color.a >= 1)
        {
            audioAnimator.SetBool("BeginSelection", true);
        }

        if (Input.GetAxisRaw("Horizontal") == 0f)
        {
            flag = true;
        }

        switch (audioAnimator.GetInteger("MenuIndex"))
        {
            case 0:
                if ((Input.GetAxisRaw("Horizontal") == 1f || Input.GetKeyDown(KeyCode.RightArrow)) && flag)
                {
                    if (MainVolumeSlider.value >= 1f)
                    {
                        MainVolumeSlider.value = 1f;
                    }
                    else
                    {
                        MainVolumeSlider.value += 0.01f;
                    }

                    flag = false;
                }
                else if ((Input.GetAxisRaw("Horizontal") == -1f || Input.GetKeyDown(KeyCode.LeftArrow)) && flag)
                {
                    if (MainVolumeSlider.value <= 0f)
                    {
                        MainVolumeSlider.value = 0f;
                    }
                    else
                    {
                        MainVolumeSlider.value -= 0.01f;
                    }

                    flag = false;
                }
                temp = (int)(MainVolumeSlider.value * 100);
                MainVolumeNumber.text = temp.ToString();
                Main.SetFloat("Master", Mathf.Log10(MainVolumeSlider.value) * 20);

                break;
            case 1:
                if ((Input.GetAxisRaw("Horizontal") == 1f || Input.GetKeyDown(KeyCode.RightArrow)) && flag)
                {
                    if (BGMVolumeSlider.value >= 1f)
                    {
                        BGMVolumeSlider.value = 1f;
                    }
                    else
                    {
                        BGMVolumeSlider.value += 0.01f;
                    }

                    flag = false;
                }
                else if ((Input.GetAxisRaw("Horizontal") == -1f || Input.GetKeyDown(KeyCode.LeftArrow)) && flag)
                {
                    if (BGMVolumeSlider.value <= 0f)
                    {
                        BGMVolumeSlider.value = 0f;
                    }
                    else
                    {
                        BGMVolumeSlider.value -= 0.01f;
                    }

                    flag = false;
                }
                temp = (int)(BGMVolumeSlider.value * 100);
                BGMVolumeNumber.text = temp.ToString();
                Main.SetFloat("BGM", Mathf.Log10(BGMVolumeSlider.value) * 20);

                break;
            case 2:
                if ((Input.GetAxisRaw("Horizontal") == 1f || Input.GetKeyDown(KeyCode.RightArrow)) && flag)
                {
                    if (BGSVolumeSlider.value >= 1f)
                    {
                        BGSVolumeSlider.value = 1f;
                    }
                    else
                    {
                        BGSVolumeSlider.value += 0.01f;
                    }

                    flag = false;
                }
                else if ((Input.GetAxisRaw("Horizontal") == -1f || Input.GetKeyDown(KeyCode.LeftArrow)) && flag)
                {
                    if (BGSVolumeSlider.value <= 0f)
                    {
                        BGSVolumeSlider.value = 0f;
                    }
                    else
                    {
                        BGSVolumeSlider.value -= 0.01f;
                    }

                    flag = false;
                }
                temp = (int)(BGSVolumeSlider.value * 100);
                BGSVolumeNumber.text = temp.ToString();
                Main.SetFloat("BGS", Mathf.Log10(BGSVolumeSlider.value) * 20);

                break;
            case 3:
                if (audioAnimator.GetBool("pressed") && audioAnimator.GetBool("BeginSelection"))
                {
                    StartCoroutine(OptionsSceneChange());
                }
                break;
            default:
                break;
        }
    }

    IEnumerator OptionsSceneChange()
    {
        yield return new WaitForSeconds(0.7f);
        MainMenuMgr.inst.SetActiveMenu("OptionsMenu");
    }
}
