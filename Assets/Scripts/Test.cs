using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        private Coroutine test;

        private void Start()
        {
            test=StartCoroutine(TestCor());
        }

        private void Update()
        {
            if (test!=null)
            {
                print("协程不为空");
            }
            else
            {
                print("协程为空");
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                StopCoroutine(test);
            }
        }

        private IEnumerator TestCor()
        {
            while (true)
            {
                yield return null;
                print("执行中");
            }
        }
    }
}
