using System;
using UnityEngine;

namespace ItIsNotOnlyMe.OctreeHeap
{
    public struct Nodo<TTipo> where TTipo : IComparable
    {
        private const int _cantidadHijos = 8, _estaJunto = 0, _estaSubdividido = 1;

        private Vector3 _posicion, _dimensiones;
        private int _subdivido;

        private TTipo _valor;

        public Nodo(Vector3 posicion, Vector3 dimensiones, TTipo valor = default(TTipo))
        {
            _posicion = posicion;
            _dimensiones = dimensiones;
            _subdivido = _estaJunto;

            _valor = valor;
        }

        public TTipo Valor => _valor;
        public bool EstaSubdividido => _subdivido == _estaSubdividido;

        private Vector3 ExtremoOpuesto => _posicion + _dimensiones;

        public bool Contiene(Vector3 posicion)
        {
            bool contiene = true;
            for (int i = 0; i < 3; i++)
                contiene &= _posicion[i] < posicion[i] && posicion[i] < ExtremoOpuesto[i];
            return contiene;
        }

        public void Insertar(TTipo valor)
        {
            _valor = valor;
        }

        public int PosicionHijo(Vector3 posicion, Nodo<TTipo>[] nodos, int indice)
        {
            int indiceHijo = 0;
            foreach (int indiceActual in IndicesDeHijos(indice))
                if (nodos[indiceActual].Contiene(posicion))
                    indiceHijo = indiceActual;

            return indiceHijo;
        }

        public bool TieneHijosIguales(TTipo valor, Nodo<TTipo>[] nodos, int indice) 
        {
            bool tieneHijosIguales = true;
            foreach (int indiceActual in IndicesDeHijos(indice))
                tieneHijosIguales &= nodos[indiceActual].TieneMismoValor(valor);
            return tieneHijosIguales;
        }

        private bool TieneMismoValor(TTipo valor)
        {
            if (EstaSubdividido)
                return false;

            if (_valor == null)
                return valor == null;

            return _valor.CompareTo(valor) == 0;
        }

        public void Subdividir()
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