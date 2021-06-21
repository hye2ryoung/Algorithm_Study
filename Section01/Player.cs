using System;
using System.Collections.Generic;

namespace Section01
{
    class Pos
    {
        public Pos(int y, int x) { Y = y; X = x; }
        public int Y;
        public int X;
    }

    class Player
    {
        public int PosY { get; private set; }
        public int PosX { get; private set; }
        Random _random = new Random();

        Board _board;

        enum Dir
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }

        int _dir = (int)Dir.Up;
        List<Pos> _points = new List<Pos>();

        //초기정보
        public void Initialize(int posY, int posX, Board board)
        {
            PosY = posY;
            PosX = posX;
            _board = board;
            //RightHand();

            //BFS(); //BFS를 이용한 길찾기

            AStar();
        }

        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                    return 0;
                return F < other.F ? 1 : -1;
            }
        }

        void AStar()
        {
                                    //상 좌 하 우    
            int[] deltaY = new int[] { -1, 0, 1, 0,};
            int[] deltaX = new int[] { 0, -1, 0, 1 };
            int[] cost = new int[] { 10, 10, 10, 10};


            //점수 매기기
            //F = G + H
            //F = 최종점수 (작을수록 좋음, 경로에따라 달라짐)
            //G = 시작점에서 해당 좌표까지 이동하는데 드는 비용(작을수록 좋음, 경로에 따라 달라짐)
            //H = 목적지에서 얼마나 가까운지 (작을 수록 좋음, 고정)

            //(y, x) 이미 방문했는지 여부 기록 (방문 = closed 상태)
            bool[,] closed = new bool[_board.Size, _board.Size]; //CloseList

            //(y,x) 가는 길을 한번이라도 발견했는지
            //발견X => MaxValue
            //발견O => F = G + H
            int[,] open = new int[_board.Size, _board.Size]; //OpenList
            for (int y = 0; y < _board.Size; y++)
                for (int x = 0; x < _board.Size; x++)
                    open[y, x] = Int32.MaxValue; //초기값

            Pos[,] parent = new Pos[_board.Size, _board.Size];

            //오픈리스트에 있는 정보들 중에서, 가장 좋은 후보를 빠르게 뽑아오기 위한 도구
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

            // 시작점 발견 (예약 진행)
            // 절대값 Abs
            open[PosY, PosX] = 10 * (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX));
            pq.Push(new PQNode() { F = 10 * (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX)), G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while(pq.Count>0)
            {
                //제일 좋은 후보를 찾는다
                PQNode node = pq.Pop();
                //동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문(closed)된 경우 스킵
                if (closed[node.Y, node.X])
                    continue;
                //방문한다
                closed[node.Y, node.X] = true;
                //목적지에 도착했으면 바로 종료
                if (node.Y == _board.DestY && node.X == _board.DestX)
                    break;
                //상하좌우 등 이동할 수 있는 좌표인지 확인해서 예약(open)한다
                for (int i = 0; i < deltaY.Length; i++)
                {
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    // 유효 범위를 벗어났으면 스킵
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    // 벽으로 막혀서 갈 수 없으면 스킵
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    // 이미 방문한 곳이면 스킵
                    if (closed[nextY, nextX])
                        continue;

                    // 비용 계산
                    int g = node.G + cost[i];
                    int h = 10 * (Math.Abs(_board.DestY - nextY) + Math.Abs(_board.DestX - nextX));
                    // 다른 경로에서 더 빠른 길 이미 찾았으면 스킵
                    if (open[nextY, nextX] < g + h)
                        continue;

                    // 예약 진행
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);
                }
            }

            CalcPathFromParent(parent);
        }

        void CalcPathFromParent(Pos[,] parent)
        {

            int y = _board.DestY;
            int x = _board.DestX;
            while (parent[y, x].Y != y || parent[y, x].X != x)
            {
                //부모 좌표로 타고 올라가서 시작점이 나올 때 까지 역추적
                _points.Add(new Pos(y, x));
                Pos pos = parent[y, x];
                y = pos.Y;
                x = pos.X;
            }
            _points.Add(new Pos(y, x)); //while문에서 빠져나온 후 예외적으로 시작점만 따로 경로 List에 추가
            _points.Reverse(); // List를 뒤집는다
        }

        void RightHand()
        {
            // 현재 바라보고 있는 방향을 기준으로, 좌표 변화를 나타낸다
            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };
            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

            _points.Add(new Pos(PosY, PosX));
            // 목적지 도착하기 전에는 계속 실행
            while (PosY != _board.DestY || PosX != _board.DestX)
            {
                // 1. 현재 바라보는 방향을 기준으로 오른쪽으로 갈 수 있는지 확인.
                if (_board.Tile[PosY + rightY[_dir], PosX + rightX[_dir]] == Board.TileType.Empty)
                {
                    // 오른쪽 방향으로 90도 회전
                    _dir = (_dir - 1 + 4) % 4;
                    // 앞으로 한 보 전진.
                    PosY = PosY + frontY[_dir];
                    PosX = PosX + frontX[_dir];
                    _points.Add(new Pos(PosY, PosX));
                }
                // 2. 현재 바라보는 방향을 기준으로 전진할 수 있는지 확인.
                else if (_board.Tile[PosY + frontY[_dir], PosX + frontX[_dir]] == Board.TileType.Empty)
                {
                    // 앞으로 한 보 전진.
                    PosY = PosY + frontY[_dir];
                    PosX = PosX + frontX[_dir];
                    _points.Add(new Pos(PosY, PosX));
                }
                else
                {
                    // 왼쪽 방향으로 90도 회전
                    _dir = (_dir + 1 + 4) % 4;
                }
            }
        }

        void BFS()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 }; //상 좌 하 우
            int[] deltaX = new int[] { 0, -1, 0, 1 };

            bool[,] found = new bool[_board.Size, _board.Size];
            Pos[,] parent = new Pos[_board.Size, _board.Size];

            Queue<Pos> q = new Queue<Pos>();
            q.Enqueue(new Pos(PosY, PosX)); //정점 예약
            found[PosY, PosX] = true; // 찾았다고 인식
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (q.Count > 0)
            {
                Pos pos = q.Dequeue(); //예약한 정점 정보를 추출
                int nowY = pos.Y;
                int nowX = pos.X;

                for (int i = 0; i < 4; i++)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue; //예외처리
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue; //탐색할 점이 벽에막혀있으면 스킵
                    if (found[nextY, nextX])
                        continue; //이미 발견한 점이었다면 스킵

                    q.Enqueue(new Pos(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowY, nowX);

                }
            }

            CalcPathFromParent(parent);


            //역산 하는 이유
            //BFS는 처음부터 목적지로 향한 최단거리 길로 향해서 서칭한다는 보장이 없다.
            //목적지에 도착했을 때의 최단거리를 추출하고 싶기 때문에 역산 과정이 필요하다.
            //최종점을 어느 곳으로 바꾸더라도 BFS가 끝나고 나면 길이 있는지, 최단거리는 무엇인지 답은 무조건 나오게 된다.
        }

        const int MOVE_TICK = 100;
        int _sumTick = 0;
        int _lastIndex = 0;
        public void Update(int deltaTick)
        {
            if (_lastIndex >= _points.Count)
            {
                //길찾기를 끝낼때마다 map을 업데이트 (무제한)
                _lastIndex = 0;
                _points.Clear();
                _board.Initialize(_board.Size, this);
                Initialize(1, 1, _board);
            }
                


            _sumTick += deltaTick;
            if (_sumTick >= MOVE_TICK)
            {
                _sumTick = 0;

                PosY = _points[_lastIndex].Y;
                PosX = _points[_lastIndex].X;
                _lastIndex++;


            }
        }

    }
}
