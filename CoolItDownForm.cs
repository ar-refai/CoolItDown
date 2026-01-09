using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoolItDown
{
    public partial class CoolItDownForm : Form
    {
        private enum GameState
        {
            Playing,
            Won
        }

        private static readonly Random rng = new();

        private List<PictureBox> faceBoxes = new();
        private GameState currentState = GameState.Playing;
        private PictureBox correctFace;

        // Distance (in pixels) from last wrong guess to correct face; null if none yet.
        private double? previousDistance;

        public CoolItDownForm()
        {
            InitializeComponent();
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void CoolItDownForm_Load(object sender, EventArgs e)
        {
            CreateFaceGrid();
            ChooseCorrectFace();
        }

        private void CreateFaceGrid()
        {
            int columns = 5;
            int rows = 4;

            int faceSize = 80;
            int spacing = 10;

            int totalWidth = columns * faceSize + (columns - 1) * spacing;
            int totalHeight = rows * faceSize + (rows - 1) * spacing;

            int startX = (GamePanel.Width - totalWidth) / 2;
            int startY = (GamePanel.Height - totalHeight) / 2;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    PictureBox face = new PictureBox
                    {
                        Width = faceSize,
                        Height = faceSize,
                        Left = startX + col * (faceSize + spacing),
                        Top = startY + row * (faceSize + spacing),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Cursor = Cursors.Hand,
                        Image = Properties.Resources.sad1,
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.Transparent
                    };

                    face.Click += Face_Click;

                    GamePanel.Controls.Add(face);
                    faceBoxes.Add(face);
                }
            }
        }

        private void Face_Click(object? sender, EventArgs e)
        {
            if (currentState != GameState.Playing)
            {
                return;
            }

            if (sender is not PictureBox clickedFace)
            {
                return;
            }

            if (correctFace == null)
            {
                ChooseCorrectFace();
            }

            if (clickedFace == correctFace)
            {
                PlayCorrectSound();
                HandleWin(clickedFace);
            }
            else
            {
                PlayWrongSound();
                // compute hotter/colder and show bubble
                var currentDistance = DistanceBetweenCenters(clickedFace, correctFace);
                string hint = ComputeHint(currentDistance);
                previousDistance = currentDistance;
                _ = ShowBubbleAsync(clickedFace, hint);
                _ = HandleWrongGuessAsync(clickedFace);
            }
        }

        private void HandleWin(PictureBox face)
        {
            currentState = GameState.Won;
            face.Image = Properties.Resources.happy;
            MessageBox.Show("You found the correct face! You win!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task HandleWrongGuessAsync(PictureBox face)
        {
            // Provide quick visual feedback: flash border and briefly change image.
            var originalBorder = face.BorderStyle;
            var originalBack = face.BackColor;
            var originalImage = face.Image;

            try
            {
                face.BorderStyle = BorderStyle.FixedSingle;
                face.BackColor = Color.LightCoral;

                await Task.Delay(350);
            }
            finally
            {
                face.BorderStyle = originalBorder;
                face.BackColor = originalBack;
                face.Image = originalImage;
            }

            using (ToolTip tip = new())
            {
                tip.Show("Ouch, wrong face!", face, face.Width / 2, face.Height / 2, 1000);
                await Task.Delay(1000);
            }
        }

        private void ChooseCorrectFace()
        {
            previousDistance = null;

            if (faceBoxes == null || faceBoxes.Count == 0)
            {
                correctFace = null;
                return;
            }

            foreach (var f in faceBoxes)
            {
                f.Image = Properties.Resources.sad1;
                f.BorderStyle = BorderStyle.None;
                f.BackColor = Color.Transparent;
            }

            int index = rng.Next(faceBoxes.Count);
            correctFace = faceBoxes[index];

            // Debug mark:
            // correctFace.BorderStyle = BorderStyle.FixedSingle;
            // correctFace.BackColor = Color.LightGreen;
        }

        // Public helper to reset/restart the game. Call from a button click or menu command.
        public void ResetGame()
        {
            currentState = GameState.Playing;
            previousDistance = null;
            ChooseCorrectFace();
        }

        // Optional helper to briefly reveal the correct face (for testing/hint).
        public async Task RevealCorrectFaceTemporary(int milliseconds = 800)
        {
            if (correctFace == null) return;

            var originalImage = correctFace.Image;
            correctFace.Image = Properties.Resources.happy;
            correctFace.BorderStyle = BorderStyle.FixedSingle;
            correctFace.BackColor = Color.LightGreen;

            await Task.Delay(milliseconds);

            if (currentState == GameState.Playing)
            {
                correctFace.Image = originalImage;
                correctFace.BorderStyle = BorderStyle.None;
                correctFace.BackColor = Color.Transparent;
            }
        }

        // --- New helper methods for sound + hint bubble ---

        private void PlayWrongSound()
        {
            try
            {
                // Properties.Resources.WrongSound should be a WAV resource (UnmanagedMemoryStream)
                using var s = Properties.Resources.wrong;
                if (s != null)
                {
                    var player = new SoundPlayer(s);
                    player.Play();
                }
            }
            catch
            {
                // ignore sound errors
            }
        }

        private void PlayCorrectSound()
        {
            try
            {
                using var s = Properties.Resources.correct;
                if (s != null)
                {
                    var player = new SoundPlayer(s);
                    player.Play();
                }
            }
            catch
            {
                // ignore sound errors
            }
        }

        private static double DistanceBetweenCenters(Control a, Control b)
        {
            var ax = a.Left + a.Width / 2.0;
            var ay = a.Top + a.Height / 2.0;
            var bx = b.Left + b.Width / 2.0;
            var by = b.Top + b.Height / 2.0;
            var dx = ax - bx;
            var dy = ay - by;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private string ComputeHint(double currentDistance)
        {
            if (!previousDistance.HasValue)
            {
                // No previous guess: provide a neutral hint - treat first click as "Hotter" (closer) only if within half diagonal distance
                double maxPossible = Math.Sqrt(GamePanel.Width * GamePanel.Width + GamePanel.Height * GamePanel.Height);
                return currentDistance <= (maxPossible / 2) ? "Hotter" : "Colder";
            }

            return currentDistance < previousDistance.Value ? "Hotter" : "Colder";
        }

        private async Task ShowBubbleAsync(PictureBox face, string text, int durationMs = 900)
        {
            if (face == null) return;

            // Prepare bubble panel (background image + label)
            Panel bubble = new Panel
            {
                Size = new Size(120, 60),
                BackgroundImageLayout = ImageLayout.Zoom,
                BackColor = Color.Transparent
            };

            // If a speech-bubble image exists in resources use it
            try
            {
                var bubbleImage = Properties.Resources.bubble;
                if (bubbleImage != null)
                {
                    bubble.BackgroundImage = bubbleImage;
                    // size to resource aspect ratio
                    bubble.Size = new Size(Math.Min(140, bubbleImage.Width), Math.Min(80, bubbleImage.Height));
                }
            }
            catch
            {
                // ignore if resource missing; panel will be plain
            }

            Label lbl = new Label
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            bubble.Controls.Add(lbl);

            // Position bubble above the face, centered
            int bubbleX = face.Left + (face.Width / 2) - (bubble.Width / 2);
            int bubbleY = face.Top - bubble.Height - 8;

            // Clamp within GamePanel
            bubbleX = Math.Max(GamePanel.Padding.Left, bubbleX);
            bubbleX = Math.Min(GamePanel.Width - bubble.Width - GamePanel.Padding.Right, bubbleX);
            bubbleY = Math.Max(GamePanel.Padding.Top, bubbleY);

            bubble.Left = bubbleX;
            bubble.Top = bubbleY;

            GamePanel.Controls.Add(bubble);
            bubble.BringToFront();

            try
            {
                await Task.Delay(durationMs);
            }
            finally
            {
                GamePanel.Controls.Remove(bubble);
                bubble.Dispose();
            }
        }
    }
}
