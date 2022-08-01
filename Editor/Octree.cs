using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItIsNotOnlyMe.OctreeHeap
{
    public struct Octree<TType> where TType : IComparable
    {
        private const int _cantidadHijos = 8;

        private readonly Nodo<TType>[] _nodos;
        private readonly int _profundidadNodos;

        public Octree(Vector3 posicion, float ancho, float alto, float profundidad, int profundidadNodos) 
        {
            _profundidadNodos = profundidadNodos;

            int cantidadDeNodos = 0;
            for (int i = 0; i < _profundidadNodos; i++)
                cantidadDeNodos += (int)Mathf.Pow(_cantidadHijos, i);

            _nodos = new Nodo<TType>[cantidadDeNodos];
            InicializarNodos(posicion, ancho, alto, profundidad, 0);
        }

        private void InicializarNodos(Vector3 posicion, float ancho, float alto, float profundidad, int profundidadActual)
        {
            if (profundidadActual > _profundidadNodos)
                return;

            int indiceActual = 0;
            int cantidadDeHijos = 1;
            int cantidadDeHijosPorEje = (int)Mathf.Pow(2, profundidad);

            for (int i = 0; i < profundidadActual; i++)
            {
                indiceActual += cantidadDeHijos;
                cantidadDeHijos *= _cantidadHijos;
            }

            for (int i = 0, contador = indiceActual; i < cantidadDeHijosPorEje; i++)
                for (int j = 0; j < cantidadDeHijosPorEje; j++)
                    for (int k = 0; k < cantidadDeHijosPorEje; k++, contador++)
                    {
                        Vector3 posicionActual = posicion + new Vector3(i * ancho, j * alto, k * profundidad);
                        _nodos[contador] = new Nodo<TType>(posicionActual, ancho, alto, profundidad);
                    }

            InicializarNodos(posicion, ancho / 2, alto / 2, profundidad / 2, profundidadActual + 1);
        }

        public bool Insertar(TType valor, Vector3 posicion)
        {
            return Insertar(valor, posicion, 0, 0);
        }

        private bool Insertar(TType valor, Vector3 posicion, int indice, int profundiadActual)
        {
            if (!_nodos[indice].Contiene(posicion))
                return false;

            if (profundiadActual == _profundidadNodos)
            {
                _nodos[indice].Insertar(valor);
                return true;
            }

            int nuevoIndice = _nodos[indice].PosicionHijo(posicion, _nodos, indice);
            bool resultado = Insertar(valor, posicion, nuevoIndice, profundiadActual + 1);

            if (_nodos[indice].TieneHijosIguales(valor, _nodos, indice))
                _nodos[indice].Insertar(valor);

            return resultado;
        }
    }
}