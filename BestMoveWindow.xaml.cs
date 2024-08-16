using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static Arvore_De_Jogos_1.GameTree;

namespace Arvore_De_Jogos_1
{
    /// <summary>
    /// Lógica interna para BestMoveWindow.xaml
    /// </summary>
    public partial class BestMoveWindow : Window
    {

        private string[,] board;
        private bool turno; // true = X, false = O                
        public BestMoveWindow(string[,] board, bool turno)
        {
            InitializeComponent();
            this.board = board;
            this.turno = turno;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();                        
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateBestMove();
        }

        private void UpdateBestMove()
        {
            GameTree gameTree = new GameTree(board, turno);
            string bestMove = gameTree.GetBestMove();
            MoveSuggestionTextBlock.Text = $"Melhor jogada: {bestMove}";
            // Atualiza a árvore de decisões
            PrintTree(gameTree);
        }

        private void PrintTree(GameTree gameTree)
        {
            StringBuilder treeStringBuilder = new StringBuilder();
            Node root = gameTree.BuildTree(board, turno, 3); // Profundidade pode ser ajustada

            // Função recursiva para construir a string da árvore
            void BuildTreeString(Node node, int depth)
            {
                if (node == null) return;

                treeStringBuilder.AppendLine($"{new string(' ', depth * 2)}{node.Move ?? "Root"}: Score = {node.Score}");

                if (node.Children != null)
                {
                    foreach (Node child in node.Children)
                    {
                        BuildTreeString(child, depth + 1);
                    }
                }
            }

            BuildTreeString(root, 0);
            TreeViewTextBox.Text = treeStringBuilder.ToString();
        }
    }
}
