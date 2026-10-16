namespace MO32_1_Daniil.NeuroNet
{
    enum MemoryMode
    {
        GET,    // считывание памяти
        SET,    // сохранение памяти
        INIT    // инициализации памяти
    }
    enum NeuronType
    {
        Hidden, //
        Output  //
    }
    enum NetworkMod
    {
        Train,
        Test,
        Demo
    }
}
