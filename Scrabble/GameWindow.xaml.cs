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
using System.IO;

namespace Scrabble
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Page
    {
        //Declaring important fields
        #region 
        //create player names
        static string p1Name;
        static string p2Name;
        
        // create player 'hands'
        static char[] P1Hand = new char[7];
        static char[] P2Hand = new char[7];
        //bool to determine whos turn it is
        static bool P1Turn = true;
        //player scores
        static int p1Score = 0;
        static int p2Score = 0;
        //turn count
        int turncount = 0;

        //dictionary
        IEnumerable<string> Scrabble_dict;

        //create Queue of Letters (tiles) and shuffle them, for them to be dealt
        Button selectedTile;
        List<Button> BoardRecallTiles = new List<Button>();


        //Create List of Words that have been played
        List<string> Wordsplayed = new List<string>();

        static char[] Tiles = {'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E' , 'E' , 'E' , 'A', 'A' , 'A' , 'A' , 'A' , 'A' , 'A' , 'A' , 'A' , 'A' ,'I', 'I' , 'I' , 'I' , 'I' , 'I' , 'I' , 'I' , 'I' ,'O', 'O' , 'O' , 'O' , 'O' , 'O' , 'O' , 'O' ,'N', 'N' , 'N' , 'N' , 'N' , 'N' ,'R', 'R' , 'R' , 'R' , 'R' , 'R' ,'T', 'T' , 'T' , 'T' , 'T' , 'T' ,'L', 'L' , 'L' , 'L' ,'S', 'S' , 'S' , 'S' ,'U', 'U' , 'U' , 'U' , 'U',
        'D', 'D', 'D','D', 'G','G','G','B','B','C','C','M','M','P','P','F','F','H','H','V','V','W','W','Y','Y','K','J','X','Q','Z' };
        static Random rnd = new Random();
        Queue<char> ShuffledTiles = new Queue<char>(Tiles.OrderBy(x => rnd.Next()).Take(50));

        Dictionary<char, int> LetterValues = new Dictionary<char, int>() { { 'A', 1 }, { 'E', 1 }, { 'I', 1 }, { 'O', 1 }, { 'U', 1 }, { 'L', 1 }, { 'N', 1 }, { 'S', 1 }, { 'T', 1 }, { 'R', 1 },
            { 'D', 2 }, { 'G', 2 }, { 'B', 3 }, { 'C', 3 }, { 'M', 3 }, { 'P', 3 }, { 'F', 4 }, { 'H', 4 }, { 'V', 4 }, { 'W', 4 }, { 'Y', 4 }, { 'K', 5 }, { 'J', 8 }, { 'X', 8 }, { 'Q', 10 }, { 'Z', 10 } };

        //Create grid of letters
        char[,] CoordinateGrid = new char[9, 9];
        char[,] TempGrid = new char[9, 9];
        Dictionary<Button, (int, int)> ButtonGrid = new Dictionary<Button, (int, int)>();

        #endregion

        public GameWindow()
        {
            InitializeComponent();
            Initialise();
        }

        private void Initialise()
        {

            using (var db = new ScoresEntities())
            {
                var p1 = new Score();
                p1 = db.Scores.OrderByDescending(i => i.Id).Skip(1).First();
                p1Name = p1.Name;
                
                db.Scores.Remove(p1);

                var p2 = new Score();
                p2 = db.Scores.OrderByDescending(i => i.Id).First();
                p2Name = p2.Name;
                
                db.Scores.Remove(p2);
                db.SaveChanges();


            }
            //create IEnumerable of scrabble words
            Scrabble_dict = File.ReadLines(@"Collins Scrabble Words (2019).txt");
            //Assign tiles to both hands and display player1 on the rack
            NewTiles(P1Hand);
            NewTiles(P2Hand);
            RackRefresh(P1Hand);
            dummyButton.Content = '\0';
            selectedTile = dummyButton;
            PlayersTurnText.Text = $"{p1Name}'s turn";
            RefreshScores();
            WordSubmit_Button.IsEnabled = false;





            //Button Grid -> CoordinateGrid
            #region

            ButtonGrid.Add(A1, (0, 0));
            ButtonGrid.Add(B1, (0, 1));
            ButtonGrid.Add(C1, (0, 2));
            ButtonGrid.Add(D1, (0, 3));
            ButtonGrid.Add(E1, (0, 4));
            ButtonGrid.Add(F1, (0, 5));
            ButtonGrid.Add(G1, (0, 6));
            ButtonGrid.Add(H1, (0, 7));
            ButtonGrid.Add(I1, (0, 8));
            ButtonGrid.Add(A2, (1, 0));
            ButtonGrid.Add(B2, (1, 1));
            ButtonGrid.Add(C2, (1, 2));
            ButtonGrid.Add(D2, (1, 3));
            ButtonGrid.Add(E2, (1, 4));
            ButtonGrid.Add(F2, (1, 5));
            ButtonGrid.Add(G2, (1, 6));
            ButtonGrid.Add(H2, (1, 7));
            ButtonGrid.Add(I2, (1, 8));
            ButtonGrid.Add(A3, (2, 0));
            ButtonGrid.Add(B3, (2, 1));
            ButtonGrid.Add(C3, (2, 2));
            ButtonGrid.Add(D3, (2, 3));
            ButtonGrid.Add(E3, (2, 4));
            ButtonGrid.Add(F3, (2, 5));
            ButtonGrid.Add(G3, (2, 6));
            ButtonGrid.Add(H3, (2, 7));
            ButtonGrid.Add(I3, (2, 8));
            ButtonGrid.Add(A4, (3, 0));
            ButtonGrid.Add(B4, (3, 1));
            ButtonGrid.Add(C4, (3, 2));
            ButtonGrid.Add(D4, (3, 3));
            ButtonGrid.Add(E4, (3, 4));
            ButtonGrid.Add(F4, (3, 5));
            ButtonGrid.Add(G4, (3, 6));
            ButtonGrid.Add(H4, (3, 7));
            ButtonGrid.Add(I4, (3, 8));
            ButtonGrid.Add(A5, (4, 0));
            ButtonGrid.Add(B5, (4, 1));
            ButtonGrid.Add(C5, (4, 2));
            ButtonGrid.Add(D5, (4, 3));
            ButtonGrid.Add(E5, (4, 4));
            ButtonGrid.Add(F5, (4, 5));
            ButtonGrid.Add(G5, (4, 6));
            ButtonGrid.Add(H5, (4, 7));
            ButtonGrid.Add(I5, (4, 8));
            ButtonGrid.Add(A6, (5, 0));
            ButtonGrid.Add(B6, (5, 1));
            ButtonGrid.Add(C6, (5, 2));
            ButtonGrid.Add(D6, (5, 3));
            ButtonGrid.Add(E6, (5, 4));
            ButtonGrid.Add(F6, (5, 5));
            ButtonGrid.Add(G6, (5, 6));
            ButtonGrid.Add(H6, (5, 7));
            ButtonGrid.Add(I6, (5, 8));
            ButtonGrid.Add(A7, (6, 0));
            ButtonGrid.Add(B7, (6, 1));
            ButtonGrid.Add(C7, (6, 2));
            ButtonGrid.Add(D7, (6, 3));
            ButtonGrid.Add(E7, (6, 4));
            ButtonGrid.Add(F7, (6, 5));
            ButtonGrid.Add(G7, (6, 6));
            ButtonGrid.Add(H7, (6, 7));
            ButtonGrid.Add(I7, (6, 8));
            ButtonGrid.Add(A8, (7, 0));
            ButtonGrid.Add(B8, (7, 1));
            ButtonGrid.Add(C8, (7, 2));
            ButtonGrid.Add(D8, (7, 3));
            ButtonGrid.Add(E8, (7, 4));
            ButtonGrid.Add(F8, (7, 5));
            ButtonGrid.Add(G8, (7, 6));
            ButtonGrid.Add(H8, (7, 7));
            ButtonGrid.Add(I8, (7, 8));
            ButtonGrid.Add(A9, (8, 0));
            ButtonGrid.Add(B9, (8, 1));
            ButtonGrid.Add(C9, (8, 2));
            ButtonGrid.Add(D9, (8, 3));
            ButtonGrid.Add(E9, (8, 4));
            ButtonGrid.Add(F9, (8, 5));
            ButtonGrid.Add(G9, (8, 6));
            ButtonGrid.Add(H9, (8, 7));
            ButtonGrid.Add(I9, (8, 8));

            #endregion

            TempGrid = (char[,])CoordinateGrid.Clone();



        }
        //rack buttons
        #region 

        private void Rack1_Click(object sender, RoutedEventArgs e)
        {
            selectedTile = Rack1;

        }

        private void Rack2_Click(object sender, RoutedEventArgs e)
        {
            selectedTile = Rack2;
        }

        private void Rack3_Click(object sender, RoutedEventArgs e)
        {
            selectedTile = Rack3;
        }

        private void Rack4_Click(object sender, RoutedEventArgs e)
        {
            selectedTile = Rack4;
        }

        private void Rack5_Click(object sender, RoutedEventArgs e)
        {
            selectedTile = Rack5;
        }

        private void Rack6_Click(object sender, RoutedEventArgs e)
        {
            selectedTile = Rack6;
        }

        private void Rack7_Click(object sender, RoutedEventArgs e)
        {
            selectedTile = Rack7;
        }

        #endregion 

        //board buttons
        #region 
        private void A1_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(A1);

        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(B1);
        }

        private void C1_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(C1);
        }

        private void D1_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(D1);
        }

        private void E1_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(E1);
        }

        private void A2_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(A2);
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(B2);
        }

        private void C2_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(C2);
        }

        private void D2_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(D2);
        }

        private void E2_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(E2);
        }

        private void A3_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(A3);
        }

        private void B3_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(B3);
        }

        private void C3_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(C3);
        }

        private void D3_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(D3);
        }

        private void E3_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(E3);
        }

        private void A4_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(A4);
        }

        private void B4_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(B4);
        }

        private void C4_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(C4);
        }

        private void D4_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(D4);
        }

        private void E4_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(E4);

        }

        private void A5_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(A5);

        }

        private void B5_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(B5);

        }

        private void C5_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(C5);
        }

        private void D5_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(D5);
        }

        private void E5_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(E5);
        }
        //7 extension
        private void A6_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(A6);
        }

        private void B6_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(B6);
        }

        private void C6_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(C6);
        }

        private void D6_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(D6);
        }

        private void E6_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(E6);
        }

        private void A7_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(A7);
        }

        private void B7_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(B7);
        }

        private void C7_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(C7);
        }

        private void D7_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(D7);
        }

        private void E7_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(E7);
        }

        private void F1_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(F1);
        }

        private void F2_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(F2);
        }

        private void F3_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(F3);
        }

        private void F4_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(F4);
        }

        private void F5_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(F5);
        }

        private void F6_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(F6);
        }

        private void F7_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(F7);
        }

        private void G1_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(G1);
        }

        private void G2_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(G2);
        }

        private void G3_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(G3);
        }

        private void G4_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(G4);
        }

        private void G5_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(G5);
        }

        private void G6_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(G6);
        }

        private void G7_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(G7);
        }
        //8 rack
        private void A8_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(A8);
        }

        private void B8_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(B8);
        }

        private void C8_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(C8);
        }

        private void D8_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(D8);
        }

        private void E8_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(E8);
        }

        private void F8_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(F8);
        }

        private void G8_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(G8);
        }

        private void H1_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(H1);
        }

        private void H2_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(H2);
        }

        private void H3_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(H3);
        }

        private void H4_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(H4);
        }

        private void H5_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(H5);
        }

        private void H6_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(H6);
        }

        private void H7_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(H7);
        }

        private void H8_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(H8);
        }

        private void A9_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(A9);
        }

        private void B9_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(B9);
        }

        private void C9_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(C9);
        }

        private void D9_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(D9);
        }

        private void E9_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(E9);
        }

        private void F9_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(F9);
        }

        private void G9_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(G9);
        }

        private void H9_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(H9);
        }

        private void I1_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(I1);
        }

        private void I2_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(I2);
        }

        private void I3_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(I3);
        }

        private void I4_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(I4);
        }

        private void I5_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(I5);
        }

        private void I6_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(I6);
        }

        private void I7_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(I7);
        }

        private void I8_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(I8);
        }

        private void I9_Click(object sender, RoutedEventArgs e)
        {
            PlaceTileOnBoard(I9);
        }
        #endregion

        public void NewTiles(char[] hand)
        {
            int nullCount = hand.Count(s => s == '\0');

           
            if (nullCount < ShuffledTiles.Count)
            {
                for (int i = 1; i <= nullCount; i++)
                {

                    int findNull = Array.IndexOf(hand, '\0');
                    char newTile = ShuffledTiles.Dequeue();
                    hand[findNull] = newTile;
                }
            }
            else
            {
                for (int i = 1; i <= ShuffledTiles.Count; i++)
                {

                    int findNull = Array.IndexOf(hand, '\0');
                    char newTile = ShuffledTiles.Dequeue();
                    hand[findNull] = newTile;
                }
            }

        } //Counts how many empty letters in the hand, takes letters from queue and puts into hand

        public void RackRefresh(char[] hand)
        {
            Rack1.Content = hand[0];
            Rack2.Content = hand[1];
            Rack3.Content = hand[2];
            Rack4.Content = hand[3];
            Rack5.Content = hand[4];
            Rack6.Content = hand[5];
            Rack7.Content = hand[6];
            EnableRack();

        } // Refreshes the rack

        public void EnableRack()
        {
            Rack1.IsEnabled = true;
            Rack2.IsEnabled = true;
            Rack3.IsEnabled = true;
            Rack4.IsEnabled = true;
            Rack5.IsEnabled = true;
            Rack6.IsEnabled = true;
            Rack7.IsEnabled = true;
        } //enables all buttons in the 'rack'

        public void DisableRack()
        {
            Rack1.IsEnabled = false;
            Rack2.IsEnabled = false;
            Rack3.IsEnabled = false;
            Rack4.IsEnabled = false;
            Rack5.IsEnabled = false;
            Rack6.IsEnabled = false;
            Rack7.IsEnabled = false;
        } //disable all buttons in the 'rack'
        
        public void PlaceTileOnBoard(Button boardTile)
        {
            boardTile.Content = selectedTile.Content.ToString()[0];
            selectedTile.IsEnabled = false;
            selectedTile = dummyButton;
            if (boardTile.Content.ToString() != dummyButton.Content.ToString())
            {
                boardTile.IsEnabled = false;
                BoardRecallTiles.Add(boardTile);
                AddToGrid(boardTile);

            }

        } //set content of Tile, disable rack-tile, remembers board tile for 'recalling', reset selectedTile

        public void Recall_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var button in BoardRecallTiles)
            {
                button.Content = '\0';
                button.IsEnabled = true;
            }
            EnableRack();
            BoardRecallTiles.Clear();
            //reset grid to state before your turn
            TempGrid = (char[,])CoordinateGrid.Clone();

            if (WordSubmit_Button.IsEnabled == true)
            {
                WordSubmit_Button.IsEnabled = false;
                EnableRack();
            }

        }// returns unsubmitted tiles to the rack, 

        public void RefreshScores()
        {
            Player1Score.Text = $"{p1Name}: {p1Score.ToString()}";
            Player2Score.Text = $"{p2Name}: {p2Score.ToString()}";
        } // refresh the player scores

        public int WordValues(string word)
        {
            int value = 0;
            if (Scrabble_dict.Contains(word))
            {

                foreach (char letter in word)
                {
                    value += LetterValues[letter];
                }

            }

            return value;
        } // calculate if a word is valid and value of a word

        public void WordCheck_Button_Click(object sender, RoutedEventArgs e)
        {
            //int TotalWordsValue = 0;

            //string[] NewWords = CheckGridForWords(TempGrid);
            string[] NewWords = CheckSurroundingWords2(BoardRecallTiles);
            Log_TextBlock.Items.Clear();
            if (BoardRecallTiles.Count == 0)
            {
                Log_TextBlock.Items.Add($"No word played");
            }
            else if (IfAttachedToWord(BoardRecallTiles)==false)
            {
                Log_TextBlock.Items.Add($"Word unattached");
            }
            else if (NewWords.Length == 0)
            {
                Log_TextBlock.Items.Add($"Word not valid");

            }
            else if (turncount == 0 && IsFirstWordCentered(BoardRecallTiles) == false)
            {
                Log_TextBlock.Items.Add("First word must be centered");
            }
            else if (HasWordBeenPlayed(NewWords))
            {
                Log_TextBlock.Items.Add("Word has already been played");
            }
            else if (NewWords[0] == "Not In Line")
            {
                Log_TextBlock.Items.Add($"Letters not placed in a line");
            }

            else
            {

                List<(string, int)> validWords = new List<(string, int)>();

                foreach (var nw in NewWords)
                {
                    if (Scrabble_dict.Contains(nw))
                    {
                        int WordValue = WordValues(nw);
                        validWords.Add((nw, WordValue));
                    }
                }
                if (validWords.Count != NewWords.Length)
                {
                    Log_TextBlock.Items.Add("Word(s) not valid");
                }
                else
                {
                    foreach (var nw in validWords)
                    {
                        Log_TextBlock.Items.Add($"{nw.Item1}: {nw.Item2} pts");
                    }
                    WordSubmit_Button.IsEnabled = true;
                    DisableRack();
                }


            }


        } // checks if word is valid and how much it is worth  

        public void WordSubmit_Button_Click(object sender, RoutedEventArgs e)
        {
            int TotalWordsValue = 0;
            //creating words
            string[] NewWords = CheckSurroundingWords2(BoardRecallTiles);

            Log_TextBlock.Items.Clear();
            if (NewWords.Length == 0)
            {
                Log_TextBlock.Items.Add($"Word is not valid");
            }
            
            else
            {
                WordSubmit_Button.IsEnabled = false;
                foreach (var nw in NewWords)
                {
                    int WordValue = WordValues(nw);

                    Wordsplayed.Add(nw);
                    TotalWordsValue += WordValue;

                }
                if (P1Turn == true)
                {
                    p1Score += TotalWordsValue;
                    Log_TextBlock.Items.Add($"{p1Name} gained {TotalWordsValue} points!");
                }
                else
                {
                    p2Score += TotalWordsValue;
                    Log_TextBlock.Items.Add($"{p2Name} gained {TotalWordsValue} points!");
                }
                //Refresh Scores, clear boardrecalltiles
                RefreshScores();
                if (P1Turn == true)
                {
                    DrawNewTiles(P1Hand);
                }
                else
                {
                    DrawNewTiles(P2Hand);
                }
                BoardRecallTiles.Clear();
                CoordinateGrid = (char[,])TempGrid.Clone();

                //next turn
                if (P1Turn == true)
                {
                    P1Turn = false;
                    RackRefresh(P2Hand);
                    PlayersTurnText.Text = $"{p2Name}'s Turn";
                }
                else
                {
                    P1Turn = true;
                    RackRefresh(P1Hand);
                    PlayersTurnText.Text = $"{p1Name}'s Turn";
                }
                turncount++;
                if(HaveWordsRunOut() == true)
                {
                    Replace_Button.IsEnabled = false;
                }
                if(ShuffledTiles.Count > 0)
                {
                    Replace_Button.IsEnabled = true;
                }
                

            }





        } // submits the word and adds points to score 

        public void AddToGrid(Button AddButton)
        {
            (int, int) Coordinate = ButtonGrid[AddButton];
            TempGrid[Coordinate.Item1, Coordinate.Item2] = AddButton.Content.ToString()[0];
        } // adds tiles that you play to a temporary grid

        public string[] CheckSurroundingWords2(List<Button> buttons)
        {
            List<string> SurroundingWords = new List<string>();


            // all added buttons turned into coordinate Lists
            #region
            List<int> iCoordinates = new List<int>();
            List<int> jCoordinates = new List<int>();

            var orderedICoords = iCoordinates.OrderBy(i => i).ToList();
            var orderedJCoords = jCoordinates.OrderBy(j => j).ToList();


            foreach (var but in buttons)
            {
                iCoordinates.Add(ButtonGrid[but].Item1);
                jCoordinates.Add(ButtonGrid[but].Item2);
            }
            #endregion

            bool sameColumn = jCoordinates.Where(j => j == jCoordinates[0]).Count() == jCoordinates.Count();
            bool sameRow = iCoordinates.Where(i => i == iCoordinates[0]).Count() == iCoordinates.Count();


            if (buttons.Count == 0)
            {
                SurroundingWords.Add("No Buttons");
            } //return null if no buttons clicked
            else if (sameColumn == false && sameRow == false) // check if in line
            {
                SurroundingWords.Add("Not In Line");
            }
            else if (sameRow)
            {

                //grid row
                int i = iCoordinates[0];
                if(CheckForGapSameRowBug(i, jCoordinates) == false)
                {
                    SurroundingWords.Add("Not In Line");
                }
                else
                {
                    SurroundingWords.Add(FindWordAlongRow(i, jCoordinates));
                    foreach (var j in jCoordinates)
                    {
                        SurroundingWords.Add(FindWordAlongColumn(j, iCoordinates));
                    }
                }
                

            }
            else if (sameColumn)
            {
                //grid column
                int j = jCoordinates[0];
                if(CheckForGapSameColumnBug(j, iCoordinates) == false)
                {
                    SurroundingWords.Add("Not In Line");
                }
                else
                {
                    SurroundingWords.Add(FindWordAlongColumn(j, iCoordinates));
                    foreach (var i in iCoordinates)
                    {
                        SurroundingWords.Add(FindWordAlongRow(i, jCoordinates));
                    }
                }
                
            }

            List<string> notNullSurroundingWords = new List<string>();
            foreach (var sw in SurroundingWords)
            {
                if (sw != null)
                {
                    notNullSurroundingWords.Add(sw);
                }
            }

            return notNullSurroundingWords.ToArray();




        } //finds all words attached to the tiles that you placed

        public void DrawNewTiles(char[] hand) //remove played tiles from hand and take new tiles
        {

            foreach (var tile in BoardRecallTiles)
            {
                char tileChar = (char)tile.Content;
                int index = Array.IndexOf(hand, tileChar);
                hand[index] = '\0';
            }

            NewTiles(hand);
        }  

        private void Replace_Button_Click(object sender, RoutedEventArgs e)
        {
            //clear recallboardtiles
            foreach (var button in BoardRecallTiles)
            {
                button.Content = '\0';
                button.IsEnabled = true;
            }
            EnableRack();
            BoardRecallTiles.Clear();
            //reset grid to state before your turn
            TempGrid = (char[,])CoordinateGrid.Clone();

            //clear hand & draw new tiles
            if (P1Turn == true)
            {
                Array.Clear(P1Hand, '\0', P1Hand.Length - 1);
                NewTiles(P1Hand);
                
                P1Turn = false;
                RackRefresh(P2Hand);
                PlayersTurnText.Text = $"{p2Name}'s Turn";
                Replace_Button.IsEnabled = true;
                if (ShuffledTiles.Count == 0)
                {
                    Replace_Button.IsEnabled = false;
                }
            }
            else
            {
                Array.Clear(P2Hand, '\0', P2Hand.Length - 1);
                NewTiles(P2Hand);
                
                
                P1Turn = true;
                RackRefresh(P1Hand);
                PlayersTurnText.Text = $"{p1Name}'s Turn";
                Replace_Button.IsEnabled = true;
                if (ShuffledTiles.Count == 0)
                {
                    Replace_Button.IsEnabled = false;
                }
            }
            Log_TextBlock.Items.Clear();
            WordSubmit_Button.IsEnabled = false;
            


        } //replace all all tiles in your hand with new tiles

        private void Pass_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ShuffledTiles.Count > 0)
            {
                foreach (var button in BoardRecallTiles)
                {
                    button.Content = '\0';
                    button.IsEnabled = true;
                }
                EnableRack();
                BoardRecallTiles.Clear();
                //reset grid to state before your turn
                TempGrid = (char[,])CoordinateGrid.Clone();

                if (P1Turn == true)
                {
                    P1Turn = false;
                    RackRefresh(P2Hand);
                    PlayersTurnText.Text = $"{p2Name}'s Turn";
                    Replace_Button.IsEnabled = true;
                }
                else
                {
                    P1Turn = true;
                    RackRefresh(P1Hand);
                    PlayersTurnText.Text = $"{p1Name}'s Turn";
                    Replace_Button.IsEnabled = true;
                }
                WordSubmit_Button.IsEnabled = false;
                Log_TextBlock.Items.Clear();
            }
            else
            {
                EndGame();
            }

        } //Pass on your turn, next person's turn

        public void EndGame()
        {
            if (p1Score > p2Score)
            {
                Log_TextBlock.Items.Clear();
                Log_TextBlock.Items.Add($"{p1Name} wins!");
                DisableButtons();


            }
            else if (p1Score < p2Score)
            {
                Log_TextBlock.Items.Clear();
                Log_TextBlock.Items.Add($"{p2Name} wins!");
                DisableButtons();

            }
            else
            {
                Log_TextBlock.Items.Clear();
                Log_TextBlock.Items.Add($"It's a tie!");
                DisableButtons();
            }
            AddScoresToDB();

        } //runs when game ends (adds scores to db etc)

        public void DisableButtons()
        {
            WordCheck_Button.IsEnabled = false;
            Recall_Button.IsEnabled = false;
            WordSubmit_Button.IsEnabled = false;
            Replace_Button.IsEnabled = false;
            Pass_Button.IsEnabled = false;
        } //disables all buttons on the sideboard

        public void AddScoresToDB()
        {
            using (var db = new ScoresEntities())
            {
                Score updatePlayer1 = new Score();
                updatePlayer1.Name = p1Name;
                updatePlayer1.Score1 = p1Score;
                db.Scores.Add(updatePlayer1);

                Score updatePlayer2 = new Score();
                updatePlayer2.Name = p2Name;
                updatePlayer2.Score1 = p2Score;
                db.Scores.Add(updatePlayer2);
                db.SaveChanges();
            }
        } //adds scores to the database

        public string FindWordAlongRow(int i, List<int> jrow)
        {

            int lowestj = jrow.OrderBy(j => j).First();
            int findStart = lowestj;
            while (findStart > 0 && TempGrid[i, findStart - 1] != '\0')
            {
                findStart--;
            }
            int findEnd = lowestj;
            while (findEnd < TempGrid.GetLength(0) - 1 && TempGrid[i, findEnd + 1] != '\0')
            {
                findEnd++;
            }
            char[] row = Enumerable.Range(0, TempGrid.GetLength(0)).Select(x => TempGrid[i, x]).ToArray();
            string wordcheckrow = new string(row.Skip(findStart).Take(findEnd - findStart + 1).ToArray());
            if (wordcheckrow.Length > 1)
            {
                return wordcheckrow;
            }
            else
            {
                return null;
            }

        } //algorithm that searches along a row to find longest string

        public string FindWordAlongColumn(int j, List<int> icolumn)
        {
            int lowesti = icolumn.OrderBy(i => i).First();
            int findStart = lowesti;
            while (findStart > 0 && TempGrid[findStart - 1, j] != '\0')
            {
                findStart--;
            }
            int findEnd = lowesti;
            while (findEnd < TempGrid.GetLength(1) - 1 && TempGrid[findEnd + 1, j] != '\0')
            {
                findEnd++;
            }
            char[] column = Enumerable.Range(0, TempGrid.GetLength(1)).Select(x => TempGrid[x, j]).ToArray();
            string wordcheckcolumn = new string(column.Skip(findStart).Take(findEnd - findStart + 1).ToArray());
            if (wordcheckcolumn.Length > 1)
            {
                return wordcheckcolumn;
            }
            else
            {
                return null;
            }

        } //algorithm that search along a column to find longest string

        public bool CheckForGapSameRowBug(int i, List<int> jCoords)
        {
            jCoords.Sort();
            int findStart = jCoords[0];
            while (findStart > 0 && TempGrid[i, findStart - 1] != '\0')
            {
                findStart--;
            }
            int findEnd = jCoords[0];
            while (findEnd < TempGrid.GetLength(0) - 1 && TempGrid[i, findEnd + 1] != '\0')
            {
                findEnd++;
            }

            if(findEnd - findStart < jCoords[jCoords.Count-1] - jCoords[0])
            {
                return false;
            }
            else
            {
                return true;
            }
        } //returns false if there is a gap between letters in the same row

        public bool CheckForGapSameColumnBug(int j, List<int> iCoords)
        {
            iCoords.Sort();
            int findStart = iCoords[0];
            while (findStart > 0 && TempGrid[findStart - 1, j] != '\0')
            {
                findStart--;
            }
            int findEnd = iCoords[0];
            while (findEnd < TempGrid.GetLength(1) - 1 && TempGrid[findEnd + 1,j] != '\0')
            {
                findEnd++;
            }

            if (findEnd - findStart < iCoords[iCoords.Count-1] - iCoords[0])
            {
                return false;
            }
            else
            {
                return true;
            }
        } //returns false if there is a gap between letters in the same column

        public bool IfAttachedToWord(List<Button> buttons) //returns true if  attached to another word
        {
            
            //check if its attached
            bool attached = false;
            List<(int, int)> Coords = new List<(int, int)>();
            foreach(var but in buttons)
            {
                Coords.Add(ButtonGrid[but]);
            }
            foreach(var coord in Coords)
            {
                if(turncount == 0 || SurroundingTilesEmpty(coord.Item1, coord.Item2) == false)
                {
                    attached = true;
                }
                
            }
            
            return attached;
        }

        public bool SurroundingTilesEmpty(int i, int j) //True if  adjacent tiles are empty
        {
            //if a surrounding tile isnt empty
            bool empty = false;
            if(i == 0)
            {
                if(j == 0)
                {
                    empty = CoordinateGrid[i + 1, j] == '\0' && CoordinateGrid[i, j + 1] != '\0';
                }
                else if(j == TempGrid.GetLength(1)-1)
                {
                    empty = CoordinateGrid[i + 1, j] == '\0' && CoordinateGrid[i, j - 1] == '\0';
                }
                else
                {
                    empty = CoordinateGrid[i + 1, j] == '\0' && CoordinateGrid[i, j + 1] == '\0' && CoordinateGrid[i, j - 1] == '\0';
                }
            }
            else if(i == TempGrid.GetLength(0)-1)
            {
                if(j == TempGrid.GetLength(1)-1)
                {
                    empty = CoordinateGrid[i - 1, j] == '\0' && CoordinateGrid[i, j - 1] == '\0';
                }
                else if (j == 0)
                {
                    empty = CoordinateGrid[i - 1, j] == '\0' && CoordinateGrid[i, j + 1] == '\0';
                }
                else
                {
                    empty = CoordinateGrid[i - 1, j] == '\0' && CoordinateGrid[i, j + 1] == '\0' && CoordinateGrid[i, j - 1] == '\0';
                }
            }
            else if (j == TempGrid.GetLength(1)-1)
            {
                empty = CoordinateGrid[i + 1, j] == '\0' && CoordinateGrid[i - 1, j] == '\0' && CoordinateGrid[i, j - 1] == '\0';
            }
            else if (j == 0)
            {
                empty = CoordinateGrid[i + 1, j] == '\0' && CoordinateGrid[i - 1, j] == '\0' && CoordinateGrid[i, j + 1] == '\0';
            }
            else
            {
                empty = CoordinateGrid[i+ 1, j] == '\0' && CoordinateGrid[i - 1, j] == '\0' && CoordinateGrid[i, j + 1] == '\0' && CoordinateGrid[i, j - 1] == '\0';
            }

            return empty;
        }

        public bool HasWordBeenPlayed(string[] words)
        {
            bool iftrue = false;
            foreach(var word in words)
            {
                if (Wordsplayed.Contains(word))
                {
                    iftrue = true;
                }
            }

            return iftrue;
        } //checks if the word has already been played

        public bool IsFirstWordCentered(List<Button> buttonlist)
        {
            bool centered = false;
            foreach(Button but in buttonlist)
            {
                if(but == E5)
                {
                    centered = true;
                }
            }

            return centered;
        } //checks if the first word is centred

        public bool HaveWordsRunOut()
        {
            bool runout = false;
            if(ShuffledTiles.Count == 0)
            {
                runout = true;
            }
            return runout;
        }  //checks if the tile bag is empty
    }
}
