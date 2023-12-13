using UnityEngine;

public static class StaticExtensions
{
    #region Layer Mask
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }


    public static int GetLayerNumberFromMask(this LayerMask mask)
    {
        int layerNumber = 0;
        int layer = mask.value;

        while (layer > 1)
        {
            layer = layer >> 1;
            layerNumber++;
        }

        return layerNumber;
    }
    #endregion
}
