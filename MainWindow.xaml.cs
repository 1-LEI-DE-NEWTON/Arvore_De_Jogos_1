using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arvore_De_Jogos_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Variáveis
        private string[,] tabuleiro = new string[3, 3];

        //Turno true = X, false = O
        private bool turno = true;
        public MainWindow()
        {
            InitializeComponent();
            OpenBestMoveWindow();            
        }

        private void OpenBestMoveWindow()
        {
            BestMoveWindow bestMoveWindow = new BestMoveWindow(tabuleiro, turno);
            bestMoveWindow.Show();
        }

        private void OnCellClick(object sender, RoutedEventArgs e)
        {            
            Button btn = (Button)sender;

            // Determina a posição do botão
            int row = Grid.GetRow(btn);
            int col = Grid.GetColumn(btn);

            // Verifica se a célula está vazia
            if (btn.Content == null)
            {
                if (turno)
                {
                    btn.Content = "X";
                    tabuleiro[row, col] = btn.Content.ToString();
                    turno = false;                    
                }
                else
                {
                    btn.Content = "O";
                    tabuleiro[row, col] = btn.Content.ToString();                    
                    turno = true;
                }

                // Verifica se houve um vencedor
                if (CheckWinner(btn.Content.ToString()))
                {
                    MessageBox.Show("Vencedor: " + btn.Content);
                    RestartBoard();
                }
                
                if (IsBoardFull())
                {
                    MessageBox.Show("Empate!");
                    RestartBoard();
                }

                UpdateStatus();
            }
        }

        // Checa se houve um vencedor
        private bool CheckWinner(string player)
        {
            // Verifica as linhas
            for (int i = 0; i < 3; i++)
            {
                if (tabuleiro[i, 0] != null && tabuleiro[i, 0] == tabuleiro[i, 1] && tabuleiro[i, 1] == tabuleiro[i, 2])
                {                    
                    return true;
                }
            }

            // Verifica as colunas
            for (int i = 0; i < 3; i++)
            {
                if (tabuleiro[0, i] != null && tabuleiro[0, i] == tabuleiro[1, i] && tabuleiro[1, i] == tabuleiro[2, i])
                {                    
                    return true;
                }
            }

            // Verifica as diagonais
            if (tabuleiro[0, 0] != null && tabuleiro[0, 0] == tabuleiro[1, 1] && tabuleiro[1, 1] == tabuleiro[2, 2])
            {                
                return true;
            }
            if (tabuleiro[0, 2] != null && tabuleiro[0, 2] == tabuleiro[1, 1] && tabuleiro[1, 1] == tabuleiro[2, 0])
            {                
                return true;
            }            
            
            return false;
        }

        private void UpdateStatus()
        {
            if (turno)
            {
                StatusTextBlock.Text = "Vez do jogador X";
            }

            else
            {
                StatusTextBlock.Text = "Vez do jogador O";
            }
        }

        private void RestartBoard()
        {
            // Limpa o tabuleiro
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tabuleiro[i, j] = null;
                }
            }
            //Limpa apenas o button
            foreach (Button btn in tabuleiroGrid.Children.OfType<Button>())
            {
                btn.Content = null;
            }

            // Redefine o turno para o jogador X
            turno = true;

            // Atualiza o status
            UpdateStatus();
        }



        private bool IsBoardFull()
        {
            foreach (string cell in tabuleiro)
            {
                if (cell == null)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
