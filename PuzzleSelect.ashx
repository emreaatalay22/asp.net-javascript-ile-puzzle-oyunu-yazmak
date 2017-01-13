using System;
using System.Web;

namespace Puzzle.Game.Client
{
    /// <summary>
    /// Summary description for PuzzleSelect
    /// </summary>
    public class PuzzleSelect : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            PuzzleGame puzzleGame = new PuzzleGame();

            var puzzleBoard = context.Session["PuzzleBoard"] as PuzzleBoardUI;

            var dragIndex = Convert.ToInt32(context.Request["dragIndex"]);

            var result = puzzleGame.DragDropIsFinish(puzzleBoard, dragIndex);


            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
