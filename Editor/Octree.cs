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
        private readonly int _profundidad;

        public Octree(Vector3 posicion, Vector3 dimensiones, int profundidad)
        {
            _profundidad = profundidad;

            int cantidadDeNodos = 0;
            for (int i = 0; i <= _profundidad; i++)
                cantidadDeNodos += (int)Mathf.Pow(_cantidadHijos, i);

            _nodos = new Nodo<TType>[cantidadDeNodos];
            InicializarNodos(posicion, dimensiones, 0, 0);
        }

        private void InicializarNodos(Vector3 posicion, Vector3 dimensiones, int profundidadActual, int indice)
        {
            if (profundidadActual > _profundidad)
                return;

            int indiceActual = 0;
            int cantidadDeHijos = 1;
            int cantidadDeHijosPorEje = (int)Mathf.Pow(2, profundidadActual);

            for (int i = 0; i < profundidadActual; i++)
            {
                indiceActual += cantidadDeHijos;
                cantidadDeHijos *= _cantidadHijos;
            }

            for (int i = 0; i < cantidadDeHijosPorEje; i++)
                for (int j = 0; j < cantidadDeHijosPorEje; j++)
                    for (int k = 0; k < cantidadDeHijosPorEje; k++)
                    {
                        Vector3 posicionActual = new Vector3(
                            posicion.x + dimensiones.x * i,
                            posicion.y + dimensiones.y * j,
                            posicion.z + dimensiones.z * k
                        );

                        _nodos[indice++] = new Nodo<TType>(posicionActual, dimensiones);
                    }

            InicializarNodos(posicion, dimensiones / 2, profundidadActual + 1, indice);
        }

        public bool Insertar(Vector3 posicion, TType valor)
        {
            return Insertar(posicion, valor, 0, 0);
        }

        public void Visitar(IVisitor<TType> visitor)
        {
            foreach (Nodo<TType> nodo in _nodos)
                visitor.Visitar(nodo);
        }

        private bool Insertar(Vector3 posicion, TType valor, int indice, int profundiadActual)
        {
            if (!_nodos[indice].Contiene(posicion))
                return false;

            if (profundiadActual == _profundidad)
            {
                _nodos[indice].Insertar(valor);
                return true;
            }

            int nuevoIndice = _nodos[indice].PosicionHijo(posicion, _nodos, indice);
            bool resultado = Insertar(posicion, valor, nuevoIndice, profundiadActual + 1);
            _nodos[indice].SeSubdivide();

            if (_nodos[indice].TieneHijosIguales(valor, _nodos, indice))
            {
                _nodos[indice].Insertar(valor);
                _nodos[indice].SeJunta();
            }

            return resultado;
        }

        public bool Eliminar(Vector3 posicion)
        {
            return Insertar(posicion, default(TType));
        }
    }
}