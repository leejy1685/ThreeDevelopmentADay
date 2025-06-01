using System.Collections.Generic;
using _02._Scripts.Objects.LaserMachine;
using UnityEngine;

namespace _02._Scripts.Pipe.LinkedPipe
{
    public class LinkedPipeManager : MonoBehaviour
    {
        [Header(" [ 에디터 할당 ]")]
        [SerializeField] public List<LinkedPipe> allPipes;

        void Start()
        {
            InvokeRepeating(nameof(InitializeColorPropagation), 0.2f, 0.5f); // 일정 주기로 상태 갱신
        }

        public void InitializeColorPropagation()
        {
            HashSet<LinkedPipe> visited = new HashSet<LinkedPipe>();

            foreach (var pipe in allPipes)
            {
                foreach (var port in pipe.ports)
                {
                    if (port.IsConnectedToLaser(out LASER_COLOR color))
                    {
                        //Debug.Log($"[Init] {pipe.name}이 LaserMachine과 연결됨. 색: {color}");
                        pipe.SetPipeColor(color);
                        pipe.PropagateColor(color, visited);
                        break;
                    }
                }
            }

            foreach (var pipe in allPipes)
            {
                if (!visited.Contains(pipe))
                {
                    pipe.SetPipeColor(LASER_COLOR.White);
                }
            }
        }
    }
}