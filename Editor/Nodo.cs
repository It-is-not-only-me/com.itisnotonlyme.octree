using System;
using UnityEngine;

namespace ItIsNotOnlyMe.OctreeHeap
{
    public struct Nodo<TType> where TType : IComparable
    {
        private const int _cantidadHijos = 8, _estaJunto = 0, _estaSubdividido = 1;

        private Vector3 _posicion, _dimensiones;
        private int _subdivido;

        private TType _valor;

        public Nodo(Vector3 posicion, Vector3 dimensiones, TType valor = default(TType))
        {
            _posicion = posicion;

            _dimensiones = dimensiones;
            _subdivido = _estaJunto;

            _valor = valor;
        }

        public TType Valor => _valor;
        public bool EstaSubdividido => _subdivido == _estaSubdividido;

        private Vector3 ExtremoOpuesto => _posicion + _dimensiones;

        public bool Contiene(Vector3 posicion)
        {
            bool contiene = true;
            for (int i = 0; i < 3; i++)
                contiene &= _posicion[i] < posicion[i] && posicion[i] < ExtremoOpuesto[i];
            return contiene;
        }

        public void Insertar(TType valor)
        {
            _valor = valor;
        }

        public int PosicionHijo(Vector3 posicion, Nodo<TType>[] nodos, int indice)
        {
            int indiceHijo = 0;
            foreach (int indiceActual in IndicesDeHijos(indice))
                if (nodos[indiceActual].Contiene(posicion))
                    indiceHijo = indiceActual;

            return indiceHijo;
        }

        public bool TieneHijosIguales(TType valor, Nodo<TType>[] nodos, int indice) 
        {
            bool tieneHijosIguales = true;
            foreach (int indiceActual in IndicesDeHijos(indice))
                tieneHijosIguales &= valor.Equals(nodos[indiceActual]._valor);
            return tieneHijosIguales;
        }

        public void SeSubdivide()
        {
            _subdivido = _estaSubdividido;
        }

        public void SeJunta()
        {
            _subdivido = _estaJunto;
        }

        private int[] IndicesDeHijos(int indice)
        {
            int[] indices = new int[_cantidadHijos];

            for (int i = 0; i < _cantidadHijos; i++)
                indices[i] = indice * _cantidadHijos + 1 + i;

            return indices;
        }
    }
}