using System;

public class PuntoPrueba : IComparable
{
    public int ValorActual => _valor;

    private int _valor;

    public PuntoPrueba(int valor = 0)
    {
        _valor = valor;
    }

    public int CompareTo(object obj)
    {
        if (obj == null)
            return 1;

        PuntoPrueba punto = obj as PuntoPrueba;
        if (punto._valor == _valor)
            return 0;
        return _valor > punto._valor ? 1 : -1;
    }
}
