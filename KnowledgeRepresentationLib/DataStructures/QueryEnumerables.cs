using System;

namespace KR_Lib.DataStructures
{
    /// <summary>
    /// Rodzaj kwerendy, doprecyzowanie czy pytamy o 'zawsze', czyli spełnialność odpowiedzi
    /// w każdym modelu czy 'kiedykolwiek' - przynajmniej w jednym modelu
    /// </summary>
    public enum QueryType
    {
        Always,
        Ever
    }
}
