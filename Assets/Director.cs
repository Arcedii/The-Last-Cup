using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    public Volume globalVolume;
    private Vignette vignette;

    public GameObject Player;
    public GameObject MainCamera;
    public GameObject CutSceneCamera;

    public GameObject CoffeCup;
    public GameObject Client1;
    public GameObject Baricade;


    public AudioSource MusikSceneNull;
    public AudioSource audioSource;
    public AudioSource bell;

    public AudioClip sceneZeroReplic1;   // ������ ��� ����� 1
    public AudioClip sceneZeroReplic2;   // ������ ��� ����� 2
    public AudioClip sceneZeroReplic3; // ������ ��� ����� 3

    public AudioClip sceneOneReplic1; 
    public AudioClip sceneOneReplic2; 

    public GameObject Text;
    public Image DarkScreen;

    void Start()
    {
        Text.SetActive(false);
        CutSceneCamera.SetActive(true);
        Player.SetActive(false);
        BoxCollider boxColliderCoffeCup = CoffeCup.GetComponent<BoxCollider>();
        boxColliderCoffeCup.enabled = false;



        if (globalVolume.profile.TryGet<Vignette>(out vignette))
        {
            // ��������� �������� ��� ��������
            StartCoroutine(AnimateVignette());
        }


    }

    void Update()
    {
        
    }

    private System.Collections.IEnumerator AnimateVignette()
    {
        float duration = 4.0f; // ������������ �������� � ��������
        float elapsedTime = 0f;

        // ��������� �������� Intensity
        vignette.intensity.value = 1.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            // ������� ��������� �� 1 �� 0.3 � �������������� Lerp
            vignette.intensity.value = Mathf.Lerp(1.0f, 0.3f, t);
            yield return null;
        }

        // ����������, ��� �������� ����� ����������� � 0.3
        vignette.intensity.value = 0.3f;
        CutSceneCamera.SetActive(false);
        Player.SetActive(true);
        MusikSceneNull.Play();
        Text.SetActive(false);

        StartCoroutine(PlayScenesSequentially());
    }


    private System.Collections.IEnumerator PlayScenesSequentially()
    {
        yield return new WaitForSeconds(1f);
        SceneZeroReplic1();
        yield return new WaitForSeconds(1f);
        SceneZeroReplic2();
        yield return new WaitForSeconds(1f);
        SceneZeroReplic3();
        yield return StartCoroutine(FadeInDarkScreen());
        LoadSceneTwo();
    }

    public IEnumerator FadeInDarkScreen()
    {
        float duration = 1f;
        float elapsed = 0f;
        Color color = DarkScreen.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            DarkScreen.color = color;
            yield return null;
        }
    }

    public IEnumerator FadeOutDarkScreen()
    {
        float duration = 1f;
        float elapsed = 0f;
        Color color = DarkScreen.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            DarkScreen.color = color;
            yield return null;
        }
    }


    public void SceneZeroReplic1()
    {   
            audioSource.clip = sceneZeroReplic1;
            audioSource.Play();
            Debug.Log("Playing Scene 1 music.");   
    }

    public void SceneZeroReplic2()
    {       
            audioSource.clip = sceneZeroReplic2;
            audioSource.Play();
            Debug.Log("Playing Scene 2 music."); 
    }

    public void SceneZeroReplic3()
    {
       
            audioSource.clip = sceneZeroReplic3;
            audioSource.Play();
            Debug.Log("Playing Scene 3 music.");
        
    }

    public void LoadSceneTwo()
    {
        StartCoroutine(FadeOutDarkScreen());
        Player.transform.position = new Vector3(-3.62f, 2.904f, -19.86f);
        Client1.SetActive(true);
        StartCoroutine(PlaySceneOneReplics());
        BoxCollider boxColliderCoffeCup = CoffeCup.GetComponent<BoxCollider>();
        boxColliderCoffeCup.enabled = true;
        Text.SetActive(true);
        Baricade.SetActive(true);
    }


    private IEnumerator PlaySceneOneReplics()
    {
        audioSource.clip = sceneOneReplic1;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length); // ��� ���� �����������

        yield return new WaitForSeconds(1f); // ����� �������������� ��������

        audioSource.clip = sceneOneReplic2;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length); // ��� ���� �����������

 
    }

}
