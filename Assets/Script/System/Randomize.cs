using System.Collections.Generic;

public static class RandomizeObject{
    public static void RandomizeList<T>(ref List<T> list, int MixCount){
        for(int i=0; i<MixCount; i++){
            int a = UnityEngine.Random.Range(0, list.Count);
            int b = UnityEngine.Random.Range(0, list.Count);
            T temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }
    }
}
