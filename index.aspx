<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Puzzle.Game.Client.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Puzzle</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="puzzle-wrapper" id="puzzle-board">
        </div>

        <script type="text/javascript">

            function createPuzzle(imageAdrres) {

                var puzzleBoard = "<%=_puzzleBoard%>";  //"[1,4,6,7,2,3,5,8,0]"; CreatePuzzleBoard methodundan dönen random list
                var rows = 3;
                var cols = 3;
                var numCells = rows * cols - 1;
                var board_left = 0;
                var board_top = 0;
                var board_width = 270;
                var board_height = 270;
                var width = board_width / cols;
                var height = board_height / rows;
                var spacing = 0;
                var slide_interval = 20;
                var boardState = [];
                var emptyRow, emptyCol;
                //var degisken1 = make_random_board(rows, cols);
                var degisken2 = listToMatrix(puzzleBoard, rows);
                empty_row = parseInt(puzzleBoard.indexOf((rows * cols) - 1) / rows);
                empty_col = puzzleBoard.indexOf((rows * cols) - 1) % rows;
                var selectElement = document['getElementById']('puzzle-board');
                var imageUrl = imageAdrres;
                $(document).on("click", "#puzzle-board div", function () {
                    objectClicked(this);
                });
                $('#puzzle-board').css({ 'width': board_width, 'height': board_height, 'position': 'relative', 'left': board_left, 'top': board_top, 'margin': '0 auto' });

                for (var i = 0; i < rows; i++) {
                    boardState['push']([]);
                    for (var j = 0; j < cols; j++) {
                        var degisken4 = degisken2[i][j];
                        if (degisken4 == numCells) {
                            emptyRow = i;
                            emptyCol = j;
                            boardState[i]['push'](null);
                            continue;
                        };
                        var imageHeight = Math['floor'](degisken4 / cols);
                        var imageWidth = degisken4 % cols;
                        var createElem = document.createElement('div');

                        $(createElem).addClass('block');
                        $(createElem).attr('id', 'block' + degisken4);
                        $(createElem).attr('data-index', degisken4);
                        $(createElem).css({
                            'left': (j * (width + spacing)),
                            'top': (i * (height + spacing)),
                            'width': width,
                            'height': height,
                            'position': 'absolute',
                            'margin': '0px auto',
                            'background-image': 'url(' + imageUrl + ')',
                            'background-position': (-imageWidth * width) + 'px ' + (-imageHeight * height) + 'px'
                        })
                        $(createElem).appendTo(selectElement);
                        boardState[i]['push'](createElem);
                    }
                }


                function objectClicked(ob2) {
                    var i, j;
                    var ob3 = false;
                    for (i = 0; i < rows; i++) {
                        for (j = 0; j < cols; j++) {
                            if (boardState[i][j] === ob2) {
                                ob3 = true;
                                break;
                            };
                        };
                        if (ob3) {
                            break;
                        };
                    };
                    if (i == emptyRow && j - 1 == emptyCol || i == emptyRow && j + 1 == emptyCol || i - 1 == emptyRow && j == emptyCol || i + 1 == emptyRow && j == emptyCol) {
                        $("#puzzleMask").css("position", "relative").append('<div class="pzmask" style="position:absolute;top:0;left:50%;z-index:2;width:270px;height:270px;background:#ccc;opacity:0.01;margin-left:-135px;" />')
                        animate_block(ob2, i, j, emptyRow, emptyCol);
                        emptyRow = i;
                        emptyCol = j;
                    };
                }

                function animate_block(veri1, i, j, veri2, veri3) {
                    boardState[i][j] = null;
                    boardState[veri2][veri3] = veri1;
                    var an1 = j * (width + spacing);
                    var an2 = i * (height + spacing);
                    var an3 = veri3 * (width + spacing);
                    var an4 = veri2 * (height + spacing);
                    var an5 = an1;
                    var an6 = an2;
                    var an7 = 1;
                    var an8 = function () {
                        an5 += (an3 - an5) / 2;
                        an6 += (an4 - an6) / 2;
                        var an9 = Math['sqrt']((an3 - an5) * (an3 - an5) + (an4 - an6) * (an4 - an6));
                        if (an9 < 0.01) {
                            an5 = an3;
                            an6 = an4;
                            veri1['style']['left'] = String(an5) + 'px';
                            veri1['style']['top'] = String(an6) + 'px';
                        } else {
                            veri1['style']['left'] = String(an5) + 'px';
                            veri1['style']['top'] = String(an6) + 'px';
                            setTimeout(an8, slide_interval);
                        };
                    };
                    an8();

                    var _dragIndex = parseInt($(veri1).attr("data-index"));

                    $.ajax({
                        type: "POST",
                        url: "/PuzzleSelect.ashx",
                        cache: false,
                        data: { dragIndex: _dragIndex },
                        dataType: "json",
                        success: function (data) {
                            if (data == 0) {
                                alert("Kural dışı hamle yapıldı.");
                            }
                            else if (ddata == 2) {
                                //Oyun tamamlandı...
                            }
                            $("#puzzleMask .pzmask").remove();
                        },
                        error: function (error) {
                        }
                    });
                }
            }

        </script>
    </form>
</body>
</html>
