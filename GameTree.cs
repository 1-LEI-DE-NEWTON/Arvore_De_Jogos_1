using System;
using System.Collections.Generic;
using System.Linq;

namespace Arvore_De_Jogos_1
{
    public class GameTree
    {
        private string[,] tabuleiro;
        private bool turno; // true = X, false = O

        public GameTree(string[,] initialBoard, bool initialTurn)
        {
            tabuleiro = initialBoard;
            turno = initialTurn;
        }

        public string GetBestMove()
        {
            Node root = BuildTree(tabuleiro, turno, 3); // Profundidade pode ser ajustada
            Node bestMoveNode = BestBranch(root);
            return bestMoveNode?.Move ?? "Nenhuma jogada disponível";
        }

        public Node BuildTree(string[,] board, bool currentTurn, int depth)
        {
            Node root = new Node { Board = (string[,])board.Clone(), Turn = currentTurn };

            if (depth == 0 || CheckWinner(board) || IsBoardFull(board))
            {
                root.Score = EvaluateBoard(board);
                return root;
            }

            root.Children = Expand(root, currentTurn, depth);
            return root;
        }

        private List<Node> Expand(Node parent, bool currentTurn, int depth)
        {
            List<Node> children = new List<Node>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (parent.Board[i, j] == null)
                    {
                        string[,] newBoard = (string[,])parent.Board.Clone();
                        newBoard[i, j] = currentTurn ? "X" : "O";
                        Node child = BuildTree(newBoard, !currentTurn, depth - 1);
                        child.Move = $"{(char)('A' + i)}{j + 1}";
                        children.Add(child);
                    }
                }
            }
            return children;
        }

        private Node BestBranch(Node root)
        {
            if (root.Children == null || root.Children.Count == 0)
                return root;

            Node bestNode = root.Children[0];
            foreach (Node child in root.Children)
            {
                if ((root.Turn && child.Score > bestNode.Score) || (!root.Turn && child.Score < bestNode.Score))
                {
                    bestNode = child;
                }
            }
            return bestNode;
        }

        private int EvaluateBoard(string[,] board)
        {
            int score = 0;

            // Verifica linhas
            for (int i = 0; i < 3; i++)
            {
                score += EvaluateLine(board[i, 0], board[i, 1], board[i, 2]);
            }

            // Verifica colunas
            for (int i = 0; i < 3; i++)
            {
                score += EvaluateLine(board[0, i], board[1, i], board[2, i]);
            }

            // Verifica diagonais
            score += EvaluateLine(board[0, 0], board[1, 1], board[2, 2]);
            score += EvaluateLine(board[0, 2], board[1, 1], board[2, 0]);

            return score;
        }

        private int EvaluateLine(string a, string b, string c)
        {
            int score = 0;

            // Verifica se a linha representa uma vitória ou derrota
            if (a == "X" && b == "X" && c == "X")
                score += 10;
            else if (a == "O" && b == "O" && c == "O")
                score -= 10;
            // Verifica linhas parcialmente preenchidas
            else if (a == "X" && b == "X" || b == "X" && c == "X" || a == "X" && c == "X")
                score += 5;
            else if (a == "O" && b == "O" || b == "O" && c == "O" || a == "O" && c == "O")
                score -= 5;

            return score;
        }

        private bool CheckWinner(string[,] board)
        {
            // Verifica linhas
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] != null && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                {
                    return true;
                }
            }

            // Verifica colunas
            for (int i = 0; i < 3; i++)
            {
                if (board[0, i] != null && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                {
                    return true;
                }
            }

            // Verifica diagonais
            if (board[0, 0] != null && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            {
                return true;
            }
            if (board[0, 2] != null && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            {
                return true;
            }

            return false;
        }

        private bool IsBoardFull(string[,] board)
        {
            foreach (string cell in board)
            {
                if (cell == null)
                {
                    return false;
                }
            }
            return true;
        }

        public class Node
        {
            public string[,] Board { get; set; }
            public bool Turn { get; set; }
            public int Score { get; set; }
            public string Move { get; set; }
            public List<Node> Children { get; set; }
        }
    }
}
