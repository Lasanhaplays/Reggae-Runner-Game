using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoCamera : MonoBehaviour{

    [SerializeField]
    Transform alvo;
    [SerializeField]
    [Range(0.1f, 1f)]
    float interpolacao = 0.25f;

    bool resetarCamera;
    Vector3 posInicial;
    Vector3 deslocamentoDaCam;
    Vector3 velocidade = Vector3.zero;



    /*Funcao resetar():
    * Metodo responsavel por alterar a condicao de Resetar a Camera
    * para a posicao Inicial (utilizado pelo Script dos Menus).*/
    public void resetar(bool VF) {

        resetarCamera = VF;
    }



    private void Start() {

        resetarCamera = false;
        posInicial = transform.position;
        deslocamentoDaCam = transform.position - alvo.position;
    }



    void FixedUpdate() {

        if (resetarCamera == false)
        {

            /*Movimenta a Camera.*/
            Vector3 posicaoFinal = new Vector3(alvo.position.x + deslocamentoDaCam.x, alvo.position.y + deslocamentoDaCam.y, transform.position.z - deslocamentoDaCam.z);
            transform.position = Vector3.SmoothDamp(transform.position, posicaoFinal, ref velocidade, interpolacao);
        }
        else {

            /*Retorna a Camera para a posicao Inicial.*/
            transform.position = posInicial;
            resetarCamera = false;
        }
    }
}