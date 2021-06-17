using System;
using System.Collections.Generic;

namespace Section01
{ 
    class Board
    {
        const char CIRCLE = '\u25cf';

        public TileType[,] Tile { get; private set; } //배열
        public int Size { get; private set; }


        public int DestY { get; private set; }
        public int DestX { get; private set; }

        Player _player;
       
        public enum TileType
        {
            Empty,
            Wall,
        }

        public void Initialize(int size, Player player)
        {
            if (size % 2 == 0)
                return;

            _player = player;

            Tile = new TileType[size, size];
            Size = size;

            DestY = Size - 2;
            DestX = Size - 2;


            //Mazes for Programmers
            //GenerateByBinaryTree();
            GenerateBySideWinder();
        }

        void GenerateBySideWinder()
        {
            //일단 길을 다 막아버리는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    //벽
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }

            }

            //랜덤으로 우측 혹은 아래로 길을 뚫는 작업
            //Binary Tree Algorithm
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                int count = 1;
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    if (y == Size - 2 && x == Size - 2) //마지막 점
                    {
                        continue; //출구로 인식하지 않도록 막아줌
                    }

                    if (y == Size - 2) //외곽 - 1 좌표
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue; //랜덤함수를 타지 않도록 continue
                    }

                    if (x == Size - 2) //외곽 - 1 좌표
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue; //랜덤함수를 타지 않도록 continue
                    }

                    if (rand.Next(0, 2) == 0)
                    {
                        //우측
                        Tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                    else
                    {
                        int randomIndex = rand.Next(0, count);
                        //아래
                        Tile[y + 1, x - randomIndex * 2] = TileType.Empty;
                        count = 1;
                    }
                    
                }

            }
        }

        /*
        void GenerateByBinaryTree()
        {
            //일단 길을 다 막아버리는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    //벽
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }

            }

            //랜덤으로 우측 혹은 아래로 길을 뚫는 작업
            //Binary Tree Algorithm
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    if (y == Size - 2 && x == Size - 2) //마지막 점
                    {
                        continue; //출구로 인식하지 않도록 막아줌
                    }

                    if (y == Size - 2) //외곽 - 1 좌표
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue; //랜덤함수를 타지 않도록 continue
                    }

                    if (x == Size - 2) //외곽 - 1 좌표
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue; //랜덤함수를 타지 않도록 continue
                    }

                    if (rand.Next(0, 2) == 0)
                    {
                        //우측
                        Tile[y, x + 1] = TileType.Empty;
                    }
                    else
                    {
                        //아래
                        Tile[y + 1, x] = TileType.Empty;

                    }
                }

            }
        }
        */

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    //플레이어 좌표를 갖고와서, 그 좌표랑 현재 y,x가 일치하면 플레이어 전용 색상으로 표시
                    if (y == _player.PosY && x == _player.PosX)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if (y == DestY && x == DestX)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);

                    Console.Write(CIRCLE + " ");
                }
                Console.WriteLine();
            }

            //랜더링을 하게돼도 이전 상태에 영향을 주지 않기 위해 복원
            Console.ForegroundColor = prevColor;
        }

        ConsoleColor GetTileColor(TileType type)
        {
            switch (type)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.Wall:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Green;
            }
        }


    }
}
