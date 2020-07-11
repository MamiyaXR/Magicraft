﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface
{
    private TextureData _textureData;
    public TextureData textureData { get => _textureData; }
    public Surface(TextureType type)
    {
        _textureData = TextureData.textureDict[type];
    }
}
