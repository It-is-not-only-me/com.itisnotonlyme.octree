using System.Collections.Generic;
using UnityEngine;
using ItIsNotOnlyMe.OctreeHeap;

public class VisitanteValoresDeLosNodosPrueba : IVisitor<PuntoPrueba>
{
    public struct DatosPrueba
    {
        public bool EstaSubdividido;
        public PuntoPrueba Valor;

        public DatosPrueba(bool estaSubidivido, PuntoPrueba valor)
        {
            EstaSubdividido = estaSubidivido;
            Valor = valor;
        }
    }

    public List<DatosPrueba> Datos;

    public VisitanteValoresDeLosNodosPrueba()
    {
        Datos = new List<DatosPrueba>();
    }

    public void Visitar(Nodo<PuntoPrueba> nodo)
    {
        Datos.Add(new DatosPrueba(nodo.EstaSubdividido, nodo.Valor));
    }
}
