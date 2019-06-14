using System;
using System.Collections;
using System.Collections.Generic;

namespace DoubleLinkedList
{
    public class DoublyNode<T>
    {
        public DoublyNode()
        {

        }
        public DoublyNode(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public DoublyNode<T> Previous { get; set; }
        public DoublyNode<T> Next { get; set; }
    }

    public class DoublyLinkedList<T> : IEnumerable<T>
    {
        DoublyNode<T> head;
        DoublyNode<T> tail;
        int count;

        public void Add(T data)
        {
            DoublyNode<T> node = new DoublyNode<T>(data);

            if (head == null)
                head = node;
            else
            {
                tail.Next = node;
                node.Previous = tail;
            }
            tail = node;
            count++;
        }

        public void AddFront(T data)
        {
            DoublyNode<T> node = new DoublyNode<T>(data);
            DoublyNode<T> temp = head;
            node.Next = temp;
            head = node;
            if (count == 0)
                tail = head;
            else
                temp.Previous = node;
            count++;
        }

        public void RemoveFront()
        {
            if (head.Next != null)
            {
                head.Next.Previous = null;
            }

            head = head.Next;
            count--;
        }

        public void RemoveBack()
        {
            if (tail.Previous != null)
            {
                tail.Previous.Next = null;
            }

            tail = tail.Previous;
            count--;
        }

        public DoublyNode<T> PeekFront()
        {
            return head;
        }

        public DoublyNode<T> PeekBack()
        {
            return tail;
        }

        public bool Remove(T data)
        {
            DoublyNode<T> current = head;

            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    break;
                }
                current = current.Next;
            }

            if (current != null)
            {
                if (current.Next != null)
                {
                    current.Next.Previous = current.Previous;
                }
                else
                {
                    tail = current.Previous;
                }

                if (current.Previous != null)
                {
                    current.Previous.Next = current.Next;
                }
                else
                {
                    head = current.Next;
                }

                count--;
                return true;
            }
            return false;
        }

        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public bool Contains(T data)
        {
            DoublyNode<T> current = head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            DoublyNode<T> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        public IEnumerable<T> BackEnumerator()
        {
            DoublyNode<T> current = tail;
            while (current != null)
            {
                yield return current.Data;
                current = current.Previous;
            }
        }
    }

    public class DLOperator
    {
        private static int filesAmount;
        private static int step;

        private static bool nextNeedToRebuild = false;
        private static bool prevNeedToRebuild = false;

        private static DoublyLinkedList<int> linkedList = new DoublyLinkedList<int>();

        public DLOperator(int inputArrLengh, int inputStep)
        {
            filesAmount = inputArrLengh;
            step = inputStep;

            for (int i = 0; i < filesAmount; i++)
            {
                linkedList.Add(i);
            }
        }

        public int[] pickNext()
        {
            List<int> elsList = new List<int>();
            DoublyNode<int> el = new DoublyNode<int>();

            if (nextNeedToRebuild)
            {
                for (int i = 0; i < step; i++)
                {
                    el = linkedList.PeekFront();
                    moveUp(el);
                }
            }

            prevNeedToRebuild = true;
            nextNeedToRebuild = false;

            for (int i = 0; i < step; i++)
            {
                el = linkedList.PeekFront();
                moveUp(el);
                elsList.Add(el.Data);
            }

            if (step > 1)
            {
                el = linkedList.PeekFront();
                elsList.Add(el.Data);
            }

            return elsList.ToArray();
        }

        public int[] pickPrevious()
        {
            List<int> elsList = new List<int>();
            DoublyNode<int> el = new DoublyNode<int>();

            if (prevNeedToRebuild)
            {
                for (int i = 0; i < step; i++)
                {
                    el = linkedList.PeekBack();
                    moveDown(el);
                }
            }

            prevNeedToRebuild = false;
            nextNeedToRebuild = true;

            if (step > 1)
            {
                el = linkedList.PeekFront();
                elsList.Add(el.Data);
            }

            for (int i = 0; i < step; i++)
            {
                el = linkedList.PeekBack();
                moveDown(el);
                elsList.Add(el.Data);
            }

            int[] elsArr = elsList.ToArray();
            Array.Reverse(elsArr);

            return elsArr;
        }

        private void moveUp(DoublyNode<int> el)
        {
            linkedList.RemoveFront();
            linkedList.Add(el.Data);
        }

        private void moveDown(DoublyNode<int> el)
        {
            linkedList.RemoveBack();
            linkedList.AddFront(el.Data);
        }

        public void printList()
        {
            Console.WriteLine();
            Console.WriteLine("--------");

            foreach (var item in linkedList)
            {
                Console.Write("[ ");
                Console.Write(item);
                Console.Write(" ] ");
            }

            Console.WriteLine();
            Console.WriteLine("--------");
            Console.WriteLine();
        }
    }
}