﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MegaMan.Common.Tests
{
    [TestClass]
    public class SpriteTests
    {
        [TestMethod, TestCategory("Sprite")]
        public void Update_Playing_Runs()
        {
            var sprite = GetEmptySprite(5);

            Assert.AreEqual(0, sprite.CurrentIndex);

            sprite.Play();
            sprite.Update();

            Assert.AreEqual(1, sprite.CurrentIndex);
        }

        [TestMethod, TestCategory("Sprite")]
        public void Update_GreaterThanOne_SkipsFrames()
        {
            var sprite = GetEmptySprite(4);
            sprite.Play();

            sprite.Update(3);

            Assert.AreEqual(3, sprite.CurrentIndex);
        }

        [TestMethod, TestCategory("Sprite")]
        public void Update_Stopped_DoesntRun()
        {
            var sprite = GetEmptySprite(5);
            sprite.Play();
            sprite.Update();

            // should be at 1...

            sprite.Pause();
            sprite.Update();

            Assert.AreEqual(1, sprite.CurrentIndex);
        }

        [TestMethod, TestCategory("Sprite")]
        public void Update_Stop_Resets()
        {
            var sprite = GetEmptySprite(5);
            sprite.Play();
            sprite.Update();

            sprite.Stop();

            Assert.AreEqual(0, sprite.CurrentIndex);
        }

        [TestMethod, TestCategory("Sprite")]
        public void Update_LongFrame_Stays()
        {
            var sprite = GetEmptySprite(5);
            sprite[0].Duration = 2;
            sprite.Play();

            sprite.Update();
            Assert.AreEqual(0, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(1, sprite.CurrentIndex);
        }

        [TestMethod, TestCategory("Sprite")]
        public void Update_ForwardOnce()
        {
            var sprite = GetEmptySprite(3);
            sprite.AnimDirection = AnimationDirection.Forward;
            sprite.AnimStyle = AnimationStyle.PlayOnce;
            sprite.Play();

            Assert.AreEqual(0, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(1, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(2, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(2, sprite.CurrentIndex);
        }

        [TestMethod, TestCategory("Sprite")]
        public void Update_BackwardOnce()
        {
            var sprite = GetEmptySprite(3);
            sprite.AnimDirection = AnimationDirection.Backward;
            sprite.AnimStyle = AnimationStyle.PlayOnce;
            sprite.Play();

            Assert.AreEqual(2, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(1, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(0, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(0, sprite.CurrentIndex);
        }

        [TestMethod, TestCategory("Sprite")]
        public void Update_ForwardRepeat()
        {
            var sprite = GetEmptySprite(3);
            sprite.AnimDirection = AnimationDirection.Forward;
            sprite.AnimStyle = AnimationStyle.Repeat;
            sprite.Play();

            Assert.AreEqual(0, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(1, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(2, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(0, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(1, sprite.CurrentIndex);
        }

        [TestMethod, TestCategory("Sprite")]
        public void Update_BackwardRepeat()
        {
            var sprite = GetEmptySprite(3);
            sprite.AnimDirection = AnimationDirection.Backward;
            sprite.AnimStyle = AnimationStyle.Repeat;
            sprite.Play();

            Assert.AreEqual(2, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(1, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(0, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(2, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(1, sprite.CurrentIndex);
        }

        [TestMethod, TestCategory("Sprite")]
        public void Update_Bounce()
        {
            var sprite = GetEmptySprite(3);
            sprite.AnimDirection = AnimationDirection.Forward;
            sprite.AnimStyle = AnimationStyle.Bounce;
            sprite.Play();

            Assert.AreEqual(0, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(1, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(2, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(1, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(0, sprite.CurrentIndex);

            sprite.Update();
            Assert.AreEqual(1, sprite.CurrentIndex);
        }

        private static Sprite GetEmptySprite(int frames)
        {
            var sprite = new Sprite(1, 1);

            for (int i = 0; i < frames; i++)
            {
                sprite.AddFrame(0, 0, 1);
            }

            return sprite;
        }
    }
}
