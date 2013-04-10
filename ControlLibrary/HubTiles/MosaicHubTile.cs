using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ControlLibrary
{
    /// <summary>
    /// Defines a tile that mimics the WP OS's people hub tile.
    /// </summary>
    public class MosaicHubTile : PictureHubTile
    {
        /// <summary>
        /// Identifies the FlipMode dependency property.
        /// </summary>
        public static readonly DependencyProperty FlipModeProperty =
            DependencyProperty.Register("FlipMode", typeof(MosaicFlipMode), typeof(MosaicHubTile), new PropertyMetadata(MosaicFlipMode.Individual));

        private readonly int[][] cornersIndices = new int[][]
        {
           new int[] { 0, 1, 3, 4 }, // Upper left corner
           new int[] { 1, 2, 4, 5 }, // Upper right corner
           new int[] { 3, 4, 6, 7 }, // Lower left corner
           new int[] { 4, 5, 7, 8 }  // Lower right corner
        };

        private Random randomGenerator = new Random();
        private List<int> rowIndices = new List<int>();
        private Dictionary<int, bool> pictureFrameFlipHistory = new Dictionary<int, bool>();
        private int pictureFrameCounter = 0;
        private List<int> pictureFrameIndices = new List<int>();
        private ImageSource pictureCurrentFrameImage;
        private int updateCounter = 0;
        private Panel tilesPanel;

        public Brush MosaicTileBrush
        {
            get { return (Brush)GetValue(MosaicTileBrushProperty); }
            set { SetValue(MosaicTileBrushProperty, value); }
        }

        public static readonly DependencyProperty MosaicTileBrushProperty = DependencyProperty.Register("MosaicTileBrush", typeof(Brush), typeof(MosaicHubTile), new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(onMosaicTileBrushPropertyChanged)));

        private static void onMosaicTileBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var mosaicHubTile = sender as MosaicHubTile;
            if (mosaicHubTile != null)
            {

            }
        }

        /// <summary>
        /// Initializes a new instance of the RadMosaicHubTile class.
        /// </summary>
        public MosaicHubTile()
        {
            this.DefaultStyleKey = typeof(MosaicHubTile);
        }
        
        /// <summary>
        /// Determines how the cells of the mosaic tile are flipped.
        /// </summary>
        public MosaicFlipMode FlipMode
        {
            get
            {
                return (MosaicFlipMode)this.GetValue(MosaicHubTile.FlipModeProperty);
            }

            set
            {
                this.SetValue(MosaicHubTile.FlipModeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.tilesPanel = this.LayoutRoot as Panel;
        }

        protected override void OnUpLoadSources()
        {
            base.OnUpLoadSources();

            if (this.tilesPanel != null)
            {
                switch (this.FlipMode)
                {
                    case MosaicFlipMode.Individual:
                        this.FlipIndividually();
                        break;
                    default:
                        this.FlipRow();
                        break;
                }
            }
        }

        /// <summary>
        /// A virtual callback that is called periodically when the tile is not frozen. It can be used to
        /// update the tile visual states or to perform other necessary operations.
        /// </summary>
        protected override void Update()
        {
            if (this.BackContent != null)
            {
                this.updateCounter++;

                if (this.updateCounter == 5)
                {
                    base.Update();
                    this.updateCounter = 0;
                }
            }

            switch (this.FlipMode)
            {
                case MosaicFlipMode.Individual:
                    this.FlipIndividually();
                    break;
                default:
                    this.FlipRow();
                    break;
            }
        }

        private async void FlipIndividually()
        {
            if (this.pictureFrameCounter == 20 || this.pictureFrameIndices.Count > 0)
            {
                if (this.pictureFrameIndices.Count == 0)
                {
                    int randomCorner = this.randomGenerator.Next(this.cornersIndices.Length / this.cornersIndices.Rank);
                    this.pictureFrameIndices = this.cornersIndices[randomCorner].ToList<int>();
                    this.pictureCurrentFrameImage = await this.GetImageSource();
                    for (int i = 0; i < this.pictureFrameIndices.Count; ++i)
                    {
                        this.pictureFrameFlipHistory[i] = false;
                    }
                }

                int randomIndexIndex = this.randomGenerator.Next(this.pictureFrameIndices.Count);
                while (this.pictureFrameFlipHistory[randomIndexIndex])
                {
                    randomIndexIndex = this.randomGenerator.Next(this.pictureFrameIndices.Count);
                }
                this.pictureFrameFlipHistory[randomIndexIndex] = true;

                int randomTileIndex = this.pictureFrameIndices[randomIndexIndex];

                CustomMosaicTile frameTile = this.tilesPanel.Children[randomTileIndex] as CustomMosaicTile;
                frameTile.FrameCorner = (FrameCorner)randomIndexIndex;
                frameTile.Flip(this.pictureCurrentFrameImage);

                bool allFlipped = true;
                this.pictureFrameFlipHistory.Apply<KeyValuePair<int, bool>>((pair) => allFlipped = allFlipped && pair.Value);
                if (allFlipped)
                {
                    this.pictureFrameCounter = 0;
                    this.pictureFrameFlipHistory.Clear();
                    this.pictureFrameIndices.Clear();
                }
                return;
            }

            this.pictureFrameCounter++;

            int randomIndex = this.randomGenerator.Next(this.tilesPanel.Children.Count);
            CustomMosaicTile tile = this.tilesPanel.Children[randomIndex] as CustomMosaicTile;
            tile.FrameCorner = null;

            if (tile.IsContentDisplayed)
            {
                tile.Flip(null);
            }
            else
            {
                tile.Flip(await this.GetImageSource());
            }
        }

        private async void FlipRow()
        {
            if (this.rowIndices.Count == 0)
            {
                this.FillRowIndicesCollection();
                this.pictureCurrentFrameImage = await this.GetImageSource();
            }

            int rowIndex = this.rowIndices[this.randomGenerator.Next(this.rowIndices.Count)];

            for (int i = 0; i < 3; ++i)
            {
                MosaicTile tile = this.tilesPanel.Children[(rowIndex * 3) + i] as MosaicTile;
            }

            this.rowIndices.Remove(rowIndex);
        }

        private void FillRowIndicesCollection()
        {
            do
            {
                int nextIndex = this.randomGenerator.Next(3);
                while (this.rowIndices.Contains(nextIndex))
                {
                    nextIndex = this.randomGenerator.Next(3);
                }

                this.rowIndices.Add(nextIndex);
            }
            while (this.rowIndices.Count < 3);
        }
    }
}
