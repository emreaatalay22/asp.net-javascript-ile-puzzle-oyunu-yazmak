using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Puzzle.Game.Client
{
    public partial class Index : System.Web.UI.Page
    {
        public string _puzzleBoard = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                PuzzleGame puzzleGame = new PuzzleGame();

                var puzzleBoard = puzzleGame.CreatePuzzleBoard(3, 3);

                System.Web.Script.Serialization.JavaScriptSerializer srv = new System.Web.Script.Serialization.JavaScriptSerializer();

                _puzzleBoard = srv.Serialize(puzzleBoard.LastPuzzleBoard.Select(c => c.Value).ToList());


                Session["PuzzleBoard"] = puzzleBoard;
            }
        }
    }
}
