using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class ExeModel : MonoBehaviour
    {
        public void Start()
        {
            StartCoroutine(getModel());
        }

        IEnumerator getModel()
        {
            ModelTest chatModel = new ModelTest(); 
            // var chatModel = new ModelPostTest();
            yield return StartCoroutine(chatModel.ApiRequest());
                    // 以下表示させるなど
        }
    }
}
