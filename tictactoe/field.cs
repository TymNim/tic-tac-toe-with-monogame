﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tictactoe
{
    public class Field : Component {
        
        #region      
        private MouseState _currentMouse;
        private SpriteFont _font;
        private bool _isHovering;
        private MouseState _previousMouse;
        private Texture2D _texture;
        #endregion
       
         
        #region Properties
        public event EventHandler Click;
        public bool CLicked { get; private set; }
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle {
            get {
                return new Rectangle( (int)Position.X, (int)Position.Y, FieldSize, FieldSize );
            }
        }
        public string Text;
        public int FieldSize;
        #endregion


        #region Methods 
        public Field( Texture2D texture, SpriteFont font ) {
        
            _texture = texture;
            _font = font;

            PenColour = Color.White;
        
        }

        public override void Draw( GameTime gameTime, SpriteBatch spriteBatch ) {
        
            var colour = Color.DimGray;

            if ( _isHovering )
                colour = Color.Gray;

            spriteBatch.Draw( _texture, Rectangle, colour );
            
            if (! string.IsNullOrEmpty( Text ) ) {
        
                var x = Rectangle.X + ( Rectangle.Width / 2 ) - ( _font.MeasureString( Text ).X / 2 );
                var y = Rectangle.Y + ( Rectangle.Height / 1.75f ) - ( _font.MeasureString( Text ).Y / 2 );

                spriteBatch.DrawString( _font, Text, new Vector2( x, y ), PenColour );
            
            }
            
        }

        public override void Update( GameTime gameTime ) {
        
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState( );

            var mouseRectangle = new Rectangle( _currentMouse.X, _currentMouse.Y, 1, 1 );

            _isHovering = false;

            if ( mouseRectangle.Intersects( Rectangle ) ) {
            
                _isHovering = true;

                if ( _currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed ) {
                
                    Click?.Invoke( this, new EventArgs( ) );
                    
                }
                
            }
            
        }
        #endregion

    }
}
