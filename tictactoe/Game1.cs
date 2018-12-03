using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tictactoe.Desktop
{

    // Main class for the game
    public class Game1 : Game {

        #region

        private Color _backgroundColour = Color.Black;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<List<Component>> _gameComponents = new List<List<Component>>( );
        
        char turn = 'X';            // whose turn it is
        string gameMessage = "";    // the message will be shown when someone wins
        
        SpriteFont font;            // font will be used for all components
        Texture2D uselessTexture; 
        
        bool gameDisabled = true;   // disables the game whan someone wins

        #endregion


        public Game1( ) {

            graphics = new GraphicsDeviceManager( this );
            Content.RootDirectory = "Content";

        }

        #region Main Methods for UI

        // Allows the game to perform any initialization it needs to before starting to run.
        // This is where it can query for any required services and load any non-graphic related content.
        // Calling base.Initialize will enumerate through any components and initialize them as well.
        protected override void Initialize( ) { 

            IsMouseVisible = true;

            // setting up the window size
            graphics.PreferredBackBufferWidth = 760;
            graphics.PreferredBackBufferHeight = 640;
            graphics.ApplyChanges();

            base.Initialize( );

        }

        // LoadContent will be called once per game and is the place to load all of your content.
        protected override void LoadContent( ) {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch( GraphicsDevice );
            
            // a font and texture for the game components
            font = Content.Load<SpriteFont>( "Fonts/elementric" );
            uselessTexture = Content.Load<Texture2D>( "Controls/whiteRect" );

            Button threeButton = new Button( uselessTexture, font )
            {
                Position = new Vector2( 20, 130 ),
                Text = "3 x 3",
            };
            threeButton.Click += threeButton_Click;

            Button fourButton = new Button( uselessTexture, font )
            {
                Position = new Vector2( 20, 250 ),
                Text = "4 x 4",
            };
            fourButton.Click += fourButton_Click;

            Button fiveButton = new Button( uselessTexture, font )
            {
                Position = new Vector2( 20, 370 ),
                Text = "5 x 5",
            };
            fiveButton.Click += fiveButton_Click;

            
            _gameComponents.Add( new List<Component>( )
            {
                threeButton,
                fourButton,
                fiveButton
            });

        }

                /// UnloadContent will be called once per game and is the place to unload game-specific content.
        protected override void UnloadContent( ) {
            // TODO: Unload any non ContentManager content here
        }

        // Allows the game to run logic such as updating the world,
        // checking for collisions, gathering input, and playing audio.
        protected override void Update( GameTime gameTime ) {
            
            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState( ).IsKeyDown( Keys.Escape ) )
                Exit( );
            
            
            for ( int i = 0; i < _gameComponents.Count; i++ )
                for ( int j = 0; j < _gameComponents[i].Count; j++ )
                    _gameComponents[i][j].Update( gameTime );
            

            base.Update( gameTime );

        }


        // This is called when the game should draw itself.
        protected override void Draw( GameTime gameTime ) {
        
            GraphicsDevice.Clear( _backgroundColour );

            spriteBatch.Begin( );

            foreach ( var componentList in _gameComponents )
                foreach ( var component in componentList )
                    component.Draw( gameTime, spriteBatch );
            spriteBatch.DrawString( font, gameMessage, new Vector2( 60, 20 ), Color.White );
            

            spriteBatch.End( );

            base.Draw( gameTime );

        }

        #endregion


        #region Methods for click events
        
        // new bame button clicks
        private void threeButton_Click( object sender, System.EventArgs e ) {
        
            newGame( 3 );
        
        }
        private void fourButton_Click( object sender, System.EventArgs e ) {
        
            newGame( 4 );
        
        }
        private void fiveButton_Click( object sender, System.EventArgs e ) {

            newGame( 5 );
        
        }

        private void field_Click( object sender, System.EventArgs e ) {
        
            if ( gameDisabled )
                return;
            
            Field theField = ( (Field)sender );
            
            if ( theField.Text == "" ) {
                theField.Text = currentTurn( );
                
                checkGreed( );
            }

        }

        #endregion


        #region 

        // generates a new map based on the option was selected
        private void newGame( int greedSize ) {
            
            resetGame( );
            
            List<Component> greed = new List<Component>( );
            
            const int paddingTop = 100;
            const int paddingRight = 260;
            const int fieldSize = 80;
            const int margin = 20;
            
            for ( int i = 0; i < greedSize; i++ ) {
                for ( int j = 0; j < greedSize; j++ ) {
                    Field newField = new Field( uselessTexture, font )
                    {
                        Position = new Vector2( (paddingRight + j * (fieldSize + margin)) , (paddingTop + margin + i * (fieldSize + margin)) ),
                        Text = "",
                        FieldSize = fieldSize
                    };
                    
                    newField.Click += field_Click;
                    
                    greed.Add( newField );
                }
            }
            
            _gameComponents.Add( greed );
            
            gameDisabled = false;
            
        }
        
        private void resetGame( ) {
            gameMessage = "";
            turn = 'X';
            if ( _gameComponents.Count > 1 ) 
                _gameComponents.RemoveAt( 1 );
        }
        
        private string currentTurn( ) {
            if ( turn == 'X')
            {
                turn = 'O';
                return "X";
            } else {
                turn = 'X';
                return "O";
            }
        }

        private void checkGreed( ) {
            
            int freeFields = countFreeFields( );
             
            // the message will be displayed
            string msg = "";
            
            //checking verticals
            checkVertically( ref msg );
            
            //checking horisontals
            checkHorisontally( ref msg );
            
            //checking diagonals
            checkDiagonals( ref msg );
            
            gameMessage += msg;
            
            if ( ( msg == "" ) && ( freeFields == 0 ) ) {
                gameMessage = "Nobody wins!";
                gameDisabled = true; 
            } else {
                if ( msg != "" ) {
                    gameMessage = msg + " won!";
                    gameDisabled = true;
                } else {
                    gameMessage = "";
                }
            }

        }

        #endregion

        
        #region Greed Cheking Methods

        // counts all free fields
        private int countFreeFields( ) {
            
            int number = 0;
            
            foreach ( Field f in _gameComponents[1] )
                if ( f.Text == "" ) number++;
             
            return number; 

        }
        
        // checks if all fields of a vertical are the same 
        private void checkVertically( ref string msg ) {
            
            for ( int j = 0; j < (int)System.Math.Sqrt( _gameComponents[1].Count ); j++ ) { 
            
                bool allMatches = true;
                
                for ( int i = j; i < _gameComponents[1].Count; i += (int)System.Math.Sqrt( _gameComponents[1].Count ) ) {
                    
                    if ( ( (Field)_gameComponents[1][j] ).Text != ( (Field)_gameComponents[1][i] ).Text )
                        allMatches = false;
                        
                }
                
                if ( allMatches && ( ( (Field)_gameComponents[1][j] ).Text != "" ) ) 
                    msg = ( (Field)_gameComponents[1][j] ).Text;
            }
            
        }
        
        // checks if all fields of a horisontal are the same 
        private void checkHorisontally( ref string msg ) {
            
            for ( int j = 0; j < _gameComponents[1].Count; j += (int)System.Math.Sqrt( _gameComponents[1].Count ) ) { 
            
                bool allMatches = true;
                
                for ( int i = j; i < j + (int)System.Math.Sqrt( _gameComponents[1].Count ); i++ ) {
                    
                    if ( ( (Field)_gameComponents[1][j] ).Text != ( (Field)_gameComponents[1][i] ).Text )
                        allMatches = false;
                        
                }
                
                if ( allMatches && ( ( (Field)_gameComponents[1][j] ).Text != "" ) ) 
                    msg = ( (Field)_gameComponents[1][j] ).Text;
            }
 
        }

        // checks if all fields of a diagonal are the same 
        private void checkDiagonals ( ref string msg ) {
            
            bool leftDiagonal = true;
            bool rightDiagonal = true;
            
            
            for ( int j = 0; j < _gameComponents[1].Count; j +=  (int)System.Math.Sqrt( _gameComponents[1].Count ) + 1 ) {
                if ( ( (Field)_gameComponents[1][0] ).Text != ( (Field)_gameComponents[1][j] ).Text )
                    leftDiagonal = false;
                    
            }
            
            if ( leftDiagonal && ( ( (Field)_gameComponents[1][0] ).Text != "" ) )
                msg = ( (Field)_gameComponents[1][0] ).Text;
            
            for ( int j = (int)System.Math.Sqrt( _gameComponents[1].Count ) - 1; j < _gameComponents[1].Count-1; j += (int)System.Math.Sqrt( _gameComponents[1].Count ) - 1 ) {
                if ( ( (Field)_gameComponents[1][(int)System.Math.Sqrt( _gameComponents[1].Count ) - 1] ).Text != ( (Field)_gameComponents[1][j] ).Text )
                    rightDiagonal = false;
            }
            
            if ( rightDiagonal && ( ( (Field)_gameComponents[1][(int)System.Math.Sqrt( _gameComponents[1].Count ) - 1] ).Text != "" ) )
                msg = ( (Field)_gameComponents[1][(int)System.Math.Sqrt( _gameComponents[1].Count ) - 1] ).Text;

        }

        #endregion

    }
}
