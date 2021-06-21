using System;
using System.Collections.Generic;

namespace Section02
{
    public class Stack_Que
    {
        //스택 : LIFO(후입선출) Last in First Out
        //큐 : FIFO(선입선출) First in First Out

        //static void Main(string[] args)
        public Stack_Que()
        {
            Stack<int> stack = new Stack<int>();
            /*
            stack.Push(101);
            stack.Push(102);
            stack.Push(103);
            stack.Push(104);
            stack.Push(105);

            int data = stack.Pop();
            int data2 = stack.Peek();
            */

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(101);
            queue.Enqueue(102);
            queue.Enqueue(103);
            queue.Enqueue(104);
            queue.Enqueue(105);

            int data3 = queue.Dequeue();
            int data4 = queue.Peek();


        }
    }
}
