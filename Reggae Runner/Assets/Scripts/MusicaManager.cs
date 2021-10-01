using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaManager : MonoBehaviour
{

    /*Audios.*/
    [SerializeField]
    AudioClip audioMusica;
    [SerializeField]
    AudioClip audioMorte;
    [SerializeField]
    AudioClip audioDerrota;

    AudioSource Audio;



    // Start is called before the first frame update
    void Start()
    {

        Audio = GetComponentInChildren<AudioSource>();
        iniciarMusica();
    }



    /*Funcao iniciarMusica():
    * Metodo responsavel por iniciar a Musica do Jogo.*/
    public void iniciarMusica()
    {

        Audio.loop = true;
        Audio.clip = audioMusica;
        Audio.Play();
        StartCoroutine(aumentarVolume());   //Aumenta o volume da Musica.
    }



    /*Funcao iniciarAudioMorte():
    * Metodo responsavel por iniciar o Audio de Morte.*/
    public void iniciarAudioMorte() 
    {

        Audio.loop = false;
        Audio.clip = audioMorte;
        Audio.Play();
        StartCoroutine(iniciarAudioDerrota());   //Inicia o audio de Derrota.
        StartCoroutine(reiniciarMusica());       //Reinicia a Musica do Jogo.
    }



    /*Corrotina para iniciar o Audio de Derrota.*/
    private IEnumerator iniciarAudioDerrota() {

        yield return new WaitForSeconds(0.5f);
        Audio.clip = audioDerrota;
        Audio.Play();
    }



    /*Corrotina para reiniciar a Musica do Jogo.*/
    private IEnumerator reiniciarMusica()
    {

        float pausarTempo = Time.realtimeSinceStartup + 5f;
        while (Time.realtimeSinceStartup < pausarTempo) yield return 0;
        iniciarMusica();
    }



    /*Corrotina para aumentar o volume do audio Gradualmente.*/
    private IEnumerator aumentarVolume()
    {

        if (Audio.volume < 1f)
        {

            /*Caso onde a Corritina ja esta em Execucao.
            * (Ex.: Morrer nos 2 primeiros Obstaculos)*/
            yield return 0;
        }
        else
        {

            float Vol = 0.1f;
            while (Vol < 1.1f)
            {

                /*Aumenta o Volume do Audio.*/
                Audio.volume = Vol;
                float pausarTempo = Time.realtimeSinceStartup + 1f;
                while (Time.realtimeSinceStartup < pausarTempo) yield return 0;
                Vol += 0.1f;
            }
        }
    }
}