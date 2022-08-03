using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ItIsNotOnlyMe.OctreeHeap;
using System.Collections.Generic;

public class OctreeTest
{
    private Vector3 _posicion = Vector3.zero;
    private Vector3 _dimensiones = Vector3.one * 1024;

    private int _valorBase = 1;

    [Test]
    public void Test01NoSeInsertaYTieneQueTenerSoloUnNodo()
    {
        int profundidad = 3;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, _dimensiones, profundidad);

        VisitanteCantidadDeNodosPrueba visitor = new VisitanteCantidadDeNodosPrueba();
        octree.Visitar(visitor);

        int cantidadDeNodos = visitor.CantidadDeNodos;

        Assert.AreEqual(1, cantidadDeNodos);
    }

    [Test]
    public void Test02SeInsertaUnElementoAfueraDelRangoYSeDevuelveFalse()
    {
        int profundidad = 3;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, _dimensiones, profundidad);

        Vector3 posicionFueraDeRango = -Vector3.one;
        PuntoPrueba valor = new PuntoPrueba(_valorBase);

        bool sePudoInsertar = octree.Insertar(posicionFueraDeRango, valor);

        Assert.IsFalse(sePudoInsertar);
    }

    [Test]
    public void Test03SePuedeInsertarUnElementoDentroDelRandoDeDimensiones()
    {
        int profundidad = 0;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, _dimensiones, profundidad);

        Vector3 oosicion = Vector3.one;
        PuntoPrueba valor = new PuntoPrueba(_valorBase);

        bool sePuedeInsertar = octree.Insertar(oosicion, valor);
        Assert.IsTrue(sePuedeInsertar);
    }

    [Test]
    public void Test04SeInsertaUnElementoConUnArbolDeCeroDeProfundidadSoloTieneUnNodo()
    {
        int profundidad = 0;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, _dimensiones, profundidad);

        Vector3 oosicion = Vector3.one;
        PuntoPrueba valor = new PuntoPrueba(_valorBase);

        octree.Insertar(oosicion, valor);

        VisitanteCantidadDeNodosPrueba visitor = new VisitanteCantidadDeNodosPrueba();
        octree.Visitar(visitor);

        int cantidadDeNodos = visitor.CantidadDeNodos;

        Assert.AreEqual(1, cantidadDeNodos);
    }

    [Test]
    public void Test05SeInsertaUnElementoConUnArbolDeUnoDeProfunidadTieneNueveNodos()
    {
        int profundidad = 1;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, _dimensiones, profundidad);

        Vector3 posicion = Vector3.one;
        PuntoPrueba valor = new PuntoPrueba(_valorBase);

        octree.Insertar(posicion, valor);

        VisitanteCantidadDeNodosPrueba visitor = new VisitanteCantidadDeNodosPrueba();
        octree.Visitar(visitor);

        int cantidadDeNodos = visitor.CantidadDeNodos;

        Assert.AreEqual(9, cantidadDeNodos);
    }

    [Test]
    public void Test06SeInsertaOchoElementosUnoEnCadaNodoQueSeGeneraProduciendoQueSoloEsteLaRaiz()
    {
        int profundidad = 1;
        Vector3 dimensiones = Vector3.one * 2;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, dimensiones, profundidad);

        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
                for (int k = 0; k < 2; k++)
                {
                    Vector3 posicion = new Vector3(
                        0.5f + 1 * i,
                        0.5f + 1 * j,
                        0.5f + 1 * k
                    );
                    PuntoPrueba valor = new PuntoPrueba(_valorBase);
                    octree.Insertar(posicion, valor);
                }

        VisitanteCantidadDeNodosPrueba visitor = new VisitanteCantidadDeNodosPrueba();
        octree.Visitar(visitor);

        int cantidadDeNodos = visitor.CantidadDeNodos;

        Assert.AreEqual(1, cantidadDeNodos);
    }

    [Test]
    public void Test07SeInsertaOchoElementosUnoEnCadaNodoQueSeGeneraProduciendoQueElValorDeLaRaizSeaElInsertado()
    {
        int profundidad = 1;
        Vector3 dimensiones = Vector3.one * 2;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, dimensiones, profundidad);

        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
                for (int k = 0; k < 2; k++)
                {
                    Vector3 posicion = new Vector3(
                        0.5f + 1 * i,
                        0.5f + 1 * j,
                        0.5f + 1 * k
                    );
                    PuntoPrueba valor = new PuntoPrueba(_valorBase);
                    octree.Insertar(posicion, valor);
                }

        VisitanteValoresDeLosNodosPrueba visitor = new VisitanteValoresDeLosNodosPrueba();
        octree.Visitar(visitor);

        List<VisitanteValoresDeLosNodosPrueba.DatosPrueba> datos = visitor.Datos;
        Assert.AreEqual(1, datos.Count);

        PuntoPrueba valorDelArbol = datos[0].Valor;
        Assert.AreEqual(0, valorDelArbol.CompareTo(new PuntoPrueba(_valorBase)));
    }

    [Test]
    public void Test08SeInsertaUnElementoConUnArbolDeUnoDeProfunidadYSoloUnoNodoTieneElValorDelElemento()
    {
        int profundidad = 1;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, _dimensiones, profundidad);

        Vector3 oosicion = Vector3.one;
        PuntoPrueba valor = new PuntoPrueba(_valorBase);

        octree.Insertar(oosicion, valor);

        VisitanteValoresDeLosNodosPrueba visitor = new VisitanteValoresDeLosNodosPrueba();
        octree.Visitar(visitor);

        List<VisitanteValoresDeLosNodosPrueba.DatosPrueba> datos = visitor.Datos;

        int cantidadDeDatosCambiados = 0;
        foreach (VisitanteValoresDeLosNodosPrueba.DatosPrueba dato in datos)
        {
            if (dato.Valor != null && dato.Valor.ValorActual == _valorBase)
                cantidadDeDatosCambiados++;
        }

        Assert.AreEqual(1, cantidadDeDatosCambiados);
    }

    [Test]
    public void Test09SeInsertaYSePuedeEliminarUnElementoDeUnArbol()
    {
        int profundidad = 3;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, _dimensiones, profundidad);

        Vector3 posicion = Vector3.one;
        PuntoPrueba valor = new PuntoPrueba(_valorBase);

        octree.Insertar(posicion, valor);
        bool sePuedeEliminar = octree.Eliminar(posicion);

        Assert.IsTrue(sePuedeEliminar);
    }

    [Test]
    public void Test10SeInsertaYEliminaUnElementoDeUnArbolConVariasProfundidadesTieneSoloUnNodo()
    {
        int profundidad = 3;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, _dimensiones, profundidad);

        Vector3 posicion = Vector3.one;
        PuntoPrueba valor = new PuntoPrueba(_valorBase);

        octree.Insertar(posicion, valor);
        octree.Eliminar(posicion);

        VisitanteCantidadDeNodosPrueba visitor = new VisitanteCantidadDeNodosPrueba();
        octree.Visitar(visitor);

        int cantidadDeNodos = visitor.CantidadDeNodos;

        Assert.AreEqual(1, cantidadDeNodos);
    }

    [Test]
    public void Test11SeInsertaYEliminaUnElementoDeUnArbolYSuValorEsElDefault()
    {
        int profundidad = 3;
        Octree<PuntoPrueba> octree = new Octree<PuntoPrueba>(_posicion, _dimensiones, profundidad);

        Vector3 posicion = Vector3.one;
        PuntoPrueba valor = new PuntoPrueba(_valorBase);

        octree.Insertar(posicion, valor);
        octree.Eliminar(posicion);

        VisitanteValoresDeLosNodosPrueba visitor = new VisitanteValoresDeLosNodosPrueba();
        octree.Visitar(visitor);

        List<VisitanteValoresDeLosNodosPrueba.DatosPrueba> datos = visitor.Datos;
        Assert.AreEqual(1, datos.Count);

        PuntoPrueba valorDelArbol = datos[0].Valor;
        Assert.AreEqual(default(PuntoPrueba), valorDelArbol);
    }
}
