using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AbrirMenu : MonoBehaviour{

    /*Menus.*/
    public TMP_Text Placar;
    public GameObject MenuStart;
    public GameObject MenuMorte;
    public GameObject MenuRe_Start;

    /*Placar.*/
    string textoPlacar;
    static int maiorPontuacao = 0;
    static string maiorPlacar = "Maior placar:  0 pts";

    /*Componentes Extras.*/
    public Movimentacao Personagem;
    public MovimentoCamera posicaoCamera;



    /*Funcao setPlacar():
    * Metodo responsavel por atualizar os Pontos no menu
    * de Placar (utilizado pelo Script da Movimentacao).*/
    public void setPlacar(int Score){

        if (Score > maiorPontuacao)
        {

            /*Atualiza a maior Pontuacao Obtida (Variaveis persistentes).*/
            maiorPontuacao = Score;
            maiorPlacar = "Maior placar:  " + maiorPontuacao + " pts";
        }

        /*Atualiza o texto do Placar atual e o texto do Menu.*/
        textoPlacar = "Placar:  " + Score + " pts";
        Placar.text = textoPlacar;
    }



    void Start(){

        /*Habilita o menu de Start exibindo
         * o Placar com a maior Pontuacao.*/
        MenuStart.SetActive(true);
        MenuStart.GetComponentInChildren<TMP_Text>().text = maiorPlacar;

        //Desabilita os demais Menus.
        MenuMorte.SetActive(false);
        MenuRe_Start.SetActive(false);

        //Desabilita a condicao de andar do Personagem.
        Personagem.estaAndando(false);
    }



    /*Funcao iniciarGame():
    * Metodo responsavel por dar inicio ao Jogo.*/
    public void iniciarGame()
    {

        MenuStart.SetActive(false);     //Desabilita o menu de Start.
        posicaoCamera.resetar(true);    //Reseta a Camera para a posicao Inicial.
        Personagem.estaAndando(true);   //Habilita a condicao de andar do Personagem.
    }



    /*Funcao LigarMenuMorte():
    * Metodo responsavel por exibir o menu de Morte.*/
    public void LigarMenuMorte() {

        MenuMorte.SetActive(true);
        StartCoroutine(contadorDeTempo());
    }


   
    private IEnumerator contadorDeTempo()
    {

        /*Pausa o jogo apos 0.9 segundos para haver tempo de exibir a animacao de Morte.*/
        float pausarTempo = Time.realtimeSinceStartup + 2.5f;
        while (Time.realtimeSinceStartup < pausarTempo) yield return 0;
        Time.timeScale = 0f;

        /*Pausa o jogo apos alguns segundos para exibir o menu de Re_Start.*/
        pausarTempo = Time.realtimeSinceStartup + 1.5f;
        while (Time.realtimeSinceStartup < pausarTempo) yield return 0;
        LigarMenuRe_Start();
    }



    /*Funcao LigarMenuRe_Start():
    * Metodo responsavel por exibir o menu de Re_Start.*/
    public void LigarMenuRe_Start()
    {

        //Desabilita o menu de Morte.
        MenuMorte.SetActive(false);

        /*Habilita o menu de Re_Start exibindo
         * o Placar de pontos Atual.*/
        MenuRe_Start.SetActive(true);
        MenuRe_Start.GetComponentInChildren<TMP_Text>().text = textoPlacar;
    }



    /*Funcao reiniciarGame():
    * Metodo responsavel por reiniciar o Jogo.*/
    public void reiniciarGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;   //Faz o tempo do Jogo correr novamente.
    }



    /*Funcao sairDoGame():
    * Metodo responsavel por fechar o Jogo.*/
    public void sairDoGame()
    {

        Debug.Log("Quit Game!");
        Application.Quit();
    }
}