using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SystemExtension{
    public static class RandomizeObject{
        public static void RandomizeObjectList<T>(ref List<T> list, int MixCount) where T : MonoBehaviour{
            for(int i=0; i<MixCount; i++){
                int a = Random.Range(0, list.Count);
                int b = Random.Range(0, list.Count);

                Vector3 pos1 = list[a].transform.position;
                Vector3 pos2 = list[b].transform.position;
                list[a].transform.position = pos2;
                list[b].transform.position = pos1;
                

                T temp = list[a];
                list[a] = list[b];
                list[b] = temp;

            }
        }
    }

    public static class UseFulUnityFunction{
        public static void Swap<T>(ref T a, ref T b){
            (a,b) = (b,a);
        }

        public static void SwapGameObject<T>(ref T a, ref T b) where T : MonoBehaviour{
            (a.transform.position, b.transform.position) = (b.transform.position, a.transform.position);
            (a,b) = (b,a);
        }
    }

}


