using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SystemExtension{
    public static class RandomizeObject{
        public static void RandomizeObjectList<T>(ref List<T> list, int MixCount) where T : MonoBehaviour{
            for(int i=0; i<MixCount; i++){
                
                int a = Random.Range(0, list.Count);
                int b = Random.Range(0, list.Count);

                T temp1 = list[a];
                T temp2 = list[b];
                ExtensionFunction.SwapGameObject(ref temp1, ref temp2);                            
                list.SwapElement(a,b);

            }
        }
    }

    public static class ExtensionFunction{
        public static void Swap<T>(ref T a, ref T b){
            (a,b) = (b,a);
        }
        
        public static void SwapElement<T>(this List<T> list, int idx1, int idx2) where T : MonoBehaviour{
            (list[idx1], list[idx2]) = (list[idx2], list[idx1]);
        }   

        public static void SwapGameObject<T>(ref T a, ref T b) where T : MonoBehaviour{
            (a.transform.position, b.transform.position) = (b.transform.position, a.transform.position);
            //(a,b) = (b,a);
        }
        
    }

}


