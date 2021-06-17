using System;
using System.Collections.Generic;

namespace Section01
{
    //동적 배열
    /*
    class MyList<T>
    {
        const int DEFAULTSize = 1;
        T[] _data = new T[DEFAULTSize];

        public int Count = 0; //실제로 사용중인 데이터 개수 
        public int Capacity { get { return _data.Length; } } //예약된 데이터 개수

        // O(1) 예외 케이스 : 이사 비용은 무시한다.
        public void Add(T item)
        {
            //1. 공간이 충분히 남아있는지 확인한다.
            if (Count >= Capacity)
            {
                //공간을 다시 늘려서 확보한다.
                T[] newArray = new T[Count * 2];
                for (int i = 0; i < Count; i++)
                    newArray[i] = _data[i];
                _data = newArray;
            }

            //2. 공간에다가 데이터를 넣어준다.
            _data[Count] = item;
            Count++;
        }

        // O(1)
        public T this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        // O(N)
        public void RemoveAt(int index)
        {
            for (int i = index; i < Count - 1; i++)
                _data[i] = _data[i + 1];
            _data[Count - 1] = default(T); //Index값 초기화
            Count--;
        }
    }
    */

    //연결 리스트
    class MyLinkedListNode<T>
    {
        public T Data;
        public MyLinkedListNode<T> Next; //참조값. 방의 이전/다음 주소
        public MyLinkedListNode<T> Prev;

    }

    class MyLinkedList<T>
    {
        public MyLinkedListNode<T> Head = null; //첫번째 방
        public MyLinkedListNode<T> Tail = null; //마지막 방
        public int Count = 0;

        public MyLinkedListNode<T> AddLast(T data)
        {
            MyLinkedListNode<T> newRoom = new MyLinkedListNode<T>();
            newRoom.Data = data;

            //만약에 아직 방이 아예 없엇다면,새로 추가한 첫번째 방이 곧 Head
            if (Head == null)
                Head = newRoom;
            //기존의 Tail과 새로 추가되는 방을 연결
            if (Tail != null)
            {
                Tail.Next = newRoom;
                newRoom.Prev = Tail;
            }
            //새로 추가되는 방을 Tail로 인정
            Tail = newRoom;
            Count++;
            return newRoom;
        }

        //101 102 103 104 105
        public void Remove(MyLinkedListNode<T> room)
        {
            //기존의 첫번째 방의 다음 방을 첫번째 방으로 인정
            if (Head == room)
                Head = Head.Next;

            //기존의 마지막 방의 이전 방을 마지막 방으로 인정
            if (Tail == room)
                Tail = Tail.Prev;

            //삭제될 방의 앞 뒤 방을 연결
            if (room.Prev != null)
                room.Prev.Next = room.Next;
            if (room.Next != null)
                room.Next.Prev = room.Prev;

            Count--;
        }

    }



    //배열, 동적 배열, 연결 리스트 비교
    class ListType
    {
        public int[] _data = new int[25]; //배열
        //public MyList<int> _data2 = new MyList<int>(); //동적 배열
        public MyLinkedList<int> _data3 = new MyLinkedList<int>(); //연결 리스트

        public void Initialize()
        {
            //동적 배열
            /*
            _data2.Add(101);
            _data2.Add(102);
            _data2.Add(103);
            _data2.Add(104);
            _data2.Add(105);

            int temp = _data2[2];

            _data2.RemoveAt(2);
            */

            //연결 리스트
            _data3.AddLast(101);
            _data3.AddLast(102);
            MyLinkedListNode<int> node = _data3.AddLast(103);
            _data3.AddLast(104);
            _data3.AddLast(105);

            _data3.Remove(node);
        }
    }
}