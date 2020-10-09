using System;
using System.Collections.Generic;
using System.Text;

namespace Compresor.ColaLabED1
{
    public class ColaED1<T> where T : IComparable
    {
        List<Node<T>> priorityQueue = new List<Node<T>>();
        public int size = -1;
        T generico;

        int reverseEquation(int x)
        {
            return (x - 1) / 2;
        }

        void change(int x, int y)
        {

            var aux = priorityQueue[x];
            priorityQueue[x] = priorityQueue[y];
            priorityQueue[y] = aux;

        }

        int left(int x)
        {
            return (x * 2) + 1;
        }

        int right(int x)
        {
            return (x * 2) + 2;
        }

        void maxHeap(int x)
        {
            int nleft = left(x);
            int nright = right(x);
            int h = x;

            if ((nleft <= size) && (priorityQueue[h].priority < priorityQueue[nleft].priority))
            {
                h = nleft;
            }
            if ((nright <= size) && priorityQueue[h].priority < priorityQueue[nright].priority)
            {
                h = nright;
            }
            if (h != x)
            {
                change(h, x);
                maxHeap(h);
            }
        }

        void heapMax(int x)
        {
            while ((x >= 0) && (priorityQueue[reverseEquation(x)].priority < priorityQueue[x].priority))
            {
                change(x, reverseEquation(x));
                x = reverseEquation(x);
            }
        }

        void Inserting(decimal p, T data)
        {
            /*if (size == 1)
            {
                size = -1;
            }*/

            Node<T> newNode = new Node<T>();
            newNode.priority = p;
            newNode.value = data;

            priorityQueue.Add(newNode);
            size++;
            heapMax(size);
        }


        T Deleting()
        {
            Node<T> aux = new Node<T>();
            if (size > -1)
            {
                ordenar();
                //maxHeap(0);
                T data = priorityQueue[size].value;
                priorityQueue.RemoveAt(size);
                size--;
                return data;
            }
            else
            {
                return generico;
            }
        }

        public void ordenar()
        {
            Node<T> aux = new Node<T>();
            for (int j = 0; j < priorityQueue.Count; j++)
            {
                if (j < 7)
                {
                    for(int i = 0; i < priorityQueue.Count - 1; i++)
                    {
                        if (priorityQueue[i].priority.CompareTo(priorityQueue[i + 1].priority) == -1)
                        {
                            aux = priorityQueue[i + 1];
                            priorityQueue[i + 1] = priorityQueue[i];
                            priorityQueue[i] = aux;
                        }
                    }
                }
            }
        }
        public void Insert(decimal value, T data)
        {
            Inserting(value, data);
        }

        public T Delete()
        {
            return Deleting();
        }

        public T Peek()
        {
            if (size > -1)
                return priorityQueue[0].value;
            else
                return generico;
        }

        public List<Node<T>> CopyOfData()
        {
            var CopyDataList = this.priorityQueue;

            return CopyDataList;
        }
    }
}
