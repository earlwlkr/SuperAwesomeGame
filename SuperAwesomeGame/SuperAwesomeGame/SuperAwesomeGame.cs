using System;
using System.Collections.Generic;
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

        private SpriteFont _font;
        private List<Texture2D> _buttonTextures;
        private List<Texture2D> _sliderTextures;
        private List<Texture2D> _checkboxTextures; 
        private SoundEffect _soundEffect;

        private float _difficultyValue = 0f;
        private bool _enableSubtitles = false;

        private Menu _mainMenu;
        private Menu _optionsMenu;

        private MouseState _lastMouseState;

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
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Show mouse on screen.
            IsMouseVisible = true;
            Mouse.WindowHandle = Window.Handle;

            _font = Content.Load<SpriteFont>(@"Fonts\KenVector_Future");
            _buttonTextures = new List<Texture2D>
            {
                Content.Load<Texture2D>(@"Sprite2D\yellow_button"),
                Content.Load<Texture2D>(@"Sprite2D\yellow_button_pressed")
            };
            _sliderTextures = new List<Texture2D>
            {
                Content.Load<Texture2D>(@"Sprite2D\slider"),
                Content.Load<Texture2D>(@"Sprite2D\slider_marker")
            };
            _checkboxTextures = new List<Texture2D>
            {
                Content.Load<Texture2D>(@"Sprite2D\checkbox_checked"),
                Content.Load<Texture2D>(@"Sprite2D\checkbox")
            };
            _soundEffect = Content.Load<SoundEffect>(@"SFX\rollover");

            // Create menu with items.
            _mainMenu = CreateMainMenu();
            _entityCollection.Add(_mainMenu);
        }

        private Button CreateButton(string content, float left, float top)
        {
            return new Button(_soundEffect, _font, content, left, top, _buttonTextures[0], _buttonTextures[1]);
        }

        private Slider CreateSlider(string content, float left, float top, float value)
        {
            return new Slider(_soundEffect, _font, content, left, top, _sliderTextures[0], _sliderTextures[1], value);
        }

        private Checkbox CreateCheckbox(string content, float left, float top, bool value)
        {
            return new Checkbox(_soundEffect, _font, content, left, top, _checkboxTextures[0], _checkboxTextures[1], value);
        }

        private Menu CreateMainMenu()
        {
            var menu = new Menu();

            var left = GraphicsDevice.Viewport.Width/2 - _buttonTextures[0].Width/2;
            var top = Constants.MainMenuTop;

            menu.AddMenuItem(CreateButton("New Game", left, top));

            top += Constants.MenuButtonHeightOffset;
            var optionsItem = CreateButton("Options", left, top);
            optionsItem.OnClick += OnOptionsMenuButtonClicked;
            menu.AddMenuItem(optionsItem);

            top += Constants.MenuButtonHeightOffset;
            menu.AddMenuItem(CreateButton("Statistics", left, top));

            top += Constants.MenuButtonHeightOffset;
            var exitItem = CreateButton("Exit", left, top);
            exitItem.OnClick += OnExitMenuButtonClicked;
            menu.AddMenuItem(exitItem);

            return menu;
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

            var left = GraphicsDevice.Viewport.Width / 2 - _buttonTextures[0].Width / 2;
            var top = Constants.MainMenuTop;

            var backButton = CreateButton("Back", left, 400);
            backButton.OnClick += OnOptionsBackButtonClicked;
            menu.AddMenuItem(backButton);

            menu.AddMenuItem(CreateSlider("Difficulty", left, top, _difficultyValue));
            menu.AddMenuItem(CreateCheckbox("Subtitles", menu.Area.Left + menu.Area.Width - _checkboxTextures[0].Width, top + 40, _enableSubtitles));
            return menu;
        }

        private void OnOptionsBackButtonClicked(object sender, EventArgs e)
        {
            _difficultyValue = _optionsMenu.GetSliderValue("Difficulty");
            _enableSubtitles = _optionsMenu.GetCheckboxValue("Subtitles");
            _optionsMenu.SlideLeft(-_optionsMenu.Area.Width);
            _entityCollection.Remove(_optionsMenu);
           
            var left = GraphicsDevice.Viewport.Width / 2 - _buttonTextures[0].Width / 2;
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            var mouseState = Mouse.GetState();
            var entitiesAtPos = _entityCollection.GetEntitiesAtPos(mouseState.X, mouseState.Y);

            // Left mouse button events.
            switch (mouseState.LeftButton)
            {
                case ButtonState.Pressed:
                    entitiesAtPos.Select(true);
                    break;
                case ButtonState.Released:
                    if (_lastMouseState.LeftButton == ButtonState.Pressed)
                    {
                        _entityCollection.Select(false);
                    }
                    break;
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
            _spriteBatch.Begin();

            // Draw all sprites in collection.
            _entityCollection.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
