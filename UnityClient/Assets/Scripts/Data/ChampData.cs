using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampData 
{
    public static ChampData Data { get; } = new ChampData();

    public Dictionary<string, Champion> _dic = new Dictionary<string, Champion>();

    public ChampData()
    {
        _dic.Add("Alencia", new Champion()
        {
            Hp = 1000,
            Attack = 500,
            Speed = 200,
            Name = "Alencia",
            Path = $"Model/AlenciaInGame",
        });

        _dic.Add("Ras", new Champion()
        {
            Hp = 2000,
            Attack = 200,
            Speed = 200,
            Name = "Ras",
            Path = $"Model/RasInGame",
        });

        _dic.Add("Senya", new Champion()
        {
            Hp = 2000,
            Attack = 200,
            Speed = 200,
            Name = "Senya",
            Path = $"Model/SenyaInGame",
        });

        _dic.Add("Violet", new Champion()
        {
            Hp = 2000,
            Attack = 200,
            Speed = 200,
            Name = "Violet",
            Path = $"Model/VioletInGame",
        });

    }

}
