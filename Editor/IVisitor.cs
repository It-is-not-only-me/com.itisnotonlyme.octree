using System;

namespace ItIsNotOnlyMe.OctreeHeap
{
    public interface IVisitor<TTipo> where TTipo : IComparable
    {
        public void Visitar(Nodo<TTipo> nodo);
    }
}
