using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cinematica : MonoBehaviour
{
    public GameObject[] camaras;
    public Image fade, wasd;
    public static bool cinem;
    public GameObject inicio, pj;
    void Start()
    {
        //camara de cinematia, bloqueo de pj y desactivado canvas
        camaras = GameObject.FindGameObjectsWithTag("camara");
        camaras[1].GetComponent<Camera>().targetDisplay = PlayerController.displayID-1;
        StartCoroutine(cinemAnim());
        PlayerController.movimiento = false;
        pj.SetActive(false);
        wasd.enabled = false;
        inicio.SetActive(false);
    }

   
    IEnumerator cinemAnim()
    {
        //cambio de camara a la de cinematica, desbloqueo de pj
        cinem = true;
        yield return new WaitForSeconds(12);
        yield return StartCoroutine(fadeIn());
        camaras[1].GetComponent<Camera>().targetDisplay = PlayerController.displayID;
        PlayerController.movimiento = true;

    }



    //fadeIn
    IEnumerator fadeIn()
    {
        fade.GetComponent<Animator>().SetBool("fadeIn", true);
        yield return new WaitForSeconds(1);
        fade.GetComponent<Animator>().SetBool("fadeIn", false);
        fade.GetComponent<Animator>().SetBool("fadeOut", true);
        yield return null;
        yield return StartCoroutine(fadeOut());
        pj.SetActive(true);


    }

    //fadeOut
    IEnumerator fadeOut()
    {
        fade.GetComponent<Animator>().SetBool("fadeIn", true);
        //yield return new WaitForSeconds(1);
        fade.GetComponent<Animator>().SetBool("fadeIn", false);
        fade.GetComponent<Animator>().SetBool("fadeOut", true);
        yield return null;
        cinem = false;
        wasd.enabled = true;
        wasd.GetComponent<Animator>().SetBool("wasd", true);
        inicio.SetActive(true);

    }
}
