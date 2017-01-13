using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle.Game
{
    public class PuzzleGame
    {

        /// <summary>
        /// Satır sayısı
        /// </summary>
        private int puzzleRows
        {
            get;
            set;
        }

        /// <summary>
        /// Sütun sayısı
        /// </summary>
        private int puzzleCols
        {
            get;
            set;
        }

        /// <summary>
        /// Yapılan hamle sayısı bilgisini döner.
        /// </summary>
        public int counter
        {
            get;
            set;
        }

        /// <summary>
        /// Boş olacak index bilgisi
        /// </summary>
        private int emptySquare
        {
            get;
            set;
        }


        private Random rng = new Random();

        /// <summary>
        ///Satır ve sütun sayısı boyutunda puzzle tablosunu oluşturur.
        ///1. tablo index ile value eşit
        ///2. tablo karıştırılmış
        /// </summary>
        /// <param name="puzzleRows">Satır sayısı</param>
        /// <param name="puzzleCols">Sütun sayısı</param>
        /// <returns></returns>
        public PuzzleBoardUI CreatePuzzleBoard(int puzzleRows, int puzzleCols)
        {
            PuzzleBoardUI result = new PuzzleBoardUI();

            result.FirstPuzzleBoard = GetPuzzleGameBoard(puzzleRows, puzzleCols);
            result.LastPuzzleBoard = GetPuzzleGameBoard(puzzleRows, puzzleCols);

            List<int> list = Shuffle(puzzleRows, puzzleCols);
            int counter = 0;
            foreach (var item in list)
            {
                var entity = result.LastPuzzleBoard.Where(c => c.Index == counter).First();
                entity.Value = item;
                counter++;
            }

            //Boş olan kare
            emptySquare = (puzzleRows * puzzleCols) - 1;
            //rng.Next(1, puzzleRows * puzzleCols);

            result.LastPuzzleBoard.Where(c => c.Value == emptySquare).First().IsEmpty = true;

            result.StartDate = DateTime.Now;

            return result;
        }


        /// <summary>
        /// Puzzle oyunununda yapılan hamlelerin geçerliliği kontrol ediliyor.
        /// Yapılan her hamle sonucunda, oyun tamamlandı mı kontrolü yapılıyor.
        /// </summary>
        /// <param name="entity">CreatePuzzleBoard methodundan dönen (PuzzleBoardUI)</param>
        /// <returns>
        /// 0 => hamle geçersiz.
        /// 1 => hamle başarılı.
        /// 2 => oyun tamamlandı.
        /// </returns>
        public byte DragDropIsFinish(PuzzleBoardUI entity, int dragMoveIndex)
        {
            byte result = 0;

            bool isDrage = false;

            entity.Counter++;
            counter = entity.Counter;


            if (entity.LastPuzzleBoard.Where(c => c.Index == dragMoveIndex).Any())
            {
                var square = entity.LastPuzzleBoard.Where(c => c.Value == dragMoveIndex).First();

                var emptySquare = entity.LastPuzzleBoard.Where(c => c.IsEmpty).First();// entity.LastPuzzleBoard.Where(c => c.Index == dropMoveIndex).First();


                if (square.ListMovePosition.Where(c => c.MoveIndex == emptySquare.Index).Any())
                {
                    result = 1;
                    int value = emptySquare.Value;
                    emptySquare.Value = square.Value;
                    square.Value = value;

                    square.IsEmpty = true;
                    emptySquare.IsEmpty = false;


                }




                //var dropIndex = entity.LastPuzzleBoard.Where(c => c.Index == square.Value).First();

                //var dragIndex = entity.LastPuzzleBoard.Where(c => c.Index == dropMoveIndex).First();

                //if (dragIndex.IsEmpty && dropIndex.ListMovePosition.Where(c => c.MoveIndex == dragIndex.Value).Any())
                //{
                //    result = 1;

                //    int value = entity.LastPuzzleBoard.Where(c => c.Index == dropMoveIndex).First().Value;

                //    entity.LastPuzzleBoard.Where(c => c.Index == dropMoveIndex).First().Value = entity.LastPuzzleBoard.Where(c => c.Index == dragMoveIndex).First().Value;

                //    entity.LastPuzzleBoard.Where(c => c.Index == dragMoveIndex).First().Value = value;

                //    entity.LastPuzzleBoard.Where(c => c.Index == dragMoveIndex).First().IsEmpty = false;
                //    entity.LastPuzzleBoard.Where(c => c.Index == dropMoveIndex).First().IsEmpty = true;
                //}
            }


            if (result == 1)
            {
                bool isFinish = true;
                foreach (var item in entity.FirstPuzzleBoard)
                {
                    if (item.Index != entity.LastPuzzleBoard.Where(c => c.Value == item.Index).First().Index)
                    {
                        isFinish = false;
                        break;
                    }
                }

                if (isFinish)
                    result = 2;
            }

            return result;
        }


        /// <summary>
        /// Puzzle tablosunu oluşturuyor.
        /// </summary>
        /// <param name="matris"></param>
        /// <returns></returns>
        private List<PuzzleBoard> GetPuzzleGameBoard(int rows, int cols)
        {
            List<PuzzleBoard> puzzleBoard = new List<PuzzleBoard>();

            List<MovePosition> listMovePosition = new List<MovePosition>();

            //başlangıç index'i
            int startIndexBoard = 0;
            //bitiş index',
            int endIndexBoard = (rows * cols) - 1;

            //Sol üst kenar index id
            int leftUpEdgeIndex = 0;
            //Sağ üst kenar index id
            int rightUpEdgeIndex = cols - 1;
            //Sol alt kenar index id
            int leftDowEdgeIndex = (endIndexBoard - cols) + 1;
            //Sağ alt kenar index id
            int rightDownEdgeIndex = endIndexBoard;


            //Üst kenar
            for (int i = startIndexBoard; i < cols; i++)
            {
                if (i == leftUpEdgeIndex)
                {
                    listMovePosition = new List<MovePosition>();
                    listMovePosition.Add(new MovePosition() { MoveIndex = i + 1, MovePositions = Position.Right });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i + cols, MovePositions = Position.Down });

                    puzzleBoard.Add(new PuzzleBoard() { Index = i, Value = i, ListMovePosition = listMovePosition });
                }
                else if (i == rightUpEdgeIndex)
                {
                    listMovePosition = new List<MovePosition>();
                    listMovePosition.Add(new MovePosition() { MoveIndex = i - 1, MovePositions = Position.Left });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i + cols, MovePositions = Position.Down });
                    puzzleBoard.Add(new PuzzleBoard() { Index = i, Value = i, ListMovePosition = listMovePosition });
                }
                else
                {
                    listMovePosition = new List<MovePosition>();
                    listMovePosition.Add(new MovePosition() { MoveIndex = i - 1, MovePositions = Position.Left });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i + 1, MovePositions = Position.Right });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i + cols, MovePositions = Position.Down });
                    puzzleBoard.Add(new PuzzleBoard() { Index = i, Value = i, ListMovePosition = listMovePosition });
                }
            }

            //Alt kenar
            for (int i = leftDowEdgeIndex; i <= endIndexBoard; i++)
            {
                if (i == leftDowEdgeIndex)
                {
                    listMovePosition = new List<MovePosition>();
                    listMovePosition.Add(new MovePosition() { MoveIndex = i - cols, MovePositions = Position.Up });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i + 1, MovePositions = Position.Right });
                    puzzleBoard.Add(new PuzzleBoard() { Index = i, Value = i, ListMovePosition = listMovePosition });
                }
                else if (i == rightDownEdgeIndex)
                {
                    listMovePosition = new List<MovePosition>();
                    listMovePosition.Add(new MovePosition() { MoveIndex = i - cols, MovePositions = Position.Up });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i - 1, MovePositions = Position.Left });
                    puzzleBoard.Add(new PuzzleBoard() { Index = i, Value = i, ListMovePosition = listMovePosition });
                }
                else
                {
                    listMovePosition = new List<MovePosition>();
                    listMovePosition.Add(new MovePosition() { MoveIndex = i - 1, MovePositions = Position.Left });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i + 1, MovePositions = Position.Right });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i - cols, MovePositions = Position.Up });
                    puzzleBoard.Add(new PuzzleBoard() { Index = i, Value = i, ListMovePosition = listMovePosition });
                }
            }

            //Sol kenar
            for (int i = cols; i < leftDowEdgeIndex; i = i + cols)
            {
                listMovePosition = new List<MovePosition>();
                listMovePosition.Add(new MovePosition() { MoveIndex = i - cols, MovePositions = Position.Up });
                listMovePosition.Add(new MovePosition() { MoveIndex = i + cols, MovePositions = Position.Down });
                listMovePosition.Add(new MovePosition() { MoveIndex = i + 1, MovePositions = Position.Right });
                puzzleBoard.Add(new PuzzleBoard() { Index = i, Value = i, ListMovePosition = listMovePosition });
            }

            //Sağ kenar
            for (int i = rightUpEdgeIndex + cols; i < rightDownEdgeIndex; i = i + cols)
            {

                listMovePosition = new List<MovePosition>();
                listMovePosition.Add(new MovePosition() { MoveIndex = i - cols, MovePositions = Position.Up });
                listMovePosition.Add(new MovePosition() { MoveIndex = i - 1, MovePositions = Position.Left });
                listMovePosition.Add(new MovePosition() { MoveIndex = i + cols, MovePositions = Position.Down });
                puzzleBoard.Add(new PuzzleBoard() { Index = i, Value = i, ListMovePosition = listMovePosition });
            }

            //Orta alanlar
            for (int i = 0; i < endIndexBoard; i++)
            {
                //İndex id bilgisi puzzle tablosunda yok ise
                if (!puzzleBoard.Where(c => c.Index == i).Any())
                {
                    listMovePosition = new List<MovePosition>();
                    listMovePosition.Add(new MovePosition() { MoveIndex = i - 1, MovePositions = Position.Left });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i + 1, MovePositions = Position.Right });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i - rows, MovePositions = Position.Down });
                    listMovePosition.Add(new MovePosition() { MoveIndex = i + rows, MovePositions = Position.Up });
                    puzzleBoard.Add(new PuzzleBoard() { Index = i, Value = i, ListMovePosition = listMovePosition });
                }
            }

            return puzzleBoard.OrderBy(c => c.Index).ToList();
        }

        /// <summary>
        /// Puzzle tablosunu karıştırıyor.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        private List<int> Shuffle(int rows, int cols)
        {
            List<int> list = new List<int>();

            int[,] degisken5 = new int[rows, cols];
            for (var i = 0; i < rows; i++)
            {
                //  degisken5['push']([]);
                for (var j = 0; j < cols; j++)
                {
                    degisken5[i, j] = i * cols + j;
                };
            };
            var degisken6 = rows - 1;
            var degisken7 = cols - 1;
            for (var k = 0; k < 1000; k++)
            {
                var degisken8 = rng.Next(0, 4);

                int degisken9 = degisken6, degisken10 = degisken7;

                if (degisken8 == 0 && degisken6 > 0)
                {
                    degisken9 = degisken6 - 1;
                }
                else
                {
                    if (degisken8 == 1 && degisken6 < rows - 1)
                    {
                        degisken9 = degisken6 + 1;
                    }
                    else
                    {
                        if (degisken8 == 2 && degisken7 > 0)
                        {
                            degisken10 = degisken7 - 1;
                        }
                        else
                        {
                            if (degisken7 < cols - 1)
                            {
                                degisken10 = degisken7 + 1;
                            }
                        }
                    }
                }

                degisken5[degisken6, degisken7] = degisken5[degisken9, degisken10];
                degisken5[degisken9, degisken10] = rows * cols - 1;
                degisken6 = degisken9;
                degisken7 = degisken10;
            }

            //return [degisken5, degisken6, degisken7];

            foreach (var item in degisken5)
                list.Add(item);

            return list;


        }
    }

    /// <summary>
    /// Puzzle tablosu
    /// </summary>
    public class PuzzleBoardUI
    {
        /// <summary>
        /// İlk oluşturulan puzzle tablosu
        /// </summary>
        public List<PuzzleBoard> FirstPuzzleBoard { get; set; }

        /// <summary>
        /// Karıştırılan puzzle tablosu
        /// </summary>
        public List<PuzzleBoard> LastPuzzleBoard { get; set; }

        public int Counter { get; set; }

        public DateTime? StartDate { get; set; }
    }

    public class PuzzleBoard
    {
        public int Index { get; set; }

        public int Value { get; set; }

        public bool IsEmpty { get; set; }

        public List<MovePosition> ListMovePosition { get; set; }
    }

    public class MovePosition
    {
        public int MoveIndex { get; set; }

        public Position MovePositions { get; set; }

    }

    public enum Position
    {
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4
    }
}
