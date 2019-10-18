using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paraProva : MonoBehaviour
{
    string[] nomeDaVariavel = new string[10];
    List<int> nomeDaLista = new List<int>();
    void Start()
    {
        //Array
        int tamanho = nomeDaVariavel.Length;

        //List
        int tamanhoDaLista = nomeDaLista.Count;
        nomeDaLista.Add(1);
        nomeDaLista.Clear();
        nomeDaLista.Contains(2); //verificar se existe
        nomeDaLista.Insert(2, 2); // onde \ o quê
        nomeDaLista.Remove(1); // remover algo especificado
        nomeDaLista.RemoveAt(2); // reover a posição
    }
}
