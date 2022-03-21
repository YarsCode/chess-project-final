using System;

namespace ChessProjectFixed
{
    class ChessGame
    {
        static void Main(string[] args)
        {
            new ChessGame().play();
        }
        ChessPieces[,] chessBoard = new ChessPieces[8, 8] {
            { new Rook(false), new Knight(false), new Bishop(false), new Queen(false), new King(false), new Bishop(false), new Knight(false), new Rook(false)},
            { new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false)},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            { new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true)},
            { new Rook(true), new Knight(true), new Bishop(true), new Queen(true), new King(true), new Bishop(true), new Knight(true), new Rook(true)}
        };
        /*ChessPieces[,] chessBoard = new ChessPieces[8, 8] {        // Helps with the debugging of the Stalemate situation
            { new EmptyPiece(), new EmptyPiece(), new Queen(true), new EmptyPiece(), new EmptyPiece(), new Bishop(false), new Knight(false), new Rook(false)},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new Pawn(false), new EmptyPiece(), new Pawn(false), new Queen(false)},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new Pawn(false), new King(false), new Rook(false)},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new Pawn(false)},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new Pawn(true)},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            { new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true)},
            { new Rook(true), new Knight(true), new Bishop(true), new Queen(true), new King(true), new Bishop(true), new Knight(true), new Rook(true)}
        };*/
        /*ChessPieces[,] chessBoard = new ChessPieces[8, 8] {        // Helps with the debugging of the insufficient material situation
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new Bishop(false), new EmptyPiece(), new EmptyPiece()},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new King(false), new EmptyPiece()},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new Pawn(false), new Pawn(false), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new King(true), new EmptyPiece(), new Knight(true), new EmptyPiece()}
        };*/
        protected bool isWhiteKingUnderCheck, isBlackKingUnderCheck;
        bool overByMate, overByStalemate, overByThreefold, overByFiftyMoves, overByInsufficientMaterial;
        int fiftyMovesCount, boardsCount, numOfPiecesOnBoard;
        ChessPieces[,,] storeBoardCopies = new ChessPieces[50, 8, 8];
        public ChessGame(ChessPieces[,] chessBoard)
        {
            Array.Copy(chessBoard, this.chessBoard, 64);
        }
        public ChessGame() { }
        public void play()
        {
            welcomeMessage();
            string input;
            bool isWhiteTurn = true, legalMove, gameOver = false;
            Location fromLocation = null, toLocation = null, whiteKingLocation = null, blackKingLocation = null;
            Move move = null;
            printBoard();
            copyBoardFunc();
            while (!gameOver)
            {
                legalMove = false;
                Console.Write((isWhiteTurn ? "White" : "Black") + " Player's Turn: ");
                while (!legalMove)
                {
                    input = isValidInput();
                    fromLocation = new Location(getRowFromUserInput(input[1]), getColumnFromUserInput(input[0])); // Sets the current location of the piece
                    toLocation = new Location(getRowFromUserInput(input[3]), getColumnFromUserInput(input[2]));  // Sets the destinated location of the piece
                    move = new Move(fromLocation, toLocation);
                    legalMove = chessBoard[fromLocation.row, fromLocation.column].isLegalMove(move, isWhiteTurn, chessBoard, false);
                    if (legalMove) // If move is legal ---> make the move
                    {
                        numOfPiecesOnBoard = getNumOfPiecesOnBoard(); // Counts number of pieces on the board for the 50 moves rule
                        move.makeMove(move, chessBoard); // Makes move
                        whiteKingLocation = getKingLocation(true);
                        blackKingLocation = getKingLocation(false);
                        if ((isWhiteTurn && isCheck(whiteKingLocation, false)) || (!isWhiteTurn && isCheck(blackKingLocation, true))) // Checks if the current player didn't open himself to a check
                        {
                            Array.Copy(move.undoMove(), chessBoard, 64);
                            Console.WriteLine("Illegal move. Please try again");
                            legalMove = false;
                        }
                    }
                    else
                        Console.WriteLine("Illegal move. Please try again");
                }
                if (chessBoard[move.toLocation.row, move.toLocation.column] is Pawn) // Pawn promotion
                {
                    if ((isWhiteTurn && move.toLocation.row == 0) || (!isWhiteTurn && move.toLocation.row == 7))
                        pawnPromotion(move, isWhiteTurn);                    
                }
                else if (getNumOfPiecesOnBoard() == numOfPiecesOnBoard)
                    copyBoardFunc();
                emptyPieceAfterEnPassant(isWhiteTurn);
                isWhiteKingUnderCheck = isCheck(whiteKingLocation, false);
                isBlackKingUnderCheck = isCheck(blackKingLocation, true);
                checkMessage(isWhiteKingUnderCheck, isBlackKingUnderCheck);
                gameOver = hasGameEnded(whiteKingLocation, blackKingLocation, move, numOfPiecesOnBoard);
                printBoard();
                isWhiteTurn = switchToNextTurn(isWhiteTurn);
            }
            gameOverMessages();
        }
        public ChessPieces[,] getChessBoard()
        {
            return this.chessBoard;
        }
        public void copyBoardFunc()
        {
            for (int i = 0; i < 50; i++)
            {
                if (storeBoardCopies[i, 0, 0] == null)
                {
                    boardsCount = i;
                    break;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    storeBoardCopies[boardsCount, i, j] = chessBoard[i, j].copy();
                }
            }
        }
        public void welcomeMessage()
        {
            Console.WriteLine("Hello and welcome to the chess game!");
            Console.WriteLine();
            Console.WriteLine("To move a chess piece please first type (in capital letter) the location of the tile in which the piece is located, and then type in lowercase letter the new location");
        }
        public void gameOverMessages()
        {
            if (overByMate)
                Console.WriteLine("Checkmate! " + (isBlackKingUnderCheck ? "White" : "Black") + " player wins!");
            else if (overByStalemate)
                Console.WriteLine("Game Over. Stalemate!");
            else if (overByFiftyMoves)
                Console.WriteLine("Game Over. Draw by 50 Moves Rule!");
            else if (overByInsufficientMaterial)
                Console.WriteLine("Game Over. Draw by Insufficient Material!");
            else if (overByThreefold)
                Console.WriteLine("Game Over. Draw by Threefold Repetition!");
        }
        public void checkMessage(bool isWhiteKingUnderCheck, bool isBlackKingUnderCheck)
        {
            if (isWhiteKingUnderCheck)
                Console.WriteLine("White king is under Check");
            if (isBlackKingUnderCheck)
                Console.WriteLine("Black king is under Check");
        }
        public void printBoard()
        {
            Console.WriteLine();
            Console.WriteLine("    A   B   C   D   E   F   G   H");
            Console.WriteLine("  ---------------------------------");
            Console.WriteLine();
            for (int i = 0, rows = 8; i < 8; i++, rows--)
            {
                Console.Write(rows + "| ");
                for (int j = 0; j < 8; j++)
                {
                    if (j == 7)
                        Console.Write(chessBoard[i, j].ToString());
                    else
                        Console.Write(chessBoard[i, j].ToString() + "  ");
                }
                Console.Write(" |" + rows);
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine("  ---------------------------------");
            Console.WriteLine("   A   B   C   D   E   F   G   H");
            Console.WriteLine();
        }
        public bool switchToNextTurn(bool isWhitePlayerTurn)
        {
            return !isWhitePlayerTurn;
        }
        public string isValidInput()
        {
            string input = Console.ReadLine();
            bool isValid = false;
            while (!isValid)
            {
                isValid = true;
                input = input.Trim();
                if (input.Length != 4)
                {
                    Console.WriteLine("Invalid input. Please insert a valid input and press ENTER");
                    input = Console.ReadLine();
                    isValid = false;
                    continue;
                }
                string checkLetters, checkNumbers;
                checkLetters = "ABCDEFGH";
                checkNumbers = "12345678";
                bool legalChar1 = false, legalChar2 = false, legalChar3 = false, legalChar4 = false;
                for (int firstChar = 0; !legalChar1 && firstChar < 8; firstChar++)
                {
                    if (input[0] == checkLetters[firstChar])
                        legalChar1 = true;
                }
                for (int secondChar = 0; legalChar1 && !legalChar2 && secondChar < 8; secondChar++)
                {
                    if (input[1] == checkNumbers[secondChar])
                        legalChar2 = true;
                }
                for (int thirdChar = 0; legalChar2 && !legalChar3 && thirdChar < 8; thirdChar++)
                {
                    if (input[2] == checkLetters.ToLower()[thirdChar])
                        legalChar3 = true;
                }
                for (int fourthChar = 0; legalChar3 && !legalChar4 && fourthChar < 8; fourthChar++)
                {
                    if (input[3] == checkNumbers[fourthChar])
                        legalChar4 = true;
                }
                if (!legalChar1 || !legalChar2 || !legalChar3 || !legalChar4)
                {
                    isValid = false;
                    Console.WriteLine("Invalid input. Please insert a valid input and press ENTER");
                    input = Console.ReadLine();
                }
            }
            return input;
        }
        public int getRowFromUserInput(char rowIndex)
        {
            int row = 0;
            switch (rowIndex)
            {
                case '1':
                    row = 7;
                    break;
                case '2':
                    row = 6;
                    break;
                case '3':
                    row = 5;
                    break;
                case '4':
                    row = 4;
                    break;
                case '5':
                    row = 3;
                    break;
                case '6':
                    row = 2;
                    break;
                case '7':
                    row = 1;
                    break;
                case '8':
                    row = 0;
                    break;
            }
            return row;
        }
        public int getColumnFromUserInput(char columnIndex)
        {
            columnIndex = char.ToUpper(columnIndex);
            int column = 0;
            switch (columnIndex)
            {
                case 'A':
                    column = 0;
                    break;
                case 'B':
                    column = 1;
                    break;
                case 'C':
                    column = 2;
                    break;
                case 'D':
                    column = 3;
                    break;
                case 'E':
                    column = 4;
                    break;
                case 'F':
                    column = 5;
                    break;
                case 'G':
                    column = 6;
                    break;
                case 'H':
                    column = 7;
                    break;
            }
            return column;
        }
        public int getNumOfPiecesOnBoard()
        {
            int currentNumOfPiecesOnBoard = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (chessBoard[i, j].ToString() != "EE")
                        currentNumOfPiecesOnBoard++;
                }
            }
            return currentNumOfPiecesOnBoard;
        }
        public void pawnPromotion(Move move, bool isWhiteColor)
        {
            ChessGame cancelPromotionIfCheckingCheck = new ChessGame();
            bool illegalInput = true;
            while (illegalInput)
            {
                Console.Write("Which piece would you like? Choose between: Q/R/N/B - ");
                string choice = Console.ReadLine();
                illegalInput = false;
                switch (choice)
                {
                    case "Q":
                        chessBoard[move.toLocation.row, move.toLocation.column] = new Queen(isWhiteColor);
                        break;
                    case "R":
                        chessBoard[move.toLocation.row, move.toLocation.column] = new Rook(isWhiteColor);
                        break;
                    case "N":
                        chessBoard[move.toLocation.row, move.toLocation.column] = new Knight(isWhiteColor);
                        break;
                    case "B":
                        chessBoard[move.toLocation.row, move.toLocation.column] = new Bishop(isWhiteColor);
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please insert a valid input and press ENTER");
                        illegalInput = true;
                        break;
                }
            }
        }
        public bool isShortCastleLegal(Move move, bool isWhiteTurn, ChessPieces[,] board)
        {
            Location fromTile = move.fromLocation;
            if (isCheck(getKingLocation(isWhiteTurn), !isWhiteTurn))
                return false;
            if (isWhiteTurn && board[7, 7].getHasRightRookMoved())
                return false;
            if (!isWhiteTurn && board[0, 7].getHasRightRookMoved())
                return false;
            if (isWhiteTurn && (!(board[7, 5] is EmptyPiece && board[7, 6] is EmptyPiece)))
                return false;
            if (!isWhiteTurn && (!(board[0, 5] is EmptyPiece && board[0, 6] is EmptyPiece)))
                return false;
            for (int i = 0, index = 5; i < 2; i++, index++)
            {
                move = new Move(fromTile, new Location(fromTile.row, index));
                move.makeMove(move, board);
                if (isCheck(getKingLocation(isWhiteTurn), isWhiteTurn))
                    return false;
                Array.Copy(move.undoMove(), board, 64);
            }
            move = new Move(new Location(move.fromLocation.row, 7), new Location(move.fromLocation.row, 5));
            move.makeMove(move, board);
            return true;
        }
        public bool isLongCastleLegal(Move move, bool isWhiteTurn, ChessPieces[,] board)
        {
            Location fromTile = move.fromLocation;
            if (isCheck(getKingLocation(isWhiteTurn), !isWhiteTurn))
                return false;
            if (isWhiteTurn && board[7, 0].getHasLeftRookMoved())
                return false;
            if (!isWhiteTurn && board[0, 0].getHasLeftRookMoved())
                return false;
            if (isWhiteTurn && (!(board[7, 3] is EmptyPiece && board[7, 2] is EmptyPiece && board[7, 1] is EmptyPiece)))
                return false;
            if (!isWhiteTurn && (!(board[0, 3] is EmptyPiece && board[0, 2] is EmptyPiece && board[0, 1] is EmptyPiece)))
                return false;
            for (int i = 0, index = 4; i < 3; i++, index--)
            {
                move = new Move(fromTile, new Location(fromTile.row, index));
                move.makeMove(move, board);
                if (isCheck(getKingLocation(isWhiteTurn), isWhiteTurn))
                    return false;
                Array.Copy(move.undoMove(), board, 64);
            }
            move = new Move(new Location(move.fromLocation.row, 0), new Location(move.fromLocation.row, 3));
            move.makeMove(move, board);
            return true;
        }
        public bool hasGameEnded(Location whiteKingLocation, Location blackKingLocation, Move pieceType, int numOfPiecesOnBoardInPreviousMove)
        {
            if (isMate(whiteKingLocation, true, false) || isMate(blackKingLocation, false, false)) // Checks if there is mate
            {
                overByMate = true;
                return true;
            }
            if (isStalemate(whiteKingLocation, false) || isStalemate(blackKingLocation, true))
            {
                overByStalemate = true;
                return true;
            }
            if (isDrawByFiftyMovesRule(pieceType, numOfPiecesOnBoardInPreviousMove))
            {
                overByFiftyMoves = true;
                return true;
            }
            if (isDrawByInsufficientMaterial())
            {
                overByInsufficientMaterial = true;
                return true;
            }
            if (isDrawThreefoldRepetition(pieceType))
            {
                overByThreefold = true;
                return true;
            }
            return false;
        }
        public bool isMate(Location kingLocation, bool isWhiteTurn, bool onlyForStalemate)
        {
            bool mate;
            Move move;
            Location fromTile = new Location(0, 0);
            Location toTile = new Location(0, 0);
            if (!onlyForStalemate) // Ignores the "isCheck" ONLY if checking for stalemate
            {
                if (!isCheck(kingLocation, !isWhiteTurn))
                    return false;
            }
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    for (int toRow = 0; toRow < 8; toRow++)
                    {
                        for (int toColumn = 0; toColumn < 8; toColumn++)
                        {
                            fromTile.row = row;
                            fromTile.column = column;
                            toTile.row = toRow;
                            toTile.column = toColumn;
                            move = new Move(fromTile, toTile);
                            if (chessBoard[row, column].isLegalMove(move, isWhiteTurn, chessBoard, onlyForStalemate))
                            {
                                move.makeMove(move, chessBoard);
                                mate = isCheck(getKingLocation(isWhiteTurn), !isWhiteTurn);
                                Array.Copy(move.undoMove(), chessBoard, 64);
                                if (!mate)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        public Location getKingLocation(bool isWhiteColor)
        {
            Location kingLocation = null;
            int row, column;
            for (row = 0; row < 8; row++)
            {
                for (column = 0; column < 8; column++)
                {
                    if (getChessBoard()[row, column] is King && getChessBoard()[row, column].getIsWhiteColor() == isWhiteColor)
                    {
                        kingLocation = new Location(row, column);
                        return kingLocation;
                    }
                }
            }
            return kingLocation;
        }
        public bool isCheck(Location kingLocation, bool isWhiteTurn)
        {
            Move move;
            Location fromTile = new Location(0, 0);
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    fromTile.row = row;
                    fromTile.column = column;
                    move = new Move(fromTile, kingLocation);
                    if (getChessBoard()[row, column].isLegalMove(move, isWhiteTurn, getChessBoard(), false))
                        return true;
                }
            }
            return false;
        }
        public bool isStalemate(Location kingLocation, bool isWhiteTurn)
        {
            if (isMate(kingLocation, isWhiteTurn, true))
                return true;
            return false;
        }
        public bool isDrawByFiftyMovesRule(Move pieceType, int numOfPiecesOnBoardInPreviousMove)
        {
            int currentNumOfPiecesOnBoard = getNumOfPiecesOnBoard();
            if (!(chessBoard[pieceType.toLocation.row, pieceType.toLocation.column] is Pawn))
            {
                if (currentNumOfPiecesOnBoard == numOfPiecesOnBoardInPreviousMove)
                    fiftyMovesCount++;
                else
                    fiftyMovesCount = 0;
            }
            else
                fiftyMovesCount = 0;
            if (fiftyMovesCount == 50)
                return true;
            return false;
        }
        public bool isDrawByInsufficientMaterial()
        {
            int bishopAndKnightCounter = 0, otherPiecesCounter = 0;
            if (getNumOfPiecesOnBoard() <= 4)
            {
                for (int i = 0; otherPiecesCounter == 0 && i < 8; i++)
                {
                    for (int j = 0; otherPiecesCounter == 0 && j < 8; j++)
                    {
                        if (chessBoard[i, j] is Knight || chessBoard[i, j] is Bishop)
                            bishopAndKnightCounter++;
                        else if (!(chessBoard[i, j] is King) && (chessBoard[i, j].ToString() != "EE"))
                            otherPiecesCounter++;
                    }
                }
                if (bishopAndKnightCounter < 3 && otherPiecesCounter == 0)
                    return true;
            }
            return false;
        }
        public bool isDrawThreefoldRepetition(Move move)
        {
            if (!(chessBoard[move.toLocation.row, move.toLocation.column] is Pawn))
            {
                int threefoldCounter = 0;
                bool isSameBoardFound = true;
                for (int previousBoards = 0; previousBoards < boardsCount && isSameBoardFound; previousBoards++)
                {
                    for (int row = 0; row < 8 && isSameBoardFound; row++)
                    {
                        for (int col = 0; col < 8 && isSameBoardFound; col++)
                        {
                            if (chessBoard[row, col].ToString() != storeBoardCopies[previousBoards, row, col].ToString())
                                isSameBoardFound = false;
                        }
                    }
                    if (isSameBoardFound)
                        threefoldCounter++;
                    isSameBoardFound = true;
                }
                if (threefoldCounter == 3)
                    return true;
            }
            return false;
        }
        public void emptyPieceAfterEnPassant(bool isWhiteTurn)
        {
            Location location = new Location();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (isWhiteTurn && (!chessBoard[i, j].getIsWhiteColor()))
                    {
                        chessBoard[i, j].setEnPassantAvailable(false);
                    }
                    else if (!isWhiteTurn && chessBoard[i, j].getIsWhiteColor())
                        chessBoard[i, j].setEnPassantAvailable(false);
                    if (chessBoard[i, j].isCapturedByEnPassant())
                    {
                        location = chessBoard[i, j].getEnPassantLocation();
                        chessBoard[location.row, location.column] = new EmptyPiece();
                    }
                }
            }
        }
    }
    class ChessPieces
    {
        bool isWhiteColor;
        public ChessPieces(bool isWhiteColor)
        {
            this.isWhiteColor = isWhiteColor;
        }
        public ChessPieces() { }
        public bool getIsWhiteColor()
        {
            return this.isWhiteColor;
        }
        public virtual bool isLegalMove(Move move, bool isWhiteTurn, ChessPieces[,] board, bool OnlyForStalemate)
        {
            if (board[move.fromLocation.row, move.fromLocation.column].getIsWhiteColor() != isWhiteTurn) // Checks that the player is not trying to move the opposite team's pieces
                return false;
            if ((board[move.toLocation.row, move.toLocation.column].getIsWhiteColor() == isWhiteTurn) && (!(board[move.toLocation.row, move.toLocation.column] is EmptyPiece))) // Checks that the player is not trying to capture his own pieces
                return false;
            if (move.fromLocation == move.toLocation)
                return false;
            return true;
        }
        public virtual bool isEnPassantAvailable()
        {
            return false;
        }
        public virtual bool isCapturedByEnPassant()
        {
            return false;
        }
        public virtual Location getEnPassantLocation() { return null; }
        public virtual bool getHasRightRookMoved() { return true; }
        public virtual bool getHasLeftRookMoved() { return true; }
        public virtual void setEnPassantAvailable(bool isEnPassantAvailable) { }
        public virtual ChessPieces copy()
        {
            return this;
        }
    }
    class Pawn : ChessPieces
    {
        bool enPassantAvailable, capturedByEnPassant;
        public Location locationOfCapturedPiece = new Location();
        public Pawn(bool isWhiteColor) : base(isWhiteColor) { }
        public override string ToString()
        {
            return (getIsWhiteColor() ? "W" : "B") + "P";
        }
        public override bool isLegalMove(Move move, bool isWhiteTurn, ChessPieces[,] board, bool OnlyForStalemate)
        {
            if (!base.isLegalMove(move, isWhiteTurn, board, false))
                return false;
            int firstMove = Math.Abs(move.fromLocation.row - move.toLocation.row);
            if ((isWhiteTurn && move.fromLocation.row - move.toLocation.row < 0) || (!isWhiteTurn && move.fromLocation.row - move.toLocation.row > 0)) // Check if pawn doesn't go backwards
                return false;
            if ((move.fromLocation.row == 1 || move.fromLocation.row == 6) && firstMove > 2) // First move of the pawn allows moving foreward 2 tiles
                return false;
            if ((move.fromLocation.row != 6 && move.fromLocation.row != 1) && firstMove > 1) // If not first move of the pawn, it's only allowed to move 1 tile at a time
                return false;
            if (firstMove == 2)
            {
                if (!(isWhiteTurn && board[move.toLocation.row + 1, move.toLocation.column] is EmptyPiece) && (!(!isWhiteTurn && board[move.toLocation.row - 1, move.toLocation.column] is EmptyPiece))) // If it's the first move - makes sure it doesn't go through another piece
                    return false;
            }
            if (move.fromLocation.column == move.toLocation.column && (!(board[move.toLocation.row, move.toLocation.column] is EmptyPiece))) // Checks if not capturing piece while moving foreward
                return false;
            if (move.fromLocation.row == move.toLocation.row) // Makes sure the pawn doesn't move horizontally
                return false;
            if (move.fromLocation.column != move.toLocation.column) // Check if trying to capture other piece
            {
                if (Math.Abs(move.fromLocation.column - move.toLocation.column) > 1 || Math.Abs(move.fromLocation.row - move.toLocation.row) > 1) // Makes sure the pawn doesn't move diagonally more than 1 row or 1 column
                    return false;
                if (board[move.toLocation.row, move.toLocation.column] is EmptyPiece) // Makes sure the pawn doesn't go diagonally without capturing another piece in the process
                {
                    if (isEnPassantMoveLegal(move, isWhiteTurn, board))
                        return true;
                    return false;
                }
            }
            if (firstMove == 2)
                enPassantAvailable = true;
            return true;
        }
        public bool isEnPassantMoveLegal(Move move, bool isWhiteTurn, ChessPieces[,] board)
        {
            if (isWhiteTurn && ((move.toLocation.row + 1) < 7))
            {
                if (board[move.toLocation.row + 1, move.toLocation.column].getIsWhiteColor() != isWhiteTurn)
                {
                    if (board[move.toLocation.row + 1, move.toLocation.column].isEnPassantAvailable())
                    {
                        locationOfCapturedPiece.row = move.toLocation.row + 1;
                        locationOfCapturedPiece.column = move.toLocation.column;
                        capturedByEnPassant = true;
                        return true;
                    }
                }
            }
            if (!isWhiteTurn && ((move.toLocation.row - 1) > 0))
            {
                if (board[move.toLocation.row - 1, move.toLocation.column].getIsWhiteColor() != isWhiteTurn)
                {
                    if (board[move.toLocation.row - 1, move.toLocation.column].isEnPassantAvailable())
                    {
                        locationOfCapturedPiece.row = move.toLocation.row - 1;
                        locationOfCapturedPiece.column = move.toLocation.column;
                        capturedByEnPassant = true;
                        return true;
                    }
                }
            }
            return false;
        }
        public override bool isEnPassantAvailable()
        {
            return enPassantAvailable;
        }
        public override void setEnPassantAvailable(bool isEnPassantAvailable)
        {
            this.enPassantAvailable = isEnPassantAvailable;
        }
        public override bool isCapturedByEnPassant()
        {
            return capturedByEnPassant;
        }
        public override Location getEnPassantLocation()
        {
            return locationOfCapturedPiece;
        }
        public override ChessPieces copy()
        {
            return this;
        }
    }
    class Rook : ChessPieces
    {
        bool hasRightRookMoved, hasLeftRookMoved;
        public Rook(bool isWhiteColor) : base(isWhiteColor) { }
        public override string ToString()
        {
            return (getIsWhiteColor() ? "W" : "B") + "R";
        }
        public override bool isLegalMove(Move move, bool isWhiteTurn, ChessPieces[,] board, bool OnlyForStalemate)
        {
            if (!base.isLegalMove(move, isWhiteTurn, board, false))
                return false;
            if ((move.fromLocation.row != move.toLocation.row) && (move.fromLocation.column != move.toLocation.column))
                return false;
            if (!isAnyPieceBlockingPath(move, isWhiteTurn, board))
                return false;
            if (!OnlyForStalemate && move.fromLocation.column == 7)
                hasRightRookMoved = true;
            if (!OnlyForStalemate && move.fromLocation.column == 0)
                hasLeftRookMoved = true;
            return true;
        }
        public bool isAnyPieceBlockingPath(Move move, bool isWhiteTurn, ChessPieces[,] board)
        {
            int beginning;
            int end;
            if (move.fromLocation.row != move.toLocation.row) // Checks blocking pieces if moves vertically
            {
                Math.Abs(beginning = Math.Min(move.fromLocation.row, move.toLocation.row));
                Math.Abs(end = Math.Max(move.fromLocation.row, move.toLocation.row));
                for (int i = beginning + 1; i <= end - 1; i++)
                {
                    if (!(board[i, move.fromLocation.column] is EmptyPiece))
                        return false;
                }
            }
            if (move.fromLocation.column != move.toLocation.column) // Checks blocking pieces if moves horizontally
            {
                Math.Abs(beginning = Math.Min(move.fromLocation.column, move.toLocation.column));
                Math.Abs(end = Math.Max(move.fromLocation.column, move.toLocation.column));
                for (int i = beginning + 1; i <= end - 1; i++)
                {
                    if (!(board[move.fromLocation.row, i] is EmptyPiece))
                        return false;
                }
            }
            return true;
        }
        public override bool getHasRightRookMoved() { return hasRightRookMoved; }
        public override bool getHasLeftRookMoved() { return hasLeftRookMoved; }
        public override ChessPieces copy()
        {
            return this;
        }
    }
    class Bishop : ChessPieces
    {
        public Bishop(bool isWhiteColor) : base(isWhiteColor) { }
        public override string ToString()
        {
            return (getIsWhiteColor() ? "W" : "B") + "B";
        }
        public override bool isLegalMove(Move move, bool isWhiteTurn, ChessPieces[,] board, bool OnlyForStalemate)
        {
            if (!base.isLegalMove(move, isWhiteTurn, board, false))
                return false;
            int biggestRow = Math.Max(move.fromLocation.row, move.toLocation.row);
            int smallestRow = Math.Min(move.fromLocation.row, move.toLocation.row);
            int biggestColumn = Math.Max(move.fromLocation.column, move.toLocation.column);
            int smallestColumn = Math.Min(move.fromLocation.column, move.toLocation.column);
            if ((biggestRow - smallestRow) != (biggestColumn - smallestColumn)) // Checks that it is a legal diagonal move
                return false;
            if (!isAnyPieceBlockingPath(move, isWhiteTurn, board))
                return false;
            return true;
        }
        public bool isAnyPieceBlockingPath(Move move, bool isWhiteTurn, ChessPieces[,] board)
        {
            int smallestRow = Math.Min(move.fromLocation.row, move.toLocation.row);
            int biggestRow = Math.Max(move.fromLocation.row, move.toLocation.row);
            int smallestColumn = Math.Min(move.fromLocation.column, move.toLocation.column);
            if ((move.toLocation.row < move.fromLocation.row && move.toLocation.column < move.fromLocation.column) || (move.toLocation.row > move.fromLocation.row && move.toLocation.column > move.fromLocation.column)) // Check if queen moves right
            {
                for (smallestRow++, smallestColumn++; smallestRow <= biggestRow - 1; smallestRow++, smallestColumn++)
                {
                    if (!(board[smallestRow, smallestColumn] is EmptyPiece))
                        return false;
                }
            }
            else // Check if queen moves left
            {
                for (biggestRow--, smallestColumn++; biggestRow >= smallestRow + 1; biggestRow--, smallestColumn++)
                {
                    if (!(board[biggestRow, smallestColumn] is EmptyPiece))
                        return false;
                }
            }
            return true;
        }
        public override ChessPieces copy()
        {
            return this;
        }
    }
    class Knight : ChessPieces
    {
        public Knight(bool isWhiteColor) : base(isWhiteColor) { }
        public override string ToString()
        {
            return (getIsWhiteColor() ? "W" : "B") + "N";
        }
        public override bool isLegalMove(Move move, bool isWhiteTurn, ChessPieces[,] board, bool OnlyForStalemate)
        {
            if (!base.isLegalMove(move, isWhiteTurn, board, false))
                return false;
            int checkRow = Math.Abs(move.fromLocation.row - move.toLocation.row);
            int checkColumn = Math.Abs(move.fromLocation.column - move.toLocation.column);
            if (checkRow == 0 || checkRow < 1 || checkRow > 2 || checkColumn == 0 || checkColumn < 1 || checkColumn > 2) // Checks legal knight moves - combinations of 2's and 1's
                return false;
            if (checkRow == checkColumn)
                return false;
            return true;
        }
        public override ChessPieces copy()
        {
            return this;
        }
    }
    class Queen : ChessPieces
    {
        public Queen(bool isWhiteColor) : base(isWhiteColor) { }
        public override string ToString()
        {
            return (getIsWhiteColor() ? "W" : "B") + "Q";
        }
        public override bool isLegalMove(Move move, bool isWhiteTurn, ChessPieces[,] board, bool OnlyForStalemate)
        {
            if (!base.isLegalMove(move, isWhiteTurn, board, false))
                return false;
            Rook queenAsRook = new Rook(this.getIsWhiteColor());
            Bishop queenAsBishop = new Bishop(this.getIsWhiteColor());
            if (!queenAsRook.isLegalMove(move, isWhiteTurn, board, false) && !queenAsBishop.isLegalMove(move, isWhiteTurn, board, false))
                return false;
            return true;
        }
        public override ChessPieces copy()
        {
            return this;
        }
    }
    class King : ChessPieces
    {
        bool hasKingMoved;
        public King(bool isWhiteColor) : base(isWhiteColor) { }
        public override string ToString()
        {
            return (getIsWhiteColor() ? "W" : "B") + "K";
        }
        public override bool isLegalMove(Move move, bool isWhiteTurn, ChessPieces[,] board, bool OnlyForStalemate)
        {
            ChessGame isCastlingLegal = new ChessGame(board);
            if (!base.isLegalMove(move, isWhiteTurn, board, false))
                return false;
            int checkRow = Math.Abs(move.fromLocation.row - move.toLocation.row);
            int checkColumn = Math.Abs(move.fromLocation.column - move.toLocation.column);
            if (!hasKingMoved)
            {
                if (move.toLocation.column == 6 && (isWhiteTurn && move.toLocation.row == 7 || !isWhiteTurn && move.toLocation.row == 0)) // Is short castling legal for king
                {
                    if (isCastlingLegal.isShortCastleLegal(move, isWhiteTurn, board))
                    {
                        hasKingMoved = true;
                        return true;
                    }
                }
                if (move.toLocation.column == 2 && (isWhiteTurn && move.toLocation.row == 7 || !isWhiteTurn && move.toLocation.row == 0)) // Is long castling legal for white king
                {
                    if (isCastlingLegal.isLongCastleLegal(move, isWhiteTurn, board))
                    {
                        hasKingMoved = true;
                        return true;
                    }
                }
            }
            if (checkRow > 1 || checkColumn > 1) // Makes sure the king moves only 1 tile at a time
                return false;
            hasKingMoved = true;
            return true;
        }
        public override ChessPieces copy()
        {
            return this;
        }
    }
    class EmptyPiece : ChessPieces
    {
        public EmptyPiece() { }
        public override string ToString()
        {
            return "EE";
        }
        public override bool isLegalMove(Move move, bool isWhiteTurn, ChessPieces[,] board, bool OnlyForStalemate)
        {
            return false;
        }
        public override ChessPieces copy()
        {
            return this;
        }
    }
    class Location
    {
        public int row;
        public int column;
        public Location(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
        public Location() { }
    }
    class Move
    {
        public Location fromLocation;
        public Location toLocation;
        ChessPieces[,] copyBoard = new ChessPieces[8, 8];
        public Move(Location fromTile, Location toTile)
        {
            this.fromLocation = fromTile;
            this.toLocation = toTile;
        }
        public Move() { }
        public void makeMove(Move move, ChessPieces[,] board)
        {
            copyPreviousBoard(board);
            board[move.toLocation.row, move.toLocation.column] = board[move.fromLocation.row, move.fromLocation.column];
            board[move.fromLocation.row, move.fromLocation.column] = new EmptyPiece();
        }
        public void copyPreviousBoard(ChessPieces[,] board)
        {
            Array.Copy(board, copyBoard, 64);
        }
        public ChessPieces[,] undoMove()
        {
            return copyBoard;
        }
    }
}