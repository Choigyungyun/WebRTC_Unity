using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public class UniqueInstance <T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool hasInstance = false;

        private void Awake()
        {
            if (hasInstance)
            {
                // �̹� �ν��Ͻ��� �����ϹǷ� �� �ν��Ͻ��� �ı��մϴ�.
                Destroy(gameObject);
            }
            else
            {
                // �� �ν��Ͻ��� �����ϴٴ� ���� ǥ���մϴ�.
                hasInstance = true;
                // �ٸ� ������ �̵��� �� �ı����� �ʵ��� �����մϴ�.
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
