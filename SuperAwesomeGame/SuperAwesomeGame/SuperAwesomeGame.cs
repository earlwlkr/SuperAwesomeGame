using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperAwesomeGame.Common;
using SuperAwesomeGame.Controls;

namespace SuperAwesomeGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SuperAwesomeGame : Game
    {
        private SpriteBatch _spriteBatch;
        private readonly EntityCollection _entityCollection;

        private float _difficultyValue = 0f;
        private bool _enableSubtitles = false;

        private Menu _mainMenu;
        private Menu _optionsMenu;

        private MouseState _lastMouseState;
        private bool _holding = false;
        private Vector2 _firstClickPosition;

        private TileMap _map = new TileMap();
        private int _currentCharacterType = 0;
        private List<Character> _characters = new List<Character>();
        private Button _changeButton;

        public SuperAwesomeGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            _entityCollection = new EntityCollection();
            Content.RootDirectory = "Content";
        }

        public GraphicsDeviceManager Graphics { get; set; }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new _spriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Show mouse on screen.
            IsMouseVisible = true;
            Mouse.WindowHandle = Window.Handle;

            Manager.Content = Content;

            // Create menu with items.
            _mainMenu = CreateMainMenu();
            
            _entityCollection.Add(_mainMenu);
        }

        private Menu CreateMainMenu()
        {
            var menu = new Menu();

            var left = GraphicsDevice.Viewport.Width/2 - Manager.ButtonTextures[0].Width/2;
            var top = Constants.MainMenuTop;

            var newGameItem = new Button("New Game", left, top);
            newGameItem.OnClick += OnNewGameMenuButtonClicked;
            menu.AddMenuItem(newGameItem);

            top += Constants.MenuButtonHeightOffset;
            var optionsItem = new Button("Options", left, top);
            optionsItem.OnClick += OnOptionsMenuButtonClicked;
            menu.AddMenuItem(optionsItem);

            top += Constants.MenuButtonHeightOffset;
            menu.AddMenuItem(new Button("Statistics", left, top));

            top += Constants.MenuButtonHeightOffset;
            var exitItem = new Button("Exit", left, top);
            exitItem.OnClick += OnExitMenuButtonClicked;
            menu.AddMenuItem(exitItem);

            return menu;
        }

        private void OnNewGameMenuButtonClicked(object sender, EventArgs e)
        {
            _mainMenu.SlideLeft(-_mainMenu.Area.Width);
            _entityCollection.Remove(_mainMenu);
            Thread.Sleep(100);
            Manager.Camera = new Camera(_map);
            _entityCollection.Add(_map);

            var left = GraphicsDevice.Viewport.Width / 2 - Manager.ButtonTextures[0].Width / 2;
            var top = Constants.MainMenuTop;

            _changeButton = new Button("Change", left, 400);
            _changeButton.OnClick += OnChangeButtonClicked;
            //_entityCollection.Add(_changeButton);
        }

        private void OnChangeButtonClicked(object sender, EventArgs e)
        {
            _currentCharacterType++;
        }

        private void OnOptionsMenuButtonClicked(object sender, EventArgs e)
        {
            _mainMenu.SlideLeft(-_mainMenu.Area.Width);
            Thread.Sleep(100);
            _optionsMenu = CreateOptionsMenu();
            _entityCollection.Add(_optionsMenu);
        }

        private void OnExitMenuButtonClicked(object sender, EventArgs e)
        {
            Exit();
        }

        private Menu CreateOptionsMenu()
        {
            var menu = new Menu();

            var left = GraphicsDevice.Viewport.Width / 2 - Manager.ButtonTextures[0].Width / 2;
            var top = Constants.MainMenuTop;

            var backButton = new Button("Back", left, 400);
            backButton.OnClick += OnOptionsBackButtonClicked;
            menu.AddMenuItem(backButton);

            menu.AddMenuItem(new Slider("Difficulty", left, top, _difficultyValue));
            menu.AddMenuItem(new Checkbox("Subtitles", menu.Area.Left + menu.Area.Width - Manager.CheckboxTextures[0].Width, top + 40, _enableSubtitles));
            return menu;
        }

        private void OnOptionsBackButtonClicked(object sender, EventArgs e)
        {
            _difficultyValue = _optionsMenu.GetSliderValue("Difficulty");
            _enableSubtitles = _optionsMenu.GetCheckboxValue("Subtitles");
            _optionsMenu.SlideLeft(-_optionsMenu.Area.Width);
            _entityCollection.Remove(_optionsMenu);
           
            var left = GraphicsDevice.Viewport.Width / 2 - Manager.ButtonTextures[0].Width / 2;
            _mainMenu.SlideRight(left);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Delete))
            {
                foreach (var character in _characters.Where(character => character.State == EntityState.Selected))
                {
                    _characters.Remove(character);
                    _entityCollection.Remove(character);
                    break;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                // Increase character type index.
                _currentCharacterType++;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                _currentCharacterType--;
                if (_currentCharacterType < 0)
                {
                    _currentCharacterType = 2;
                }
            }

            var mouseState = Mouse.GetState();

            // Left mouse button events.
            switch (mouseState.LeftButton)
            {
                case ButtonState.Pressed:
                    
                    if (_changeButton != null && _changeButton.Area.Contains(mouseState.X, mouseState.Y))
                    {
                        _changeButton.Select(true);
                    }
                    else
                    {
                        var mouseWorldPos = Utils.ScreenToWorld(new Vector2(mouseState.X, mouseState.Y));
                        var entitiesAtPos = _entityCollection.GetEntitiesAtPos(mouseWorldPos.X, mouseWorldPos.Y);

                        entitiesAtPos.Select(true);
                        foreach (var character in _characters)
                        {
                            if (!entitiesAtPos.Contains(character)) continue;
                            var state = character.State;
                            var clicked = ((Character) character).Clicked;
                            // Unselect all characters except for the clicked one.
                            foreach (var c in _characters)
                            {
                                c.State = EntityState.Default;
                                ((Character) c).Clicked = false;
                            }
                            character.State = state;
                            ((Character)character).Clicked = clicked;
                            break;
                        }

                        if (_lastMouseState.LeftButton == ButtonState.Pressed)
                        {
                            if (Manager.Camera != null && _holding)
                            {

                                Manager.Camera.MoveX(_firstClickPosition.X - mouseState.X);
                                Manager.Camera.MoveY(_firstClickPosition.Y - mouseState.Y);
                            }
                        }
                        else if (entitiesAtPos.IsEmpty())
                        {
                            _holding = true;
                        }
                    }
                    

                    _firstClickPosition.X = mouseState.X;
                    _firstClickPosition.Y = mouseState.Y;
                    break;

                case ButtonState.Released:
                    if (_lastMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (_changeButton != null)
                        {
                            _changeButton.Select(false);
                        }
                        
                        _entityCollection.Select(false);
                        _holding = false;
                    }
                    
                    break;
            }

            switch (mouseState.RightButton)
            {
                case ButtonState.Pressed:

                    break;

                case ButtonState.Released:
                    if (_lastMouseState.RightButton == ButtonState.Pressed)
                    {
                        var pos = Utils.ScreenToWorld(new Vector2(mouseState.X, mouseState.Y));
                        var character = new Character(pos.X, pos.Y, _currentCharacterType);
                        _characters.Add(character);
                        _entityCollection.Add(character);
                    }

                    break;
            }
            

            if (mouseState.ScrollWheelValue > _lastMouseState.ScrollWheelValue)
            {
                Manager.Camera.Scale += 0.1f;
            }
            else if (mouseState.ScrollWheelValue < _lastMouseState.ScrollWheelValue)
            {
                Manager.Camera.Scale -= 0.1f;
            }

            _lastMouseState = mouseState;

            // Update all sprites in collection.
            _entityCollection.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MediumTurquoise);
            if (Manager.Camera != null)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        Manager.Camera.GetTransform());
            }
            else
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred,
                    BlendState.AlphaBlend);
            }

            // Draw all sprites in collection.
            _entityCollection.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            if (_changeButton != null)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred,
                    BlendState.AlphaBlend);

                _changeButton.Draw(gameTime, _spriteBatch);

                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
