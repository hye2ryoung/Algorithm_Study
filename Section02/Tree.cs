using System;
using System.Collections.Generic;

namespace Section02
{
    class TreeNode<T>
    {
        public T Data { get; set; }
        public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>();
    }

    class Tree
    {
        static TreeNode<string> MakeTree()
        {
            TreeNode<string> root = new TreeNode<string>() { Data = "R1 개발실" };
            {
                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "디자인팀" };
                    root.Children.Add(node);
                    node.Children.Add(new TreeNode<string>() { Data = "전투" });
                    node.Children.Add(new TreeNode<string>() { Data = "경제" });
                    node.Children.Add(new TreeNode<string>() { Data = "스토리" });
                }
                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "프로그래밍팀" };
                    root.Children.Add(node);
                    node.Children.Add(new TreeNode<string>() { Data = "서버" });
                    node.Children.Add(new TreeNode<string>() { Data = "클라" });
                    node.Children.Add(new TreeNode<string>() { Data = "엔진" });
                }
                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "아트팀" };
                    root.Children.Add(node);
                    node.Children.Add(new TreeNode<string>() { Data = "배경" });
                    node.Children.Add(new TreeNode<string>() { Data = "캐릭터" });
                }
            }
            return root;
        }

        static void PrintTree(TreeNode<string> root)
        {
            //접근 
            Console.WriteLine(root.Data);

            foreach (TreeNode<string> child in root.Children)
                PrintTree(child); //재귀적 동작
        }

        //트리 높이 구하기
        static int GetHeight(TreeNode<string> root) 
        {
            int height = 0;

            foreach(TreeNode<string> child in root.Children)
            {
                int newHeight = GetHeight(child) + 1; //예상되는 높이
                if (height < newHeight)
                    height = newHeight;
                //최종결과 반환 높이는 결국 자식들 중 가장 큰 높이로 덮어씌워짐
                //height = Math.Max(height, newHeight);
            }

            return height;
        }
        /*
        static void Main(string[] args)
        {
            TreeNode<string> root = MakeTree();
            //PrintTree(root);
            Console.WriteLine(GetHeight(root));

           
        }
        */
    }
}
