﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.ViewportAdapters;

namespace Projet
{
    internal class Camera
    {
        private OrthographicCamera _orthographicCamera;
        private Vector2 _cameraPosition;


        public OrthographicCamera OrthographicCamera
        {
            get
            {
                return _orthographicCamera;
            }

            set
            {
                _orthographicCamera = value;
            }
        }

        public Vector2 CameraPosition
        {
            get
            {
                return this._cameraPosition;
            }

            set
            {
                this._cameraPosition = value;
            }
        }

        public void Initialize(GameWindow window, GraphicsDevice device, int largeurFen, int hauteurFen)
        {
            var viewportAdapter = new BoxingViewportAdapter(window, device, largeurFen, hauteurFen);
            OrthographicCamera = new OrthographicCamera(viewportAdapter);
            OrthographicCamera.Zoom = 0.5f;
            CameraPosition = new Vector2(largeurFen, hauteurFen);
        }

        public void Update(GameTime gameTime, Pingouin pingouin)
        {
            MoveCamera(gameTime);
            //CameraPosition = pingouin.Position;
            OrthographicCamera.LookAt(CameraPosition);
        }

        private Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down))
            {
                movementDirection += Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                movementDirection -= Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                movementDirection += Vector2.UnitX;
            }

            // Can't normalize the zero vector so test for it before normalizing
            if (movementDirection != Vector2.Zero)
            {
                movementDirection.Normalize();
            }

            return movementDirection;
        }

        private void MoveCamera(GameTime gameTime)
        {
            var speed = 200;
            var seconds = gameTime.GetElapsedSeconds();
            var movementDirection = GetMovementDirection();
            _cameraPosition += speed * movementDirection * seconds;
        }

    }
}
